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
        [SerializeField] GameTransitionRefHolder r_ffa2;
        [SerializeField] GameTransitionRefHolder r_ffa3;
        [SerializeField] GameTransitionRefHolder r_ffa4;
        [SerializeField] GameTransitionRefHolder r_team1v1;
        [SerializeField] GameTransitionRefHolder r_team2v1;
        [SerializeField] GameTransitionRefHolder r_team1v2;
        [SerializeField] GameTransitionRefHolder r_team3v1;
        [SerializeField] GameTransitionRefHolder r_team2v2;
        [SerializeField] GameTransitionRefHolder r_team1v3;

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

            if (r_ffa2) {
                r_ffa2.gameObject.SetActive(false);
                r_ffa3.gameObject.SetActive(false);
                r_ffa4.gameObject.SetActive(false);
                r_team1v1.gameObject.SetActive(false);
                r_team2v1.gameObject.SetActive(false);
                r_team1v2.gameObject.SetActive(false);
                r_team3v1.gameObject.SetActive(false);
                r_team2v2.gameObject.SetActive(false);
                r_team1v3.gameObject.SetActive(false);
            }
        }

        #endregion

        public static void StartOutTransition() {
            if (MatchManager.s_currentMatch.m_isNextSceneMap) {
                StartGameTransition();
                return;
            }

            StartOutTransitionForReal();
        }

        static void StartOutTransitionForReal() {
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
            GameTransitionRefHolder animRef = null;

            if (DataHolder.s_modis[DataHolder.s_currentModi].m_isTeamMode) {
                List<Player> teamA = Player.s_references.FindAll(x => x.m_team == 0);
                List<Player> teamB = Player.s_references.FindAll(x => x.m_team == 1);
                if (teamA.Count == 2 && teamB.Count == 2) {
                    animRef = r_team2v2;
                } else if (teamA.Count == 3 && teamB.Count == 1) {
                    animRef = r_team3v1;
                } else if (teamA.Count == 1 && teamB.Count == 3) {
                    animRef = r_team1v3;
                } else if (teamA.Count == 1 && teamB.Count == 2) {
                    animRef = r_team1v2;
                } else if (teamA.Count == 2 && teamB.Count == 1) {
                    animRef = r_team2v1;
                } else if (teamA.Count == 1 && teamB.Count == 1) {
                    animRef = r_team1v1;
                } else {
                    Debug.Log("no animation found");
                    goto NoAnimation;
                }

                for (int i = 0; i < teamA.Count; i++) {
                    animRef.r_chars[i].r_character.sprite = teamA[i].m_modelRef.m_icon;
                    animRef.r_chars[i].r_charName.text = teamA[i].m_modelRef.m_name;
                    Color color = teamA[i].GetPlayerColor();
                    foreach (var it in animRef.r_chars[i].r_backgrounds) {
                        it.color = color;
                    }
                }
                for(int i = 0; i < teamB.Count; i++) {
                    animRef.r_chars[i + teamA.Count].r_character.sprite = teamA[i].m_modelRef.m_icon;
                    animRef.r_chars[i + teamA.Count].r_charName.text = teamA[i].m_modelRef.m_name;
                    Color color = teamA[i].GetPlayerColor();
                    foreach (var it in animRef.r_chars[i + teamA.Count].r_backgrounds) {
                        it.color = color;
                    }
                }
            } else {
                switch (Player.s_references.Count) {
                case 2:
                    animRef = r_ffa2;
                    break;
                case 3:
                    animRef = r_ffa3;
                    break;
                case 4:
                    animRef = r_ffa4;
                    break;
                default:
                    Debug.Log("no animation found");
                    goto NoAnimation;
                }
                for (int i = 0; i < Player.s_references.Count; i++) {
                    animRef.r_chars[i].r_character.sprite = Player.s_references[i].m_modelRef.m_icon;
                    animRef.r_chars[i].r_charName.text = Player.s_references[i].m_modelRef.m_name;
                    Color color = Player.s_references[i].GetPlayerColor();
                    foreach(var it in animRef.r_chars[i].r_backgrounds) {
                        it.color = color;
                    }
                }
            }

            animRef.r_name.text = DataHolder.s_modis[DataHolder.s_currentModi].m_name;
            animRef.r_flavour.text = DataHolder.s_modis[DataHolder.s_currentModi].m_text;

            animRef.gameObject.SetActive(true);
            animRef.r_anim.Play(animRef.r_anim.GetCurrentAnimatorClipInfo(0)[0].clip.name);
            float GameClipLength = animRef.r_anim.GetCurrentAnimatorClipInfo(0)[0].clip.length;
            print(GameClipLength);
            yield return new WaitForSeconds(GameClipLength);

NoAnimation:

            StartOutTransitionForReal();
        }
    }
}