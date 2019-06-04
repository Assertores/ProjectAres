using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace PPBC {
    public class MenuManager : MonoBehaviour {

        #region Variables

        ScriptQueueManager m_sqm = new ScriptQueueManager();

        #endregion
        #region MonoBehaviour
        #region Singelton

        static MenuManager s_singelton_ = null;
        public static MenuManager s_singelton {
            get {
                if (!s_singelton_)
                    s_singelton_ = new GameObject {
                        name = "MenuManager"
                    }.AddComponent<MenuManager>();
                return s_singelton_;
            }
        }

        void Awake() {
            if (s_singelton_ == null) {
                s_singelton_ = this;
            } else if (s_singelton_ != this) {
                Destroy(gameObject);
                return;
            }
        }

        void OnDestroy() {
            if (s_singelton_ == this)
                s_singelton_ = null;
        }

        #endregion

        void Start() {
        }

        // Update is called once per frame
        void Update() {
            if (m_sqm.Tick()) {
                SceneManager.LoadScene(StringCollection.INGAME);
            }
        }

        #endregion

        public void AddItem(IScriptQueueItem item, int sortingLayer) {
            m_sqm.AddItemToQueue(item, sortingLayer);
        }
    }
}