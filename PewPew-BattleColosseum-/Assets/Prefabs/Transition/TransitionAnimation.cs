using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransitionAnimation : MonoBehaviour
{
    [SerializeField] ParticleSystem[] FX_vsEffect;

   public void DoVSEffect() {
        for (int i = 0; i < FX_vsEffect.Length; i++) {
            FX_vsEffect[i].Play();
        }
        
    }
}
