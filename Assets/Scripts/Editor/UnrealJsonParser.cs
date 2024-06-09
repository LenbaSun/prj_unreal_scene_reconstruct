using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace LenbaSun.Rendering.Editor
{
    public struct UnrealMeshInfo
    {
        public string ActorName;
        public string MeshName;
        public Vector3 WorldPos;
        public Quaternion WorldRot;
        public Vector3 WorldScale;
    }

    public class UnrealJsonParser
    {
        public static UnrealMeshInfo[] Parse(string jsonFilePath)
        {
            // 读取 JSON 文件内容
            string jsonContent = File.ReadAllText(jsonFilePath);

            // 反序列化 JSON 内容到一个 JObject
            JObject rootObject = JObject.Parse(jsonContent);

            // 获取 Actors 数组
            JArray actorsArray = (JArray)rootObject["Actors"];

            // 创建一个 List 来存储 UnrealMeshInfo 对象
            List<UnrealMeshInfo> meshInfoList = new List<UnrealMeshInfo>();

            // 遍历 Actors 数组
            foreach (JObject actorObject in actorsArray)
            {
                UnrealMeshInfo meshInfo = new UnrealMeshInfo
                {
                    ActorName = actorObject["ActorName"].ToString(),
                    MeshName = actorObject["MeshName"].ToString(),
                    WorldPos = new Vector3(
                        actorObject["PositionX"].ToObject<float>(),
                        actorObject["PositionY"].ToObject<float>(),
                        actorObject["PositionZ"].ToObject<float>()
                    ),
                    WorldRot = Quaternion.Euler(
                        actorObject["RotationRoll"].ToObject<float>(),
                        actorObject["RotationYaw"].ToObject<float>(),
                        actorObject["RotationPitch"].ToObject<float>()
                    ),
                    WorldScale = new Vector3(
                        actorObject["ScaleX"].ToObject<float>(),
                        actorObject["ScaleY"].ToObject<float>(),
                        actorObject["ScaleZ"].ToObject<float>()
                    )
                };

                // 添加到列表中
                meshInfoList.Add(meshInfo);
            }

            // 将 List 转换为数组并返回
            return meshInfoList.ToArray();
        }
    }
}