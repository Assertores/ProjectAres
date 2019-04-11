using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProjectAres {
    public class ProjectileWeapon : MonoBehaviour, IWeapon {

        #region Variables

        [Header("References")]
        [SerializeField] protected GameObject m_bullet;
        [SerializeField] protected Transform m_barrel;

        [Header("Balancing")]
        //[SerializeField] float _rPM = 1;
        [SerializeField] protected float m_muzzleEnergy = 800;
        [SerializeField] protected int m_damage = 1;

        protected Player m_player = null;

        #endregion
        #region MonoBehaviour



        #endregion
        #region IWeapon

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

        #endregion

        protected virtual void ShootBullet() {
            Rigidbody2D bulletRB = Instantiate(m_bullet,m_barrel == null? transform.position : m_barrel.position,m_barrel == null ? transform.rotation : m_barrel.rotation)
                .GetComponent<IHarmingObject>()?.Init(m_player);

            if (bulletRB) {
                //bulletRB.velocity = m_player.m_rb.velocity;
                bulletRB.AddForce(transform.right * m_muzzleEnergy);
            }
            m_player.m_rb.AddForce(-transform.right * m_muzzleEnergy);
        }
    }
}