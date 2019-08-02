using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PPBC { 

    public class ParticleAnimEvent : MonoBehaviour
    {
        [SerializeField] AudioSource SFX_Curtainimpact;

        public void DoImpactSound() {
            SFX_Curtainimpact.Play();
            CameraShake.DoCamerashake(2, 0.5f);
        }
    }
}
