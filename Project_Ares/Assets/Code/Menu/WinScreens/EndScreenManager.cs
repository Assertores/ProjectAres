using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PPBC {
    public class EndScreenManager : MonoBehaviour {

        public static EndScreenManager s_ref = null;

        #region Variables

        ScriptQueueManager queue = new ScriptQueueManager();

        #endregion
        #region MonoBehaviour

        private void Awake() {
            if(s_ref != null && s_ref != this) {
                Destroy(this);
                return;
            }

            s_ref = this;
        }

        private void OnDestroy() {
            if(s_ref == this) {
                s_ref = null;
            }
        }

        private void Start() {
            for (int i = 0; i < Player.s_references.Count; i++) {
                Player.s_references[i].Invincible(true);

                Player.s_references[i].DoReset();
                    
                Player.s_references[i].InControle(false);
            }
        }

        void Update() {
            queue.Tick();
        }

        #endregion

        public void AddItem(IScriptQueueItem item, int sortingLayer) {
            queue.AddItemToQueue(item, sortingLayer);
        }
    }
}