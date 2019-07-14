﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Rendering.PostProcessing;

namespace PPBC {
    public class ChangeGraphics : MonoBehaviour {

        [Header("References")]
        [SerializeField] Slider s_fullScreen;
        [SerializeField] TextMeshProUGUI r_fullScreen;
        [SerializeField] Slider s_quality;
        [SerializeField] TextMeshProUGUI r_quality;
        [SerializeField] Slider s_brightness;
        [SerializeField] PostProcessProfile p_profile;
        ColorGrading m_gamma;
        [SerializeField] Slider s_res;
        [SerializeField] TextMeshProUGUI r_res;
        [SerializeField] Slider s_vSync;
        [SerializeField] TextMeshProUGUI r_vSync;

        [Header("Balancing")]
        [SerializeField] string[] m_TF;
        [SerializeField] string[] m_qText;

        private void Awake() {
            p_profile.TryGetSettings(out m_gamma);

            s_fullScreen.value = Screen.fullScreen ? 0 : 1;
            Fullscreen(s_fullScreen.value);
            s_quality.value = QualitySettings.GetQualityLevel();
            Quality(s_quality.value);
            s_brightness.value = Mathf.RoundToInt((m_gamma.gamma.value.w + 1) * 5);

            s_res.maxValue = Screen.resolutions.Length-1;
            int i = 0;
            for (; i < Screen.resolutions.Length && (Screen.resolutions[i].width != Screen.currentResolution.width || Screen.resolutions[i].height != Screen.currentResolution.height); i++)
                ;
            s_res.value = i;
            r_res.text = Screen.currentResolution.width + " x " + Screen.currentResolution.height;

            s_vSync.value = QualitySettings.vSyncCount;
            VSync(s_vSync.value);
        }

        public void Fullscreen(float value) {// 0 or 1
            if(value > 0) {
                //no
                Screen.fullScreen = false;
                r_fullScreen.text = m_TF[1];
            } else {
                //yes
                Screen.fullScreen = true;
                r_fullScreen.text = m_TF[0];
            }
        }

        public void ChangeResolution(float value) {
            Resolution tmp = Screen.resolutions[Mathf.RoundToInt(value)];
            Screen.SetResolution(tmp.width, tmp.height, Screen.fullScreen);
            r_res.text = tmp.width + " x " + tmp.height;
        }

        public void Quality(float value) {// 0, 1 or 2
            if(value < 1) {
                //low
                r_quality.text = m_qText[0];
            } else if(value < 2) {
                //medium
                r_quality.text = m_qText[1];
            } else {
                //high
                r_quality.text = m_qText[2];
            }
            QualitySettings.SetQualityLevel((int)value);
        }

        public void Brightness(float value) {// from 0 to 10
            m_gamma.gamma.value.w = value / 5 - 1;
        }

        public void VSync(float value) {
            if (value > 0) {
                //yes
                r_vSync.text = m_TF[0];
            } else {
                //no
                r_vSync.text = m_TF[1];
            }
            QualitySettings.vSyncCount = Mathf.RoundToInt(value);
        }
    }
}