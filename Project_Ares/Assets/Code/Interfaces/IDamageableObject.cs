using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProjectAres {
    public interface IDamageableObject {

        bool m_alive { get; set; }

        void TakeDamage(int damage, Player source);
        void Die(Player source);
        int GetHealth();
    }
}