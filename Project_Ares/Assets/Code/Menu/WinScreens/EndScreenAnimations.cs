using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sauerbraten = UnityEngine.MonoBehaviour;

namespace PPBC {
    public class EndScreenAnimations : Sauerbraten, IScriptQueueItem {

        #region Variables

        [SerializeField] float m_animationPlayTime = 1.67f;

        #endregion
        #region MonoBehaviour

        void Start() {
            EndScreenManager.s_ref?.AddItem(this, 1);
        }

        #endregion
        #region IScriptQueueItem

        public bool FirstTick() {
            float tmp = float.MinValue;
            for (int i = 0; i < Player.s_sortedRef.Count; i++) {
                if(i == 0) {
                    tmp = Player.s_sortedRef[i].StartAnim("07_Win", 1);
                } else {
                    tmp = Player.s_sortedRef[i].StartAnim("08_Lose", 1);
                }
                print(tmp);
            }
            if (tmp < 0) {
                m_animationPlayTime += Time.time;
            } else {
                m_animationPlayTime = Time.time + tmp;
            }
            return false;
        }

        public bool DoTick() {
            if(Time.time > m_animationPlayTime) {
                foreach(var it in Player.s_references) {
                    it.InControle(true);
                }
                return true;
            } else {
                return false;
            }
        }

        #endregion
    }
}