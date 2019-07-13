using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

namespace PPBC {
    public class ChangeVolume : MonoBehaviour {

        [SerializeField] AudioMixer m_mixer;

        public void ChangeMasterVolume(float value) {
            m_mixer.SetFloat(StringCollection.G_MASTER, Mathf.Lerp(-80, 20, value / 10));
        }

        public void ChangeMusicVolume(float value) {
            m_mixer.SetFloat(StringCollection.G_MUSIC, Mathf.Lerp(-80, 20, value / 10));
        }

        public void ChangeEffectsVolume(float value) {
            m_mixer.SetFloat(StringCollection.G_EFFECTS, Mathf.Lerp(-80, 20, value / 10));
        }
    }
}