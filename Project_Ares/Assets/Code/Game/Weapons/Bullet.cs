using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProjectAres {
    public class Bullet : MonoBehaviour, IHarmingObject {

        [Header("Balancing")]
        [SerializeField] float _killDistance = 1000;

        int _damage = 1;
        Player _source = null;

        public Rigidbody2D Init(Player source, int damage) {
            Rigidbody2D rb = GetComponent<Rigidbody2D>();
            if (!rb) {
                Destroy(gameObject);
                return null;
            }

            _source = source;
            _damage = damage;
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
                tmp.TakeDamage(_damage, _source);
            }
            Destroy(gameObject);
        }
    }
}