using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransitionAnimation : MonoBehaviour
{
    [SerializeField] ParticleSystem FX_vsEffect;
   public void DoVSEffect() {
        FX_vsEffect.Play();
    }
}
