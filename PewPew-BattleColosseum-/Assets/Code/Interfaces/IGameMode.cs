using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace PPBC {
    public interface IGameMode {
        
        Sprite m_icon { get; }
        string m_name { get; }
        string m_text { get; }

        /// <summary>
        /// will deturman if teamselect is called befor the map
        /// </summary>
        bool m_isTeamMode { get; }

        /// <summary>
        /// if bool false then the gamemode is no longer active
        /// </summary>
        bool m_isActive { get; }

        /// <summary>
        /// will be called when the map is finished loading
        /// </summary>
        void SetUpGame();

        /// <summary>
        /// will be called when the transition into the map is finished
        /// </summary>
        void StartGame();

        /// <summary>
        /// is called if the game should end normaly
        /// </summary>
        void DoEndGame();

        /// <summary>
        /// will be called if the game should stop imetiatly and return to main menu
        /// </summary>
        void AbortGame();

        /// <summary>
        /// true if normal, false if abort
        /// </summary>
        System.Action<bool> EndGame { get; set; }

        e_mileStones[] GetMileStones();

        //===== inGame =====

        void PlayerDied(IHarmingObject killer, Player victim);

        void ScorePoint(Player scorer, float amount);
    }
}