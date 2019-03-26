using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace ProjectAres {
    public class WinScreen : MonoBehaviour {
        
        public virtual void StartMainMenu() {
            SceneManager.LoadScene(StringCollection.MAINMENU);
        }
    }
}