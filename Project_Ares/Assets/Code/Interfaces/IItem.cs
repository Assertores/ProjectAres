using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PPBC {

    /// <summary>
    /// Legacy
    /// </summary>
    public interface IItem {

        void Init();
        void Collect();
        void Activate();
    }
}