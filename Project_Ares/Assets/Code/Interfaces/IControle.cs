using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProjectAres {
    public interface IControl {

        /// <summary>
        /// die richtung, in die die waffe schauen soll
        /// </summary>
        Vector2 m_dir { get; set; }

        /// <summary>
        /// Initialisiere diese Action wenn der spieler anfangen soll zu schiesen
        /// </summary>
        Action StartShooting { get; set; }

        /// <summary>
        /// Initialisiere diese Action wenn der spieler auf höhren soll zu schiesen
        /// </summary>
        Action StopShooting { get; set; }

        /// <summary>
        /// Legacy. wenn der spieler dashen soll
        /// </summary>
        Action Dash { get; set; }

        /// <summary>
        /// Legacy. setzt eine waffe in den selected status aber wählt sie noch nicht aus
        /// </summary>
        /// <param type=int>der index der waffe</param>
        Action<int> SelectWeapon { get; set; }

        /// <summary>
        /// wächselt den character
        /// </summary>
        /// <param type=bool>true für relatieve answahl. false für index auswahl</param>
        /// <param type=int>um wie fiele charactere er weiter gehen soll / der index des characters</param>
        Action<int, bool> ChangeCharacter { get; set; }

        /// <summary>
        /// wächselt die waffe
        /// </summary>
        /// <param type=bool>true für relatieve answahl. false für index auswahl</param>
        /// <param type=int>um wie fiele waffen er weiter gehen soll / der index der waffe</param>
        Action<int, bool> ChangeWeapon { get; set; }

        /// <summary>
        /// Legacy. benutzt ein item
        /// </summary>
        /// <param type=int>der index des items</param>
        Action<int> UseItem { get; set; }

        /// <summary>
        /// call this to leave the game
        /// </summary>
        Action Disconnect { get; set; }
    }
}