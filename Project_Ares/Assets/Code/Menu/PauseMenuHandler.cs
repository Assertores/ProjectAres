using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

namespace PPBC
{
    public class PauseMenuHandler : MonoBehaviour
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
