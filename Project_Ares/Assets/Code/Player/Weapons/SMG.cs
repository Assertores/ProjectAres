using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PPBC {
    public class SMG : MonoBehaviour, IWeapon {

        #region Variables

        float m_shootingTime;
        float m_weaponChangeTime;
        bool m_isShooting = false;
        bool m_forceCoolDown = false;

        float m_startPitch;
        float m_startVolume;

        Player m_player = null;

        #endregion

        #region MonoBehaviour
        private void Start() {
            
            if (m_player.m_modellRefHolder.m_sMG.m_modelAnim != null) {
                //m_player.m_modellRefHolder.m_sMG.m_modelAnim.animation.Play("SMG_Idle");
            }
        }

        private void Update()
        {
            m_player.m_modellRefHolder.m_sMG.m_muzzleflash.transform.rotation = transform.rotation;
            if (m_isShooting)
            {
                m_shootingTime += Time.deltaTime;
            }
            else
            {
                m_shootingTime -= Time.deltaTime * m_player.m_modellRefHolder.m_sMG.m_coolDownRatio;
            }

            if(m_shootingTime >= m_player.m_modellRefHolder.m_sMG.m_shootForSec)
            {
                m_forceCoolDown = true;
                StopShooting();
            }else if(m_shootingTime <= 0)
            {
                m_forceCoolDown = false;
                m_shootingTime = 0;
            }

            m_value = m_shootingTime / m_player.m_modellRefHolder.m_sMG.m_shootForSec;

            if (m_player.m_modellRefHolder.m_sMG.m_modelAnim != null /*&& !m_player.m_modellRefHolder.m_sMG.m_modelAnim.animation.isPlaying*/) {
                //m_player.m_modellRefHolder.m_sMG.m_modelAnim.animation.Play("SMG_Idle");
            }

        }

        #endregion
        #region IWeapon

        public Sprite m_icon { get { return m_player.m_modellRefHolder.m_sMG.m_icon_; } }

        public float m_value { get; private set; }

        public void Init(Player player) {
            m_player = player;
            m_player.m_modellRefHolder.m_sMG.m_muzzleflash.SetActive(false);
            m_startPitch = m_player.m_modellRefHolder.m_sMG.m_audio.pitch;
            m_startVolume = m_player.m_modellRefHolder.m_sMG.m_audio.volume;
        }

        public void SetActive(bool activate) {
            if (!activate) {
                m_weaponChangeTime = Time.time;
            } else {
                m_shootingTime -= (Time.time - m_weaponChangeTime) * m_player.m_modellRefHolder.m_sMG.m_coolDownRatio;
                if(m_shootingTime <= 0) {
                    m_forceCoolDown = false;
                    m_shootingTime = 0;
                }
            }
            m_player.m_modellRefHolder.m_sMG.m_muzzleflash.SetActive(false);
            m_player.m_modellRefHolder.m_sMG.gameObject.SetActive(activate);
            if (m_player.m_modellRefHolder.m_sMG.m_modelAnim != null) {
                //m_player.m_modellRefHolder.m_sMG.m_modelAnim.animation.Play("SMG_Weapon_Change",1);
            }
        }

        public void StartShooting() {
            if (m_forceCoolDown)
                return;
            if (m_player.m_modellRefHolder.m_sMG.m_modelAnim != null) {
                //m_player.m_modellRefHolder.m_sMG.m_modelAnim.animation.Play("SMG_Shoot");
            }
            m_isShooting = true;

            Invoke("ShootBullet", 60 / m_player.m_modellRefHolder.m_sMG.m_rPM);
            m_player.m_modellRefHolder.m_sMG.m_muzzleflash.SetActive(true);
           
        }

        public void StopShooting() {

            if (m_player.m_modellRefHolder.m_sMG.m_modelAnim != null) {
                //m_player.m_modellRefHolder.m_sMG.m_modelAnim.animation.Stop("SMG_Shoot");
            }

            m_player.m_modellRefHolder.m_sMG.m_muzzleflash.SetActive(false);
            m_isShooting = false;
            CancelInvoke();
        }

        #endregion

        void ShootBullet() {
            SMGRefHolder srh = m_player.m_modellRefHolder.m_sMG;
            Rigidbody2D bulletRB = Instantiate(srh.m_bullet, srh.m_barrel.position, srh.m_barrel.rotation).GetComponent<IHarmingObject>()?.Init(m_player, m_icon);
            
            if (bulletRB) {
                //bulletRB.velocity = m_player.m_rb.velocity;
                bulletRB.AddForce(srh.transform.right * srh.m_muzzleEnergy);
            }
            m_player.m_rb.AddForce(-srh.transform.right * srh.m_muzzleEnergy);

            if (srh.m_sounds.Length > 0) {
                srh.m_audio.pitch = Random.Range(m_startPitch- srh.m_halfPitchRange, m_startPitch + srh.m_halfPitchRange);
                srh.m_audio.volume = Random.Range(m_startVolume - srh.m_halfVolumeRange, m_startVolume + srh.m_halfVolumeRange);
                srh.m_audio.PlayOneShot(srh.m_sounds[Random.Range(0, srh.m_sounds.Length)]);
            }
                

            Invoke("ShootBullet", 60 / srh.m_rPM);
        }
    }
}
