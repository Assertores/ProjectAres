using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PPBC {
    public class FootBall : MonoBehaviour, IGameMode {

        #region Variables

        [Header("References")]
        [SerializeField] Sprite r_icon_;
        [SerializeField] string r_text_;

        [Header("Balancing")]
        [SerializeField] int m_goalsToWin = 3;

        #endregion
        #region IGameMode

        public Sprite m_icon => r_icon_;

        public string m_name => StringCollection.M_TDM;

        public string m_text => r_text_;

        public bool m_isTeamMode => true;

        public bool m_isActive { get; private set; } = false;

        public System.Action<bool> EndGame { get; set; }

        public void AbortGame() {
            throw new NotImplementedException();
        }

        public void DoEndGame() {
            throw new NotImplementedException();
        }

        public e_mileStones[] GetMileStones() {
            throw new NotImplementedException();
        }

        public void PlayerDied(IHarmingObject killer, Player victim) {
            throw new NotImplementedException();
        }

        public void ScorePoint(Player scorer, float amount) {
            throw new NotImplementedException();
        }

        public void SetUpGame() {
            throw new NotImplementedException();
        }

        public void StartGame() {
            throw new NotImplementedException();
        }

        #endregion
    }
}