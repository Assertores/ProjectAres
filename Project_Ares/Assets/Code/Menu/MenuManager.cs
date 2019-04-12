using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using XInputDotNetPure;

namespace ProjectAres {
    public class MenuManager : MonoBehaviour {

        #region Variables

        [Header("References")]
        [SerializeField] GameObject m_playerRev;
        [SerializeField] GameObject m_SpawnPoint;

        GamePadState[] m_lastStates = new GamePadState[4];

        #endregion
        #region MonoBehaviour
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

        private void Start() {
            foreach (var it in Player.s_references) {
                it.Invincible(true);
                it.m_rb.velocity = Vector2.zero;
                it.transform.position = m_SpawnPoint.transform.position;
            }
        }

        private void Update() {
            if (Input.GetKeyDown(KeyCode.Return)) {
                GameObject tmp = Instantiate(m_playerRev);
                if (tmp) {
                    GameObject tmpControle = new GameObject("Controler");
                    //tmpControle.transform.parent = tmp.transform;

                    IControl reference = tmpControle.AddComponent<KeyboardControl>();//null reference checks
                    
                    //tmp.GetComponent<Player>().Init(KeyboardControl);
                    tmp.GetComponentInChildren<Player>().Init(tmpControle);//dirty
                    //tmp.GetComponentInChildren<Player>().Init(reference);//null reference checks
                    tmp.GetComponentInChildren<Player>().Invincible(true);//TODO: playerscript wird doppeld gesucht
                }
            }
            for(int i = 0; i < 4; i++) {
                if(m_lastStates[i].IsConnected && m_lastStates[i].Buttons.Start == ButtonState.Pressed && GamePad.GetState((PlayerIndex)i).Buttons.Start == ButtonState.Released) {
                    GameObject tmp = Instantiate(m_playerRev);
                    if (tmp) {
                        GameObject tmpControle = new GameObject("Controler");
                        tmpControle.transform.parent = tmp.transform;

                        ControllerControl reference = tmpControle.AddComponent<ControllerControl>();//null reference checks
                        reference.m_controlerIndex = i;

                        tmp.GetComponent<Player>().Init(tmpControle);//dirty
                        //tmp.GetComponent<Player>().Init(reference);//null reference checks
                        tmp.GetComponent<Player>().Invincible(true);//TODO: playerscript wird doppeld gesucht
                    }
                }
                m_lastStates[i] = GamePad.GetState((PlayerIndex)i);
            }
        }

        #endregion

        public void StartGame() {
            SceneManager.LoadScene(StringCollection.COLOSSEUM);
            //SceneManager.LoadScene(StringCollection.EXAMPLESZENE);
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