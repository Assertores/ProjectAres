using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PPBC {
    public class RespawnAnimLogic : MonoBehaviour {

        [SerializeField] Player m_owner;

        #region AnimCalls

        public void SetPlayerActive() {
            m_owner.SetPlayerActive(true);
        }

        public void SetPlayerInactive() {
            m_owner.SetPlayerActive(false);
        }

        public void SetPlayerVisable() {
            m_owner.SetPlayerVisable(true);
        }

        public void SetPlayerInvisable() {
            m_owner.SetPlayerVisable(false);
        }

        public void PlayerInControle() {
            m_owner.InControle(true);
        }

        public void PlayerNoControle() {
            m_owner.InControle(false);
        }

        public void PlayDeathEffect() {
            m_owner.DoDeathEffect();
        }

        public void StartDeathOrbs() {
            m_owner.DeathOrb(true);
        }

        public void StopDeathOrbs() {
            m_owner.DeathOrb(false);
        }

        public void PlayRespawnEffect() {
            m_owner.DoRespawnEffect();
        }

        public void PlayDieAnim() {
            m_owner.StartAnim(StringCollection.A_DIE);
        }

        public void PlayRespawnAnim() {
            m_owner.StartAnim(StringCollection.A_RESPAWN);
        }

        public void PlayDieSound() {
            m_owner.DoDieSFX();
        }

        public void PlayRespawnSound() {
            m_owner.DoRespawnSFX();
        }

        #endregion
    }
}