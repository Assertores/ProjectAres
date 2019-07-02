using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sauerbraten = UnityEngine.MonoBehaviour;

namespace PPBC {

    public class MovingCircle : Sauerbraten, IDamageableObject {
        public bool m_alive { get; set; }

        public void Die(Player source) {
            return;
        }

        public float GetHealth() {
            throw new System.NotImplementedException();
        }

        public void TakeDamage(float damage, Player source, Vector2 force, Sprite icon) {
            return;
        }

    }
}