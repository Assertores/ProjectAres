using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using XInputDotNetPure;

namespace ProjectAres {
    public class MenuManager : MonoBehaviour {

        [Header("References")]
        [SerializeField] GameObject m_playerRev;

        GamePadState[] m_lastStates = new GamePadState[4];

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
                GameObject tmp = Instantiate(m_playerRev);
                if (tmp) {
                    GameObject tmpControle = new GameObject("Controler");
                    tmpControle.transform.parent = tmp.transform;

                    IControl reference = tmpControle.AddComponent<KeyboardControl>();//null reference checks
                    tmp.GetComponent<Player>().Init(reference);//null reference checks
                }
            }
            for(int i = 0; i < 4; i++) {
                if(m_lastStates[i].IsConnected && m_lastStates[i].Buttons.Start == ButtonState.Pressed && GamePad.GetState((PlayerIndex)i).Buttons.Start == ButtonState.Released) {
                    GameObject tmp = Instantiate(m_playerRev);
                    if (tmp) {
                        GameObject tmpControle = new GameObject("Controler");
                        tmpControle.transform.parent = tmp.transform;

                        ControllerControl reference = tmpControle.AddComponent<ControllerControl>();//null reference checks
                        reference._controlerIndex = i;
                        tmp.GetComponent<Player>().Init(reference);//null reference checks
                    }
                }
                m_lastStates[i] = GamePad.GetState((PlayerIndex)i);
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