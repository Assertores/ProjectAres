using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Sauerbraten = UnityEngine.MonoBehaviour;

namespace PPBC {
    public class GameModeManager : Sauerbraten {

        static bool isInit = false;

        #region Variables

        [Header("References")]
        [SerializeField] GameObject m_gMParent;
        [SerializeField] GameObject m_menuRef;

        bool m_return = false;

        #endregion
        #region MonoBehaviour
        #region Singelton

        static GameModeManager s_singelton_ = null;
        public static GameModeManager s_singelton {
            get {
                if (!s_singelton_)
                    s_singelton_ = new GameObject {
                        name = "GameModeManager"
                    }.AddComponent<GameModeManager>();
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

            if (!isInit)
                DontDestroyOnLoad(m_gMParent);
            else
                Destroy(m_gMParent);

            isInit = true;
        }

        void OnDestroy() {
            if (s_singelton_ == this)
                s_singelton_ = null;
        }

        #endregion

        void Start() {

            this.gameObject.SetActive(false);
        }

        private void Update() {
            if (DataHolder.s_gameModes.ContainsKey(DataHolder.s_gameMode) && DataHolder.s_gameModes[DataHolder.s_gameMode].ReadyToChange()) {
                SceneManager.LoadScene(StringCollection.INGAME);
            }
        }

        #endregion

        public void SetUp() {

            foreach(Transform it in transform) {
                Destroy(it.gameObject);
            }

            if(DataHolder.s_gameModes.ContainsKey(DataHolder.s_gameMode))
                DataHolder.s_gameModes[DataHolder.s_gameMode].SetMenuSpecific(this.transform);

            this.gameObject.SetActive(true);
            m_menuRef.SetActive(false);
        }

    }
}