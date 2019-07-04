using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PPBC {
    public class TransitionHandler : MonoBehaviour {

        public static TransitionHandler s_singelton = null;

        public System.Action ReadyToStart;
        public System.Action ReadyToChange;

        #region Variables

        [Header("References")]
        [SerializeField] Animator r_anim;

        #endregion
        #region MonoBehaviour

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
            if (r_anim)
                StartCoroutine(IEInTransition());
            else
                ReadyToStart?.Invoke();
        }

        #endregion

        public void StartOutTransition() {
            if (r_anim)
                StartCoroutine(IEOutTransition());
            else
                ReadyToChange?.Invoke();
        }

        IEnumerator IEInTransition() {
            yield return new WaitForSeconds(r_anim.GetCurrentAnimatorClipInfo(0)[0].clip.length);
            ReadyToStart?.Invoke();
        }

        IEnumerator IEOutTransition() {
            r_anim.SetBool("FadeOut", true);
            yield return new WaitForSeconds(r_anim.GetCurrentAnimatorClipInfo(0)[0].clip.length);
            ReadyToChange?.Invoke();
        }
    }
}