using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PPBC {
    public class Player : MonoBehaviour, IDamageableObject {

        public static List<Player> s_references = new List<Player>(8);
        public static List<Player> s_sortRef = new List<Player>(8);

        #region Variables

        [Header("References")]
        [SerializeField] GameObject r_player;
        [SerializeField] GameObject r_model;
        [SerializeField] GameObject r_static;

        [Header("Balancing")]
        [SerializeField] float m_maxHealth = 100;
        float m_currentHealth;
        [SerializeField] float m_iFrameTime = 1;

        [HideInInspector] public int m_team = -1;
        [HideInInspector] public float m_distanceToGround = 0.5f;
        [HideInInspector] public float m_distanceToTop = 0.75f;

        ModelRefHolder m_modelRef = null;
        Rigidbody2D m_rb = null;
        IControl m_controler = null;

        bool m_invincible = false;
        int m_levelColCount = 0;

        #endregion
        #region MonoBehaviour

        void Awake() {
            s_references.Add(this);
            s_sortRef.Add(this);
        }

        void OnDestroy() {
            s_references.Remove(this);
            s_sortRef.Remove(this);
        }

        void Start() {
            m_rb = r_player.GetComponent<Rigidbody2D>();
            if (!m_rb) {
                print("Player: no RigitBody");
                Destroy(this);
                return;
            }

            ResetFull();
        }

        void Update() {
            string currentAnim = GetCurrentAnim();
            if(m_levelColCount > 0) {//Air
                if(currentAnim == null || currentAnim == StringCollection.A_IDLE) {
                    StartAnim(StringCollection.A_IDLEAIR);
                }
            } else {//Gronded
                if (currentAnim == null || currentAnim == StringCollection.A_IDLEAIR) {
                    StartAnim(StringCollection.A_IDLE);
                }
            }
        }

        #endregion
        #region IDamageableObject

        public bool m_alive { get; private set; }

        public void Die(IHarmingObject source, bool doTeamDamage = true) {
            if (!m_alive)
                return;
            if (!doTeamDamage && source.m_owner && source.m_owner.m_team == m_team)
                return;

            //--> can die && should die <--

            StartCoroutine(IEDie(source));
        }

        IEnumerator IEDie(IHarmingObject source) {
            m_alive = false;

            yield return new WaitForSeconds(StartAnim(StringCollection.A_DIE));

            r_player.SetActive(false);

            //TODO: GameMode player died
        }

        public void TakeDamage(IHarmingObject source, float damage, Vector2 recoilDir, bool doTeamDamage = true) {
            if (!m_alive)
                return;
            if (source.m_owner == this)
                return;
            if (m_invincible)
                return;
            if (!doTeamDamage && source.m_owner && source.m_owner.m_team == m_team)
                return;

            if (damage >= m_currentHealth) {
                Die(source, doTeamDamage);
                return;
            }

            //--> damage is valid && won't die from it <--

            m_currentHealth -= damage;
            StartAnim(StringCollection.A_HIT, false);
        }

        #endregion

        public IControl Init(int index) {
            switch (index) {
            case 0:
            case 1:
            case 2:
            case 3:
                m_controler = r_static.AddComponent<ControlerControl>();
                break;
            case 4:
                m_controler = r_static.AddComponent<KeyboardControl>();
                break;
            default:
                break;
            }
            m_controler.index = index;

            return m_controler;
        }

        #region Resets

        public void ResetFull() {
            Respawn(transform.position);
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

            //StopShooting();
            r_player.SetActive(true);
            StartCoroutine(IEIFrame());
            yield return new WaitForSeconds(StartAnim(StringCollection.A_RESPAWN, false));
            //InControle(true);

        }

        public void ResetHealth() {
            m_currentHealth = m_maxHealth;
            m_alive = true;
        }

        public void ResetVelocity() {
            m_rb.velocity = Vector2.zero;
        }

        #endregion

        IEnumerator IEIFrame() {
            IsInvincable(true);
            yield return new WaitForSeconds(m_iFrameTime);
            IsInvincable(false);
        }

        public void IsInvincable(bool value) {
            m_invincible = value;
        }

        /// <summary>
        /// starts the animation if it isn't already running
        /// </summary>
        /// <param name="animName"></param>
        /// <param name="loop"></param>
        /// <param name="track"></param>
        /// <returns>the time of the animation in seconds (-1 if looping) (float.MinValue if animation is already running or Anim not found)</returns>
        float StartAnim(string animName, bool loop = true, int track = 0) {
            if(GetCurrentAnim() == animName) {
                return float.MinValue;
            }
            if (m_modelRef.r_modelAnim != null) {
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
            return m_modelRef.r_modelAnim.state.GetCurrent(0)?.Animation.Name;
        }

        #region Physics

        private void OnCollisionEnter2D(Collision2D collision) {
            if(collision.gameObject.tag == StringCollection.T_LEVEL) {
                m_levelColCount++;
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