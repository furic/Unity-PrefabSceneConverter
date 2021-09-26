# Unity-PrefabSceneConverter
> A tool allows you to convert all level prefabs into scenes, so you can use them for addictive scene loading.

## Usage
Open the tool from menu `RF Games -> Prefab Scene Converter`. Simply put the names of all your prefabs into the textbox, click convert and it will search and convert all prefabs found in the given folder. It also provides an option to add the scene into the Build Scenes automatically (which you have to do for scene loading).

Each prefab is simply put inside a scene with the same name, and becomes the only object in the corresponding scene. The original prefab is untouched so the level designer can still use the prefab for level design.

![The screenshot of prefab scene converter](https://i0.wp.com/www.richardfu.net/wp-content/uploads/prefab-scene-converter.jpg)

Further reading for [Optimize Large Prefab Loading To Addictive Scene Loading with PrefabSceneConverter](https://www.richardfu.net/optimize-large-prefab-loading-to-addictive-scene-loading-with-prefabsceneconverter/), which explain why and when you will want to use this tool.

## License
Unity-PrefabSceneConverter is licensed under a MIT License.