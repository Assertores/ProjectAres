using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spine.Unity;
using Sauerbraten = UnityEngine.MonoBehaviour;

namespace PPBC {
    public class RocketLauncherRefHolder : Sauerbraten {

        [Header("References")]
        public GameObject m_bullet;
        public Transform m_barrel;
        public AudioSource m_audio;
        public Sprite m_icon_;
        public GameObject m_explosionRef;
        public GameObject VFX_overcharge;

        public SkeletonAnimation m_modelAnim;

        [Header("Balancing")]
        public float m_muzzleEnergy = 20000;
        public float m_shootDelay = 2;
        public float m_overchargeMaxTime = 4;
        public float m_overchargeAdd = 0.4f;
        public float m_overchargeFail = 1.5f;

    }
}