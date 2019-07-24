using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PPBC {
    public class InitMenu : MonoBehaviour {

        void Start() {
            foreach (var it in Player.s_references) {
                it.ResetStatsFull();
                it.InControle(true);
                it.CanChangeCharacter(true);
                it.Invincable(true);
            }

            Destroy(this);
        }
    }
}