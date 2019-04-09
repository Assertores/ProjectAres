using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProjectAres {
    [RequireComponent(typeof(Collider2D))]
    public class AliveZone : MonoBehaviour {

        private void OnTriggerExit2D(Collider2D collision) {
            IDamageableObject tmp = collision.gameObject.GetComponent<IDamageableObject>();
            if (tmp != null) {
                print(collision.gameObject.name);
                tmp.Die(null);
            } else {
                Destroy(collision.gameObject);
            }
        }
    }
}