using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProjectAres {
    public class Bullet : MonoBehaviour {

        [Header("Balancing")]
        [SerializeField] int _damage = 1;
        [SerializeField] float _killDistance = 1000;

        Player _source = null;

        public void Init(Player source, Vector2 velocity) {
            _source = source;
            Rigidbody2D rig = GetComponent<Rigidbody2D>();
            if (!rig) {
                Destroy(gameObject);
                return;
            }
            rig.velocity = velocity;
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
                        _source._stuts.DamageDealed += realDamage;
                        _source._stuts.Assists++;
                    }
                    if (kill) {
                        _source._stuts.Kills++;
                    }
                }
            }
            Destroy(gameObject);
        }
    }
}