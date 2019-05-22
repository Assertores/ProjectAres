using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PPBC {
    public interface IScriptQueueItem {

        bool FirstTick();
        bool DoTick();
    }
}