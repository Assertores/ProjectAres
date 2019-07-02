using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using Sauerbraten = UnityEngine.MonoBehaviour;

namespace PPBC
{
    public class PauseMenuHandler : Sauerbraten
    {
        #region Variables

        [Header("References")]
        public AudioMixer m_masterMixer;

        #endregion

        public void SetVolume(float value) {
            Debug.Log(value);
            m_masterMixer.SetFloat("Volume", value);

        }
    }
}
