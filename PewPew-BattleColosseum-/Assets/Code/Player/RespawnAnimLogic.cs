using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PPBC {
    [RequireComponent(typeof(Player))]
    public class RespawnAnimLogic : MonoBehaviour {

        Player m_owner;

        private void Start() {
            m_owner = GetComponent<Player>();
        }

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

        public void PlayDieAnim() {
            m_owner.StartAnim(StringCollection.A_DIE);
        }

        public void PlayRespawnAnim() {
            m_owner.StartAnim(StringCollection.A_RESPAWN);
        }

        #endregion
    }
}