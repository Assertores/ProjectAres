using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProjectAres {
    [RequireComponent(typeof(AudioSource))]
    public class PauseAudioHandler : MonoBehaviour {

        static List<PauseAudioHandler> s_references = new List<PauseAudioHandler>();

        #region Variables

        AudioSource m_source;
        float m_startPitch;
        float m_startTimeScale;

        #endregion
        #region MonoBehaviour

        void Start() {
            m_source = GetComponent<AudioSource>();
            m_startPitch = m_source.pitch;
            m_startTimeScale = Time.timeScale;
            s_references.Add(this);
        }

        private void OnDestroy() {
            s_references.Remove(this);
        }

        #endregion

        public static void UpdateAudio() {
            foreach(var it in s_references)
                it.m_source.pitch = it.m_startPitch * (Time.timeScale / it.m_startTimeScale);
        }
    }
}