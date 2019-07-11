using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PPBC {
    public class RocketLauncher : MonoBehaviour, IHarmingObject {

        #region Variables

        float m_startChargeTime = float.MinValue;
        public float m_stamina { get; private set; } = 0;

        bool m_imActive;
        bool m_charging;

        #endregion
        #region MonoBehaviour

        void Update() {
            if (!m_owner || !m_owner.m_modelRef)
                return;

            if (m_charging) {
                m_stamina += Time.deltaTime;

                if (m_stamina > m_owner.m_modelRef.m_rocket.m_overchargeMaxTime) {
                    //TODO: make overcharge
                    m_charging = false;
                }
            } else {
                m_stamina = 0;
            }
        }

        #endregion
        #region IHarmingObject

        public Sprite m_icon => m_owner?.m_modelRef?.m_rocket.m_Icon;

        public e_HarmingObjectType m_type => e_HarmingObjectType.ROCKED;

        public Player m_owner { get; private set; }

        #endregion

        public void Init(Player owner) {
            m_owner = owner;
        }

        public void StartShooting() {
            m_startChargeTime = Time.time;
            m_charging = true;
        }

        public void StopShooting() {
            if (m_charging)
                ShootBullet();
            m_charging = false;
        }

        public void ChangeWeapon(bool toMe) {
            m_imActive = toMe;

            if (!toMe) {
                StopShooting();
            }

            m_owner.m_modelRef?.m_rocket.r_weapon.SetActive(toMe);
        }

        void ShootBullet() {
            Rigidbody2D bulletRB = Instantiate(m_owner.m_modelRef.m_rocket.p_rocket, m_owner.m_modelRef.m_rocket.r_barrel.position, m_owner.m_modelRef.m_rocket.r_barrel.rotation).GetComponent<ITracer>()?.Init(this);//TODO: objectPooling
            
            if (bulletRB) {
                bulletRB.AddForce(m_owner.m_modelRef.m_rocket.r_weapon.transform.right * m_owner.m_modelRef.m_rocket.m_muzzleEnergy);
            }

            m_owner.m_rb.AddForce(-m_owner.m_modelRef.m_rocket.r_weapon.transform.right * m_owner.m_modelRef.m_rocket.m_muzzleEnergy);
        }
    }
}