using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace ProjectAres {
    public class Fair_Tournament : MonoBehaviour, IGameMode {

        #region Variables

        [Header("References")]
        [SerializeField] Transform m_respawnParent;

        [Header("Balancing")]
        [SerializeField] int m_maxKills = 8;
        [SerializeField] float m_respawnTime = 2.0f;

        float m_startTime;

        #endregion
        #region MonoBehaviour

        void Start() {
            Stop();
        }

        void Update() {
        }

        #endregion
        #region IGameMode

        public void Init() {
            m_startTime = Time.timeSinceLevelLoad;
            foreach (var it in Player.s_references) {
                it.Respawn(m_respawnParent.GetChild(Random.Range(0, m_respawnParent.childCount)).position);
            }
            gameObject.SetActive(true);
        }

        public void Stop() {
            gameObject.SetActive(false);
        }

        public void PlayerDied(Player player) {
            int max = 0;
            foreach(var it in Player.s_references) {
                if(max < it.m_stats.m_kills)
                    max = it.m_stats.m_kills;
            }
            if(max >= m_maxKills) {
                //disconecte die schlechtesten zwei/die besten zwei
                
                DataHolder.s_firstMatch = !DataHolder.s_firstMatch;

                SceneManager.LoadScene(StringCollection.FFACASUAL);
                return;
            }

            StartCoroutine(RespawnPlayer(player));
        }

        #endregion

        IEnumerator RespawnPlayer(Player player) {
            yield return new WaitForSeconds(m_respawnTime);

            player.Respawn(m_respawnParent.GetChild(Random.Range(0, m_respawnParent.childCount)).position);

        }
    }
}