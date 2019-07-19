using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PPBC {
    public class DDOLScript : MonoBehaviour {
        private void Start() {
            DontDestroyOnLoad(this.gameObject);
            Destroy(this);
        }
    }
}