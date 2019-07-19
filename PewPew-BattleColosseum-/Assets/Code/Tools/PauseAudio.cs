using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PPBC {
    [RequireComponent(typeof(AudioSource))]
    public class PauseAudio : MonoBehaviour {
        
        
        float m_startTimeScale;

        [SerializeField] AudioSource[] m_sources;
        float[] m_startPitch;

        private void Awake() {
            if(m_sources.Length == 0) {
                m_sources = new AudioSource[1];
                m_sources[0] = GetComponent<AudioSource>();
            }

            m_startPitch = new float[m_sources.Length];
            for (int i = 0; i < m_sources.Length; i++) {
                m_startPitch[i] = m_sources[i].pitch;
            }
            m_startTimeScale = Time.timeScale;
        }

        private void Update() {
            float value = (Time.timeScale / m_startTimeScale);
            for (int i = 0; i < m_sources.Length; i++) {
                m_sources[i].pitch = m_startPitch[i] * value;
            }
        }
    }
}