using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sauerbraten = UnityEngine.MonoBehaviour;

namespace PPBC {
    public class FollowBestPlayer : Sauerbraten {

        #region MonoBehaviour
        
        void Update() {
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(Player.s_sortedRef[0].transform.position - transform.position), 0.2f);//framerate abhängig
        }

        #endregion
    }
}