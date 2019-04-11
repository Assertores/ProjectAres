using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace ProjectAres {

    public struct d_playerStats {
        public int m_kills;
        public int m_deaths;
        public int m_assists;
        public int m_damageDealt;
        public int m_damageTaken;
    }

    [RequireComponent(typeof(Rigidbody2D))]
    public class Player : MonoBehaviour, IDamageableObject {

        public static List<Player> s_references = new List<Player>();

        #region Variables

        [Header("References")]
        [SerializeField] GameObject[] m_weaponInit;
        [SerializeField] Transform m_weaponAnchor;
        [SerializeField] Transform m_weaponRotationAncor;
        [SerializeField] Image m_healthBar;
        [SerializeField] Transform m_weaponWheel;
        [SerializeField] GameObject m_controlObject;
        [SerializeField] LayerMask m_dashColliders;

        [Header("Balancing")]
        [SerializeField] int m_maxHealth = 100;
        [SerializeField] float m_dashForce = 2;
        [Tooltip("Immer in Sekunden angeben")]
        [SerializeField] float m_iFrames = 1;
        [SerializeField] float m_gravity = 1;
        [Range(0,1)]
        [SerializeField] float m_airResistance = 0.25f;

        public d_playerStats m_stats;

        IControl m_control;
        List<IWeapon> m_weapons = new List<IWeapon>();
        List<IItem> m_items = new List<IItem>();

        Dictionary<Collider2D, Vector2> m_collisionNormals = new Dictionary<Collider2D, Vector2>();
        List<Player> m_assistRefs = new List<Player>();

        float m_respawntTime = float.MaxValue;
        int m_currentHealth;
        int m_currentWeapon = 0;
        bool m_isShooting = false;

        public Rigidbody2D m_rb { get; private set; }

        #endregion
        #region MonoBehaviour

        void Start() {
            DontDestroyOnLoad(this.gameObject);
            //GameManager test = GameManager._singelton;
            m_rb = GetComponent<Rigidbody2D>();
            //Init(null);
            s_references.Add(this);
        }
        private void OnDestroy() {
            s_references.Remove(this);
        }
        
        void Update() {
            if(m_control != null && m_currentHealth > 0)
                m_weaponRotationAncor.rotation = Quaternion.LookRotation(transform.forward,new Vector2(-m_control.m_dir.y,m_control.m_dir.x));//vektor irgendwie drehen, damit es in der 2d plain bleibt

            m_healthBar.fillAmount = (float)m_currentHealth / m_maxHealth;
        }

        //void FixedUpdate() {
        //    m_rb.velocity -= m_rb.velocity * m_airResistance * Time.fixedDeltaTime;
        //    m_rb.velocity += Vector2.down * m_gravity * Time.fixedDeltaTime;
        //}

        #endregion
        #region IDamageableObject

        public bool m_alive { get; set; }

        public void TakeDamage(int damage, Player source, Vector2 force) {
            if (Time.timeSinceLevelLoad - m_respawntTime < m_iFrames) {
                return;
            }
            if(source == this) {
                return;
            }
            if (damage > m_currentHealth) {
                m_stats.m_damageTaken += m_currentHealth;
                m_stats.m_deaths++;
                if (source) {
                    source.m_stats.m_damageDealt += m_currentHealth;
                    source.m_stats.m_kills++;
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
                GameManager.s_singelton.PlayerDied(this);
            } else {
                m_stats.m_damageTaken += damage;
                m_rb.velocity += (force / m_rb.mass);//AddForce will irgendwie nicht funktionieren
                //m_rb.AddForce(force);
                if (source) {
                    source.m_stats.m_damageDealt += damage;
                    m_assistRefs.Add(source);
                }

                m_currentHealth -= damage;
            }
        }

        public void Die(Player source) {
            m_stats.m_deaths++;
            if (source) {
                source.m_stats.m_kills++;
            }
            foreach (var it in m_assistRefs) {//eventuell gegen funktion ersetzten
                it.m_stats.m_assists++;
            }
            m_assistRefs.Clear();

            m_currentHealth = 0;
            m_alive = false;
            m_rb.velocity = Vector2.zero;
            gameObject.SetActive(false);

            InControle(false);
            GameManager.s_singelton.PlayerDied(this);
        }

        public int GetHealth() {
            return m_currentHealth;
        }

        #endregion

        public void Init(IControl control) {
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
                m_control = control;
            }

            InControle(true);

            GameObject tmp;
            IWeapon tmpInterface;
            foreach (var it in m_weaponInit) {
                tmpInterface = it.GetComponent<IWeapon>();
                if (tmpInterface != null) {
                    tmp = Instantiate(it, m_weaponAnchor);
                    tmpInterface = tmp.GetComponent<IWeapon>();
                    tmpInterface.Init(this);
                    tmpInterface.SetActive(false);
                    m_weapons.Add(tmpInterface);
                }
            }

            ChangeWeapon(0);//damit die erste waffe ausgewählt ist

            Respawn(transform.position);//hier die richtige position eingeben
            //WeaponIcons in WheaponWheel einfügen;
        }

        public void InControle(bool controle) {
            if (controle) {
                m_control.StartShooting += StartShooting;
                m_control.StopShooting += StopShooting;
                m_control.Dash += Dash;

                m_control.SelectWeapon += SelectWeapon;
                m_control.ChangeWeapon += ChangeWeapon;
                m_control.UseItem += UseItem;
                m_control.Disconnect += Disconect;
            } else {
                m_control.StartShooting = null;
                m_control.StopShooting = null;
                m_control.Dash = null;

                m_control.SelectWeapon = null;
                m_control.ChangeWeapon = null;
                m_control.UseItem = null;
                m_control.Disconnect = null;
                StopShooting();
            }
        }

        public void Respawn(Vector2 pos) {
            transform.position = pos;

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
        }

        void SelectWeapon(int selectedWeapon) {
            if (selectedWeapon >= m_weapons.Count || selectedWeapon < 0)
                selectedWeapon = 0;
            m_weaponWheel.gameObject.SetActive(true);

            //_weaponWheel.GetChild(selectedWeapon) highlight selected item
        }

        void ChangeWeapon(int newWeapon) {
            if (newWeapon < m_weapons.Count && newWeapon >= 0) {
                m_weapons[m_currentWeapon].StopShooting();
                m_weapons[m_currentWeapon].SetActive(false);
                m_currentWeapon = newWeapon;
                m_weapons[m_currentWeapon].SetActive(true);
                if (m_isShooting)
                    m_weapons[m_currentWeapon].StartShooting();
                m_weaponWheel.gameObject.SetActive(false);
            }
        }

        void StartShooting() {
            if(!m_isShooting)
                m_weapons[m_currentWeapon].StartShooting();
            m_isShooting = true;
        }

        void StopShooting() {
            m_isShooting = false;
            m_weapons[m_currentWeapon].StopShooting();
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
            Destroy(this.gameObject);
            s_references.Remove(this);
        }

        #region Physics

        private void OnTriggerEnter2D(Collider2D collision) {
            IItem tmpItem = collision.GetComponent<IItem>();
            if (tmpItem != null) {
                m_items.Add(tmpItem);
                tmpItem.Collect();
            }
        }

        private void OnCollisionEnter2D(Collision2D collision) {
            if (m_dashColliders.value == (m_dashColliders | 1<<collision.gameObject.layer)) {//wir nehmen eine 1(true) und schieben es um collision.gameObject.layer nach links, nehmen dann die _dashColiders LayerMask, setzen dieses bool auf true und fragen dann ob dass was da rauskommt dass selbe ist wie die _dashColiders LayerMask
                Vector2 tmpNormal = new Vector2(0, 0);
                foreach (var it in collision.contacts) {
                    tmpNormal += it.normal;
                }
                m_collisionNormals[collision.collider] = tmpNormal.normalized;
            }
        }

        private void OnCollisionExit2D(Collision2D collision) {
            if (m_dashColliders.value == (m_dashColliders | 1 << collision.gameObject.layer)) {
                m_collisionNormals.Remove(collision.collider);
            }
        }

        #endregion
    }
}