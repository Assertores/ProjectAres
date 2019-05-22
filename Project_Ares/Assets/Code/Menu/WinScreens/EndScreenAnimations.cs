using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PPBC {
    public class EndScreenAnimations : MonoBehaviour, IScriptQueueItem {

        #region MonoBehaviour
        // Start is called before the first frame update
        void Start() {
            EndScreenManager.s_ref?.AddItem(this, 1);
        }

        // Update is called once per frame
        void Update() {

        }

        #endregion
        #region IScriptQueueItem

        public bool FirstTick() {
            for (int i = 0; i < Player.s_sortedRef.Count; i++) {
                if(i == 0) {
                    Player.s_sortedRef[i].m_modelAnim?.animation.Play("07_Win");
                } else {
                    Player.s_sortedRef[i].m_modelAnim?.animation.Play("08_Lose");
                }
            }
            return false;
        }

        public bool DoTick() {
            throw new System.NotImplementedException();
        }

        #endregion
    }
}