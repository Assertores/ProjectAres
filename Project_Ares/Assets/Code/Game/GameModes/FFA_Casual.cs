using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

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
            gameObject.SetActive(true);
        }

        public void Stop() {
            gameObject.SetActive(false);
        }

        public void PlayerDied(Player player) {
            StartCoroutine(RespawnPlayer(player));
        }

        IEnumerator RespawnPlayer(Player player) {
            yield return new WaitForSeconds(_respawnTime);

            player.Respawn(_respawnParent.GetChild(Random.Range(0,_respawnParent.childCount)).position);

        }
        
        void Start() {
            Stop();
        }
        
        void Update() {
            print(Time.timeSinceLevelLoad - _startTime);
            if(_gameTime <= _startTime + Time.timeSinceLevelLoad) {
                //Player._references.Sort((lhs, rhs) => lhs._stuts.Kills - rhs._stuts.Kills);//TEST ob es in der richtigen reihenfolge ist.//pasiert im winscreen
                SceneManager.LoadScene(StringCollection.FFACASUAL);
                //auf WinScreen wächseln
            }
        }
    }
}