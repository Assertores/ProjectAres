using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PPBC {
    public interface IDamageableObject {

        /// <summary>
        /// ist dieses ding noch am leben oder nicht
        /// </summary>
        bool m_alive { get; set; }

        /// <summary>
        /// fügt dem DamageableObject schaden zu
        /// wird meist von HarmingObjects aufgerufen
        /// </summary>
        /// <param name="damage">die menge an schaden, die gemacht wurde</param>
        /// <param name="source">referenc auf den spieler, der für den schaden verantwortlich ist. falls es keinen spieler gibt sollte diese variable "null" sein</param>
        /// <param name="force">mit welcher kraft das object in welche richtung geschleudert werden soll</param>
        void TakeDamage(float damage, Player source, Vector2 force, Sprite icon);

        /// <summary>
        /// wird diese funktion aufgerufen, soll das object sofort und ohne ausnahme serben
        /// </summary>
        /// <param name="source">wer für diesen tot verantwortlich ist. "null" falls es keinen gibt</param>
        void Die(Player source);

        /// <summary>
        /// falls jemand mal wissen will wie fiel leben das ding noch hat
        /// </summary>
        /// <returns>die health, die dieses object noch haben</returns>
        float GetHealth();
    }
}