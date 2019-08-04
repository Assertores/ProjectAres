﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Spine.Unity;

namespace PPBC {
    public class ModelRefHolder : MonoBehaviour {

        [System.Serializable]
        public struct d_rocketLauncherData {
            [Header("References")]
            public GameObject r_weapon;
            public Sprite m_Icon;
            public SkeletonAnimation r_RocketAnim;

            public Transform r_barrel;
            public AudioClip[] m_sounds;
            public GameObject r_overcharge;
            public ParticleSystem fx_muzzleFlash;

            public GameObject p_rocket;
            public GameObject p_explosion;
        }

        [System.Serializable]
        public struct d_smgData {
            [Header("References")]
            public GameObject r_weapon;
            public Sprite m_Icon;
            public SkeletonAnimation r_sMGAnim;

            public Transform r_barrel;
            public AudioClip[] m_sounds;
            public GameObject r_muzzleFlashParent;

            public GameObject p_bullet;

            [Header("Balancing")]
            public float m_halfPitchRange;
            public float m_halfVolumeRange;
        }

        public Sprite m_icon;
        public string m_name;
        public SkeletonAnimation r_modelAnim;
        public AudioSource fx_ModelAudio;

        public AudioSource fx_WeaponAudio;
        public GameObject r_weaponRot;
        public d_rocketLauncherData m_rocket;
        public d_smgData m_sMG;

        public LineRenderer m_laserPointer;

        private void Awake() {
            m_laserPointer = GetComponentInChildren<LineRenderer>();
        }
    }
}