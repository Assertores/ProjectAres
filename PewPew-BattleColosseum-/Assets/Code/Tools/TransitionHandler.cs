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
                if(teamA.Count == 2 && teamB.Count == 2) {
                    animRef = r_team2v2;

                    animRef.r_p1.sprite = teamA[0].m_modelRef.m_icon;
                    animRef.r_p2.sprite = teamA[1].m_modelRef.m_icon;
                    animRef.r_p3.sprite = teamB[0].m_modelRef.m_icon;
                    animRef.r_p4.sprite = teamB[1].m_modelRef.m_icon;
                } else if(teamA.Count == 3 && teamB.Count == 1) {
                    animRef = r_team3v1;

                    animRef.r_p1.sprite = teamA[0].m_modelRef.m_icon;
                    animRef.r_p2.sprite = teamA[1].m_modelRef.m_icon;
                    animRef.r_p3.sprite = teamA[2].m_modelRef.m_icon;
                    animRef.r_p4.sprite = teamB[1].m_modelRef.m_icon;
                } else if(teamA.Count == 1 && teamB.Count == 3) {
                    animRef = r_team1v3;

                    animRef.r_p1.sprite = teamA[0].m_modelRef.m_icon;
                    animRef.r_p2.sprite = teamB[0].m_modelRef.m_icon;
                    animRef.r_p3.sprite = teamB[1].m_modelRef.m_icon;
                    animRef.r_p4.sprite = teamB[2].m_modelRef.m_icon;
                } else if(teamA.Count == 1 && teamB.Count == 2) {
                    animRef = r_team1v2;

                    animRef.r_p1.sprite = teamA[0].m_modelRef.m_icon;
                    animRef.r_p2.sprite = teamB[0].m_modelRef.m_icon;
                    animRef.r_p3.sprite = teamB[1].m_modelRef.m_icon;
                } else if(teamA.Count == 2 && teamB.Count == 1) {
                    animRef = r_team2v1;

                    animRef.r_p1.sprite = teamA[0].m_modelRef.m_icon;
                    animRef.r_p2.sprite = teamA[1].m_modelRef.m_icon;
                    animRef.r_p3.sprite = teamB[0].m_modelRef.m_icon;
                } else if(teamA.Count == 1 && teamB.Count == 1) {
                    animRef = r_team1v1;

                    animRef.r_p1.sprite = teamA[0].m_modelRef.m_icon;
                    animRef.r_p2.sprite = teamB[0].m_modelRef.m_icon;
                }

                if (animRef) {
                    animRef.r_p1Background.color = DataHolder.s_teamColors[0];
                    animRef.r_p2Background.color = DataHolder.s_teamColors[1];
                }
            } else {
                switch (Player.s_references.Count) {
                case 2:
                    animRef = r_ffa2;

                    animRef.r_p1.sprite = Player.s_references[0].m_modelRef.m_icon;
                    animRef.r_p1Background.color = Player.s_references[0].GetPlayerColor();
                    animRef.r_p2.sprite = Player.s_references[1].m_modelRef.m_icon;
                    animRef.r_p2Background.color = Player.s_references[1].GetPlayerColor();
                    break;
                case 3:
                    animRef = r_ffa3;

                    animRef.r_p1.sprite = Player.s_references[0].m_modelRef.m_icon;
                    animRef.r_p1Background.color = Player.s_references[0].GetPlayerColor();
                    animRef.r_p2.sprite = Player.s_references[1].m_modelRef.m_icon;
                    animRef.r_p2Background.color = Player.s_references[1].GetPlayerColor();
                    animRef.r_p3.sprite = Player.s_references[2].m_modelRef.m_icon;
                    animRef.r_p3Background.color = Player.s_references[2].GetPlayerColor();
                    break;
                case 4:
                    animRef = r_ffa4;

                    animRef.r_p1.sprite = Player.s_references[0].m_modelRef.m_icon;
                    animRef.r_p1Background.color = Player.s_references[0].GetPlayerColor();
                    animRef.r_p2.sprite = Player.s_references[1].m_modelRef.m_icon;
                    animRef.r_p2Background.color = Player.s_references[1].GetPlayerColor();
                    animRef.r_p3.sprite = Player.s_references[2].m_modelRef.m_icon;
                    animRef.r_p3Background.color = Player.s_references[2].GetPlayerColor();
                    animRef.r_p4.sprite = Player.s_references[3].m_modelRef.m_icon;
                    animRef.r_p4Background.color = Player.s_references[3].GetPlayerColor();
                    break;
                default:
                    break;
                }
            }

            if (animRef) {
                animRef.r_name.text = DataHolder.s_modis[DataHolder.s_currentModi].m_name;
                animRef.r_flavour.text = DataHolder.s_modis[DataHolder.s_currentModi].m_text;

                animRef.gameObject.SetActive(true);
                animRef.r_anim.Play(animRef.r_anim.GetCurrentAnimatorClipInfo(0)[0].clip.name);
                float GameClipLength = animRef.r_anim.GetCurrentAnimatorClipInfo(0)[0].clip.length;
                yield return new WaitForSeconds(GameClipLength);
            }
            StartOutTransition();
        }
    }
}