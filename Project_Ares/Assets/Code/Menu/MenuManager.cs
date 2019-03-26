using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using XInputDotNetPure;

namespace ProjectAres {
    public class MenuManager : MonoBehaviour {

        [Header("References")]
        [SerializeField] GameObject _playerRev;

        #region Singelton

        public static MenuManager _singelton = null;

        private void Awake() {
            if (_singelton)
                Destroy(this);

            _singelton = this;
        }
        private void OnDestroy() {
            if (_singelton == this)
                _singelton = null;
        }

        #endregion

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
            for(int i = 0; i < 4; i++) {
                if(GamePad.GetState((PlayerIndex)i).Buttons.Start == ButtonState.Released) {
                    GameObject tmp = Instantiate(_playerRev);
                    if (tmp) {
                        GameObject tmpControle = new GameObject("Controler");
                        tmpControle.transform.parent = tmp.transform;

                        ControllerControle reference = tmpControle.AddComponent<ControllerControle>();//null reference checks
                        reference._controlerIndex = i;
                        tmp.GetComponent<Player>().Init(reference);//null reference checks
                    }
                }
            }
        }

        public void StartGame() {
            SceneManager.LoadScene(StringCollection.EXAMPLESZENE);
            //lade ausgewählte Szene im hintergrund
            //spiel animation für szenenwechsel ab
            //überblände die musik
            //unload Menuszene
        }

        public void Exit() {
            print("Quitting the Game");
            Application.Quit();
        }
    }
}