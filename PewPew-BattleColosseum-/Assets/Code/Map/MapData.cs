using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

namespace PPBC {
    public enum e_objType { BACKGROUND = 0, STAGE, PROP, LASERSPAWN, SPAWNPOINT, FLAG, BASKETHOOP, FORGROUND, LIGHT, GLOBALLIGHT, MUSIC, SIZE, ENUMLENGTH }

    [System.Serializable]
    public struct d_mapData {
        public e_objType type;
        public int index;
        public Vector2 position;
        public float rotation;
        public Vector2 scale;
    }

    [CreateAssetMenu(menuName = "Map/Map")]
    public class MapData : ScriptableObject {

        public void SaveToJSON() {
            Directory.CreateDirectory(StringCollection.P_MAPPARH + this.m_name);

            MapJSON value = new MapJSON();

            List<string> StringList = new List<string>();

            value.p_sizes = this.p_sizes;

            List<BackgroundJSON> BGJSONs = new List<BackgroundJSON>();
            foreach (var it in this.p_backgrounds) {
                BackgroundJSON element = new BackgroundJSON();
                element.m_image = it.m_image.name + ".png";
                byte[] png = it.m_image.texture.EncodeToPNG();
                File.WriteAllBytes(StringCollection.P_MAPPARH + this.name + "/" + element.m_image, png);

                element.m_position = it.m_position;
                element.m_size = it.m_size;

                BGJSONs.Add(element);
            }
            value.p_backgrounds = BGJSONs.ToArray();

            List<Vector3> COLORs = new List<Vector3>();
            foreach(var it in this.p_colors) {
                COLORs.Add(new Vector3(it.r, it.g, it.b));
            }
            value.p_colors = COLORs.ToArray();

            StringList.Clear();
            foreach(var it in this.p_musics) {
                StringList.Add(it.name + ".wav");

                float[] wav = new float[it.samples * it.channels];
                it.GetData(wav, 0);//https://docs.unity3d.com/ScriptReference/AudioClip.GetData.html
                byte[] bwav = new byte[wav.Length * 4];
                System.Buffer.BlockCopy(wav,0,bwav,0,bwav.Length);
                File.WriteAllBytes(StringCollection.P_MAPPARH + this.name + "/" + it.name + ".wav", bwav);
            }
            value.p_musics = StringList.ToArray();

            StringList.Clear();
            foreach(var it in this.p_stages) {
                StringList.Add(it.name + ".png");
                byte[] png = it.texture.EncodeToPNG();
                File.WriteAllBytes(StringCollection.P_MAPPARH + this.name + "/" + it.name + ".png", png);
            }
            value.p_stages = StringList.ToArray();

            List<PropJSON> PJSONs = new List<PropJSON>();
            foreach(var it in this.p_props) {
                PropJSON element = new PropJSON();
                element.m_image = it.m_image.name + ".png";
                byte[] png = it.m_image.texture.EncodeToPNG();
                File.WriteAllBytes(StringCollection.P_MAPPARH + this.name + "/" + element.m_image, png);

                element.m_collider = it.m_collider;

                PJSONs.Add(element);
            }
            value.p_props = PJSONs.ToArray();

            StringList.Clear();
            foreach(var it in this.p_forgrounds) {
                StringList.Add(it.name + ".png");
                byte[] png = it.texture.EncodeToPNG();
                File.WriteAllBytes(StringCollection.P_MAPPARH + this.name + "/" + it.name + ".png", png);
            }
            value.p_forgrounds = StringList.ToArray();

            value.m_ballSpawn = this.m_ballSpawn;

            value.m_icon = "icon.png";
            ScreenshotCam.m_camera.gameObject.SetActive(true);
            ScreenshotCam.m_camera.Render();
            ScreenshotCam.m_camera.gameObject.SetActive(false);
            Texture2D tmp = new Texture2D(ScreenshotCam.m_texture.width, ScreenshotCam.m_texture.height);
            var holder = RenderTexture.active;
            RenderTexture.active = ScreenshotCam.m_texture;
            tmp.ReadPixels(new Rect(0, 0, tmp.width, tmp.height), 0, 0, false);
            RenderTexture.active = holder;
            byte[] pngShot = tmp.EncodeToPNG();
            File.WriteAllBytes(StringCollection.P_MAPPARH + this.name + "/" + "icon.png", pngShot);

            value.m_name = this.m_name;

            value.m_size = this.m_size;
            value.m_background = this.m_background;
            value.m_globalLight = this.m_globalLight;
            value.m_music = this.m_music;

            value.m_data = this.m_data;

            StreamWriter saveFile = File.CreateText(StringCollection.P_MAPPARH + this.name + "/" + this.name + ".map");
            saveFile.Write(JsonUtility.ToJson(value));
            saveFile.Close();
        }

        public void Load() {
            MapJSON value = JsonUtility.FromJson<MapJSON>(StringCollection.P_MAPPARH + this.name + "/" + this.name + ".map");
            //size, background, light, music

            //foreach (prop)
        }
        public void EditLoad() {
            MapJSON value = JsonUtility.FromJson<MapJSON>(StringCollection.P_MAPPARH + this.name + "/" + this.name + ".map");

            //load everything
        }

        public MapData() {
        }
        public MapData(MapData original) {
            original.p_sizes.CopyTo(this.p_sizes, 0);
            original.p_backgrounds.CopyTo(this.p_backgrounds, 0);
            original.p_colors.CopyTo(this.p_colors, 0);
            original.p_musics.CopyTo(this.p_musics, 0);
            
            original.p_stages.CopyTo(this.p_stages, 0);
            original.p_props.CopyTo(this.p_stages, 0);
            original.p_forgrounds.CopyTo(this.p_forgrounds, 0);

            this.m_ballSpawn = original.m_ballSpawn;
            this.m_icon = original.m_icon;
            this.m_name = original.m_name;

            this.m_size = original.m_size;
            this.m_background = original.m_background;
            this.m_globalLight = original.m_globalLight;
            this.m_music = original.m_music;

            original.m_data.CopyTo(this.m_data, 0);
        }
        public MapData(string name) {
            this.name = name;

            MapJSON value = JsonUtility.FromJson<MapJSON>(StringCollection.P_MAPPARH + this.name + "/" + this.name + ".map");

            this.m_icon = LoadSprite(StringCollection.P_MAPPARH + this.name, value.m_icon);
            this.m_name = value.m_name;
        }

        static Sprite LoadSprite(string path, string name, int PPU = 512) {
            WWW tmp = new WWW("file:///" + path + name);
            while (!tmp.isDone) ;
            if (tmp == null) {
                return null;
            }
            if (tmp.error != null) {
                Debug.Log(tmp.error);
                return null;
            }
            Texture2D tmpTex = tmp.texture;
            if (!tmpTex)
                return null;
            Sprite value = Sprite.Create(tmpTex, new Rect(0, 0, tmpTex.width, tmpTex.height), new Vector2(tmpTex.width / 2, tmpTex.height / 2), PPU);
            if (!value)
                return null;
            value.name = name;
            return value;
        }

        //===== ===== DATA ===== =====

        public Vector2[] p_sizes;
        public BackgroundData[] p_backgrounds;
        public Color[] p_colors;
        public AudioClip[] p_musics;

        public Sprite[] p_stages;
        public PropData[] p_props;
        public Sprite[] p_forgrounds;

        public Vector2 m_ballSpawn;
        public Sprite m_icon;
        public string m_name;

        public int m_size;
        public int m_background;
        public int m_globalLight;
        public int m_music;

        public d_mapData[] m_data;
    }

    [System.Serializable]
    public class MapJSON {

        public Vector2[] p_sizes;
        public BackgroundJSON[] p_backgrounds;
        public Vector3[] p_colors;
        public string[] p_musics;

        public string[] p_stages;
        public PropJSON[] p_props;
        public string[] p_forgrounds;

        public Vector2 m_ballSpawn;
        public string m_icon;
        public string m_name;

        public int m_size;
        public int m_background;
        public int m_globalLight;
        public int m_music;

        public d_mapData[] m_data;
    }
}