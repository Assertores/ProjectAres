using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProjectAres {
    public interface IGameMode {

        void Init();
        void PlayerDied(Player player);

    }
}