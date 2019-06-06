using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PPBC {
    public interface IGameMode {

        /// <summary>
        /// wird einmalig bei start des GameModes aufgerufen
        /// </summary>
        void Init();

        /// <summary>
        /// wird aufgerufen um zu verhindern, dass mehrere Gamemodes gleichzeitig aktiv sind.
        /// kann auch aufgerufen werden, selbst wenn der modus schon gestoppt ist.
        /// </summary>
        void Stop();

        /// <summary>
        /// wird aufgerufen, wenn ein spieler stirbt
        /// </summary>
        /// <param name="player">der spieler, der gestorben ist</param>
        void PlayerDied(Player player);

        /// <summary>
        /// function that will be called befor the menu changes to the ingame scene
        /// </summary>
        /// <param name="specificRef">the referenceobject to initialice evereything into</param>
        void SetMenuSpecific(Transform specificRef);

        /// <summary>
        /// will be triggert every frame alfert SetMenuSpecific
        /// </summary>
        /// <returns>true to cary on with the game</returns>
        bool ReadyToChange();
    }
}