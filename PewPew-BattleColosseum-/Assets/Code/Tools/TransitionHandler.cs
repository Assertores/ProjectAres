using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PPBC {
    public class TransitionHandler : MonoBehaviour {

        public static TransitionHandler s_singelton = null;

        public System.Action ReadyToStart;

        public System.Action ReadyToChange;

        private void Awake() {
            if(s_singelton == null) {
                s_singelton = this;
            } else if(s_singelton != this) {
                Destroy(this);
            }
        }

        private void OnDestroy() {
            if(s_singelton == this) {
                s_singelton = null;
            }
        }

        private void Start() {
            StartCoroutine(IEInTransition());
        }

        public void StartOutTransition() {
            StartCoroutine(IEOutTransition());
        }

        IEnumerator IEInTransition() {
            yield return null;
            ReadyToStart?.Invoke();
        }

        IEnumerator IEOutTransition() {
            yield return null;
            ReadyToChange?.Invoke();
        }
    }
}