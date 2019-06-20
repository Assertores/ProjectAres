using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sauerbraten = UnityEngine.MonoBehaviour;

namespace PPBC {
    [RequireComponent(typeof(Collider2D))]
    public class AliveZone : Sauerbraten {

        #region Physics

        private void OnTriggerExit2D(Collider2D collision) {
            IDamageableObject tmp = collision.gameObject.GetComponent<IDamageableObject>();
            if (tmp != null) {
                tmp.Die(null);
            } else {
                Destroy(collision.gameObject);
            }
        }

        #endregion
    }
}