using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProjectAres {
    public class Bullet : MonoBehaviour {

        [Header("Balancing")]
        [SerializeField] int _damage = 1;
        [SerializeField] float _killDistance = 1000;

        Player _source = null;

        public Rigidbody2D Init(Player source, Vector2 velocity) {
            _source = source;
            Rigidbody2D rb = GetComponent<Rigidbody2D>();
            if (!rb) {
                Destroy(gameObject);
                return null;
            }
            rb.velocity = velocity;
            return rb;
        }

        private void Update() {
            if(Vector2.Distance(transform.position, Vector2.zero) > _killDistance) {
                Destroy(gameObject);
            }
        }

        private void OnCollisionEnter2D(Collision2D collision) {
            IDamageableObject tmp = collision.gameObject.GetComponent<IDamageableObject>();
            if (tmp != null) {                              
                int realDamage;
                bool kill = tmp.TakeDamage(_damage, out realDamage);
                if (_source) {//wass ist mit an sich selbst schaden machen (oder an teamkolegen)
                    if(realDamage > 0) {
                        _source.m_stats.m_damageDealt += realDamage;
                        _source.m_stats.m_assists++;
                    }
                    if (kill) {
                        _source.m_stats.m_kills++;
                    }
                }
            }
            Destroy(gameObject);
        }
    }
}