using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using TMPro;

namespace PPBC {
    [RequireComponent(typeof(Collider2D))]
    public class ShootButton : MonoBehaviour, IDamageableObject {

        #region Variables

        [Header("References")]
        [SerializeField] UnityEvent m_onButtonDeath;
        [SerializeField] TextMeshProUGUI m_healthText;

        [Header("Balancing")]
        [SerializeField] float m_maxLife = 100;
        [Tooltip("delay in seconds")]
        [SerializeField] float m_regDelay = 2;
        [Tooltip("regenerated life points per second")]
        [SerializeField] float m_regSpeed = 2;

        float m_currentLife;
        float m_lastHitTime;

        #endregion
        #region MonoBehaviour

        void Start() {
            m_currentLife = m_maxLife;
        }

        // Update is called once per frame
        void Update() {
            if(m_currentLife < m_maxLife && m_lastHitTime + m_regDelay <= Time.time) {
                m_currentLife += m_regSpeed * Time.deltaTime;
                if (m_currentLife > m_maxLife)
                    m_currentLife = m_maxLife;
            }
            if (m_healthText) {
                m_healthText.text = Mathf.CeilToInt(m_currentLife).ToString();
            }
        }

        #endregion
        #region IDamageableObject

        public bool m_alive { get; set; }

        public void Die(Player source) {
            m_onButtonDeath?.Invoke();
            m_currentLife = m_maxLife;
        }

        public float GetHealth() {
            return m_currentLife;
        }

        public void TakeDamage(float damage, Player source, Vector2 force, Sprite icon) {
            if(damage < m_currentLife) {
                m_lastHitTime = Time.time;
                m_currentLife -= damage;
                return;
            }

            Die(source);
        }

        #endregion
    }
}