using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using System.IO;
using System.Xml.Serialization;
using System;

namespace PPBC {
    public enum e_objType { BACKGROUND, PROP, STAGE, PLAYERSTART, LIGHT, FORGROUND, BORDER, GLOBALLIGHT, MUSIC, SIZE }

    [System.Serializable]
    public struct d_mapData {
        public e_objType type;
        public int index;
        public Vector2 position;
        public float rotation;
        public Vector2 scale;

        public override string ToString() {
            return type.ToString() + ";" + index + ";" + position.x + ";" + position.y + ";" + rotation + ";" + scale.x + ";" + scale.y;
        }
    }

    [System.Serializable]
    public struct d_prop {
        public Sprite m_sprite;
        public Vector2[] m_collider;
        public override string ToString() {
            string value;
            value = m_sprite.name + ".png";
            return value;
        }
    }

    [CreateAssetMenu(menuName = "Map")]
    public class MapDATA : ScriptableObject {
        
        //https://stackoverflow.com/questions/4266875/how-to-quickly-save-load-class-instance-to-file
        public static MapDATA LoadMap(string path) {
            //*
            Debug.Log("loading " + path);
            TextReader reader = null;
            MapDATA tmp = new MapDATA();
            try {
                var serializer = new XmlSerializer(typeof(MapDATA));
                reader = new StreamReader(StringCollection.MAPPARH + path);
                tmp = (MapDATA)serializer.Deserialize(reader);
            } catch (System.Exception e) {
                Debug.Log(e);
            }
            if (reader != null)
                reader.Close();
            if (tmp) {
                if (tmp.p_background == null)
                    tmp.p_background = new Sprite[0];
                if (tmp.p_colors == null)
                    tmp.p_colors = new Color[0];
                if (tmp.p_forground == null)
                    tmp.p_forground = new Sprite[0];
                if (tmp.p_music == null)
                    tmp.p_music = new AudioClip[0];
                if (tmp.p_props == null)
                    tmp.p_props = new d_prop[0];
                if (tmp.p_size == null)
                    tmp.p_size = new Vector2[0];
                if (tmp.p_stage == null)
                    tmp.p_stage = new Sprite[0];
            }
            return tmp;
            /*/
            TextReader saveFile = new StreamReader(StringCollection.MAPPARH + path);
            string file = "";
            file = saveFile.ReadLine();
            saveFile.Close();

            MapDATA value = new MapDATA();

            List<Vector2> sizes = new List<Vector2>();
            List<Sprite> backgrounds = new List<Sprite>();
            List<Color> colors = new List<Color>();
            List<AudioClip> musics = new List<AudioClip>();
            List<d_prop> props = new List<d_prop>();
            List<Sprite> stages = new List<Sprite>();
            List<Sprite> forgrounds = new List<Sprite>();
            List<d_mapData> datas = new List<d_mapData>();

            Texture2D tmpTex;
            Sprite tmpSprite;
            AudioClip tmpMusic;
            Vector2 tmpV2;
            Color tmpColor;
            d_prop tmpProp;
            tmpProp.m_sprite = null;
            tmpProp.m_collider = new Vector2[0];
            d_mapData tmpMData;
            string[] tmpStgArr;
            int propLine = 0;

            int readPos = 0;//1 = Resource, 2 = Data
            e_objType readType = e_objType.BORDER;//Border = non

            string[] lines = file.Split(Environment.NewLine.ToCharArray()[0]);
            for (int i = 0; i < lines.Length; i++) {
                if (lines[i] == "#Resources") {
                    readPos = 1;
                    readType = e_objType.BORDER;
                    continue;
                } else if (lines[i] == "#Data") {
                    readPos = 2;
                    readType = e_objType.BORDER;
                    continue;
                }

                if (readPos == 0)
                    continue;

                if(lines[i] == "[SIZE]") {
                    readType = e_objType.SIZE;
                    continue;
                }else if (lines[i] == "[BACKGROUND]") {
                    readType = e_objType.BACKGROUND;
                    continue;
                }else if (lines[i] == "[COLOR]") {
                    readType = e_objType.LIGHT;//light = color
                    continue;
                } else if (lines[i] == "[MUSIC]") {
                    readType = e_objType.MUSIC;
                    continue;
                } else if (lines[i] == "[PROPS]") {
                    readType = e_objType.PROP;
                    continue;
                } else if (lines[i] == "[STAGE]") {
                    readType = e_objType.STAGE;
                    continue;
                } else if (lines[i] == "[FORGROUND]") {
                    readType = e_objType.FORGROUND;
                    continue;
                } else if (lines[i] == "[GLOBALLIGHT]") {
                    readType = e_objType.GLOBALLIGHT;
                    continue;
                } else if (lines[i] == "[DATA]") {
                    readType = e_objType.PLAYERSTART;//Playerstart = data
                    continue;
                }

                if (readType == e_objType.BORDER)
                    continue;

                if (readPos == 1) {
                    switch (readType) {
                    case e_objType.BACKGROUND:
                        tmpTex = LoadTexture(StringCollection.MAPPARH + lines[i]);
                        if (tmpTex) {
                            tmpSprite = Sprite.Create(tmpTex, new Rect(0, 0, tmpTex.width, tmpTex.height), new Vector2(0, 0), 512);
                            if (tmpSprite) {
                                backgrounds.Add(tmpSprite);
                                tmpSprite = null;
                            }
                        }
                        break;
                    case e_objType.PROP://sehr sehr unsave muss anders gelöst werden
                        if (propLine == 0) {
                            tmpTex = LoadTexture(StringCollection.MAPPARH + lines[i]);
                            if (tmpTex) {
                                tmpProp.m_sprite = Sprite.Create(tmpTex, new Rect(0, 0, tmpTex.width, tmpTex.height), new Vector2(0, 0), 512);
                                if (!tmpProp.m_sprite) {
                                    //irgendwass machen um den rest zu überspringen
                                }
                                tmpProp.m_collider = new Vector2[0];
                            }
                            propLine = -1;
                        }else if(propLine == -1) {
                            if(!int.TryParse(lines[i], out propLine)) {
                                //irgendwass machen um den rest zu überspringen
                            }
                            tmpProp.m_collider = new Vector2[propLine];
                        } else {
                            if (tmpProp.m_collider.Length < propLine)
                                continue;
                            tmpStgArr = lines[i].Split(';');
                            if (tmpStgArr.Length < 2)
                                continue;
                            if (!float.TryParse(tmpStgArr[0], out tmpV2.x))
                                continue;
                            if (!float.TryParse(tmpStgArr[1], out tmpV2.y))
                                continue;

                            tmpProp.m_collider[tmpProp.m_collider.Length - propLine] = tmpV2;
                            propLine--;
                            if(propLine == 0) {
                                props.Add(tmpProp);
                            }
                        }
                        break;
                    case e_objType.STAGE:
                        tmpTex = LoadTexture(StringCollection.MAPPARH + lines[i]);
                        if (tmpTex) {
                            tmpSprite = Sprite.Create(tmpTex, new Rect(0, 0, tmpTex.width, tmpTex.height), new Vector2(0, 0), 512);
                            if (tmpSprite) {
                                stages.Add(tmpSprite);
                                tmpSprite = null;
                            }
                        }
                        break;
                    case e_objType.LIGHT://color
                        tmpStgArr = lines[i].Split(';');
                        if(tmpStgArr.Length < 3)
                            continue;
                        if (!float.TryParse(tmpStgArr[0], out tmpColor.r))
                            continue;
                        if (!float.TryParse(tmpStgArr[1], out tmpColor.g))
                            continue;
                        if (!float.TryParse(tmpStgArr[2], out tmpColor.b))
                            continue;
                        tmpColor.a = 1;

                        colors.Add(tmpColor);
                        break;
                    case e_objType.FORGROUND:
                        tmpTex = LoadTexture(StringCollection.MAPPARH + lines[i]);
                        if (tmpTex) {
                            tmpSprite = Sprite.Create(tmpTex, new Rect(0, 0, tmpTex.width, tmpTex.height), new Vector2(0, 0), 512);
                            if (tmpSprite) {
                                forgrounds.Add(tmpSprite);
                                tmpSprite = null;
                            }
                        }
                        break;
                    case e_objType.MUSIC:
                        break;
                    case e_objType.SIZE:
                        tmpStgArr = lines[i].Split(';');
                        if (tmpStgArr.Length < 2)
                            continue;
                        if (!float.TryParse(tmpStgArr[0], out tmpV2.x))
                            continue;
                        if (!float.TryParse(tmpStgArr[1], out tmpV2.y))
                            continue;

                        sizes.Add(tmpV2);
                        break;
                    default:
                        break;
                    }
                } else {
                    switch (readType) {
                    case e_objType.BACKGROUND:
                        if (!int.TryParse(lines[i], out value.m_background)) {
                            value.m_background = 0;
                        }
                        readType = e_objType.BORDER;
                        break;
                    case e_objType.PLAYERSTART://data
                        tmpStgArr = lines[i].Split(';');
                        if (tmpStgArr.Length < 7)
                            continue;
                        if (!Enum.TryParse<e_objType>(tmpStgArr[0], out tmpMData.type))
                            continue;
                        if (!int.TryParse(tmpStgArr[1], out tmpMData.index))
                            continue;
                        if (!float.TryParse(tmpStgArr[2], out tmpMData.position.x))
                            continue;
                        if (!float.TryParse(tmpStgArr[3], out tmpMData.position.y))
                            continue;
                        if (!float.TryParse(tmpStgArr[4], out tmpMData.rotation))
                            continue;
                        if (!float.TryParse(tmpStgArr[5], out tmpMData.scale.x))
                            continue;
                        if (!float.TryParse(tmpStgArr[6], out tmpMData.scale.y))
                            continue;

                        datas.Add(tmpMData);

                        break;
                    case e_objType.GLOBALLIGHT:
                        if (!int.TryParse(lines[i], out value.m_globalLight)) {
                            value.m_globalLight = 0;
                        }
                        readType = e_objType.BORDER;
                        break;
                    case e_objType.MUSIC:
                        if (!int.TryParse(lines[i], out value.m_music)) {
                            value.m_music = 0;
                        }
                        readType = e_objType.BORDER;
                        break;
                    case e_objType.SIZE:
                        if(!int.TryParse(lines[i], out value.m_size)) {
                            value.m_size = 0;
                        }
                        readType = e_objType.BORDER;
                        break;
                    default:
                        break;
                    }
                }
            }
            value.p_background = backgrounds.ToArray();
            value.p_colors = colors.ToArray();
            value.p_forground = forgrounds.ToArray();
            value.p_music = musics.ToArray();
            value.p_props = props.ToArray();
            value.p_size = sizes.ToArray();
            value.p_stage = stages.ToArray();
            value.m_data = datas.ToArray();

            return value;
            //*/
        }

        public static Texture2D LoadTexture(string FilePath) {

            // Load a PNG or JPG file from disk to a Texture2D
            // Returns null if load fails

            Texture2D Tex2D;
            byte[] FileData;

            if (File.Exists(FilePath)) {
                FileData = File.ReadAllBytes(FilePath);
                Tex2D = new Texture2D(2, 2);           // Create new "empty" texture
                if (Tex2D.LoadImage(FileData))           // Load the imagedata into the texture (size is set automatically)
                    return Tex2D;                 // If data = readable -> return texture
            }
            return null;                     // Return null if load failed
        }

        public static void SaveMap(MapDATA data) {
            //*
            Directory.CreateDirectory(StringCollection.MAPPARH);
            TextWriter writer = null;
            XmlSerializer serializer = null;
            try {
                serializer = new XmlSerializer(typeof(MapDATA));
                writer = new StreamWriter(StringCollection.MAPPARH + data.name, false);
                serializer.Serialize(writer, data);
            } catch (System.Exception e) {
                Debug.Log(e);
                return;
            } finally {
                if (writer != null)
                    writer.Close();
            }
            /*/
            string value = "";
            value += "#Resources" + Environment.NewLine;

            value += "[SIZE]" + Environment.NewLine;
            for(int i = 0; i < data.p_size.Length; i++) {
                value += data.p_size[i].x + ";" + data.p_size[i].y + Environment.NewLine;
            }
            value += "[BACKGROUND]" + Environment.NewLine;
            for (int i = 0; i < data.p_background.Length; i++) {
                value += data.p_background[i].name + ".png" + Environment.NewLine;
            }
            value += "[COLOR]" + Environment.NewLine;
            for (int i = 0; i < data.p_colors.Length; i++) {
                value += data.p_colors[i].r + ";" + data.p_colors[i].g + ";" + data.p_colors[i].b + Environment.NewLine;
            }
            value += "[MUSIC]" + Environment.NewLine;
            for (int i = 0; i < data.p_music.Length; i++) {
                value += data.p_music[i].name + ".ogg" + Environment.NewLine;
            }
            value += "[PROPS]" + Environment.NewLine;
            for (int i = 0; i < data.p_props.Length; i++) {
                value += data.p_props[i].m_sprite.name + ".png" + Environment.NewLine;
                value += data.p_props[i].m_collider.Length + Environment.NewLine;
                for(int j = 0; j < data.p_props[i].m_collider.Length; j++) {
                    value += data.p_props[i].m_collider[j].x + ";" + data.p_props[i].m_collider[j].y + Environment.NewLine;
                }
            }
            value += "[STAGE]" + Environment.NewLine;
            for (int i = 0; i < data.p_stage.Length; i++) {
                value += data.p_stage[i].name + ".png" + Environment.NewLine;
            }
            value += "[FORGROUND]" + Environment.NewLine;
            for (int i = 0; i < data.p_stage.Length; i++) {
                value += data.p_stage[i].name + ".png" + Environment.NewLine;
            }

            value += "#Data" + Environment.NewLine;

            value += "[SIZE]" + Environment.NewLine + data.m_size + Environment.NewLine;
            value += "[BACKGROUND]" + Environment.NewLine + data.m_background + Environment.NewLine;
            value += "[GLOBALLIGHT]" + Environment.NewLine + data.m_globalLight + Environment.NewLine;
            value += "[MUSIC]" + Environment.NewLine + data.m_music + Environment.NewLine;
            value += "[DATA]";
            for (int i = 0; i < data.m_data.Length; i++) {
                value += Environment.NewLine + data.m_data[i].ToString();
            }

            StreamWriter saveFile = File.CreateText(StringCollection.MAPPARH + data.name);
            saveFile.Write(value);
            saveFile.Close();
            //*/
        }

        public MapDATA Copy() {
            MapDATA value = new MapDATA();

            value.hideFlags = this.hideFlags;
            value.name = this.name;

            value.m_background = this.m_background;
            value.m_data = new d_mapData[this.m_data.Length];
            for(int i = 0; i < this.m_data.Length; i++) {
                value.m_data[i] = this.m_data[i];
            }
            value.m_globalLight = this.m_globalLight;
            value.m_music = this.m_music;
            value.m_size = this.m_size;

            value.p_background = new Sprite[this.p_background.Length];
            for(int i = 0; i < this.p_background.Length; i++) {
                value.p_background[i] = this.p_background[i];
            }
            value.p_colors = new Color[this.p_colors.Length];
            for(int i = 0; i < this.p_colors.Length; i++) {
                value.p_colors[i] = this.p_colors[i];
            }
            value.p_forground = new Sprite[this.p_forground.Length];
            for(int i = 0; i < this.p_forground.Length; i++) {
                value.p_forground[i] = this.p_forground[i];
            }
            value.p_music = new AudioClip[this.p_music.Length];
            for(int i = 0; i < this.p_music.Length; i++) {
                value.p_music[i] = this.p_music[i];
            }
            value.p_props = new d_prop[this.p_props.Length];
            for(int i = 0; i < this.p_props.Length; i++) {
                value.p_props[i] = this.p_props[i];
            }
            value.p_size = new Vector2[this.p_size.Length];
            for(int i = 0; i < this.p_size.Length; i++) {
                value.p_size[i] = this.p_size[i];
            }
            value.p_stage = new Sprite[this.p_stage.Length];
            for(int i = 0; i < this.p_stage.Length; i++) {
                value.p_stage[i] = this.p_stage[i];
            }

            return value;
        }
        
        [System.Serializable]
        public struct d_mapLights {
            public Vector2 position;
            public Color color;
        }
        [System.Serializable]
        public struct d_mapPlayerStart {
            public Vector2 position;
            public int team;
        }

        public Vector2[] p_size;
        public Sprite[] p_background;
        public Color[] p_colors;
        public AudioClip[] p_music;

        public d_prop[] p_props;
        public Sprite[] p_stage;
        public Sprite[] p_forground;

        public int m_size;
        public int m_background;
        public int m_globalLight;
        public int m_music;

        public d_mapData[] m_data;
        
    }
}