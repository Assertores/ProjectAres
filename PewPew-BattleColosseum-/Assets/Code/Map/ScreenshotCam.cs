using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PPBC {
    [RequireComponent(typeof(Camera))]
    public class ScreenshotCam : MonoBehaviour {

        public static Camera m_cam;
        public static RenderTexture m_texture;

        private void Awake() {
            gameObject.SetActive(true);
            m_cam = GetComponent<Camera>();
            m_texture = m_cam.targetTexture;
            //gameObject.SetActive(false);
        }

        public static byte[] TakeScreenShot() {
            m_cam.orthographicSize = FitCameraToAABB.m_aABB.size.y / 2;
            if (m_cam.orthographicSize * m_cam.aspect < FitCameraToAABB.m_aABB.size.x / 2) {
                m_cam.orthographicSize = (FitCameraToAABB.m_aABB.size.x / 2) / m_cam.aspect;
            }

            var holder = RenderTexture.active;

            m_cam.gameObject.SetActive(true);
            m_cam.Render();
            m_cam.gameObject.SetActive(false);

            RenderTexture.active = m_texture;

            Texture2D value = new Texture2D(m_texture.width, m_texture.height, TextureFormat.RGB24, false);
            
            value.ReadPixels(new Rect(0, 0, value.width, value.height), 0, 0, false);
            RenderTexture.active = holder;

            return value.EncodeToPNG();
        }
    }
}