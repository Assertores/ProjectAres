using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProjectAres {
    public class ProjectileWeapon : MonoBehaviour, IWeapon {

        [Header("References")]
        [SerializeField] protected GameObject _bullet;
        [SerializeField] protected Transform _barrol;

        [Header("Balancing")]
        //[SerializeField] float _rPM = 1;
        [SerializeField] protected float _bulletVelocity = 20;
        [SerializeField] protected float _recoil = 2;

        protected Player _player = null;

        public Sprite Icon => throw new System.NotImplementedException();

        public void Init(Player player) {
            _player = player;
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
            Instantiate(_bullet,_barrol == null? transform.position : _barrol.position,_barrol == null ? transform.rotation : _barrol.rotation)
                .GetComponent<Bullet>()?.Init(_player, _player._rig.velocity + (Vector2)transform.right * _bulletVelocity);

            _player._rig.AddForce(-transform.right * _recoil);
        }
    }
}