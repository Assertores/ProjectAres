using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace PPBC {
    public interface IControl {

        int index { get; set; }

        /// <summary>
        /// direction of the weapon
        /// </summary>
        Vector2 dir { get; }

        /// <summary>
        /// trigger of some sort to start shooting or charging
        /// </summary>
        Action TriggerDown { get; set; }

        /// <summary>
        /// releace of a trigger of some sort to stop shooting or start shooting
        /// </summary>
        Action TriggerUp { get; set; }

        /// <summary>
        /// changing the weapon
        /// </summary>
        Action ChangeWeapon { get; set; }

        /// <summary>
        /// changes the caracter
        /// </summary>
        /// <param type=bool>true = +1; false = -1</param>
        Action<bool> ChangeCharacter { get; set; }

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
        void DoDisconnect();
    }
}