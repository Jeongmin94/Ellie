using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

namespace TheKiwiCoder
{
    public static class EditorUtility
    {
        public static BehaviourTree CreateNewTree(string assetName, string folder)
        {
            var path = Path.Join(folder, $"{assetName}.asset");
            if (File.Exists(path))
            {
                Debug.LogError($"Failed to create behaviour tree asset: Path already exists:{assetName}");
                return null;
            }

            var tree = ScriptableObject.CreateInstance<BehaviourTree>();
            tree.name = assetName;
            AssetDatabase.CreateAsset(tree, path);
            AssetDatabase.SaveAssets();
            EditorGUIUtility.PingObject(tree);
            return tree;
        }

        public static void CreateNewScript(ScriptTemplate scriptTemplate, NodeView source, bool isSourceParent, Vector2 position)
        {
            BehaviourTreeEditorWindow.Instance.newScriptDialog.CreateScript(scriptTemplate, source, isSourceParent, position);
        }


        public static List<T> LoadAssets<T>() where T : Object
        {
            var assetIds = AssetDatabase.FindAssets($"t:{typeof(T).Name}");
            var assets = new List<T>();
            foreach (var assetId in assetIds)
            {
                var path = AssetDatabase.GUIDToAssetPath(assetId);
                var asset = AssetDatabase.LoadAssetAtPath<T>(path);
                assets.Add(asset);
            }

            return assets;
        }

        public static List<string> GetAssetPaths<T>() where T : Object
        {
            var assetIds = AssetDatabase.FindAssets($"t:{typeof(T).Name}");
            var paths = new List<string>();
            foreach (var assetId in assetIds)
            {
                var path = AssetDatabase.GUIDToAssetPath(assetId);
                paths.Add(path);
            }

            return paths;
        }

        public static PackageManifest GetPackageManifest()
        {
            // Loop through all package.json files in the project and find this one.. 
            var packageJsons = AssetDatabase.FindAssets("package");
            var packagePaths = packageJsons.Select(AssetDatabase.GUIDToAssetPath).ToArray();
            foreach (var path in packagePaths)
            {
                var asset = AssetDatabase.LoadAssetAtPath<TextAsset>(path);
                if (asset)
                {
                    try
                    {
                        var manifest = JsonUtility.FromJson<PackageManifest>(asset.text);
                        if (manifest.name == "com.thekiwicoder.behaviourtreeditor")
                        {
                            return manifest;
                        }
                    }
                    catch
                    {
                        // Ignore if the manifest file failed to parse
                    }
                }
            }

            return null;
        }

        public static float RoundTo(float value, int nearestInteger)
        {
            return Mathf.FloorToInt(value / nearestInteger) * nearestInteger;
        }

        public struct ScriptTemplate
        {
            public TextAsset templateFile;
            public string defaultFileName;
            public string subFolder;
        }

        [Serializable]
        public class PackageManifest
        {
            public string name;
            public string version;
        }
    }
}