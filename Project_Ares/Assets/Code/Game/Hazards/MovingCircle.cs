using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProjectAres {

    public class MovingCircle : MonoBehaviour, IDamageableObject {
        public bool m_alive { get; set; }

        public void Die(Player source) {
            return;
        }

        public float GetHealth() {
            throw new System.NotImplementedException();
        }

        public void TakeDamage(float damage, Player source, Vector2 force) {
            return;
        }

    }
}