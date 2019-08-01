using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PPBC { 

    public class ParticleAnimEvent : MonoBehaviour
    {
        [SerializeField] ParticleSystem FX_smokeCurtain;

        public void DoSmokeEffect() {
            FX_smokeCurtain.Play();
        }
    }
}