﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sauerbraten = UnityEngine.MonoBehaviour;

namespace PPBC {
    [RequireComponent(typeof(Collider2D))]
    public class LaserBehaviour : Sauerbraten {
        #region Variables
        [Header("References")]
        [SerializeField] ParticleSystem VFX_laserStart;
        [SerializeField] ParticleSystem VFX_laserLoop;
        [SerializeField] ParticleSystem VFX_laserEnd;

        #endregion


        #region Singelton

        static LaserBehaviour s_singelton_ = null;
        public static LaserBehaviour s_singelton {
            get {
                if (!s_singelton_)
                    s_singelton_ = Instantiate(DataHolder.s_commonLaserBariar).GetComponent<LaserBehaviour>();
                return s_singelton_;
            }
        }

        void Awake() {
            if (s_singelton_ == null) {
                s_singelton_ = this;
            } else if (s_singelton_ != this) {
                Destroy(gameObject);
                return;
            }
        }

        void OnDestroy() {
            if (s_singelton_ == this)
                s_singelton_ = null;
        }

        #endregion

        public IEnumerator ChangePosition() {
            StopAllCoroutines();

            //float delay = 1;
            VFX_laserEnd.Play();
            
            yield return new WaitForSeconds(VFX_laserLoop.main.duration);

            int newIndex = Random.Range(0, LaserSpawner.s_references.Count);
            transform.position = LaserSpawner.s_references[newIndex].transform.position;
            Vector2 target = LaserSpawner.s_references[newIndex + 1 % LaserSpawner.s_references.Count].transform.position;
            transform.rotation = Quaternion.LookRotation(transform.forward, new Vector2(-(target.y - transform.position.y), target.x - transform.position.x));

            //delay = 1;
            VFX_laserStart.Play();
            yield return new WaitForSeconds(VFX_laserStart.main.duration);

            VFX_laserLoop.Play();
        }

        private void OnCollisionEnter2D(Collision2D collision) {
            IDamageableObject tmp = collision.gameObject.GetComponent<IDamageableObject>();
            if (tmp != null) {
                tmp.Die(null);
            } else {
                Destroy(collision.gameObject);
            }
        }
    }
}