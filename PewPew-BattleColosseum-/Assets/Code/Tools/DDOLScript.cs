﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PPBC {
    public class DDOLScript : MonoBehaviour {
        private void Awake() {
            DontDestroyOnLoad(this.gameObject);
            Destroy(this);
        }
    }
}