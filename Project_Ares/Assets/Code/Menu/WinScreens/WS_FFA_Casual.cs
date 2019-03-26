using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProjectAres {
    public class WS_FFA_Casual : WinScreen {

        [Header("References")]
        [SerializeField] GameObject _pillar;
        [SerializeField] Vector3 _rightMostPlayer;
        [SerializeField] Vector3 _leftMostPlayer;

        void Start() {
            for (int i = 0; i < Player._references.Count; i++) {
                //Player._references[i].transform.position = Vector3.Lerp(_rightMostPlayer)
            }
        }

        // Update is called once per frame
        void Update() {

        }
    }
}