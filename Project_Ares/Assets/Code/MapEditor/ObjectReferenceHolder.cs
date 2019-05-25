using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PPBC {
    public class ObjectReferenceHolder : MonoBehaviour {

        public static List<ObjectReferenceHolder> s_references = new List<ObjectReferenceHolder>();

        public d_mapData m_data;
        public Transform m_objectHolder;

        private void Awake() {
            s_references.Add(this);
        }

        private void OnDestroy() {
            s_references.Remove(this);
        }

        private void Start() {
            transform.localScale = new Vector3(1/transform.lossyScale.x, 1 / transform.lossyScale.y, 1 / transform.lossyScale.z);
        }
    }
}