using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Spine.Unity;

namespace PPBC {
    public class ModelRefHolder : MonoBehaviour {

        [System.Serializable]
        public struct d_rocketLauncherData {
            [Header("References")]
            public Sprite m_Icon;
            public SkeletonAnimation r_RocketAnim;

            public Transform r_barrel;
            public AudioClip[] m_sounds;
            public GameObject r_overcharge;

            public GameObject p_rocket;
            public GameObject p_explosion;

            [Header("Balancing")]
            public float m_muzzleEnergy;
            public float m_shootDelay;
            public float m_overchargeMaxTime;
            public float m_overchargeAdd;
            public float m_overchargeFail;
        }

        [System.Serializable]
        public struct d_smgData {
            [Header("References")]
            public Sprite m_Icon;
            public SkeletonAnimation r_sMGAnim;

            public Transform r_barrel;
            public AudioClip[] m_sounds;
            public GameObject r_muzzleFlashParent;

            public GameObject p_bullet;

            [Header("Balancing")]
            public float m_rPM;
            public float m_muzzleEnergy;
            public float m_shootForSec;
            public float m_coolDownRatio;
            public float m_halfPitchRange;
            public float m_halfVolumeRange;
        }

        public Sprite m_icon;
        public string m_name;
        public SkeletonAnimation r_modelAnim;
        public AudioSource fx_ModelAudio;

        public AudioSource fx_WeaponAudio;
        public GameObject r_WeaponRot;
        public d_rocketLauncherData m_rocket;
        public d_smgData m_sMG;
    }
}