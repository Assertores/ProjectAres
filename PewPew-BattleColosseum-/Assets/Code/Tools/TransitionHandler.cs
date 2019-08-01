using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PPBC {
    public class TransitionHandler : MonoBehaviour {

        static TransitionHandler s_singelton = null;

        public static System.Action ReadyToStart;
        public static System.Action ReadyToChange;

        #region Variables

        [Header("References")]
        [Tooltip("animator with bool named 'FadeOut' whitch will be set to true when the fadeout should start")]
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
            if (r_anim) {
                r_anim.gameObject.SetActive(true);
                StartCoroutine(IEInTransition());
            } else {
                ReadyToStart?.Invoke();
            }
        }

        #endregion

        public static void StartOutTransition() {
            if (s_singelton.r_anim)
                s_singelton.StartCoroutine(s_singelton.IEOutTransition());
            else
                ReadyToChange?.Invoke();
        }

        public static void StartGameTransition() {
            if (s_singelton.r_anim)
                s_singelton.StartCoroutine(s_singelton.IEGameTransition());
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

        IEnumerator IEGameTransition() {
            ReadyToChange?.Invoke();
            Animation anim = null;

            //richtige animation auswählen
            //richtige informationen in animation geben
            if (DataHolder.s_modis[DataHolder.s_currentModi].m_isTeamMode) {
                List<Player> teamA = Player.s_references.FindAll(x => x.m_team == 0);
                List<Player> teamB = Player.s_references.FindAll(x => x.m_team == 1);
                if(teamA.Count == 2 && teamB.Count == 2) {
                    //team 2vs2
                }else if(teamA.Count == 3 && teamB.Count == 1) {
                    //team 3vs1
                }else if(teamA.Count == 1 && teamB.Count == 3) {
                    //team 1vs3
                }
            } else {
                switch (Player.s_references.Count) {
                case 2:
                    //ffa 2
                    break;
                case 3:
                    //ffa 3
                    break;
                case 4:
                    //ffa 4
                    break;
                default:
                    break;
                }
            }

            if (anim) {
                anim.gameObject.SetActive(true);
                anim.Play();
                yield return new WaitForSeconds(anim.clip.length);
                anim.gameObject.SetActive(false);
            }

            ReadyToStart?.Invoke();
        }
    }
}