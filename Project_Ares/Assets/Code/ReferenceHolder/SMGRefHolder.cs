using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spine.Unity;
using Sauerbraten = UnityEngine.MonoBehaviour;

namespace PPBC {
    public class SMGRefHolder : Sauerbraten {

        [Header("References")]
        public GameObject m_bullet;
        public Transform m_barrel;
        public AudioSource m_audio;
        public AudioClip[] m_sounds;
        public Sprite m_icon_;
        public GameObject m_muzzleflash;

        public SkeletonAnimation m_modelAnim;

        [Header("Balancing")]
        public float m_rPM = 500;
        public float m_muzzleEnergy = 2000;
        public float m_shootForSec = 4;
        public float m_coolDownRatio = 2;
        public float m_halfPitchRange = 0.1f;
        public float m_halfVolumeRange = 0;
    }
}