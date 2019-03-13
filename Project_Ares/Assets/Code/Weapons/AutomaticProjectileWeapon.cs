using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProjectAres {
    public class AutomaticProjectileWeapon : ProjectileWeapon {

        //[Header("References")]

        //[Header("Balancing")]
        [SerializeField] float _rPM = 1;

        //Player _player = null;

        //public Sprite Icon => throw new System.NotImplementedException();

        //public void Init(Player player) {
        //    _player = player;
        //}

        //public void SetActive(bool activate) {
        //    gameObject.SetActive(activate);
        //}

        public override void StartShooting() {
            print("I'm gonna shoot");
            Invoke("ShootBullet", 60 / _rPM);
        }

        public override void StopShooting() {
            CancelInvoke();
        }

        protected override void ShootBullet() {
            base.ShootBullet();
            //Instantiate(_bullet, _barrol == null ? transform.position : _barrol.position, _barrol == null ? transform.rotation : _barrol.rotation)
            //    .GetComponent<Bullet>()?.Init(_player, _player._rig.velocity + (Vector2)transform.right * _bulletVelocity);

            //_player._rig.AddForce(-transform.right * _recoil);

            Invoke("ShootBullet", 60 / _rPM);//Automatic
        }
    }
}