using System.IO;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

namespace LenbaSun.Rendering.Editor
{
    public class UnrealSceneReconstructTool
    {
        [MenuItem("Tool/Reconstruct Unreal Scene From Json")]
        static void Reconstrcut()
        {
            var path = _GetSelectedJsonFilePath();
            if (path == string.Empty)
                return;

            var meshInfos = UnrealJsonParser.Parse(path);
            _ConstructScene(meshInfos, "Assets/SampleScene/Models");
        }

        static void _ConstructScene(UnrealMeshInfo[] meshInfos, string loadModelDirectory)
        {
            foreach (var meshInfo in meshInfos)
            {
                string modelPath = Path.Combine(loadModelDirectory, meshInfo.MeshName + ".fbx");

                // 确保路径格式正确
                modelPath = modelPath.Replace("\\", "/");

                // 从模型目录加载 FBX 模型
                GameObject modelPrefab = AssetDatabase.LoadAssetAtPath<GameObject>(modelPath);

                if (modelPrefab != null)
                {
                    // 实例化模型并设置名称、位置、旋转和缩放
                    GameObject instantiatedModel = PrefabUtility.InstantiatePrefab(modelPrefab) as GameObject;
                    if (instantiatedModel != null)
                    {
                        instantiatedModel.name = meshInfo.ActorName;
                        instantiatedModel.transform.position = meshInfo.WorldPos;
                        instantiatedModel.transform.rotation = meshInfo.WorldRot;
                        instantiatedModel.transform.localScale = meshInfo.WorldScale;

                        // 确保实例化的对象添加到活动场景中
                        EditorSceneManager.MoveGameObjectToScene(instantiatedModel, EditorSceneManager.GetActiveScene());
                    }
                }
                else
                {
                    Debug.LogError($"Model '{meshInfo.MeshName}' not found at path '{modelPath}'");
                }
            }

            // 重新刷新场景视图
            SceneView.RepaintAll();
        }

        static string _GetSelectedJsonFilePath()
        {
            Object selectedObject = Selection.activeObject;
            if (null == selectedObject)
            {
                EditorUtility.DisplayDialog("No Selection", "Please select a JSON file in the Project view.", "OK");
                return string.Empty;
            }

            string path = AssetDatabase.GetAssetPath(selectedObject);
            if (Path.GetExtension(path).ToLower() == ".json")
                return path;
     
            EditorUtility.DisplayDialog("Invalid Selection", "Please select a valid JSON file.", "OK");
            return string.Empty;
        }
    }
}