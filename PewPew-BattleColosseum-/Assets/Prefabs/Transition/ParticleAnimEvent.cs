using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PPBC { 

    public class ParticleAnimEvent : MonoBehaviour
    {
        [SerializeField] AudioSource r_curtain;
        [SerializeField] AudioClip SFX_rattle;
        [SerializeField] AudioClip SFX_impact;
        [SerializeField] AudioClip SFX_pullUp;
        [SerializeField] AudioClip SFX_hum;

        public void DoImpactSound() {
            r_curtain.Stop();
            r_curtain.clip = SFX_impact;
            r_curtain.Play();
        }
        public void DoRattleSound() {
            r_curtain.Stop();
            r_curtain.clip = SFX_rattle;
            r_curtain.Play();
        }
        public void DoPullUpSound() {
            r_curtain.Stop();
            r_curtain.clip = SFX_pullUp;
            r_curtain.Play();
        }
        public void DoHumSound() {
            r_curtain.Stop();
            r_curtain.clip = SFX_hum;
            r_curtain.Play();
        }
        public void StopSound() {
            r_curtain.Stop();
        }
    }
}
