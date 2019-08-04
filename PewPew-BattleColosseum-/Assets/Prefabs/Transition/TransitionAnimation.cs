using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransitionAnimation : MonoBehaviour
{
    [SerializeField] ParticleSystem[] FX_vsEffect;

   public void DoVSEffect() {
        print("Ping");
        for (int i = 0; i < FX_vsEffect.Length; i++) {
            FX_vsEffect[i].Play();
            Debug.Log(FX_vsEffect);
        }
        print("Pong");
    }
}
