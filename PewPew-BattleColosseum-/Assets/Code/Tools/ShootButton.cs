using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using TMPro;

namespace PPBC {
    public class ShootButton : MonoBehaviour, IDamageableObject {

        #region Variables

        [Header("Reference")]
        [SerializeField] UnityEvent r_onButtonDeath;
        [SerializeField] TextMeshProUGUI r_healthText;

        [Header("Balancing")]
        [SerializeField] float m_maxLife = -1;
        float m_currentLife;
        [Tooltip("delay in seconds")]
        [SerializeField] float m_regDelay = 2;
        [Tooltip("regenerated life points per second")]
        [SerializeField] float m_regSpeed = 2;

        float m_lastHitTime;

        #endregion
        #region MonoBehaviour

        private void Start() {
            m_currentLife = m_maxLife;
        }

        private void Update() {
            if(m_currentLife < m_maxLife && Time.time > m_lastHitTime + m_regDelay) {
                m_currentLife += m_regSpeed * Time.deltaTime;

                if(m_currentLife > m_maxLife) {
                    m_currentLife = m_maxLife;
                }
            }

            if (r_healthText) {
                r_healthText.text = Mathf.CeilToInt(m_currentLife).ToString();
            }
        }

        #endregion
        #region IDamageableObject

        public bool m_alive => true;

        public void Die(ITracer source, bool doTeamDamage = true) {
            if (m_maxLife <= 0 && !(source.m_trace.m_type == e_HarmingObjectType.ROCKED && ((MonoBehaviour)source).tag == StringCollection.T_PROJECTILES))
                return;

            r_onButtonDeath?.Invoke();

            m_currentLife = m_maxLife;
        }

        public void TakeDamage(ITracer source, float damage, Vector2 recoilDir, bool doTeamDamage = true) {
            m_lastHitTime = Time.time;
            m_currentLife -= damage;

            if (m_currentLife <= 0)
                Die(source);
        }

        #endregion
    }
}