using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace PPBC {
    public class SaveStart : MonoBehaviour {

        private void Awake() {
            if (!DataHolder.s_isInit)
                SceneManager.LoadScene(StringCollection.S_GASS);
            else
                Destroy(this.gameObject);
        }
    }
}