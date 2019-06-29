using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using UnityEngine.SceneManagement;
using Sauerbraten = UnityEngine.MonoBehaviour;

namespace PPBC {
    public class LoadSceneAfterVideo : Sauerbraten
    {
        [SerializeField] string m_sceneName;
        [SerializeField] VideoClip Clip;

        private void Awake() {
            for (int i = 0; i < Player.s_references.Count; i++) {
                Player.s_references[i].gameObject.SetActive(false);
                Player.s_references[i].InControle(false);
            }
        }
        // Start is called before the first frame update
        void Start() {
            
            
        }
        private void Update() {
            if (Clip.length < Time.timeSinceLevelLoad) {
                LoadScene(m_sceneName);
                
            }
        }
        private void OnDestroy() {
            for (int i = 0; i < Player.s_references.Count; i++) {
                Player.s_references[i].gameObject.SetActive(true);
                Player.s_references[i].InControle(true);
            }
        }


        void LoadScene(string m_sceneName) {
            SceneManager.LoadScene(m_sceneName);
        }
    }
}