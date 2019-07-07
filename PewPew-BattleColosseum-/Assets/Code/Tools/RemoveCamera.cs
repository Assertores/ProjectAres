using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PPBC {
    [RequireComponent(typeof(Camera))]
    public class RemoveCamera : MonoBehaviour {

        private void Start() {
            Destroy(this.gameObject);
        }
    }
}