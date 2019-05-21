using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using UnityEngine.SceneManagement;

namespace PPBC {
    public class LoadSceneAfterVideo : MonoBehaviour
    {
        [SerializeField] string m_sceneName;
        [SerializeField] VideoClip Clip;
        // Start is called before the first frame update
        void Start() {
            
            
        }
        private void Update() {
            if (Clip.length < Time.timeSinceLevelLoad) {
                LoadScene(m_sceneName);
            }
        }

        void LoadScene(string m_sceneName) {
            SceneManager.LoadScene(m_sceneName);
        }
    }
}