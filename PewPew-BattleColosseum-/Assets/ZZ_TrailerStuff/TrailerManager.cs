using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace PPBC {
    public class TrailerManager : MonoBehaviour {

        [SerializeField] Transform r_pos1;
        [SerializeField] Transform r_pos2;

        private void Start() {
            TransitionHandler.ReadyToStart += MakeTrailer;
        }

        void MakeTrailer() {
            TransitionHandler.ReadyToStart -= MakeTrailer;

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

                yield return new WaitForSeconds(max + 1);

                foreach(var it in StartTrailerStuff.s_controlers) {
                    it.DoChangeCharacterTwice();
                }
            }

            foreach(var it in StartTrailerStuff.s_controlers) {
                it.DoDisconnect();
            }

            SceneManager.LoadScene(StringCollection.S_MAINMENU);
        }
    }
}