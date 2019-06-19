using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PPBC {
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
        /// wächselt den character
        /// </summary>
        /// <param type=bool>true = +1; false = -1</param>
        Action<bool> ChangeCharacter { get; set; }

        /// <summary>
        /// wächselt die waffe
        /// </summary>
        Action ChangeWeapon { get; set; }

        /// <summary>
        /// opens/closes the option menu
        /// </summary>
        Action OptionMenu { get; set; }

        /// <summary>
        /// a button press normally associated with accepting stuff
        /// </summary>
        Action Accept { get; set; }

        /// <summary>
        /// edited mode for changing type
        /// </summary>
        Action<bool> ChangeType { get; set; }

        /// <summary>
        /// call this to leave the game
        /// </summary>
        Action Disconnect { get; set; }

        void DoDisconect();

    }
}