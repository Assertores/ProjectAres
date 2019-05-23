﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PPBC {
    public class EndScreenAnimations : MonoBehaviour, IScriptQueueItem {

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
            float tmp;
            for (int i = 0; i < Player.s_sortedRef.Count; i++) {
                if(i == 0) {
                    tmp = Player.s_sortedRef[i].StartAnim("07_Win", 1);
                } else {
                    tmp = Player.s_sortedRef[i].StartAnim("08_Lose", 1);
                }

                if (tmp == float.MinValue) {
                    m_animationPlayTime += Time.time;
                } else {
                    m_animationPlayTime = Time.time + tmp;
                }
            }
            return false;
        }

        public bool DoTick() {
            return Time.time > m_animationPlayTime;
        }

        #endregion
    }
}