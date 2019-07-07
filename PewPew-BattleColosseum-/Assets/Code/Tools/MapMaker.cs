using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PPBC {
    public class MapMaker : MonoBehaviour {

#if UNITY_EDITOR

        [Header("References")]
        public GameObject p_globalObjects;
        GameObject r_globalObjects;
        
        [Header("Balancing")]
        [Tooltip("leave empty to overwrite map")]
        public string m_name = "";
        public d_mapData m_newObj;

        [HideInInspector] public bool m_editMap = false;

        private void Awake() {
            if (m_editMap && !DataHolder.s_isInit) {
                r_globalObjects = Instantiate(p_globalObjects);
                r_globalObjects.name = "Temporary Global Objects";
            }
        }

        private void OnDestroy() {
            if (r_globalObjects) {
                DestroyImmediate(r_globalObjects);
            }
        }
#endif
    }
}