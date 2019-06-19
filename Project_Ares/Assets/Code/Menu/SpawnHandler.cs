using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XInputDotNetPure;

namespace PPBC {
    public class SpawnHandler : MonoBehaviour, IScriptQueueItem {

        #region Variables

        [Header("References")]
        [SerializeField] GameObject m_playerRev;
        [SerializeField] GameObject m_SpawnPoint;

        GamePadState[] m_lastStates = new GamePadState[4];

        #endregion
        #region MonoBehaviour

        private void Start() {
            if (EndScreenManager.s_ref && DataHolder.s_gameMode == e_gameMode.FAIR_TOURNAMENT && DataHolder.s_firstMatch == false) {
                EndScreenManager.s_ref.AddItem(this, 2);
                this.enabled = false;
            } else if(EndScreenManager.s_ref){
                Destroy(this);
            }
        }

        private void Update() {
            if (!DataHolder.s_players[4] && Input.GetKeyDown(KeyCode.Return)) {
                GameObject tmp = Instantiate(m_playerRev);
                if (tmp) {
                    DataHolder.s_players[4] = true;
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
            for (int i = 0; i < 4; i++) {
                if (!DataHolder.s_players[i] && m_lastStates[i].IsConnected && m_lastStates[i].Buttons.Start == ButtonState.Pressed && GamePad.GetState((PlayerIndex)i).Buttons.Start == ButtonState.Released) {
                    GameObject tmp = Instantiate(m_playerRev);
                    if (tmp) {
                        DataHolder.s_players[i] = true;
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
        }

        #endregion
        #region IScriptQueueItem

        public bool FirstTick() {
            this.enabled = true;
            return true;
        }

        public bool DoTick() {
            return true;
        }

        #endregion
    }
}