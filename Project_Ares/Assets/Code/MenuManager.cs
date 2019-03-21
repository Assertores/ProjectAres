using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace ProjectAres {
    public class MenuManager : MonoBehaviour {

        [Header("References")]
        [SerializeField] GameObject _playerRev;

        private void Update() {
            if (Input.GetKeyDown(KeyCode.Return)) {
                GameObject tmp = Instantiate(_playerRev);
                if (tmp) {
                    GameObject tmpControle = new GameObject("Controler");
                    tmpControle.transform.parent = tmp.transform;

                    IControle reference = tmpControle.AddComponent<KeyboardControle>();//null reference checks
                    tmp.GetComponent<Player>().Init(reference);//null reference checks
                }
            }
        }

        public void StartGame() {
            //lade ausgewählte Szene im hintergrund
            //spiel animation für szenenwechsel ab
            //überblände die musik
            //unload Menuszene
        }

        public void Exit() {
            Application.Quit();
        }
    }
}