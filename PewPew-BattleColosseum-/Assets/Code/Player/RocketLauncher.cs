using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PPBC {
    public class RocketLauncher : MonoBehaviour, IHarmingObject {

        #region Variables

        float m_startChargeTime = float.MinValue;
        public float m_stamina { get; private set; } = 0;

        bool m_imActive;

        float m_startGravityScale;
        bool m_charging_;
        bool m_charging {get => m_charging_; set { m_charging_ = value; OnChargingChange(); } }

        float m_lastShot;
        
        #endregion
        #region MonoBehaviour

        void Update() {
            if (!m_owner || !m_owner.m_modelRef)
                return;

            if (m_charging) {
                m_stamina += Time.deltaTime;
                if (m_stamina > m_owner.m_rocket.m_overchargeMaxTime) {
                    Overcharged();
                    m_charging = false;
                } else {
                    //TODO: overcharge add effect
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
            m_startGravityScale = m_owner.m_rb.gravityScale;
        }

        public void StartShooting() {
            if (Time.time - m_lastShot < m_owner.m_rocket.m_shootDelay)
                return;

            m_startChargeTime = Time.time;
            m_charging = true;

            m_owner.m_modelRef.m_rocket.r_RocketAnim.AnimationState.SetAnimation(0, StringCollection.AR_CHARGING, true);
        }

        public void StopShooting() {
            if (m_charging)
                ShootBullet();
            m_charging = false;
            
            //Vfx_overcharge.SetActive(false);
        }

        public void ChangeWeapon(bool toMe) {
            m_imActive = toMe;

            if (!toMe) {
                StopShooting();
            }

            m_owner.m_modelRef?.m_rocket.r_weapon.SetActive(toMe);

            m_owner.m_modelRef.m_rocket.r_RocketAnim.AnimationState.SetAnimation(0, StringCollection.AR_CHANGE, false);
            m_owner.m_modelRef.m_rocket.r_RocketAnim.AnimationState.AddAnimation(0, StringCollection.AR_IDLE, true, 0);
        }

        public float GetStamina() {
            return  m_charging ? m_stamina / m_owner.m_rocket.m_overchargeMaxTime : 1-(Time.time - m_lastShot) / m_owner.m_rocket.m_shootDelay;
        }

        void OnChargingChange() {
            if (m_charging) {
                m_owner.m_rb.gravityScale = 0;
                m_owner.ResetVelocity();
                m_owner.m_modelRef.m_rocket.r_overcharge.SetActive(true);
            } else {
                m_owner.m_rb.gravityScale = m_startGravityScale;
                m_owner.m_modelRef.m_rocket.r_overcharge.SetActive(false);
            }
        }

        void ShootBullet() {
            m_lastShot = Time.time;

            m_owner.m_modelRef.m_rocket.fx_muzzleFlash.Play();

            Rigidbody2D bulletRB = Instantiate(m_owner.m_modelRef.m_rocket.p_rocket, m_owner.m_modelRef.m_rocket.r_barrel.position, m_owner.m_modelRef.m_rocket.r_barrel.rotation).GetComponent<ITracer>()?.Init(this);//TODO: objectPooling
            
            if (bulletRB) {
                bulletRB.AddForce(m_owner.m_modelRef.m_rocket.r_weapon.transform.right * m_owner.m_rocket.m_muzzleEnergy, ForceMode2D.Impulse);
            }

            m_owner.m_rb.AddForce(-m_owner.m_modelRef.m_rocket.r_weapon.transform.right * m_owner.m_rocket.m_muzzleEnergy, ForceMode2D.Impulse);

            m_owner.m_modelRef.m_rocket.r_RocketAnim.AnimationState.SetAnimation(0, StringCollection.AR_SHOOT, false);
            m_owner.m_modelRef.m_rocket.r_RocketAnim.AnimationState.AddAnimation(0, StringCollection.AR_IDLE, true, 0);
        }

        void Overcharged() {
            Instantiate(m_owner.m_modelRef.m_rocket.p_explosion, m_owner.m_modelRef.m_rocket.r_barrel.position, m_owner.m_modelRef.m_rocket.r_barrel.rotation).GetComponent<ITracer>()?.Init(this);
            m_owner.m_modelRef.m_rocket.r_RocketAnim.AnimationState.SetAnimation(0, StringCollection.AR_IDLE, true);
        }
    }
}