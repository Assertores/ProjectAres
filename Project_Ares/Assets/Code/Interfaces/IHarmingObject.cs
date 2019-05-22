using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PPBC {
    /// <summary>
    /// das gegenstück zu DamageableObject
    /// zeuch dass anderen dingen schaden macht
    /// </summary>
    public interface IHarmingObject {

        /// <summary>
        /// wird einmalig von dem vorherigen object aufgerufen
        /// e.g. waffe oder rakete
        /// </summary>
        /// <param name="reverence">die referenz zum spieler, von dem dieses objekt kommt</param>
        /// <returns>seinen eigenen rigidbody, damit noch forces aplyed werden können</returns>
        Rigidbody2D Init(Player reverence, Sprite icon);
    }
}