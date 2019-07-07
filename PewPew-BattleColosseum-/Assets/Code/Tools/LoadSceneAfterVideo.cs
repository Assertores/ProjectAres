using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;

namespace PPBC {
    public class LoadSceneAfterVideo : MonoBehaviour {

        [Header("References")]
        [SerializeField] VideoPlayer r_player;

        [Header("Balancing")]
        [SerializeField] string m_nextScene;

        float m_length;

        private void Start() {
            if (!r_player) {
                SceneManager.LoadScene(m_nextScene);
            }

            r_player.targetCamera = Camera.main;
            m_length = (float)r_player.clip.length;
        }

        private void Update() {
            if (Time.timeSinceLevelLoad > m_length)
                SceneManager.LoadScene(m_nextScene);
        }
    }
}