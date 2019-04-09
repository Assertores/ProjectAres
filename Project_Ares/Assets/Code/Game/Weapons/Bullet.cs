using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProjectAres {
    public class Bullet : MonoBehaviour, IHarmingObject {

        [Header("Balancing")]
        [SerializeField] float m_killDistance = 1000;

        int m_damage = 1;
        Player m_source = null;

        public Rigidbody2D Init(Player source, int damage) {
            Rigidbody2D rb = GetComponent<Rigidbody2D>();
            if (!rb) {
                Destroy(gameObject);
                return null;
            }

            m_source = source;
            m_damage = damage;
            return rb;
        }

        private void Update() {
            if(Vector2.Distance(transform.position, Vector2.zero) > m_killDistance) {
                Destroy(gameObject);
            }
        }

        private void OnCollisionEnter2D(Collision2D collision) {
            IDamageableObject tmp = collision.gameObject.GetComponent<IDamageableObject>();
            if (tmp != null) {
                tmp.TakeDamage(m_damage, m_source);
            }
            Destroy(gameObject);
        }
    }
}