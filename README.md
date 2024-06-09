# About This Repository
This repository contains a simple tool to help you reconstruct 3D scenes from Unreal in Unity.    
The tool primarily focuses on meshes and cannot reconstruct non-static objects.    
This code was written in my spare time and is not rigorously tested, so please use it with discretion.   

# How to Use This Tool
1. Place the code from the External\Unreal\ExportMeshJson folder into your Unreal project.
2. Execute it within your scene.
3. It will generate a SceneExport.json file in the project directory.
4. Export all meshes from the Unreal project and place them into your Unity project. Due to hard-coded paths in my script, you must place them under the Assets/SampleScene/Models directory in Unity, or they will not be found.
5. Place the SceneExport.json file into your Unity project (the location does not matter).
6. In Unity, select the SceneExport.json file.
7. Click Tool -> Reconstruct Unreal Scene From Json.
8. The script will automatically reconstruct the scene in the currently active scene in Unity.

# Try it
This project includes a simple sample that you can run to see the results directly. 

https://github.com/LenbaSun/prj_unreal_scene_reconstruct/assets/14867784/ae4587ba-728a-4dec-8374-d8e1cd48728e

The screenshots below compare the reconstructed scene in Unity with the original scene in Unreal.

![448080428_7972125676141815_7178635632149649747_n](https://github.com/LenbaSun/prj_unreal_scene_reconstruct/assets/14867784/0c501b18-fd97-488b-bf49-f4487d7c7640)
