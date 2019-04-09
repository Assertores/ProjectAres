using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProjectAres {
    public class ProjectileWeapon : MonoBehaviour, IWeapon {

        [Header("References")]
        [SerializeField] protected GameObject m_bullet;
        [SerializeField] protected Transform m_barrel;

        [Header("Balancing")]
        //[SerializeField] float _rPM = 1;
        [SerializeField] protected float m_muzzleEnergy = 800;

        protected Player m_player = null;

        public Sprite m_Icon => throw new System.NotImplementedException();

        public void Init(Player player) {
            m_player = player;
        }

        public virtual void SetActive(bool activate) {
            gameObject.SetActive(activate);
        }

        public virtual void StartShooting() {
            ShootBullet();
        }

        public virtual void StopShooting() {
        }
        
        protected virtual void ShootBullet() {
            Rigidbody2D bulletRB = Instantiate(m_bullet,m_barrel == null? transform.position : m_barrel.position,m_barrel == null ? transform.rotation : m_barrel.rotation)
                .GetComponent<Bullet>()?.Init(m_player, m_player.m_rb.velocity/* + (Vector2)transform.right * m_bulletVelocity*/);

            if (bulletRB) {
                bulletRB.AddForce(transform.right * m_muzzleEnergy);
            }
            m_player.m_rb.AddForce(-transform.right * m_muzzleEnergy);
        }
    }
}