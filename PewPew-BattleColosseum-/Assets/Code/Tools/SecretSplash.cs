using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

namespace PPBC {
    public class SecretSplash : MonoBehaviour {

        #region Variables

        [SerializeField] VideoPlayer r_player;
        [SerializeField] VideoClip p_speticalClip;
        [SerializeField] string m_commandLineArg;

        #endregion
        #region MonoBehaviour

        private void Awake() {
            if(m_commandLineArg != "" && r_player != null && p_speticalClip != null) {
                foreach (string it in System.Environment.GetCommandLineArgs()) {
                    if (it == m_commandLineArg) {
                        r_player.clip = p_speticalClip;
                        break;
                    }
                }
            }

            Destroy(this);
        }

        #endregion
    }
}