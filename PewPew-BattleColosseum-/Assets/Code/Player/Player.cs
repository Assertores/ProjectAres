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

        [Header("Balancing")]
        [SerializeField] float m_maxHealth = 100;
        float m_currentHealth;
        [SerializeField] float m_iFrameTime = 1;

        public int m_team = -1;
        public float m_distanceToGround = 0.5f;
        public float m_distanceToTop = 0.75f;

        bool m_invincible = false;
        Rigidbody2D m_rb = null;

        #endregion
        #region MonoBehaviour

        void Awake() {
            DontDestroyOnLoad(transform.root);
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

        #endregion
        #region IDamageableObject

        public bool m_alive { get; private set; }

        public void Die(IHarmingObject source, bool doTeamDamage = true) {
            if (!m_alive)
                return;
            if (!doTeamDamage && source.m_owner && source.m_owner.m_team == m_team)
                return;

            //--> can die && should die <--

            r_player.SetActive(false);
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

        }

        #endregion
        #region Resets

        public void ResetFull() {

        }

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
            ResetVelocity();
            ResetHealth();

            //StopShooting();
            r_player.SetActive(true);
            StartCoroutine(DoIFrame());
            yield return new WaitForSeconds(StartAnim(StringCollection.A_RESPAWN));
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

        IEnumerator DoIFrame() {
            IsInvincable(true);
            yield return new WaitForSeconds(m_iFrameTime);
            IsInvincable(false);
        }

        public void IsInvincable(bool value) {
            m_invincible = value;
        }

        float StartAnim(string animName, bool loop = true, int track = 0) {
            return float.MinValue;
        }
    }
}