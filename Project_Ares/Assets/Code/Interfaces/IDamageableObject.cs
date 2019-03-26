using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProjectAres {
    public interface IDamageableObject {

        bool _alive { get; set; }

        bool TakeDamage(int damage, out int realDamage, bool ignoreInvulnerable = false);
        int GetHealth();
    }
}