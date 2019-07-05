using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PPBC {
    [RequireComponent(typeof(Camera))]
    public class ScreenshotCam : MonoBehaviour {

        public static Camera m_camera;
        public static RenderTexture m_texture;

        private void Awake() {
            m_camera = GetComponent<Camera>();
            m_texture = m_camera.targetTexture;
        }
    }
}