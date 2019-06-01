﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


namespace PPBC {

    public struct d_playerData {
        public int m_points;

        public string m_name;
        public int m_kills;
        public int m_deaths;
        public int m_assists;
        public float m_damageDealt;
        public float m_damageTaken;

        //----- ----- Tracking ----- -----

        public float m_spawnTimeSinceLevelLoad;
        public float m_firstShot;
        public float m_firstWeaponChange;
        public float m_firstCaracterChange;
        public float m_firstNameChange;
        public float m_sMGTime;
        public float m_rocketTime;
        public float m_timeInLobby;
        public int m_weaponSwitchCount;
        public int m_deathBySuicide;


        public override string ToString() {
            return m_name + ";" + m_kills + ";" + m_deaths + ";" + m_assists + ";" + m_damageDealt + ";" + m_damageTaken;
        }
        public string StringWithNewLine() {
            return "Kills: " + m_kills + System.Environment.NewLine + "Deaths: " + m_deaths + System.Environment.NewLine + "Assists: " + m_assists + System.Environment.NewLine + "Damage Dealt: " + m_damageDealt + System.Environment.NewLine + "Damage Taken: " + m_damageTaken;
        }
    }
    
    [RequireComponent(typeof(Rigidbody2D))]
    public class Player : MonoBehaviour, IDamageableObject {

        public static List<Player> s_references = new List<Player>();
        public static List<Player> s_sortedRef = new List<Player>();

        #region Variables

        [Header("References")]
        [SerializeField] Transform m_modelRef;
        [SerializeField] Transform m_weaponRef;
        [SerializeField] Transform m_controlRef;

        [SerializeField] Image m_healthBar;
        [SerializeField] Image m_weaponValue;
        [SerializeField] GameObject m_controlObject;
        [SerializeField] LayerMask m_dashColliders;
        [SerializeField] PlayerGUIHandler m_GUIHandler;
        [SerializeField] Sprite m_characterIcon;//muss von ausen veränderbar sein
        [SerializeField] string m_characterName;
        [SerializeField] TMPro.TextMeshProUGUI m_killsRef;
        [SerializeField] ModellRefHolder m_modellRefHolder;
        //---- ----- Feedback ----- ----
        [SerializeField] GameObject m_laserdeathVFX;
        [SerializeField] ParticleSystem m_deathVFX;
        private DragonBones.UnityArmatureComponent m_modelAnim;

        

        [Header("Balancing")]
        [SerializeField] private float m_regenTime;
        [SerializeField] private float m_regeneration;
        [SerializeField] float m_maxHealth = 100;
        [SerializeField] float m_dashForce = 2;
        [Tooltip("Immer in Sekunden angeben")]
        [SerializeField] float m_iFrames = 1;
        [SerializeField] float m_gravity = 1;
        [Range(0,1)]
        [SerializeField] float m_airResistance = 0.25f;
        [SerializeField] float m_bounciness = 0.5f;

        public int m_team;

        public d_playerData m_stats;

        public float m_distanceToGround;

        public IControl m_control { get; private set; }
        List<IWeapon> m_weapons = new List<IWeapon>();
        List<IItem> m_items = new List<IItem>();//eventuell obsolet

        Dictionary<Collider2D, Vector2> m_collisionNormals = new Dictionary<Collider2D, Vector2>();//eventuel obsolet
        List<Player> m_assistRefs = new List<Player>();

        float m_respawntTime = float.MaxValue;
        private float m_time;
        float m_currentHealth;
        public int m_currentChar { get; private set; } = 0;
        int m_currentWeapon = 0;
        bool m_isShooting = false;
        bool m_isInvincible = false;
        Vector2 vel;

        int m_currentName;
        bool m_isColliding;
        bool m_isCollidingLaser;

        public Rigidbody2D m_rb { get; private set; }

        //----- ----- Tracking variables ----- -----

        public float m_joinTime { get; private set; }
        float m_weaponChangeTime;

        //---- ----- Feedback ----- ----
       

        #endregion
        #region MonoBehaviour

        void Awake() {
            DontDestroyOnLoad(this.gameObject.transform.parent);//dirty
            s_references.Add(this);
            s_sortedRef.Add(this);

            //----- ----- Tracking ----- -----

            m_stats.m_spawnTimeSinceLevelLoad = float.MaxValue;
            m_stats.m_firstShot = 0;
            m_stats.m_firstWeaponChange = 0;
            m_stats.m_firstCaracterChange = 0;
            m_stats.m_firstNameChange = 0;
            m_stats.m_sMGTime = 0;
            m_stats.m_rocketTime = 0;
            m_stats.m_timeInLobby = 0;
            m_stats.m_weaponSwitchCount = 0;
            m_stats.m_deathBySuicide = 0;

            m_distanceToGround = transform.position.y - m_modelRef.position.y;
    }

        void Start() {
        
            m_rb = GetComponent<Rigidbody2D>();
            
            
        }
        private void OnDestroy() {
            s_references.Remove(this);
            s_sortedRef.Remove(this);
        }
        
        void Update() {
            
            if (m_control != null && m_currentHealth > 0) {
                
                m_weaponRef.rotation = Quaternion.LookRotation(transform.forward, new Vector2(-m_control.m_dir.y, m_control.m_dir.x));//vektor irgendwie drehen, damit es in der 2d plain bleibt
                

                if (m_currentHealth < m_maxHealth) {
                    if (m_time + m_regenTime <= Time.timeSinceLevelLoad) {

                        m_currentHealth += (m_regeneration * Time.deltaTime);
                            if (m_currentHealth > m_maxHealth) {
                                m_currentHealth = m_maxHealth;
                            }
                    }
                }
            }

            if (m_control.m_dir.x < 0) {
                m_weaponRef.localScale = new Vector3(1, -1, 1);
            } else {
                m_weaponRef.localScale = new Vector3(1, 1, 1);
            }
            if (m_modellRefHolder != null) {
                m_weaponRef.position = m_modellRefHolder.m_weaponPos.position;
            }
            m_controlRef.position = m_weaponRef.position;
            m_controlRef.rotation = m_weaponRef.rotation;

            //----- ----- Feedback ----- -----
            if (m_modelAnim?.animation.lastAnimationName != "05_Sterben") {
                if (m_isColliding) {
                    if (m_modelAnim != null && !m_modelAnim.animation.isPlaying) {
                        m_modelAnim.animation.Play("02_Idle_Luft");//In stringCollection übertragen
                    }
                } else {
                    if (m_modelAnim != null && !m_modelAnim.animation.isPlaying) {
                        m_modelAnim.animation.Play("01_Idle");
                    }
                }
            }
            m_healthBar.fillAmount = (float)m_currentHealth / m_maxHealth;
            m_weaponValue.fillAmount = m_weapons[m_currentWeapon].m_value;

            m_GUIHandler.SetHealth(m_healthBar.fillAmount);

            if (!m_isInvincible) {
                m_killsRef.text = m_stats.m_points.ToString();
            } else {
                m_killsRef.text = "";
            }
                

            m_GUIHandler.m_debugStats.text = m_stats.StringWithNewLine();
        }

        void FixedUpdate() {
            vel = m_rb.velocity;
        }

        #endregion
        #region IDamageableObject

        public bool m_alive { get; set; }

        public void TakeDamage(float damage, Player source, Vector2 force, Sprite icon) {
           
            if (!m_alive) {
                return;
            }
            if (source == this) {
                return;
            }
            //---- ----- Feedback ----- ----
            if (m_modelAnim != null) {
                print("hit");
                m_modelAnim.animation.Play("04_Treffer", 1);
            }
            if (m_isInvincible) {
                return;
            }
            if (Time.timeSinceLevelLoad - m_respawntTime < m_iFrames) {
                return;
            }
          
            if (damage >= m_currentHealth) {
                m_stats.m_damageTaken += m_currentHealth;
                m_stats.m_deaths++;
                if (source) {
                    source.m_stats.m_damageDealt += m_currentHealth;
                    source.m_stats.m_kills++;
                    m_assistRefs.Remove(source);
                } else {
                    m_assistRefs[m_assistRefs.Count - 1].m_stats.m_damageDealt += m_currentHealth;
                    m_assistRefs[m_assistRefs.Count - 1].m_stats.m_kills++;
                    m_assistRefs.Remove(m_assistRefs[m_assistRefs.Count - 1]);
                }
                foreach (var it in m_assistRefs) {//bekommen alle einen assist oder gibt es ein zeit limit oder nur der letzte?
                    it.m_stats.m_assists++;
                }
                m_assistRefs.Clear();

                m_currentHealth = 0;
                m_alive = false;
                m_rb.velocity = Vector2.zero;
                gameObject.SetActive(false);

                InControle(false);
                GameManager.s_singelton?.PlayerDied(this);

                //----- ----- Kill Feed ----- -----
                KillFeedHandler.AddKill(DataHolder.s_playerNames[source.m_currentName],
                                        DataHolder.s_characterDatas[source.m_currentChar].m_icon,
                                        icon,
                                        DataHolder.s_characterDatas[m_currentChar].m_icon,
                                        DataHolder.s_playerNames[m_currentName]);//TODO: KillerWeapon herausfinden
            } else {
                m_stats.m_damageTaken += damage;
                m_rb.velocity += (force / m_rb.mass);//AddForce will irgendwie nicht funktionieren
                //m_rb.AddForce(force);
                if (source) {
                    source.m_stats.m_damageDealt += damage;
                    if(!m_assistRefs.Exists(x => x == source))
                        m_assistRefs.Add(source);
                    else {
                        m_assistRefs.Remove(source);
                        m_assistRefs.Add(source);
                    }
                }

                m_currentHealth -= damage;
                m_time = Time.timeSinceLevelLoad;
                
                //----- ----- Feedback ----- -----
                
            }
        }

        public void Die(Player source) {
            m_stats.m_deaths++;
            

            if(m_assistRefs.Count > 0) {
                if (source) {
                    source.m_stats.m_kills++;
                } else {
                    m_assistRefs[m_assistRefs.Count - 1].m_stats.m_kills++;
                    m_assistRefs.Remove(m_assistRefs[m_assistRefs.Count - 1]);
                }
                
                for (int i = 0; i < m_assistRefs.Count; i++) {//eventuell gegen funktion ersetzten
                    m_assistRefs[i].m_stats.m_assists++;
                }
                m_assistRefs.Clear();
            }
            

            m_currentHealth = 0;
            m_alive = false;
            m_rb.velocity = Vector2.zero;
            

            InControle(false);
            
            

            if(source)
                KillFeedHandler.AddKill(DataHolder.s_playerNames[source.m_currentName],
                                        DataHolder.s_characterDatas[source.m_currentChar].m_icon,
                                        null,
                                        DataHolder.s_characterDatas[m_currentChar].m_icon,
                                        DataHolder.s_playerNames[m_currentName]);//TODO: KillerWeapon herausfinden
            else
                KillFeedHandler.AddKill("suicide",
                                        null,
                                        null,
                                        DataHolder.s_characterDatas[m_currentChar].m_icon,
                                        DataHolder.s_playerNames[m_currentName]);//TODO: KillerWeapon herausfinden

            //----- ----- Tracking ----- -----
            m_stats.m_deathBySuicide++;
            //---- ----- Feedback ----- ----
            StartAnim("05_Sterben", 1);
            if (m_isCollidingLaser) {
                print("hit laser");
                m_laserdeathVFX.SetActive(true);
            } else {
                m_deathVFX.Play();
            }
            StartCoroutine(PlayerDie(1.0f));


        }

        public float GetHealth() {
            return m_currentHealth;
        }

        #endregion

        /// <summary>
        /// dirty wegen nicht direkt IControl übergeben
        /// </summary>
        public void Init(GameObject control) {//dirty wegen nicht direkt IControl übergeben
            if (control == null) {
                if(m_controlObject == null) {
                    DestroyImmediate(gameObject);
                    return;
                }
                m_control = m_controlObject.GetComponent<IControl>();
                if(m_control == null) {
                    //Destroy(gameObject);
                    DestroyImmediate(gameObject);
                    return;
                }
            } else {
                control.transform.parent = m_controlRef;
                control.transform.localPosition = Vector3.zero;
                m_control = control.GetComponent<IControl>();
            }

            m_currentName = Random.Range(-1, DataHolder.s_playerNames.Count - 2);
            ChangeName(true);
            RepositionGUI();

            InControle(true);

            ChangeCharacter(0, false);//damit der erste Charakter ausgewählt ist
            ChangeWeapon(0);//damit die erste waffe ausgewählt ist

            Respawn(transform.position);//hier die richtige position eingeben
            //WeaponIcons in WheaponWheel einfügen;

            //----- ----- Tracking ----- -----
            m_joinTime = Time.time;
            m_weaponChangeTime = Time.time;

            m_stats.m_spawnTimeSinceLevelLoad = Time.timeSinceLevelLoad;

            //---- ----- Feedback ----- ----

            m_modelAnim = GetComponentInChildren<DragonBones.UnityArmatureComponent>();
        }

        public void DoReset() {//Reset ist von MonoBehaviour benutz
            if (!m_alive) {
                Respawn(transform.position);
            }
            //RepositionGUI();//eventuell over the top
            StopShooting();
            m_currentHealth = m_maxHealth;
            if(m_rb)
                m_rb.velocity = Vector2.zero;
        }

        public void InControle(bool controle) {
            if (controle) {
                m_control.StartShooting = StartShooting;
                m_control.StopShooting = StopShooting;
                m_control.Dash = Dash;

                m_control.SelectWeapon = SelectWeapon;

                m_control.ChangeWeapon = ChangeWeapon;
                m_control.UseItem = UseItem;
                m_control.Disconnect = Disconect;
            } else {
                m_control.StartShooting = null;
                m_control.StopShooting = null;
                m_control.Dash = null;

                m_control.SelectWeapon = null;
                m_control.ChangeCharacter = null;
                m_control.ChangeWeapon = null;
                m_control.UseItem = null;
                m_control.Disconnect = null;
                StopShooting();
            }
        }


        public void SetChangeCharAble(bool able) {
            if (able) {
                m_control.ChangeCharacter = ChangeCharacter;
                m_GUIHandler.SetCharChangeActive(true);
                m_control.ChangeName = ChangeName;
            } else {
                m_control.ChangeCharacter = null;
                m_GUIHandler.SetCharChangeActive(false);
                m_control.ChangeName = null;
            }
        }

        public void Respawn(Vector2 pos) {
            transform.position = pos;

            StopShooting();

            if(m_rb)
                m_rb.velocity = Vector2.zero;

            m_currentHealth = m_maxHealth;
            m_alive = true;
            /*if(_currentWeapon != 0) {
                _weapons[_currentWeapon].SetActive(false);
                _currentWeapon = 0;
                _weapons[_currentWeapon].SetActive(true);
            }*/
            InControle(true);
            gameObject.SetActive(true);
            m_respawntTime = Time.timeSinceLevelLoad;
            //---- ----- Feedback ----- ----
            if (m_modelAnim != null) {
                m_modelAnim.animation.Play("06_Respawn",1);//In stringCollection übertragen
            }
        }

        public void Invincible(bool inv) {
            m_isInvincible = inv;
        }

        void SelectWeapon(int selectedWeapon) {
            if (selectedWeapon >= m_weapons.Count || selectedWeapon < 0)
                selectedWeapon = 0;

            //_weaponWheel.GetChild(selectedWeapon) highlight selected item
        }

        void ChangeName(bool next = true) {
            if(!next && m_currentName <= 0) {
                print("nameindex to low");
                return;
            }

            if (next) {
                m_currentName++;
            } else {
                m_currentName--;
            }

            m_stats.m_name = DataHolder.GetPlayerName(m_currentName);
            m_GUIHandler.SetName(m_stats.m_name);

            //----- ----- Tracking ----- -----
            if (m_stats.m_firstNameChange == 0)
                m_stats.m_firstNameChange = Time.time - m_joinTime;
        }

        void ChangeCharacter(int newCaracter, bool relative = true) {
            if(!relative && (newCaracter < 0 && newCaracter >= DataHolder.s_characterDatas.Count)) {
                return;
            }
            if (m_weapons != null && m_isShooting) {
                m_weapons[m_currentWeapon].StopShooting();
                m_isShooting = false;
            }

            m_weapons.Clear();

            foreach(Transform it in m_modelRef) {
                Destroy(it.gameObject);
            }
            foreach(Transform it in m_weaponRef) {
                Destroy(it.gameObject);
            }

            if (relative) {
                m_currentChar += newCaracter;
                m_currentChar = (m_currentChar % DataHolder.s_characterDatas.Count + DataHolder.s_characterDatas.Count) % DataHolder.s_characterDatas.Count;
            } else {
                m_currentChar = newCaracter;
            }

            GameObject model = Instantiate(DataHolder.s_characterDatas[m_currentChar].m_model, m_modelRef);
            m_weapons.Add(Instantiate(DataHolder.s_characterDatas[m_currentChar].m_sMG, m_weaponRef).GetComponent<IWeapon>());//null reference test
            m_weapons[m_weapons.Count - 1].Init(this);
            m_weapons.Add(Instantiate(DataHolder.s_characterDatas[m_currentChar].m_rocked, m_weaponRef).GetComponent<IWeapon>());//null reference test
            m_weapons[m_weapons.Count - 1].Init(this);
            for (int i = 0; i < m_weapons.Count; i++) {
                if(i != m_currentWeapon) {
                    m_weapons[i].SetActive(false);
                }
            }
            /*if (m_modellRefHolder != null) {
                print("model found" + m_modellRefHolder.m_modelAnim);
                m_modelAnim = m_modellRefHolder.m_modelAnim;
            }*/
            m_modelAnim = GetComponentInChildren<DragonBones.UnityArmatureComponent>();

            if (m_modelAnim != null) {
                m_modelAnim.animation.Play("Idle");//In stringCollection übertragen
            }
            m_GUIHandler.ChangeCharacter(DataHolder.s_characterDatas[m_currentChar].m_icon, DataHolder.s_characterDatas[m_currentChar].m_name);
            m_GUIHandler.ChangeWeapon(m_weapons[m_currentWeapon].m_icon);

            //----- ----- Tracking ----- -----

            if (m_stats.m_firstCaracterChange == 0)
                m_stats.m_firstCaracterChange = Time.time - m_joinTime;
        }

        void ChangeWeapon(int newWeapon, bool relative = false) {
            if ((newWeapon < m_weapons.Count && newWeapon >= 0) || relative) {
                m_weapons[m_currentWeapon].StopShooting();
                m_weapons[m_currentWeapon].SetActive(false);

                if (relative) {
                    m_currentWeapon += newWeapon;
                    m_currentWeapon = (m_currentWeapon % m_weapons.Count + m_weapons.Count) % m_weapons.Count;//https://stackoverflow.com/questions/1082917/mod-of-negative-number-is-melting-my-brain/1082938
                    //m_currentWeapon %= m_weapons.Count;//c# mod im negativen bereich ist scheise
                } else {
                    m_currentWeapon = newWeapon;
                }

                m_weapons[m_currentWeapon].SetActive(true);
                m_GUIHandler.ChangeWeapon(m_weapons[m_currentWeapon].m_icon);

                if (m_isShooting)
                    m_weapons[m_currentWeapon].StartShooting();
            }

            //----- ----- Tracking ----- -----
            if (m_stats.m_firstWeaponChange == 0)
                m_stats.m_firstWeaponChange = Time.time - m_joinTime;

            m_stats.m_weaponSwitchCount++;
            if (m_currentWeapon - newWeapon == 0)
                m_stats.m_sMGTime += Time.time - m_weaponChangeTime;
            else
                m_stats.m_rocketTime += Time.time - m_weaponChangeTime;

            m_weaponChangeTime = Time.time;
        }

        void StartShooting() {
            if(!m_isShooting)
                m_weapons[m_currentWeapon].StartShooting();
            m_isShooting = true;

            //----- ----- Tracking ----- -----
            if (m_stats.m_firstShot == 0)
                m_stats.m_firstShot = Time.time - m_joinTime;
            //---- ----- Feedback ----- ----
            if (m_currentWeapon == 1){
                if (m_modelAnim != null) {
                    m_modelAnim.animation.Play("09_Zielen");//In stringCollection übertragen
                }
            }
        }

        void StopShooting() {
            m_isShooting = false;
            m_weapons[m_currentWeapon].StopShooting();
            if (m_currentWeapon == 1) {
                if (m_modelAnim != null) {
                    m_modelAnim.animation.Play("11_RaketeSchießen",1);//In stringCollection übertragen
                }
            }
        }

        void UseItem(int item) {
            if(item < m_items.Count && item >= 0) {
                m_items[item].Activate();
                m_items.Remove(m_items[item]);
            }
        }

        void Dash() {
            Vector2 tmp = new Vector2(0, 0);
            foreach(var it in m_collisionNormals) {
                tmp += it.Value;
            }
            m_rb.AddForce(tmp.normalized * m_dashForce);
        }

        public void Disconect() {
            Destroy(this.transform.root.gameObject);
            s_references.Remove(this);
            s_sortedRef.Remove(this);
            RepositionGUI();
        }

        void RepositionGUI() {
            for (int i = 0; i < s_references.Count; i++) {
                s_references[i].m_GUIHandler.Reposition(((float)i + 1) / (s_references.Count + 1));
            }
        }
        

        public void SetStatsAble(bool doable) {
            if (doable) {
                m_control.ShowStats = ShowStatsToGUI;
            } else {
                m_control.ShowStats = null;
                m_GUIHandler.HideStats();
            }

        }

        public void ShowStatsToGUI(bool doIt) {
            if (doIt) {
                m_GUIHandler.WriteStats(m_stats);
            } else {
                m_GUIHandler.HideStats();
            }
        }
        public float StartAnim(string animName,int playTimes) {
            if (m_modelAnim != null) {
                m_modelAnim.animation.Play(animName, playTimes);
                return m_modelAnim.animation.animationConfig.duration;
            }
            return float.MinValue;
        }

        EditorHUDAndPlayerLogic m_editHud;
        public void GoIntoEditMode(bool doEdit) {
            if (doEdit)
                m_editHud.gameObject.SetActive(!m_editHud.gameObject.activeSelf);

            if (m_editHud.gameObject.activeSelf) {
                InControle(false);
                Invincible(true);
                m_rb.simulated = false;

                m_control.StartShooting = m_editHud.DragHandler;
                m_control.ChangeName = m_editHud.ChangeType;
                m_control.ChangeCharacter = m_editHud.ChangeIndex;
            } else {
                StopEdit();
            }
        }

        public void StopEdit() {
            m_control.StartShooting = null;
            m_control.ChangeName = null;
            m_control.ChangeCharacter = null;

            InControle(true);
            Invincible(false);
            m_rb.simulated = true;
        }

        public void EditAble(EditorHUDAndPlayerLogic hud) {
            if(hud != null) {
                m_editHud = hud;
                m_editHud.SetControlRef(m_control);
                m_editHud.gameObject.SetActive(false);

                m_control.ShowStats += GoIntoEditMode;
            } else {
                m_control.ShowStats -= GoIntoEditMode;

                StopEdit();
                if (m_editHud)
                    Destroy(m_editHud.gameObject);
            }
        }

        #region Physics

        private void OnCollisionEnter2D(Collision2D collision) {
           
            if (collision.gameObject.tag == "Player" || collision.gameObject.tag == "Level") {
                Vector2 tmp = collision.contacts[0].normal;               
                if (Vector2.Dot(vel.normalized, tmp) < 0) {
                    m_rb.velocity = m_bounciness * ( Vector2.Reflect(vel, tmp));
                }
                m_isColliding = true;
                //---- ----- Feedback ----- ----
                if (m_modelAnim != null) {
                    m_modelAnim.animation.Play("03_Aufprall", 1);//In stringCollection übertragen
                }
            }
            if (collision.gameObject.tag == "Laser") {
                /*Vector2 tmp = (transform.position);
                Quaternion rotation = Quaternion.LookRotation(transform.forward, new Vector2(tmp.x,tmp.y));
                m_laserdeathVFX.transform.rotation = rotation;*/
                m_isCollidingLaser = true;
                print(m_isCollidingLaser);
            }
            if (m_dashColliders.value == (m_dashColliders | 1<<collision.gameObject.layer)) {//wir nehmen eine 1(true) und schieben es um collision.gameObject.layer nach links, nehmen dann die _dashColiders LayerMask, setzen dieses bool auf true und fragen dann ob dass was da rauskommt dass selbe ist wie die _dashColiders LayerMask
                Vector2 tmpNormal = new Vector2(0, 0);
                foreach (var it in collision.contacts) {
                    tmpNormal += it.normal;
                }
                m_collisionNormals[collision.collider] = tmpNormal.normalized;
            }
            
        }

        private void OnCollisionExit2D(Collision2D collision) {
            m_isColliding = false;
            if (m_dashColliders.value == (m_dashColliders | 1 << collision.gameObject.layer)) {
                m_collisionNormals.Remove(collision.collider);
            }
        }

        #endregion
        IEnumerator PlayerDie(float m_wait) {

            yield return new WaitForSeconds(m_wait);
            gameObject.SetActive(false);
            GameManager.s_singelton?.PlayerDied(this);

        }
    }
}
