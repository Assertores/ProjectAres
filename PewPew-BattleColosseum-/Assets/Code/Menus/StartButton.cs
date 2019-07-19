using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace PPBC {
    [RequireComponent(typeof(Slider))]
    public class StartButton : MonoBehaviour {
        // Start is called before the first frame update
        void Start() {
            GetComponent<Slider>().Select();
        }
    }
}