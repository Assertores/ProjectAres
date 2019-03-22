﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProjectAres {
    public class StartGame : MonoBehaviour, IDamageableObject {

        public bool TakeDamage(int damage, out int realDamage, bool ignoreInvulnerable = false) {
            realDamage = 0;
            MenuManager._singelton?.StartGame();
            return false;
        }
    }
}