using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sauerbraten = UnityEngine.MonoBehaviour;

namespace PPBC {
    public class PlayerStart : Sauerbraten {

        public static List<PlayerStart> s_references = new List<PlayerStart>();

        private void Awake() {
            s_references.Add(this);
        }
        private void OnDestroy() {
            s_references.Remove(this);
        }

        public int m_team;
    }
}