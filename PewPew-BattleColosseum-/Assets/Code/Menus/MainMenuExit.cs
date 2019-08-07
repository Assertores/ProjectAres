using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PPBC {
    [RequireComponent(typeof(Animation))]
    public class MainMenuExit : MonoBehaviour {

        [SerializeField] AudioClip m_beamSound;
        [SerializeField] float m_delay = 3;

        Coroutine h_startGame;
        public void StartGame() {
            foreach (var it in Player.s_references) {
                it.CanChangeCharacter(false);
            }

            h_startGame = StartCoroutine(IEStartGame());
            Timer.StartTimer(m_delay);
        }

        public void AbortStartGame() {
            foreach (var it in Player.s_references) {
                it.CanChangeCharacter(true);
            }

            StopCoroutine(h_startGame);
            Timer.AbortTimer();
        }

        IEnumerator IEStartGame() {
            yield return new WaitForSeconds(m_delay);

            GetComponent<Animation>().Play();
        }


        public void StartBeamEffect() {
            foreach (var it in Player.s_references) {
                it.StartBeam();
            }
        }

        public void PlayBeamSound() {
            foreach(var it in Player.s_references) {
                it.m_modelRef.fx_ModelAudio.PlayOneShot(m_beamSound);
            }
        }

        public void SetPlayerInvicable() {
            foreach(var it in Player.s_references) {
                it.SetPlayerVisable(false);
            }
        }

        public void StartOutTransition() {
            MatchManager.s_currentMatch.StartGame();
        }
    }
}