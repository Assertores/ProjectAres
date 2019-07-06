using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PPBC {
    public class ObjRefHolder : MonoBehaviour {

        public static List<ObjRefHolder> s_references = new List<ObjRefHolder>();

        public d_mapData m_data;
        public Transform m_objectHolder;

        private void Awake() {
            s_references.Add(this);
        }

        private void OnDestroy() {
            s_references.Remove(this);
        }

        private void Start() {
            transform.localScale = new Vector3(1 / transform.lossyScale.x, 1 / transform.lossyScale.y, 1 / transform.lossyScale.z);
        }

        public d_mapData Update() {
            m_data.position = transform.position;
            m_data.rotation = transform.rotation.eulerAngles.z;
            m_data.scale = transform.localScale;

            return m_data;
        }
    }
}