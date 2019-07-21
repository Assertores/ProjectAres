using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PPBC {
    [RequireComponent(typeof(Collider2D))]
    public class LaserSpawner : MonoBehaviour {

        public static List<LaserSpawner> s_references = new List<LaserSpawner>();
        public int m_index;
        public GameObject fx_on;
        [SerializeField] ContactFilter2D m_filter;

        List<GameObject> m_ActiveList = new List<GameObject>();

        private void Awake() {
            s_references.Add(this);
        }

        private void Start() {
            Collider2D[] tmp = new Collider2D[10];
            int count = GetComponent<Collider2D>().OverlapCollider(m_filter, tmp);
            for (int i = 0; i < count; i++) {
                m_ActiveList.Add(tmp[i].gameObject);
            }
        }

        private void OnDestroy() {
            s_references.Remove(this);
        }

        public void Init(int index) {
            m_index = index;
            s_references.Sort((LaserSpawner lhs, LaserSpawner rhs) => lhs.m_index.CompareTo(rhs.m_index));
        }

        public void Reactivate() {
            foreach(var it in m_ActiveList) {
                it.SetActive(true);
            }
        }
    }
}