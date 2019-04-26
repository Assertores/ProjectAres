using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace ProjectAres {
    public interface IWeapon {

        /// <summary>
        /// das icon, dass in der GUI angezeigt werden soll
        /// </summary>
        [SerializeField] Sprite m_icon { get;}

        /// <summary>
        /// ein belibiger wert, der als weapon value am spieler angezeigt werden soll zwischen 0.0f und 1.0f
        /// </summary>
        float m_value { get; }

        /// <summary>
        /// inizialisiert die waffe.
        /// wird ein mal am anfang aufgerufen.
        /// </summary>
        /// <param name="player"> die referenz zum spiele, zu der die waffe gehöhrt</param>
        void Init(Player player);

        /// <summary>
        /// wir normalerweise nur für waffenwechsel verwendet
        /// </summary>
        /// <param name="activate">sagt ob die waffe aktiv sein soll oder nicht</param>
        void SetActive(bool activate);

        /// <summary>
        /// wird einmalig im ersten frame aufgerufen in dem der Player anfängt zu schiesen
        /// </summary>
        void StartShooting();

        /// <summary>
        /// wird einmalig im letzten frame aufgerufen in dem der Player aufhöhrt zu schiesen
        /// </summary>
        void StopShooting();
    }
}