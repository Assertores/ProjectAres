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

        bool[] m_existingPlayers = new bool[5];

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
                it.DoReset();
                it.Invincible(true);
                it.transform.position = m_SpawnPoint.transform.position;
                it.SetChangeCharAble(true);
            }
            for (int i = 0; i < m_existingPlayers.Length; i++) {
                m_existingPlayers[i] = false;
            }
        }

        private void Update() {
            if (!m_existingPlayers[4] && Input.GetKeyDown(KeyCode.Return)) {
                GameObject tmp = Instantiate(m_playerRev);
                if (tmp) {
                    m_existingPlayers[4] = true;
                    GameObject tmpControle = new GameObject("Controler");
                    //tmpControle.transform.parent = tmp.transform;

                    IControl reference = tmpControle.AddComponent<KeyboardControl>();//null reference checks

                    //tmp.GetComponent<Player>().Init(KeyboardControl);

                    tmp.transform.position = m_SpawnPoint.transform.position;

                    tmp.GetComponentInChildren<Player>().Init(tmpControle);//dirty
                    //tmp.GetComponentInChildren<Player>().Init(reference);//null reference checks
                    tmp.GetComponentInChildren<Player>().Invincible(true);//TODO: playerscript wird doppeld gesucht
                    tmp.GetComponentInChildren<Player>().SetChangeCharAble(true);
                }
            }
            for(int i = 0; i < 4; i++) {
                if(!m_existingPlayers[i] && m_lastStates[i].IsConnected && m_lastStates[i].Buttons.Start == ButtonState.Pressed && GamePad.GetState((PlayerIndex)i).Buttons.Start == ButtonState.Released) {
                    GameObject tmp = Instantiate(m_playerRev);
                    if (tmp) {
                        m_existingPlayers[i] = true;
                        GameObject tmpControle = new GameObject("Controler");
                        //tmpControle.transform.parent = tmp.transform;

                        ControllerControl reference = tmpControle.AddComponent<ControllerControl>();//null reference checks
                        reference.m_controlerIndex = i;

                        tmp.transform.position = m_SpawnPoint.transform.position;

                        tmp.GetComponentInChildren<Player>().Init(tmpControle);//dirty
                        //tmp.GetComponent<Player>().Init(reference);//null reference checks
                        tmp.GetComponentInChildren<Player>().Invincible(true);//TODO: playerscript wird doppeld gesucht
                        tmp.GetComponentInChildren<Player>().SetChangeCharAble(true);

                    }
                }
                m_lastStates[i] = GamePad.GetState((PlayerIndex)i);
            }

            if (Input.GetKeyDown(KeyCode.O))
            {
                GameObject tmp = Instantiate(m_playerRev);
                if (tmp) {
                    GameObject tmpController = new GameObject("KI Controller");
                    //tmpControle.transform.parent = tmp.transform;

                    IControl reference = tmpController.AddComponent<KI_Minion>();

                    tmp.transform.position = m_SpawnPoint.transform.position;

                    Player minion = tmp.GetComponentInChildren<Player>();
                    minion.Init(tmpController);
                    minion.Invincible(true);
                    minion.SetChangeCharAble(true);
                }
            }
        }

        #endregion

        public void StartGame() {

            foreach(var it in Player.s_references) {
                it.SetChangeCharAble(false);
            }

            SceneManager.LoadScene(StringCollection.COLOSSEUM);
            //SceneManager.LoadScene(StringCollection.EXAMPLESZENE);
            //lade ausgewählte Szene im hintergrund
            //spiel animation für szenenwechsel ab
            //überblende die musik
            //unload Menuszene
        }

        public void Exit() {
            print("Quitting the Game");
            Application.Quit();
        }
    }
}