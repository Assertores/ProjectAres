using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProjectAres {
    public class Bullet : MonoBehaviour, IHarmingObject {

        #region Variables

        [Header("Balancing")]
        [SerializeField] float m_killDistance = 1000;
        [SerializeField] int m_damage = 1;

        Player m_source = null;

        Rigidbody2D m_rb;

        #endregion
        #region MonoBehaviour

        private void Update() {
            if (Vector2.Distance(transform.position, Vector2.zero) > m_killDistance) {
                Destroy(gameObject);
            }
        }

        #endregion
        #region IHarmingObject

        public Rigidbody2D Init(Player reverence) {
            m_rb = GetComponent<Rigidbody2D>();
            if (!m_rb) {
                Destroy(gameObject);
                return null;
            }

            m_source = reverence;
            return m_rb;
        }

        #endregion
        #region Physics

        private void OnCollisionEnter2D(Collision2D collision) {
            if(collision.gameObject == m_source.gameObject) {//null reference test
                return;
            }

            IDamageableObject tmp = collision.gameObject.GetComponent<IDamageableObject>();
            if (tmp != null) {
                tmp.TakeDamage(m_damage, m_source, m_rb.velocity * m_rb.mass);
            }
            Destroy(gameObject);
        }

        #endregion
    }
}
