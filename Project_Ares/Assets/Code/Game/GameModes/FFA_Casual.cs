using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace ProjectAres {
    public class FFA_Casual : MonoBehaviour, IGameMode {

        [Header("References")]
        [SerializeField] Transform m_respawnParent;

        [Header("Balancing")]
        [SerializeField] float m_gameTime = 120.0f;
        [SerializeField] float m_respawnTime = 2.0f;

        float m_startTime;

        public void Init() {
            m_startTime = Time.timeSinceLevelLoad;
            foreach(var it in Player.s_references) {
                it.Respawn(m_respawnParent.GetChild(Random.Range(0, m_respawnParent.childCount)).position);
            }
            gameObject.SetActive(true);
        }

        public void Stop() {
            gameObject.SetActive(false);
        }

        public void PlayerDied(Player player) {
            StartCoroutine(RespawnPlayer(player));
        }

        IEnumerator RespawnPlayer(Player player) {
            yield return new WaitForSeconds(m_respawnTime);

            player.Respawn(m_respawnParent.GetChild(Random.Range(0,m_respawnParent.childCount)).position);

        }
        
        void Start() {
            Stop();
        }
        
        void Update() {
            //print(Time.timeSinceLevelLoad - _startTime);
            if(m_gameTime <= m_startTime + Time.timeSinceLevelLoad) {
                //Player._references.Sort((lhs, rhs) => lhs._stuts.Kills - rhs._stuts.Kills);//TEST ob es in der richtigen reihenfolge ist.//pasiert im winscreen
                SceneManager.LoadScene(StringCollection.FFACASUAL);
                //auf WinScreen wächseln
            }
        }
    }
}