using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace PPBC {
    public class TrailerManager : MonoBehaviour {

        [SerializeField] Transform r_pos1;
        [SerializeField] Transform r_pos2;

        float m_startTimeScale;

        private void Start() {
            TransitionHandler.ReadyToStart += MakeTrailer;
        }

        void MakeTrailer() {
            TransitionHandler.ReadyToStart -= MakeTrailer;

            m_startTimeScale = Time.timeScale;
            Time.timeScale = 0.75f;
            foreach(var it in Player.s_references) {
                it.InControle(true);
                it.Invincable(false);
            }

            StartCoroutine(IEMakeTrailer());
        }

        IEnumerator IEMakeTrailer() {
            StartTrailerStuff.s_controlers[0].DoChangeCharacterOnce();

            yield return new WaitForSeconds(1);

            for (int i = 0; i < 6; i++) {
                Player.s_references[0].Respawn(r_pos1.position);
                Player.s_references[1].Respawn(r_pos2.position);

                float max = float.MinValue;
                foreach (var it in StartTrailerStuff.s_controlers) {
                    max = Mathf.Max(max, it.StartAnim());
                }

                yield return new WaitForSeconds(5);

                foreach(var it in StartTrailerStuff.s_controlers) {
                    it.DoChangeCharacterTwice();
                }
            }

            foreach(var it in StartTrailerStuff.s_controlers) {
                it.DoDisconnect();
            }

            Time.timeScale = m_startTimeScale;
            SceneManager.LoadScene(StringCollection.S_MAINMENU);
        }
    }
}