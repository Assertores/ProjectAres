using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProjectAres {
    public class FFA_Casual : MonoBehaviour, IGameMode {

        [Header("References")]
        [SerializeField] Transform _respawnParent;

        [Header("Balancing")]
        [SerializeField] float _gameTime = 120.0f;
        [SerializeField] float _respawnTime = 2.0f;

        float _startTime;

        public void Init() {
            _startTime = Time.timeSinceLevelLoad;
        }

        public void PlayerDied(Player player) {
            StartCoroutine(RespawnPlayer(player));
        }

        IEnumerator RespawnPlayer(Player player) {
            yield return new WaitForSeconds(_respawnTime);

            player.Respawn(_respawnParent.GetChild(Random.Range(0,_respawnParent.childCount)).position);

        }
        
        void Start() {

        }
        
        void Update() {
            if(_gameTime <= _startTime + Time.timeSinceLevelLoad) {
                //auf WinScreen wächseln
            }
        }
    }
}