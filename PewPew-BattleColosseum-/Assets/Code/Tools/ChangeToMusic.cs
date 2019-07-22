using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PPBC {
    public class ChangeToMusic : MonoBehaviour {

        [SerializeField] AudioClip m_clip;

        void Start() {
            if(m_clip)
                GlobalAudioManager.ChangeAudio(m_clip);

            Destroy(this);
        }
    }
}