using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace PPBC {

    public struct d_playerStuts {
        public float m_points;
        public int m_matchPoints;

        public int m_kills;
        public int m_deaths;

        public float m_damageDealt;
        public float m_damageTaken;
    }

    [RequireComponent(typeof(Rigidbody2D))]
    [RequireComponent(typeof(Collider2D))]
    public class Player : MonoBehaviour, IDamageableObject {

        public static List<Player> s_references = new List<Player>(8);
        public static List<Player> s_sortRef = new List<Player>(8);

        #region Variables

        [Header("References")]
        [SerializeField] GameObject r_player;
        [SerializeField] GameObject r_model;
        [SerializeField] GameObject r_static;
        [SerializeField] GameObject r_vfx;
        [SerializeField] SMG r_smg;
        [SerializeField] RocketLauncher r_rocket;
        [SerializeField] GameObject r_playerClashParent;
        [SerializeField] ParticleSystem FX_playerClash;
        [SerializeField] Image r_healthBar;
        [SerializeField] Image r_staminaBar;
        [SerializeField] TextMeshProUGUI r_points;

        [Header("Balancing")]
        [SerializeField] float m_maxHealth = 100;
        float m_currentHealth;
        [SerializeField] float m_iFrameTime = 1;
        [SerializeField] float m_bounciness = 0.75f;

        [HideInInspector] public d_playerStuts m_stats;

        int m_playerIndex = -1;
        [HideInInspector] public int m_team = -1;
        [HideInInspector] public PillarRefHolder r_pillar;
        public float m_distanceToGround { get; private set; } = 0.5f;//TODO: auto create
        public float m_distanceToTop { get; private set; } = 0.75f;//TODO: auto create

        public ModelRefHolder m_modelRef { get; private set; }
        public Rigidbody2D m_rb { get; private set; }
        Collider2D m_col;
        public IControl m_controler { get; private set; }

        bool m_invincible = false;
        int m_levelColCount = 0;//collider count with level layer
        Vector2 m_inVel; //velocity befor collision

        int m_currentCaracter = 0;
        bool m_useSMG = true;

        #endregion
        #region MonoBehaviour

        void Awake() {
            s_references.Add(this);
            s_sortRef.Add(this);
        }

        void OnDestroy() {
            s_references.Remove(this);
            s_sortRef.Remove(this);

            if (r_pillar) {
                Destroy(r_pillar.gameObject);
            }
        }

        void Start() {
            m_rb = GetComponent<Rigidbody2D>();
            m_col = GetComponent<Collider2D>();

            ResetFull();
        }

        void Update() {
            r_healthBar.fillAmount = m_currentHealth / m_maxHealth;

            r_staminaBar.fillAmount = m_useSMG ? r_smg.GetStamina() : r_rocket.GetStamina();

            r_points.text = Mathf.RoundToInt(m_stats.m_points).ToString();

            RotateWeapon();
        }

        private void FixedUpdate() {
            string currentAnim = GetCurrentAnim();
            if (m_levelColCount > 0) {//Grounded
                if (currentAnim == null || currentAnim == StringCollection.A_IDLEAIR) {
                    StartAnim(StringCollection.A_IDLE, true);
                }
            } else {//Air
                if (currentAnim == null || currentAnim == StringCollection.A_IDLE) {
                    StartAnim(StringCollection.A_IDLEAIR, true);
                }
            }

            m_inVel = m_rb.velocity;
        }

        #endregion
        #region IDamageableObject

        public bool m_alive { get; private set; }

        public void Die(ITracer source, bool doTeamDamage = true) {
            if (!m_alive)
                return;
            if (!doTeamDamage && source.m_trace.m_owner && source.m_trace.m_owner.m_team == m_team)
                return;

            //--> can die && should die <--

            m_stats.m_deaths++;
            if(source != null && source.m_trace.m_owner != null)
                source.m_trace.m_owner.m_stats.m_kills++;

            StartCoroutine(IEDie(source.m_trace));
        }

        IEnumerator IEDie(IHarmingObject source) {
            m_alive = false;

            yield return new WaitForSeconds(StartAnim(StringCollection.A_DIE));

            SetPlayerActive(false);
            r_player.SetActive(false);

            DataHolder.s_modis[DataHolder.s_currentModi].PlayerDied(source, this);
        }

        public void TakeDamage(ITracer source, float damage, Vector2 recoilDir, bool doTeamDamage = true) {
            if (!m_alive)
                return;
            
            m_rb.AddForce(recoilDir, ForceMode2D.Impulse);

            if (source.m_trace.m_owner == this)
                return;
            if (m_invincible)
                return;
            if (!doTeamDamage && source.m_trace.m_owner && source.m_trace.m_owner.m_team == m_team)
                return;

            if (damage >= m_currentHealth) {
                m_stats.m_damageTaken += m_currentHealth;
                if (source != null && source.m_trace.m_owner != null)
                    source.m_trace.m_owner.m_stats.m_damageDealt += m_currentHealth;

                Die(source, doTeamDamage);
                return;
            }

            //--> damage is valid && won't die from it <--

            m_stats.m_damageTaken += damage;
            if (source != null && source.m_trace.m_owner != null)
                source.m_trace.m_owner.m_stats.m_damageDealt += damage;

            m_currentHealth -= damage;
            StartAnim(StringCollection.A_HIT);
        }

        #endregion

        public IControl Init(int index) {
            if (DataHolder.s_players[index])
                return null;

            m_controler = r_static.AddComponent<ControlerControl>();
            m_controler.m_index = index;
            m_playerIndex = index;
            DataHolder.s_players[index] = true;

            m_rb = GetComponent<Rigidbody2D>();
            
            r_smg.Init(this);
            r_rocket.Init(this);

            m_currentCaracter = -1;
            ChangeChar(true);

            m_useSMG = false;
            ChangeWeapon();

            InControle(true);

            return m_controler;
        }

        #region Resets

        public void ResetFull() {
            Respawn(transform.position);
            ResetStatsFull();
        }

        public void Respawn(Vector2 pos, float delay = 0) {
            StartCoroutine(IERespawn(pos, delay));
        }

        IEnumerator IERespawn(Vector2 pos, float delay = 0) {
            float startTime = Time.time;
            Vector2 starPos = transform.position;

            while (startTime + delay > Time.time) {
                //----- stuff that should happon in between -----
                transform.position = Vector2.Lerp(starPos, pos, (Time.time - startTime) / delay);
                yield return null;
            }

            //----- stuff that should happon after -----
            transform.position = pos;
            ResetVelocity();
            ResetHealth();

            StopShooting();
            SetPlayerActive(true);
            r_player.SetActive(true);
            StartCoroutine(IEIFrame());
            yield return new WaitForSeconds(StartAnim(StringCollection.A_RESPAWN));
            InControle(true);

        }

        public void ResetHealth() {
            m_currentHealth = m_maxHealth;
            m_alive = true;
        }

        public void ResetVelocity() {
            if(!m_rb)
                m_rb = GetComponent<Rigidbody2D>();

            m_rb.velocity = Vector2.zero;
        }

        public void ResetStatsFull() {
            ResetGameStats();
            ResetMatchStats();
        }

        public void ResetGameStats() {
            m_stats.m_points = 0;
            m_stats.m_kills = 0;
            m_stats.m_deaths = 0;
            m_stats.m_damageDealt = 0;
            m_stats.m_damageTaken = 0;
        }

        public void ResetMatchStats() {
            m_stats.m_matchPoints = 0;
        }

        public void ResetTeam() {
            m_team = -1;
        }

        bool h_CanChangeCharacter = false;
        public void CanChangeCharacter(bool character) {
            if (h_CanChangeCharacter == character)
                return;

            if (character) {
                m_controler.ChangeCharacter += ChangeChar;
            } else {
                m_controler.ChangeCharacter -= ChangeChar;
            }

            h_CanChangeCharacter = character;
        }

        bool h_inControle = false;
        public void InControle(bool controle) {
            if (h_inControle == controle)
                return;

            if (controle) {
                m_controler.ChangeWeapon += ChangeWeapon;
                m_controler.TriggerDown += StartShooting;
                m_controler.TriggerUp += StopShooting;
            } else {
                m_controler.ChangeWeapon -= ChangeWeapon;
                m_controler.TriggerDown -= StartShooting;
                m_controler.TriggerUp -= StopShooting;
            }

            h_inControle = controle;
        }

        #endregion

        IEnumerator IEIFrame() {
            Invincable(true);
            yield return new WaitForSeconds(m_iFrameTime);//TODO: IFrame effect
            Invincable(false);
        }

        public void Invincable(bool value) {
            m_invincible = value;
        }

        #region Control stuff

        void ChangeChar(bool next) {
            if (next)
                m_currentCaracter++;
            else
                m_currentCaracter--;
            m_currentCaracter = DataHolder.FixedMod(m_currentCaracter, DataHolder.s_characters.Length);

            foreach(Transform it in r_model.transform) {
                Destroy(it.gameObject);
            }
            GameObject model = Instantiate(DataHolder.s_characters[m_currentCaracter], r_model.transform);
            m_modelRef = model.GetComponent<ModelRefHolder>();
            RotateWeapon();
            //TODO: schön machen
            ChangeWeapon();
            ChangeWeapon();
        }

        void ChangeWeapon() {
            m_useSMG = !m_useSMG;
            r_smg.ChangeWeapon(m_useSMG);
            r_rocket.ChangeWeapon(!m_useSMG);
        }

        void StartShooting() {
            if (m_useSMG) {
                r_smg.StartShooting();
            } else {
                r_rocket.StartShooting();
            }
        }

        void StopShooting() {
            if (m_useSMG) {
                r_smg.StopShooting();
            } else {
                r_rocket.StopShooting();
            }
        }

        #endregion

        void RotateWeapon() {
            //creating up vector from direction vector (vector at right angle)
            m_modelRef.r_weaponRot.transform.rotation = Quaternion.LookRotation(transform.forward, new Vector2(-m_controler.m_dir.y, m_controler.m_dir.x));

            //flipping weapon
            if (m_controler.m_dir.x < 0) {
                m_modelRef.r_weaponRot.transform.localScale = new Vector3(1, -1, 1);
            } else {
                m_modelRef.r_weaponRot.transform.localScale = new Vector3(1, 1, 1);
            }
        }

        public void SetPlayerActive(bool value) {
            if (!m_rb)
                m_rb = GetComponent<Rigidbody2D>();
            m_rb.isKinematic = !value;
            if (!m_col)
                m_col = GetComponent<Collider2D>();
            m_col.enabled = value;
        }

        /// <summary>
        /// starts the animation if it isn't already running
        /// </summary>
        /// <param name="animName"></param>
        /// <param name="loop"></param>
        /// <param name="track"></param>
        /// <returns>the time of the animation in seconds (-1 if looping) (float.MinValue if animation is already running or Anim not found)</returns>
        public float StartAnim(string animName, bool loop = false, int track = 0) {
            if(GetCurrentAnim() == animName) {
                return float.MinValue;
            }
            if (m_modelRef != null && m_modelRef.r_modelAnim != null) {
                float time = m_modelRef.r_modelAnim.AnimationState.SetAnimation(track, animName, loop).Animation.Duration;
                return loop ? -1 : time;
            }
            return float.MinValue;
        }

        /// <summary>
        /// use this to check whitch animation is running;
        /// </summary>
        /// <returns>string of current animation or null if no animation is running</returns>
        string GetCurrentAnim() {
            if (!m_modelRef || !m_modelRef.r_modelAnim || m_modelRef.r_modelAnim.state.GetCurrent(0) == null)
                return null;

            return m_modelRef.r_modelAnim.state.GetCurrent(0).IsComplete ? null : m_modelRef?.r_modelAnim?.state.GetCurrent(0)?.Animation.Name;
        }

        public Color GetPlayerColor() {
            if(m_team >= 0) {
                return DataHolder.s_teamColors[m_team];
            } else {
                return DataHolder.s_playerColors[m_playerIndex];
            }
        }


        #region Editor code

        EditorHUDAndPlayerLogic m_editHud;
        public void GoIntoEditMode() {

            m_editHud.gameObject.SetActive(!m_editHud.gameObject.activeSelf);

            if (m_editHud.gameObject.activeSelf) {
                InControle(false);
                Invincable(true);
                m_rb.simulated = false;

                m_controler.TriggerDown += m_editHud.DragHandler;
                m_controler.ChangeType += m_editHud.ChangeType;
                m_controler.ChangeCharacter += m_editHud.ChangeIndex;
            } else {
                m_controler.TriggerDown -= m_editHud.DragHandler;
                m_controler.ChangeType -= m_editHud.ChangeType;
                m_controler.ChangeCharacter -= m_editHud.ChangeIndex;

                StopEdit();
            }
        }

        public void StopEdit() {

            InControle(true);
            Invincable(false);
            m_rb.simulated = true;
        }

        public void EditAble(EditorHUDAndPlayerLogic hud) {
            if (hud != null) {
                m_editHud = hud;
                m_editHud.SetControlRef(m_controler);
                m_editHud.gameObject.SetActive(false);

                m_controler.Accept += GoIntoEditMode;
            } else {
                m_controler.Accept -= GoIntoEditMode;

                StopEdit();
                if (m_editHud)
                    Destroy(m_editHud.gameObject);
            }
        }

        #endregion
        #region Physics

        private void OnCollisionEnter2D(Collision2D collision) {
            bool doEffect = false;
            if(collision.gameObject.tag == StringCollection.T_LEVEL) {
                m_levelColCount++;
                doEffect = true;
            }
            if(collision.gameObject.tag == StringCollection.T_PLAYER) {
                r_playerClashParent.transform.rotation = Quaternion.LookRotation(transform.forward, collision.contacts[0].normal);
                r_playerClashParent.transform.position = collision.contacts[0].collider.transform.position;
                FX_playerClash.Play();
                doEffect = true;
            }
            if (doEffect) {
                Vector2 tmp = collision.contacts[0].normal;
                if (Vector2.Dot(m_inVel.normalized, tmp) < 0) {
                    m_rb.velocity = m_bounciness * (Vector2.Reflect(m_inVel, tmp));
                }
                StartAnim(StringCollection.A_IMPACT);
            }
        }

        private void OnCollisionExit2D(Collision2D collision) {
            if (collision.gameObject.tag == StringCollection.T_LEVEL) {
                m_levelColCount--;
            }
        }

        #endregion
    }
}