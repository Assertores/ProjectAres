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

        public static Texture2D TakeScreenShot() {
            m_camera.gameObject.SetActive(true);
            m_camera.Render();
            m_camera.gameObject.SetActive(false);

            Texture2D value = new Texture2D(m_texture.width, m_texture.height);
            var holder = RenderTexture.active;

            RenderTexture.active = m_texture;
            value.ReadPixels(new Rect(0, 0, value.width, value.height), 0, 0, false);
            RenderTexture.active = holder;

            return value;
        }
    }
}