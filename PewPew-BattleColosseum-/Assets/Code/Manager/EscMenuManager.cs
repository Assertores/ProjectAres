using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using XInputDotNetPure;

namespace PPBC {
    public class EscMenuManager : MonoBehaviour {

        GamePadState[] m_lastStates = new GamePadState[4];

        private void Awake() {
            for(int i = 0; i < 4; i++) {
                m_lastStates[i] = GamePad.GetState((PlayerIndex)i);
            }
        }

        // Update is called once per frame
        void Update() {
            if (Input.GetKeyUp(KeyCode.Escape)) {
                ToggleEscMenu();
            }
            
            for (int i = 0; i < 4; i++) {
                if (GamePad.GetState((PlayerIndex)i).IsConnected &&
                    m_lastStates[i].Buttons.Back == ButtonState.Pressed &&
                    GamePad.GetState((PlayerIndex)i).Buttons.Back == ButtonState.Released) {
                    ToggleEscMenu();
                }
                m_lastStates[i] = GamePad.GetState((PlayerIndex)i);
            }
        }

        bool h_inEscMenu = false;
        float h_timeScale = 1;
        void ToggleEscMenu() {
            if (!h_inEscMenu) {
                SceneManager.LoadScene(StringCollection.S_ESCMENU, LoadSceneMode.Additive);
                h_timeScale = Time.timeScale;
                Time.timeScale = 0;
            } else {
                SceneManager.UnloadScene(StringCollection.S_ESCMENU);
                Time.timeScale = h_timeScale;
            }
            h_inEscMenu = !h_inEscMenu;
        }
    }
}