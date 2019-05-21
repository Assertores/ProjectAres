﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PPBC {
    public class ProjectileWeapon : MonoBehaviour, IWeapon {

        #region Variables

        [Header("References")]
        [SerializeField] GameObject m_bullet;
        [SerializeField] Transform m_barrel;
        [SerializeField] AudioSource m_audio;
        [SerializeField] Sprite m_icon_;

        private DragonBones.UnityArmatureComponent m_modelAnim;

        [Header("Balancing")]
        [SerializeField] float m_muzzleEnergy = 800;
        [SerializeField] float m_shootDelay = 2;

        Player m_player = null;
        Vector2 m_velocity;
        float m_gravetyScale;
        bool m_isShooting = false;
        float m_startShootingTime = float.MinValue;

        #endregion
        #region MonoBehaviour
        private void Start() {
            m_modelAnim = this.GetComponentInChildren<DragonBones.UnityArmatureComponent>();
            if (m_modelAnim != null) {
                m_modelAnim.animation.Play("Rocket_Idle");
            }
        }

        void Update() {
            if (!m_isShooting && m_value > 0) {
                m_value = 1 - ((Time.time - m_startShootingTime) / m_shootDelay);
                if(m_value < 0) {
                    m_value = 0;
                }
            }
            if (m_modelAnim != null && !m_modelAnim.animation.isPlaying) {
                m_modelAnim.animation.Play("Rocket_Idle");
            }
        }

        #endregion
        #region IWeapon

        public Sprite m_icon { get { return m_icon_; } }

        public float m_value { get; private set; }

        public void Init(Player player) {
            m_player = player;
        }

        public void SetActive(bool activate) {
            gameObject.SetActive(activate);
            if (m_modelAnim != null) {
                m_modelAnim.animation.Play("Rocket_Weapon_Change",1);
            }
        }

        public void StartShooting() {
            if(m_startShootingTime + m_shootDelay > Time.time) {
                return;
            }

            if (m_modelAnim != null) {
                m_modelAnim.animation.Play("Rocket_Charge");
            }

                m_isShooting = true;

            //m_velocity = m_player.m_rb.velocity;
            m_gravetyScale = m_player.m_rb.gravityScale;

            m_player.m_rb.velocity = Vector2.zero;
            m_player.m_rb.gravityScale = 0;
        }

        public void StopShooting() {
            if (!m_isShooting)
                return;

            if (m_modelAnim != null) {
                m_modelAnim.animation.Play("Rocket_Shoot",1);
            }
                //m_player.m_rb.velocity += m_velocity;
                m_player.m_rb.gravityScale = m_gravetyScale;
            m_startShootingTime = Time.time;
            m_audio.Play();
            ShootBullet();
            m_value = 1;
            m_isShooting = false;
        }

        #endregion

        void ShootBullet() {
            Rigidbody2D bulletRB = Instantiate(m_bullet,m_barrel == null? transform.position : m_barrel.position,m_barrel == null ? transform.rotation : m_barrel.rotation)
                .GetComponent<IHarmingObject>()?.Init(m_player, m_icon);

            if (bulletRB) {
                //bulletRB.velocity = m_player.m_rb.velocity;
                bulletRB.AddForce(transform.right * m_muzzleEnergy);
            }
            m_player.m_rb.AddForce(-transform.right * m_muzzleEnergy);
        }
    }
}
