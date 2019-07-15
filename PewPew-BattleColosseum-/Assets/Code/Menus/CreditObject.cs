using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spine.Unity;

namespace PPBC {
    [RequireComponent(typeof(Rigidbody2D))]
    public class CreditObject : MonoBehaviour, IDamageableObject {

        Rigidbody2D m_rb;
        Vector2 m_inVel;

        #region MonoBehaviour

        private void Awake() {
            m_rb = GetComponent<Rigidbody2D>();
        }

        private void FixedUpdate() {
            m_inVel = m_rb.velocity;
        }

        #endregion
        #region IDamageableObject

        public bool m_alive => true;

        public void Die(IHarmingObject source, bool doTeamDamage = true) {
            return;
        }

        public void TakeDamage(IHarmingObject source, float damage, Vector2 recoilDir, bool doTeamDamage = true) {
            m_rb.AddForce(recoilDir);
        }

        #endregion

        private void OnCollisionEnter2D(Collision2D collision) {
            Vector2 tmp = collision.contacts[0].normal;
            if (Vector2.Dot(m_inVel.normalized, tmp) < 0) {
                m_rb.velocity = /*m_bounciness * */(Vector2.Reflect(m_inVel, tmp));
            }
        }
    }
}