using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using XInputDotNetPure;

namespace PPBC {
    public class ScipToMM : MonoBehaviour {


        GamePadState[] m_lastStates = new GamePadState[4];

        private void Awake() {
            for (int i = 0; i < 4; i++) {
                m_lastStates[i] = GamePad.GetState((PlayerIndex)i);
            }
        }

        // Update is called once per frame
        void Update() {
            if (Input.GetKeyUp(KeyCode.Space)) {
                Scip();
            }

            for (int i = 0; i < 4; i++) {
                if (GamePad.GetState((PlayerIndex)i).IsConnected &&
                    m_lastStates[i].Buttons.B == ButtonState.Pressed &&
                    GamePad.GetState((PlayerIndex)i).Buttons.B == ButtonState.Released) {
                    Scip();
                }
                m_lastStates[i] = GamePad.GetState((PlayerIndex)i);
            }
        }

        void Scip() {
            if (!DataHolder.s_isInit)
                return;

            SceneManager.LoadScene(StringCollection.S_MAINMENU);
        }
    }
}