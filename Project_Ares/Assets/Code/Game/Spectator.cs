using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProjectAres {
    public class Spectator : MonoBehaviour {

        //static Spectator _singelton = null;

        [Header("Balancing")]
        [SerializeField] float _minCameraSize = 15;
        [SerializeField] float _padding = 10;

        Camera _myCamera = null;

        void Awake() {
            //if (_singelton) {
            //    Destroy(this);
            //    return;
            //}
            //_singelton = this;
            
            _myCamera = CameraControler._singelton?.AddCamera();
        }
        private void OnDestroy() {
            //if (_singelton == this)
            //    _singelton = null;
            CameraControler._singelton?.RemoveCamera(_myCamera);
        }

        // Update is called once per frame
        void Update() {
            if (Player._references.Count == 0)
                return;

            float minX = Player._references[0].transform.position.x;
            float maxX = minX;
            float minY = Player._references[0].transform.position.y;
            float maxY = minY;

            foreach (var it in Player._references) {
                minX = Mathf.Min(minX, it.transform.position.x);
                maxX = Mathf.Max(maxX, it.transform.position.x);
                minY = Mathf.Min(minY, it.transform.position.y);
                maxY = Mathf.Max(maxY, it.transform.position.y);
            }

            _myCamera.orthographicSize = Mathf.Max(Mathf.Max((maxX - minX + 2* _padding) / _myCamera.aspect, maxY - minY + 2 * _padding) / 2, _minCameraSize);

            _myCamera.transform.position = new Vector3(minX + (maxX-minX)/2, minY + (maxY-minY)/2, _myCamera.transform.position.z);
        }
    }
}