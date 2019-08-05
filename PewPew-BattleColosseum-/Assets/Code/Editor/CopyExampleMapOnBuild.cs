using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor.Build;
using UnityEditor;
using System.IO;

namespace PPBC {
    public class CopyExampleMapOnBuild : IPostprocessBuild {

        #region Variables

        string source_dir = Path.Combine(Application.dataPath, "Example");

        #endregion
        #region IPreprocessBuild

        public int callbackOrder => 0;

        public void OnPostprocessBuild(BuildTarget target, string path) {
            string dir = Path.Combine(Path.GetDirectoryName(path), Path.GetFileNameWithoutExtension(path) + "_Data", "MAP");

            Directory.CreateDirectory(dir);

            //https://stackoverflow.com/questions/58744/copy-the-entire-contents-of-a-directory-in-c-sharp
            // substring is to remove dir absolute path (E:\).

            // Create subdirectory structure in destination
            foreach (string it in Directory.GetDirectories(source_dir, "*", SearchOption.AllDirectories)) {
                Directory.CreateDirectory(Path.Combine(dir, it.Substring(source_dir.Length + 1)));
                // Example:
                //     > C:\sources (and not C:\E:\sources)
            }

            foreach (string file_name in Directory.GetFiles(source_dir, "*", SearchOption.AllDirectories)) {
                if (Path.GetExtension(file_name) == ".meta")
                    continue;

                File.Copy(file_name, Path.Combine(dir, file_name.Substring(source_dir.Length + 1)));
            }
        }

        #endregion
    }
}