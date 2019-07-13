using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Rendering.PostProcessing;

namespace PPBC {
    public class ChangeGraphics : MonoBehaviour {

        [SerializeField] TextMeshProUGUI m_fullScreen;
        [SerializeField] string[] m_TF;
        [SerializeField] TextMeshProUGUI m_quality;
        [SerializeField] string[] m_qText;
        [SerializeField] PostProcessProfile m_profile;
        ColorGrading m_gamma;

        private void Awake() {
            m_profile.TryGetSettings(out m_gamma);
        }

        public void Fullscreen(float value) {// 0 or 1
            if(value > 0) {
                //no
                Screen.fullScreen = false;
                m_fullScreen.text = m_TF[1];
            } else {
                //yes
                Screen.fullScreen = true;
                m_fullScreen.text = m_TF[0];
            }
        }

        public void Quality(float value) {// 0, 1 or 2
            if(value < 1) {
                //low
                m_quality.text = m_qText[0];
            } else if(value < 2) {
                //medium
                m_quality.text = m_qText[1];
            } else {
                //high
                m_quality.text = m_qText[2];
            }
            QualitySettings.SetQualityLevel((int)value);
        }

        public void Brightness(float value) {// from 0 to 10
            m_gamma.gamma.value.w = value / 5 - 1;
        }
    }
}