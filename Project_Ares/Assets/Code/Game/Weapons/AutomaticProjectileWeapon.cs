﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProjectAres {
    public class AutomaticProjectileWeapon : MonoBehaviour, IWeapon {

        #region Variables

        [Header("References")]
        [SerializeField] GameObject m_bullet;
        [SerializeField] Transform m_barrel;
        [SerializeField] AudioSource m_audio;
        [SerializeField] Sprite m_icon_;

        [Header("Balancing")]
        [SerializeField] float m_rPM = 1;
        [SerializeField] float m_muzzleEnergy = 800;
        [SerializeField] float m_shootForSec = 4;
        [SerializeField] float m_coolDownRatio = 2;


        float m_shootingTime;
        float m_weaponChangeTime;
        bool m_isShooting = false;
        bool m_forceCoolDown = false;

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
            m_audio.Play();
        }

        public void StopShooting() {
            m_isShooting = false;
            CancelInvoke();
        }

        #endregion

        void ShootBullet() {
            Rigidbody2D bulletRB = Instantiate(m_bullet, m_barrel == null ? transform.position : m_barrel.position, m_barrel == null ? transform.rotation : m_barrel.rotation)
                .GetComponent<IHarmingObject>()?.Init(m_player);
            
            if (bulletRB) {
                //bulletRB.velocity = m_player.m_rb.velocity;
                bulletRB.AddForce(transform.right * m_muzzleEnergy);
            }
            m_player.m_rb.AddForce(-transform.right * m_muzzleEnergy);

            Invoke("ShootBullet", 60 / m_rPM);
        }
    }
}
