using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PPBC {
    public class Exiter : MonoBehaviour {

        public void DisconnectPlayer() {
            if(Player.s_references.Count <= 1) {
                Application.Quit();
                return;
            }

            TriggerButton.s_hoPlayer?.m_controler.DoDisconnect();
        }
    }
}