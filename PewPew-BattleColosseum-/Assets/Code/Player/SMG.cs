using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PPBC {
    public class SMG : MonoBehaviour, IHarmingObject {

        #region Variables

        float m_lastShot = float.MinValue;
        public float m_stamina { get; private set; } = 0;

        bool m_imActive;
        bool m_shootng;
        bool m_forceCooldown;

        #endregion
        #region MonoBehaviour

        void Update() {
            if (!m_owner || !m_owner.m_modelRef)
                return;

            if (m_shootng) {
                m_stamina += Time.deltaTime;

                if(m_stamina > m_owner.m_modelRef.m_sMG.m_shootForSec) {
                    m_forceCooldown = true;
                    m_shootng = false;
                }
            } else {
                if(m_stamina > 0) {
                    m_stamina -= Time.deltaTime * m_owner.m_modelRef.m_sMG.m_coolDownRatio;

                    if (m_stamina < 0) {
                        m_stamina = 0;
                        m_forceCooldown = false;
                    }
                }
                return;
            }

            if (m_forceCooldown)
                return;

            if(Time.time > m_lastShot + 60 / m_owner.m_modelRef.m_sMG.m_rPM) {
                ShootBullet();

                m_lastShot = Time.time;
            }
        }

        #endregion
        #region IHarmingObject

        public Sprite m_icon => m_owner?.m_modelRef?.m_sMG.m_Icon;

        public e_HarmingObjectType m_type => e_HarmingObjectType.SMG;

        public Player m_owner { get; private set; }

        #endregion

        public void Init(Player owner) {
            m_owner = owner;
        }

        public void StartShooting() {
            if (m_forceCooldown)
                return;

            m_shootng = true;
        }

        public void StopShooting() {
            m_shootng = false;
        }

        public void ChangeWeapon(bool toMe) {
            m_imActive = toMe;

            if (!toMe) {
                StopShooting();
            }

            m_owner.m_modelRef?.m_sMG.r_weapon.SetActive(toMe);
        }

        void ShootBullet() {
            Rigidbody2D bulletRB = Instantiate(m_owner.m_modelRef.m_sMG.p_bullet, m_owner.m_modelRef.m_sMG.r_barrel.position, m_owner.m_modelRef.m_sMG.r_barrel.rotation).GetComponent<ITracer>()?.Init(this);//TODO: objectPooling
            
            if (bulletRB) {
                bulletRB.AddForce(m_owner.m_modelRef.m_sMG.r_weapon.transform.right * m_owner.m_modelRef.m_sMG.m_muzzleEnergy);
            }

            m_owner.m_rb.AddForce(-m_owner.m_modelRef.m_sMG.r_weapon.transform.right * m_owner.m_modelRef.m_sMG.m_muzzleEnergy);

            if (m_owner.m_modelRef.m_sMG.m_sounds.Length > 0) {
                //m_owner.m_modelRef.fx_WeaponAudio.pitch = Random.Range(m_startPitch - srh.m_halfPitchRange, m_startPitch + srh.m_halfPitchRange);
                //m_owner.m_modelRef.fx_WeaponAudio.volume = Random.Range(m_startVolume - srh.m_halfVolumeRange, m_startVolume + srh.m_halfVolumeRange);
                m_owner.m_modelRef.fx_WeaponAudio.PlayOneShot(m_owner.m_modelRef.m_sMG.m_sounds[Random.Range(0, m_owner.m_modelRef.m_sMG.m_sounds.Length)]);
            }
        }
    }
}