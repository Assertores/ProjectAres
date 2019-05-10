using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProjectAres {
    public class AutomaticProjectileWeapon : MonoBehaviour, IWeapon {

        #region Variables

        [Header("References")]
        [SerializeField] GameObject m_bullet;
        [SerializeField] Transform m_barrel;
        [SerializeField] AudioSource m_audio;
        [SerializeField] AudioClip[] m_sounds;
        [SerializeField] Sprite m_icon_;

        [Header("Balancing")]
        [SerializeField] float m_rPM = 1;
        [SerializeField] float m_muzzleEnergy = 800;
        [SerializeField] float m_shootForSec = 4;
        [SerializeField] float m_coolDownRatio = 2;
        [SerializeField] float m_halfPitchRange = 0.1f;
        [SerializeField] float m_halfVolumeRange = 0.1f;


        float m_shootingTime;
        float m_weaponChangeTime;
        bool m_isShooting = false;
        bool m_forceCoolDown = false;

        float m_startPitch;
        float m_startVolume;

        Player m_player = null;

        #endregion

        private void Update()
        {
            if (m_isShooting)
            {
                m_shootingTime += Time.deltaTime;
            }
            else
            {
                m_shootingTime -= Time.deltaTime * m_coolDownRatio;
            }

            if(m_shootingTime >= m_shootForSec)
            {
                m_forceCoolDown = true;
                StopShooting();
            }else if(m_shootingTime <= 0)
            {
                m_forceCoolDown = false;
                m_shootingTime = 0;
            }

            m_value = m_shootingTime / m_shootForSec;
        }

        #region IWeapon

        public Sprite m_icon { get { return m_icon_; } }

        public float m_value { get; private set; }

        public void Init(Player player) {
            m_player = player;
            m_startPitch = m_audio.pitch;
            m_startVolume = m_audio.volume;
        }

        public void SetActive(bool activate) {
            if (!activate) {
                m_weaponChangeTime = Time.time;
            } else {
                m_shootingTime -= (Time.time - m_weaponChangeTime) * m_coolDownRatio;
                if(m_shootingTime <= 0) {
                    m_forceCoolDown = false;
                    m_shootingTime = 0;
                }
            }
            gameObject.SetActive(activate);
        }

        public void StartShooting() {
            if (m_forceCoolDown)
                return;

            m_isShooting = true;

            Invoke("ShootBullet", 60 / m_rPM);
        }

        public void StopShooting() {
            m_isShooting = false;
            CancelInvoke();
        }

        #endregion

        void ShootBullet() {
            Rigidbody2D bulletRB = Instantiate(m_bullet, m_barrel == null ? transform.position : m_barrel.position, m_barrel == null ? transform.rotation : m_barrel.rotation)
                .GetComponent<IHarmingObject>()?.Init(m_player, m_icon);
            
            if (bulletRB) {
                //bulletRB.velocity = m_player.m_rb.velocity;
                bulletRB.AddForce(transform.right * m_muzzleEnergy);
            }
            m_player.m_rb.AddForce(-transform.right * m_muzzleEnergy);

            if (m_sounds.Length > 0) {
                m_audio.pitch = Random.Range(m_startPitch-m_halfPitchRange, m_startPitch+m_halfPitchRange);
                m_audio.volume = Random.Range(m_startVolume - m_halfVolumeRange, m_startVolume + m_halfVolumeRange);
                m_audio.PlayOneShot(m_sounds[Random.Range(0, m_sounds.Length - 1)]);
            }
                

            Invoke("ShootBullet", 60 / m_rPM);
        }
    }
}
