using System.Collections;
using System.Collections.Generic;
using UnityEditor;
//#if UNITY_EDITOR
using UnityEngine;

namespace PPBC {
    public class MapMaker : MonoBehaviour {



        [Header("References")]
        public GameObject p_globalObjects;
        GameObject r_globalObjects;
        
        [Header("Balancing")]
        [Tooltip("leave empty to overwrite map")]
        public string m_name = "";
        public d_mapData m_newObj;
        
        public bool m_editMap = false;

        private void Awake() {
            if (m_editMap && !DataHolder.s_isInit) {
                r_globalObjects = Instantiate(p_globalObjects);
                r_globalObjects.name = "Temporary Global Objects";
                EditorApplication.playModeStateChanged += ModeChanged;
                DataHolder.s_currentModi = -1;
            }
        }

        private void OnDestroy() {
            if (r_globalObjects) {
                DestroyImmediate(r_globalObjects);
            }
        }

        void ModeChanged(PlayModeStateChange state) {
            
            if (state == PlayModeStateChange.EnteredEditMode) {
                Debug.Log("Exiting playmode.");
                m_editMap = false;
                Debug.Log(m_editMap);
            }
        }
    }
}
//#endif