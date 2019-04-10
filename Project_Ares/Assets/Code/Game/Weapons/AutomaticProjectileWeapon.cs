using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProjectAres {
    public class AutomaticProjectileWeapon : ProjectileWeapon {

        #region Variables

        //[Header("References")]

        //[Header("Balancing")]
        [SerializeField] float m_rPM = 1;

        //Player _player = null;

        #endregion
        #region ProjectileWeapon
        #region IWeapon

        public override void StartShooting() {
            Invoke("ShootBullet", 60 / m_rPM);
        }

        public override void StopShooting() {
            CancelInvoke();
        }

        #endregion

        protected override void ShootBullet() {
            base.ShootBullet();
            //Instantiate(_bullet, _barrol == null ? transform.position : _barrol.position, _barrol == null ? transform.rotation : _barrol.rotation)
            //    .GetComponent<Bullet>()?.Init(_player, _player._rig.velocity + (Vector2)transform.right * _bulletVelocity);

            //_player._rig.AddForce(-transform.right * _recoil);

            Invoke("ShootBullet", 60 / m_rPM);//Automatic
        }

        #endregion
    }
}