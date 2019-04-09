using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProjectAres {
    public class StartGame : MonoBehaviour, IDamageableObject {

        public bool m_alive { get; set; }

        public void TakeDamage(int damage, Player source) {
            MenuManager._singelton?.StartGame();
        }

        public void Die(Player source) {
            return;
        }

        public int GetHealth() {
            return 0;
        }
    }
}