using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PPBC {
    public class EndScreenManager : MonoBehaviour {

        #region Variables

        [Header("References")]
        [SerializeField] PillarHandler pillarHandler;

        #endregion
        #region MonoBehaviour

        void Start() {
            switch (DataHolder.s_gameMode) {
            case e_gameMode.FFA_CASUAL:
                break;
            case e_gameMode.FAIR_TOURNAMENT:
                break;
            default:
                break;
            }

            if (pillarHandler) {
                pillarHandler.CallBack += NextOfPillarHandler;
            }
        }
        
        void Update() {

        }

        #endregion

        void NextOfPillarHandler() {
            switch (DataHolder.s_gameMode) {
            case e_gameMode.FFA_CASUAL:
                break;
            case e_gameMode.FAIR_TOURNAMENT:
                break;
            default:
                break;
            }
        }
    }
}