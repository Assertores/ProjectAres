using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProjectAres {
    public interface IHarmingObject {

        Rigidbody2D Init(Player reverence);
    }
}