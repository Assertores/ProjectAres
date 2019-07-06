using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PPBC {
    [RequireComponent(typeof(AudioSource))]
    public class GlobalAudioManager : MonoBehaviour {

        static GlobalAudioManager s_singelton = null;

        [SerializeField] AudioSource m_as1;
        [SerializeField] AudioSource m_as2;

        bool m_currentAS1 = true;

        private void Awake() {
            if(s_singelton != null && s_singelton != this) {
                Destroy(this);
                return;
            }

            s_singelton = this;
        }

        private void OnDestroy() {
            if(s_singelton == this) {
                s_singelton = null;
            }
        }

        private void Start() {
            if(m_as1 == null || m_as2 == null) {
                print("missing audio sorce");
                Destroy(this);
                return;
            }
        }

        public static void ChangeAudio(AudioClip newClip, float fadeTime = 0.5f) {
            if (s_singelton.m_currentAS1) {
                s_singelton.m_as2.clip = newClip;
                s_singelton.m_as2.volume = 0;
                s_singelton.m_as2.Play();
            } else {
                s_singelton.m_as1.clip = newClip;
                s_singelton.m_as1.volume = 0;
                s_singelton.m_as1.Play();
            }

            s_singelton.FadeAudio(fadeTime);
        }
        
        IEnumerator FadeAudio(float fadeTime) {
            float fadeStartTime = Time.time;
            float volume = m_currentAS1 ? m_as1.volume : m_as2.volume;
            
            while(fadeStartTime + fadeTime > Time.time) {
                if (m_currentAS1) {
                    m_as1.volume = Mathf.Lerp(volume, 0, (Time.time - fadeStartTime) / fadeTime);
                    m_as2.volume = Mathf.Lerp(0, volume, (Time.time - fadeStartTime) / fadeTime);
                } else {
                    m_as1.volume = Mathf.Lerp(0, volume, (Time.time - fadeStartTime) / fadeTime);
                    m_as2.volume = Mathf.Lerp(volume, 0, (Time.time - fadeStartTime) / fadeTime);
                }

                yield return null;
            }

            if (m_currentAS1) {
                m_as1.Stop();
                m_as2.volume = volume;
            } else {
                m_as1.volume = volume;
                m_as2.Stop();
            }
            
            m_currentAS1 = !m_currentAS1;
        }
    }
}