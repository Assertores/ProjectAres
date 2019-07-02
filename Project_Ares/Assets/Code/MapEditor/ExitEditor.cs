using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Sauerbraten = UnityEngine.MonoBehaviour;

namespace PPBC {
    public class ExitEditor : Sauerbraten, IDamageableObject {

        [Header("References")]
        [SerializeField] Coop_Edit m_gMRef;
        [SerializeField] TextMeshProUGUI m_liveRef;

        [Header("Balancing")]
        [SerializeField] float m_maxLife;
        [SerializeField] float m_RegDelay;
        [SerializeField] float m_RegSpeed;

        float m_currentLife;
        float m_lastHit;

        private void Start() {
            transform.localScale = new Vector3(1 / transform.lossyScale.x, 1 / transform.lossyScale.y, 1 / transform.lossyScale.z);
            m_currentLife = m_maxLife;
        }

        private void Update() {
            if(m_currentLife < m_maxLife && Time.timeSinceLevelLoad > m_lastHit + m_RegDelay) {
                m_currentLife += m_RegSpeed * Time.deltaTime;
                if(m_currentLife > m_maxLife) {
                    m_currentLife = m_maxLife;
                }
            }

            m_liveRef.text = Mathf.RoundToInt(m_currentLife).ToString();
        }

        public bool m_alive { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }

        public void Die(Player source) {
            return;
        }

        public float GetHealth() {
            return m_currentLife;
        }

        public void TakeDamage(float damage, Player source, Vector2 force, Sprite icon) {
            if (m_currentLife <= 0)
                return;

            m_currentLife -= damage;
            m_lastHit = Time.timeSinceLevelLoad;
            if(m_currentLife <= 0) {
                print("1");
                m_gMRef.SaveAndExit();
            }
        }
    }
}