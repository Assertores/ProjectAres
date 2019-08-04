using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace PPBC {
    public enum e_HarmingObjectType { SMG, ROCKED, LASOR, SHOCKWAVE, DEATHZONE } //TODO: find name for wave at the end of the game
    public interface IHarmingObject {

        /// <summary>
        /// icon of the object for the kill feed
        /// </summary>
        Sprite m_icon { get; }


        /// <summary>
        /// type of the object to destinguage it
        /// </summary>
        e_HarmingObjectType m_type { get; }

        /// <summary>
        /// the responcalbe player if he exists
        /// </summary>
        Player m_owner { get; }
    }
}