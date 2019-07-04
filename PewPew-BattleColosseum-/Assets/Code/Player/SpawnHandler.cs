using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PPBC {
    public class SpawnHandler : MonoBehaviour {

        #region Variables

        [Header("References")]
        [SerializeField] GameObject p_player;

        [HideInInspector] public bool m_spawnEnabled = true;

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
            if (m_spawnEnabled) {
                Player newPlayer = null;
                if (!DataHolder.s_players[4] && Input.anyKey) {
                    newPlayer = Instantiate(p_player).GetComponentInChildren<Player>();
                    newPlayer.Init(4);
                }

                if (newPlayer) {
                    newPlayer.ResetFull();
                    //change charakter true
                    newPlayer.Respawn(SpawnPoint.s_references[Random.Range(0, SpawnPoint.s_references.Count)].transform.position);
                }
            }
        }

        #endregion
    }
}