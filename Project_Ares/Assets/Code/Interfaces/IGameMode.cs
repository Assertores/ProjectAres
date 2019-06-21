using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PPBC {
    public interface IGameMode {

        Sprite m_icon { get; set; }

        string m_text { get; set; }

        void Unselect();

        void Select();

        /// <summary>
        /// function that will be called befor the menu changes to the ingame scene
        /// </summary>
        /// <param name="specificRef">the referenceobject to initialice evereything into</param>
        void SetMenuSpecific(Transform specificRef);

        void Start();

        /// <summary>
        /// wird aufgerufen, wenn ein spieler stirbt
        /// </summary>
        /// <param name="player">der spieler, der gestorben ist</param>
        void PlayerDied(Player player);

        void EndGame();

        /// <summary>
        /// will be triggert every frame alfert SetMenuSpecific
        /// </summary>
        /// <returns>true to cary on with the game</returns>
        bool ReadyToChange();
    }
}