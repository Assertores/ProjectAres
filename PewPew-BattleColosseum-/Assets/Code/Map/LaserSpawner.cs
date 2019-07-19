using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PPBC {
    public class LaserSpawner : MonoBehaviour {

        public static List<LaserSpawner> s_references = new List<LaserSpawner>();
        public int m_index;
        public GameObject fx_on;

        private void Awake() {
            s_references.Add(this);
        }

        private void OnDestroy() {
            s_references.Remove(this);
        }

        public void Init(int index) {
            m_index = index;
            s_references.Sort((LaserSpawner lhs, LaserSpawner rhs) => lhs.m_index.CompareTo(rhs.m_index));
        }
    }
}