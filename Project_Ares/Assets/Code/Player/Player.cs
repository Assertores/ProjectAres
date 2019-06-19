using System.Collections;
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
    [RequireComponent(typeof(SMG))]
    [RequireComponent(typeof(RocketLauncher))]
    public class Player : MonoBehaviour, IDamageableObject {

        public static List<Player> s_references = new List<Player>();
        public static List<Player> s_sortedRef = new List<Player>();

        #region Variables

        [Header("References")]
        [SerializeField] Transform m_controlRef;

        [SerializeField] GameObject m_player;
        [SerializeField] PlayerGUIHandler m_GUIHandler_;
        public PlayerGUIHandler m_GUIHandler { get { return m_GUIHandler_; } set { } }
        [SerializeField] Transform m_modelRef;
        [SerializeField] ModellRefHolder m_modellRefHolder_;
        public ModellRefHolder m_modellRefHolder { get { return m_modellRefHolder_; } set { } }
        //---- ----- Feedback ----- ----
        [SerializeField] ParticleSystem m_laserdeathVFX;
        [SerializeField] GameObject m_laserParent;
        [SerializeField] ParticleSystem m_deathVFX;

        

        [Header("Balancing")]
        [SerializeField] private float m_regenTime;
        [SerializeField] private float m_regeneration;
        [SerializeField] float m_maxHealth = 100;
        [Tooltip("Immer in Sekunden angeben")]
        [SerializeField] float m_iFrames = 1;
        [Range(0,1)]
        [SerializeField] float m_bounciness = 0.3f;

        public int m_team;
        public d_playerData m_stats;

        public float m_distanceToGround;

        public IControl m_control { get; private set; }
        
        List<Player> m_assistRefs = new List<Player>();

        float m_respawntTime = float.MaxValue;
        private float m_time;
        float m_currentHealth;
        public int m_currentChar { get; private set; } = 0;
        int m_currentName;
        bool m_useSMG = true;

        bool m_isShooting = false;
        bool m_isInvincible = false;
        Vector2 vel;

        bool m_isColliding;
        bool m_isCollidingLaser;

        public Rigidbody2D m_rb { get; private set; }
        SMG m_smg;
        RocketLauncher m_rocketLauncher;

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

            ResetTracking();

            //m_distanceToGround = transform.position.y - m_modelRef.position.y;//in modell auslagern
    }

        private void OnDestroy() {
            s_references.Remove(this);
            s_sortedRef.Remove(this);
        }

        void Start() {
            m_rb = GetComponent<Rigidbody2D>();
            m_smg = GetComponent<SMG>();
            m_smg.Init(this);
            m_rocketLauncher = GetComponent<RocketLauncher>();
            m_rocketLauncher.Init(this);
        }

        void Update() {
            if (m_control != null && m_currentHealth > 0) {

                RotateWeapon();

                //health regen only
                if (m_currentHealth < m_maxHealth) {
                    if (m_time + m_regenTime <= Time.timeSinceLevelLoad) {

                        m_currentHealth += (m_regeneration * Time.deltaTime);
                        if (m_currentHealth > m_maxHealth) {
                            m_currentHealth = m_maxHealth;
                        }
                    }
                }
            }

            //flipping weapon
            if (m_control.m_dir.x < 0) {
                m_modellRefHolder.m_weaponRot.localScale = new Vector3(1, -1, 1);
            } else {
                m_modellRefHolder.m_weaponRot.localScale = new Vector3(1, 1, 1);
            }

            //----- ----- Feedback ----- -----
            if (!m_isColliding) {
                if (true/*!m_modellRefHolder.m_modelAnim.animation.isPlaying*/) {
                    StartAnim("02_Idle_Luft");
                }
            } else {
                if (true/*!m_modellRefHolder.m_modelAnim.animation.isPlaying*/) {
                    StartAnim("01_Idle");
                }
            }

            m_GUIHandler.m_health.fillAmount = (float)m_currentHealth / m_maxHealth;

            if (!m_isInvincible) {
                m_GUIHandler.m_points.text = m_stats.m_points.ToString();
            } else {
                m_GUIHandler.m_points.text = "";
            }
        }

        void FixedUpdate() {
            //save velosity of last tick to make rebounce heapon
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
            if (m_modellRefHolder.m_modelAnim != null) {
                print("hit");
                StartAnim("04_Treffer", 1);
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
                 //m_playerParent.SetActive(false);

                 InControle(false);

                m_deathVFX.transform.position = transform.position;
               


                //----- ----- Kill Feed ----- -----
                KillFeedHandler.AddKill(DataHolder.s_playerNames[source.m_currentName],
                                        DataHolder.s_characterDatas[source.m_currentChar].m_icon,
                                        icon,
                                        DataHolder.s_characterDatas[m_currentChar].m_icon,
                                        DataHolder.s_playerNames[m_currentName]);//TODO: KillerWeapon herausfinden
                //----- ----- Feedback ----- -----
                StartAnim("05_Sterben", 1);
                m_deathVFX.Play();

                StartCoroutine(PlayerDie(1.0f));
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
            //m_playerParent.SetActive(false);
            



            if (source)
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
            

            m_laserParent.transform.position = transform.position;
            StartAnim("05_Sterben", 1);
           
            print("hit laser");
            m_laserdeathVFX.Play();
            
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
                Destroy(transform.root.gameObject);
                return;
            }
            
            control.transform.parent = m_controlRef;
            control.transform.localPosition = Vector3.zero;
            m_control = control.GetComponent<IControl>();

            if(m_control == null) {
                Destroy(transform.root.gameObject);
                return;
            }

            m_currentName = Random.Range(-1, DataHolder.s_playerNames.Count - 2);

            InControle(true);

            Respawn(transform.position);//hier die richtige position eingeben

            //----- ----- Tracking ----- -----
            m_joinTime = Time.time;
            m_weaponChangeTime = Time.time;

            m_stats.m_spawnTimeSinceLevelLoad = Time.timeSinceLevelLoad;

            //---- ----- Feedback ----- ----
            
            StartAnim("06_Respawn",1);
        }

        #region Resets

        public void FullReset() {
            DoReset();
            ResetStatsFull();
            InControle(true);
        }

        public void DoReset() {//Reset ist von MonoBehaviour benutz
            StopAllCoroutines();

            if (!m_alive) {
                Respawn(transform.position);
            }

            StopShooting();
            ResetHealth();
            ResetVelocity();
        }

        public void ResetHealth() {
            m_currentHealth = m_maxHealth;
            m_alive = true;
        }

        public void ResetVelocity() {
            if (m_rb)
                m_rb.velocity = Vector2.zero;
        }

        public void ResetStatsFull() {
            ResetTracking();
            ResetStuts();
        }

        public void ResetTracking() {
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
        }

        public void ResetStuts() {
            m_stats.m_points = 0;
            m_stats.m_kills = 0;
            m_stats.m_deaths = 0;
            m_stats.m_assists = 0;
            m_stats.m_damageDealt = 0;
            m_stats.m_damageTaken = 0;
        }

        #endregion
        #region Controlers

        public void InControle(bool controle) {
            if (controle) {
                m_control.StartShooting = StartShooting;
                m_control.StopShooting = StopShooting;
                

                m_control.ChangeWeapon = ChangeWeapon;
                m_control.Disconnect = Disconect;
            } else {
                m_control.StartShooting = null;
                m_control.StopShooting = null;
                
                m_control.ChangeCharacter = null;
                m_control.ChangeWeapon = null;
                m_control.Disconnect = null;
                StopShooting();
            }
        }
        
        public void SetChangeCharAble(bool able) {
            if (able) {
                m_control.ChangeCharacter = ChangeCharacter;
            } else {
                m_control.ChangeCharacter = null;
            }
        }

        #endregion

        IEnumerator PlayerDie(float m_wait) {
            InControle(false);
            yield return new WaitForSeconds(m_wait);
            GameManager.s_singelton?.PlayerDied(this);

        }
        
        //gameobject einschalten
        //respawn effect abfeuern
        public IEnumerator Respawn(Vector2 pos, float delay = 0) {
            float startTime = Time.time;
            Vector2 starPos = transform.position;

            while (startTime + delay > Time.time) {
                //----- stuff that should happon in between -----
                transform.position = Vector2.Lerp(starPos, pos, (Time.time - startTime) / delay);
                yield return null;
            }

            //----- stuff that should happon after -----
            transform.position = pos;
            if (m_rb)
                m_rb.velocity = Vector2.zero;
            StopShooting();
            ResetHealth();
            m_respawntTime = Time.timeSinceLevelLoad;

            if (m_modellRefHolder.m_modelAnim != null) {
                float tmp = StartAnim("06_Respawn", 1);//In stringCollection übertragen
                if (tmp > 0)
                    yield return new WaitForSeconds(tmp);
            }

            InControle(true);

            //m_playerParent.SetActive(true);//TODO auf neues system umschreiben
            
        }

        public void Invincible(bool inv) {
            m_isInvincible = inv;
        }

        void ChangeCharacter(bool next) {
            StopShooting();

            if (next)
                m_currentChar++;
            else
                m_currentChar--;

            m_currentChar = (m_currentChar % DataHolder.s_characterDatas.Count + DataHolder.s_characterDatas.Count) % DataHolder.s_characterDatas.Count;

            m_modellRefHolder_ = Instantiate(DataHolder.s_characterDatas[m_currentChar].m_model, m_modelRef).GetComponent<ModellRefHolder>();

            RotateWeapon();

            //----- ----- Tracking ----- -----

            if (m_stats.m_firstCaracterChange == 0)
                m_stats.m_firstCaracterChange = Time.time - m_joinTime;
        }

        void ChangeWeapon() {
            if (m_useSMG) {
                m_smg.SetActive(false);
                m_rocketLauncher.SetActive(true);
                if (m_isShooting)
                    m_rocketLauncher.StartShooting();
            } else {
                m_rocketLauncher.SetActive(false);
                m_smg.SetActive(true);
                if (m_isShooting)
                    m_smg.StartShooting();
            }
            
            //----- ----- Tracking ----- -----
            if (m_stats.m_firstWeaponChange == 0)
                m_stats.m_firstWeaponChange = Time.time - m_joinTime;

            m_stats.m_weaponSwitchCount++;
            if (m_useSMG)
                m_stats.m_sMGTime += Time.time - m_weaponChangeTime;
            else
                m_stats.m_rocketTime += Time.time - m_weaponChangeTime;

            m_weaponChangeTime = Time.time;


            m_useSMG = !m_useSMG;
        }

        void StartShooting() {
            if (!m_isShooting) {
                if (m_useSMG) {
                    m_smg.StartShooting();
                } else {
                    m_rocketLauncher.StartShooting();
                    StartAnim("09_Zielen");//In stringCollection übertragen
                }
            }
            m_isShooting = true;

            //----- ----- Tracking ----- -----
            if (m_stats.m_firstShot == 0)
                m_stats.m_firstShot = Time.time - m_joinTime;
        }

        void StopShooting() {
            m_isShooting = false;
            if (m_useSMG) {
                m_smg.StopShooting();
            } else {
                m_rocketLauncher.StopShooting();
                StartAnim("11_RaketeSchießen", 1);
            }
        }

        public void Disconect() {
            Destroy(this.transform.root.gameObject);
            s_references.Remove(this);
            s_sortedRef.Remove(this);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="animName">the animation name</param>
        /// <param name="playTimes">how often it should be played (-1 for loop)</param>
        /// <returns>returns duration in seconds or min value if not valide</returns>
        public float StartAnim(string animName,int playTimes = -1) {
            if (m_modellRefHolder.m_modelAnim != null) {
                //m_modellRefHolder.m_modelAnim.animation.Play(animName, playTimes);
                return -1;//m_modellRefHolder.m_modelAnim.animation.animationConfig.duration;
            }
            return float.MinValue;
        }

        void RotateWeapon() {
            m_modellRefHolder.m_weaponRot.rotation = Quaternion.LookRotation(transform.forward, new Vector2(-m_control.m_dir.y, m_control.m_dir.x));
        }

        #region Editor code

        EditorHUDAndPlayerLogic m_editHud;
        bool m_doEdit = false;
        public void GoIntoEditMode() {
            m_doEdit = !m_doEdit;

            if (m_doEdit)
                m_editHud.gameObject.SetActive(!m_editHud.gameObject.activeSelf);

            if (m_editHud.gameObject.activeSelf) {
                InControle(false);
                Invincible(true);
                m_rb.simulated = false;

                m_control.StartShooting = m_editHud.DragHandler;
                m_control.ChangeType = m_editHud.ChangeType;
                m_control.ChangeCharacter = m_editHud.ChangeIndex;
            } else {
                StopEdit();
            }
        }

        public void StopEdit() {
            m_control.StartShooting = null;
            m_control.ChangeType = null;
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

                m_control.Accept += GoIntoEditMode;
            } else {
                m_control.Accept -= GoIntoEditMode;

                StopEdit();
                if (m_editHud)
                    Destroy(m_editHud.gameObject);
            }
        }

        #endregion

        #region Physics

        private void OnCollisionEnter2D(Collision2D collision) {
           
            if (collision.gameObject.tag == "Player" || collision.gameObject.tag == "Level") {
                Vector2 tmp = collision.contacts[0].normal;               
                if (Vector2.Dot(vel.normalized, tmp) < 0) {
                    m_rb.velocity = m_bounciness * ( Vector2.Reflect(vel, tmp));
                }
                
                //---- ----- Feedback ----- ----

                StartAnim("03_Aufprall", 1);//In stringCollection übertragen
            }
            if(collision.gameObject.tag == "Ground") {
                m_isColliding = true;
                print("is colliding wall" + m_isColliding);
            }
            if (collision.gameObject.tag == "Laser") {
                Vector2 tmp = collision.contacts[0].normal; ;
                Quaternion rotation = Quaternion.LookRotation(transform.forward, new Vector2(tmp.x, tmp.y));
                m_laserParent.transform.rotation = rotation;
            }
            
        }

        #endregion
    }
}
