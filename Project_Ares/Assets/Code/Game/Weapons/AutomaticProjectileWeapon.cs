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
        [SerializeField] Sprite m_icon_;

        [Header("Balancing")]
        [SerializeField] float m_rPM = 1;
        [SerializeField] float m_muzzleEnergy = 800;
        [SerializeField] int m_damage = 1;

        protected Player m_player = null;

        #endregion
        #region IWeapon

        public Sprite m_icon { get { return m_icon_; } }

        public void Init(Player player) {
            m_player = player;
        }

        public void SetActive(bool activate) {
            gameObject.SetActive(activate);
        }

        public void StartShooting() {
            Invoke("ShootBullet", 60 / m_rPM);
            m_audio.Play();
        }

        public void StopShooting() {
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
