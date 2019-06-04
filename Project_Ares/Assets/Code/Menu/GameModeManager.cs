using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PPBC {
    public class GameModeManager : MonoBehaviour, IScriptQueueItem {

        static bool isInit = false;

        #region Variables

        [Header("References")]
        [SerializeField] GameObject m_gMParent;

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
            MenuManager.s_singelton.AddItem(this, 4);

            this.gameObject.SetActive(false);
        }

        #endregion
        #region IScriptQueueItem

        public bool FirstTick() {
            DataHolder.s_gameModes[DataHolder.s_gameMode].SetMenuSpecific(this.transform);

            this.gameObject.SetActive(true);

            return DoTick();
        }

        public bool DoTick() {
            return DataHolder.s_gameModes[DataHolder.s_gameMode].ReadyToChange();
        }

        #endregion
    }
}