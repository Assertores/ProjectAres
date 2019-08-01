using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace PPBC {
    public class StartTrailerStuff : MonoBehaviour {

        public static List<TrailerInput> s_controlers = new List<TrailerInput>();

        [Header("References")]
        [SerializeField] GameObject p_player;
        [SerializeField] GameObject[] p_controler;

        bool m_inTrailerMode = false;

        void Update() {
            if (!m_inTrailerMode) {
                if (Input.GetKeyUp(KeyCode.T)) {
                    m_inTrailerMode = true;
                    foreach(var it in Player.s_references) {
                        it.m_controler.DoDisconnect();
                    }

                    Player newPlayer = null;
                    for (int i = 0; i < 2; i++) {
                        newPlayer = Instantiate(p_player).GetComponentInChildren<Player>();
                        TrailerInput input = Instantiate(p_controler[i], newPlayer.transform.root).GetComponent<TrailerInput>();
                        input.m_index = i;
                        newPlayer.ChangeControlerUnsave(input);
                        s_controlers.Add(input);

                        newPlayer.CanChangeCharacter(true);
                        newPlayer.Invincable(true);
                        newPlayer.Respawn(SpawnPoint.s_references[Random.Range(0, SpawnPoint.s_references.Count)].transform.position);
                    }
                    StartCoroutine(IELoadSzene());
                }
            }
        }

        IEnumerator IELoadSzene() {
            yield return new WaitForSeconds(4);

            SceneManager.LoadScene("Trailer");
        }
    }
}