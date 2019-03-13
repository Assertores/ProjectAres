using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProjectAres {
    public interface IDamageableObject {

        bool TakeDamage(int damage, out int realDamage);
    }
}