using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace PPBC {
    public interface IGameMode {
        
        Sprite m_icon { get; }
        string m_name { get; }
        string m_text { get; }

        bool m_isTeamMode { get; }

        void StartTransition();

        void StartGame();

        void DoEndGame();

        void AbortGame();

        /// <summary>
        /// true if normal, false if abort
        /// </summary>
        System.Action<bool> EndGame { get; set; }

        e_mileStones[] GetMileStones();

        //===== inGame =====

        void PlayerDied(IHarmingObject killer, Player victim);

        void ScorePoint(Player scorer);
    }
}