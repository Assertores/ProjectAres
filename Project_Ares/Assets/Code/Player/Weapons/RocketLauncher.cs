using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PPBC {
    public class RocketLauncher : MonoBehaviour, IWeapon {

        #region Variables

        float m_overchargeValue = 1;

        Player m_player = null;
        Vector2 m_velocity;
        float m_gravetyScale;
        bool m_isShooting = false;
        float m_startShootingTime = float.MinValue;
        float m_time;

        #endregion
        #region MonoBehaviour
        private void Start() {
            if (m_player.m_modellRefHolder.m_rocketLauncher.m_modelAnim != null) {//muss irgendwie bei denn modellen rein
            //    m_player.m_modellRefHolder.m_rocketLauncher.m_modelAnim.animation.Play("Rocket_Idle");
            }
        }

        void Update() {
            //fills stamina bar
            if (!m_isShooting && m_value > 0) {
                m_value = 1 - ((Time.time - m_startShootingTime) / m_player.m_modellRefHolder.m_rocketLauncher.m_shootDelay);
                if(m_value < 0) {
                    m_value = 0;
                }
                m_player.m_GUIHandler.m_stamina.fillAmount = m_value;
            }
            //overcharge effects
            if (m_isShooting) {
                if(m_time + m_player.m_modellRefHolder.m_rocketLauncher.m_overchargeMaxTime > Time.timeSinceLevelLoad) {
                    m_overchargeValue += m_player.m_modellRefHolder.m_rocketLauncher.m_overchargeAdd * Time.deltaTime;

                }
                else if(m_time + m_player.m_modellRefHolder.m_rocketLauncher.m_overchargeMaxTime < Time.timeSinceLevelLoad) {
                   m_player.m_rb.AddForce(-transform.right * m_player.m_modellRefHolder.m_rocketLauncher.m_muzzleEnergy * m_player.m_modellRefHolder.m_rocketLauncher.m_overchargeFail);
                    if (m_player.m_modellRefHolder.m_rocketLauncher.m_explosionRef) {
                        GameObject temp = Instantiate(m_player.m_modellRefHolder.m_rocketLauncher.m_explosionRef, transform.position, transform.rotation);
                        temp.GetComponentInChildren<IHarmingObject>()?.Init(m_player, m_icon);
                    }
                    m_player.m_rb.gravityScale = m_gravetyScale;
                    m_isShooting = false;
                    m_overchargeValue = 1;
                    m_startShootingTime = Time.time;
                    m_value = 1;
                    m_player.m_modellRefHolder.m_rocketLauncher.VFX_overcharge.SetActive(false);
                }
            }
            
            if (m_player.m_modellRefHolder.m_rocketLauncher.m_modelAnim != null /*&& !m_player.m_modellRefHolder.m_rocketLauncher.m_modelAnim.animation.isPlaying*/) {
                //m_player.m_modellRefHolder.m_rocketLauncher.m_modelAnim.animation.Play("Rocket_Idle");
            }
        }

        #endregion
        #region IWeapon

        public Sprite m_icon { get { return m_player.m_modellRefHolder.m_rocketLauncher.m_icon_; } }

        public float m_value { get; private set; }

        public void Init(Player player) {
            m_player = player;
        }

        public void SetActive(bool activate) {
            m_player.m_modellRefHolder.m_rocketLauncher.gameObject.SetActive(activate);
            if (m_player.m_modellRefHolder.m_rocketLauncher.m_modelAnim != null) {
                //m_player.m_modellRefHolder.m_rocketLauncher.m_modelAnim.animation.Play("Rocket_Weapon_Change",1);
            }
        }

        public void StartShooting() {
            if(m_startShootingTime + m_player.m_modellRefHolder.m_rocketLauncher.m_shootDelay > Time.time) {
                return;
            }

            if (m_player.m_modellRefHolder.m_rocketLauncher.m_modelAnim != null) {
                //m_player.m_modellRefHolder.m_rocketLauncher.m_modelAnim.animation.Play("Rocket_Charge");
            }
            m_time = Time.timeSinceLevelLoad;
            m_isShooting = true;

            //m_velocity = m_player.m_rb.velocity;
            m_gravetyScale = m_player.m_rb.gravityScale;

            m_player.m_rb.velocity = Vector2.zero;
            m_player.m_rb.gravityScale = 0;
            //---- ----- Feedback ----- ----

            m_player.m_modellRefHolder.m_rocketLauncher.VFX_overcharge.SetActive(true);
        }

        public void StopShooting() {
            if (!m_isShooting)
                return;

            if (m_player.m_modellRefHolder.m_rocketLauncher.m_modelAnim != null) {
                //m_player.m_modellRefHolder.m_rocketLauncher.m_modelAnim.animation.Play("Rocket_Shoot", 1);
                
            }
            m_overchargeValue = 1;
            //m_player.m_rb.velocity += m_velocity;
            m_player.m_rb.gravityScale = m_gravetyScale;
            m_startShootingTime = Time.time;
            m_player.m_modellRefHolder.m_rocketLauncher.m_audio.Play();
            ShootBullet();
            m_value = 1;
            m_isShooting = false;
            //---- ----- Feedback ----- ----

            m_player.m_modellRefHolder.m_rocketLauncher.VFX_overcharge.SetActive(false);
        }

        #endregion

        void ShootBullet() {
            RocketLauncherRefHolder rlrh = m_player.m_modellRefHolder.m_rocketLauncher;
            Rigidbody2D bulletRB = Instantiate(rlrh.m_bullet, rlrh.m_barrel.position, rlrh.m_barrel.rotation).GetComponent<IHarmingObject>()?.Init(m_player, m_icon);

            if (bulletRB) {
                //bulletRB.velocity = m_player.m_rb.velocity;
                bulletRB.AddForce(rlrh.transform.right * rlrh.m_muzzleEnergy * m_overchargeValue);
            }

            //overwrites previous velocity
            //m_player.m_rb.velocity = -rlrh.transform.right * rlrh.m_muzzleEnergy * m_player.m_rb.mass * (1 + m_overchargeValue / 4);
            m_player.m_rb.AddForce(-rlrh.transform.right * rlrh.m_muzzleEnergy * (1 + m_overchargeValue/4));
        }
    }
}
