using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PPBC {
    public class TransitionAnimation : MonoBehaviour {
        [SerializeField] ParticleSystem[] FX_vsEffect;
        [SerializeField] AudioClip fx_vsAudio;

        public void DoVSEffect() {
            print("Ping");
            for (int i = 0; i < FX_vsEffect.Length; i++) {
                FX_vsEffect[i].Play();
                Debug.Log(FX_vsEffect);
            }
            GetComponent<AudioSource>().PlayOneShot(fx_vsAudio);
            print("Pong");
        }
    }
}