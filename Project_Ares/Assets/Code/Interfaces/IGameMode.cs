using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProjectAres {
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

    }
}