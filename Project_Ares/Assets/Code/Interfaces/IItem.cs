using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProjectAres {
    public interface IItem {

        void Init();
        void Collect();
        void Activate();
    }
}