using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace PPBC
{
    public class Timer : MonoBehaviour
    {

        static Timer s_singelton;

        #region Variables

        [SerializeField] Canvas r_timerCanvas;
        [SerializeField] TextMeshProUGUI r_countdownText;

        #endregion

        private void Awake() {
            if(s_singelton != null && s_singelton != this) {
                Destroy(this);
                return;
            }

            s_singelton = this;
        }

        private void OnDestroy() {
            if (s_singelton == this)
                s_singelton = null;
        }

        public static void StartTimer(float delay) {

        }
        public static void AbortTimer() {
            s_singelton.r_timerCanvas.gameObject.SetActive(false);
        }
    }
}
