using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProjectAres {
    public class Rocket : MonoBehaviour, IHarmingObject {//TODO: fieleicht von Bullet erben lassen.

        #region Variables

        [Header("References")]
        [SerializeField] GameManager m_explosionRef;

        [Header("Balancing")]
        [SerializeField] float m_killDistance = 1000;
        [SerializeField] int m_damage = 1;

        Player m_source;

        #endregion
        #region MonoBehaviour
        void Start() {

        }

        void Update() {
            if (Vector2.Distance(transform.position, Vector2.zero) > m_killDistance) {
                Destroy(gameObject);
            }
        }

        #endregion
        #region IHarmingObject

        public Rigidbody2D Init(Player reverence) {
            Rigidbody2D rb = GetComponent<Rigidbody2D>();
            if (!rb) {
                Destroy(gameObject);
                return null;
            }

            m_source = reverence;
            return rb;
        }

        #endregion
        #region Physics

        private void OnCollisionEnter2D(Collision2D collision) {
            Instantiate(m_explosionRef);
            IDamageableObject tmp = collision.gameObject.GetComponent<IDamageableObject>();
            if (tmp != null) {
                tmp.TakeDamage(m_damage, m_source);
            }
            Destroy(gameObject);
        }

        #endregion
    }
}