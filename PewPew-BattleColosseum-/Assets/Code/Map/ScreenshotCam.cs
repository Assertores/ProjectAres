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

        public static Texture2D TakeScreenShot() {
            m_cam.orthographicSize = FitCameraToAABB.m_aABB.size.y / 2;
            if (m_cam.orthographicSize * m_cam.aspect < FitCameraToAABB.m_aABB.size.x / 2) {
                m_cam.orthographicSize = (FitCameraToAABB.m_aABB.size.x / 2) / m_cam.aspect;
            }

            var holder = RenderTexture.active;

            RenderTexture.active = m_texture;

            m_cam.gameObject.SetActive(true);
            m_cam.Render();
            m_cam.gameObject.SetActive(false);

            Texture2D value = new Texture2D(m_texture.width, m_texture.height);
            
            value.ReadPixels(new Rect(0, 0, value.width, value.height), 0, 0, false);
            RenderTexture.active = holder;

            return value;
        }

        public static byte[] ScreenShotRipOff() {
            m_cam.orthographicSize = FitCameraToAABB.m_aABB.size.y / 2;
            if (m_cam.orthographicSize * m_cam.aspect < FitCameraToAABB.m_aABB.size.x / 2) {
                m_cam.orthographicSize = (FitCameraToAABB.m_aABB.size.x / 2) / m_cam.aspect;
            }


            int resWidthN = m_texture.width;
            int resHeightN = m_texture.height;
            RenderTexture rt = new RenderTexture(resWidthN, resHeightN, 24);
            m_cam.targetTexture = rt;

            TextureFormat tFormat;
            if (false)
                tFormat = TextureFormat.ARGB32;
            else
                tFormat = TextureFormat.RGB24;


            Texture2D screenShot = new Texture2D(resWidthN, resHeightN, tFormat, false);
            m_cam.Render();
            RenderTexture.active = rt;
            screenShot.ReadPixels(new Rect(0, 0, resWidthN, resHeightN), 0, 0);
            m_cam.targetTexture = null;
            RenderTexture.active = null;
            byte[] bytes = screenShot.EncodeToPNG();
            /*string filename = ScreenShotName(resWidthN, resHeightN);

            System.IO.File.WriteAllBytes(filename, bytes);
            Debug.Log(string.Format("Took screenshot to: {0}", filename));
            Application.OpenURL(filename);*/

            return bytes;
        }
    }
}