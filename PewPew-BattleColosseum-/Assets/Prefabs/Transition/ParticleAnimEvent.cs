using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PPBC { 

    public class ParticleAnimEvent : MonoBehaviour
    {
        [SerializeField] ParticleSystem FX_smoke;

        public void DoSmokeEffect() {
            FX_smoke.Play();
            Debug.Log(FX_smoke.isPlaying);
        }

    }
}