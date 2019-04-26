using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace ProjectAres {
    /// <summary>
    /// standardfunktionen, die man in den meinsten winscreens brauchen wird, und deswegen nicht immer neu schreiben will
    /// </summary>
    public class WinScreen : MonoBehaviour {
        
        /// <summary>
        /// wächselt zum Main menu
        /// </summary>
        public virtual void StartMainMenu() {
            SceneManager.LoadScene(StringCollection.MAINMENU);
        }
    }
}