using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace PPBC {
    public class Exiter : MonoBehaviour {

        public void DisconnectPlayer() {
            if (Player.s_references.Count <= 1) {
                Application.Quit();
#if UNITY_EDITOR
                if (EditorApplication.isPlaying) {
                    EditorApplication.ExecuteMenuItem("Edit/Play");
                }
#endif
                return;
            }

            TriggerButton.s_hoPlayer?.m_controler.DoDisconnect();
        }
    }
}