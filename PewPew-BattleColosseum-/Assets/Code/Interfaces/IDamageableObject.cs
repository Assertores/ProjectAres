using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PPBC {
    public interface IDamageableObject {

        /// <summary>
        /// is this object still alive
        /// </summary>
        bool m_alive { get; }

        /// <summary>
        /// this function will be called if the object should take some damage
        /// </summary>
        /// <param name="source">the source object, whitch enflickted the damage</param>
        /// <param name="damage">the amound of damage it should take</param>
        /// <param name="recoilDir">the direction it sould flie of</param>
        /// <param name="doTeamDamage">if it should care for the team flag or not</param>
        void TakeDamage(IHarmingObject source, float damage, Vector2 recoilDir, bool doTeamDamage = true);

        /// <summary>
        /// this function should be called if the object should die (alsow from inside)
        /// </summary>
        /// <param name="source">the source object, whitch enflickted the damage</param>
        /// <param name="doTeamDamage">if it should care for the team flag or not</param>
        void Die(IHarmingObject source, bool doTeamDamage = true);
    }
}