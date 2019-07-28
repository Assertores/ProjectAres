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
        
        [SerializeField] TextMeshProUGUI r_countdownText;

        float m_endTime = float.MinValue;

        #endregion

        private void Awake() {
            if(s_singelton != null && s_singelton != this) {
                Destroy(this);
                return;
            }

            s_singelton = this;
        }

        private void Update() {
            float duration = m_endTime - Time.time;

            if(duration < 0) {
                r_countdownText.text = "";
            } else {
                r_countdownText.text = Mathf.CeilToInt(duration).ToString();
            }
        }

        private void OnDestroy() {
            if (s_singelton == this)
                s_singelton = null;
        }

        public static void StartTimer(float delay) {
            s_singelton.m_endTime = Time.time + delay;
        }

        public static void AbortTimer() {
            s_singelton.m_endTime = float.MinValue;
        }
    }
}
