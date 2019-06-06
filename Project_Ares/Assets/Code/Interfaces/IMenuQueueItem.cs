using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PPBC {
    public interface IMenuQueueItem {

        void SetUp();
        int DoTick();
    }
}