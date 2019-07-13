using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PPBC {
    public class SpawnPlayerAtStart : MonoBehaviour {

        private void Start() {
            foreach (var it in Player.s_references) {
                it.Respawn(SpawnPoint.s_references[0].transform.position);
            }
            Destroy(this);
        }
    }
}