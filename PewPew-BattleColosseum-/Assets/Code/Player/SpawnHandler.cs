using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XInputDotNetPure;

namespace PPBC {
    public class SpawnHandler : MonoBehaviour {

        #region Variables

        [Header("References")]
        [SerializeField] GameObject p_player;

        GamePadState[] m_lastStates = new GamePadState[4];

        #endregion
        #region MonoBehaviour

        void Start() {
            if (!p_player) {
                print("no Player prefab");
                Destroy(this);
                return;
            }
            if (!p_player.GetComponentInChildren<Player>()) {
                print("Player prefab has no Player script");
                Destroy(this);
                return;
            }
        }

        // Update is called once per frame
        void Update() {
            Player newPlayer = null;
            for (int i = 0; i < 4; i++) {
                if (!DataHolder.s_players[i] && m_lastStates[i].IsConnected &&
                    m_lastStates[i].Buttons.Start == ButtonState.Pressed &&
                    GamePad.GetState((PlayerIndex)i).Buttons.Start == ButtonState.Released) {

                    newPlayer = Instantiate(p_player).GetComponentInChildren<Player>();
                    newPlayer.Init(i);
                    newPlayer.CanChangeCharacter(true);
                    newPlayer.Invincable(true);
                    newPlayer.Respawn(SpawnPoint.s_references[Random.Range(0, SpawnPoint.s_references.Count)].transform.position);
                    newPlayer.InControle(true);
                }

                m_lastStates[i] = GamePad.GetState((PlayerIndex)i);
            }
        }

        #endregion
    }
}