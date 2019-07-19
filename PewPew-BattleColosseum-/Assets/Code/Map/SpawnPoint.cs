using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PPBC {
    public class SpawnPoint : MonoBehaviour {

        public static List<SpawnPoint> s_references = new List<SpawnPoint>();

        public int m_team;

        private void Awake() {
            s_references.Add(this);
        }

        private void OnDestroy() {
            s_references.Remove(this);
        }
    }
}