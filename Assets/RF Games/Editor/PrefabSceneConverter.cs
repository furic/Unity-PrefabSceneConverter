using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO;
using UnityEditor.SceneManagement;

public class PrefabSceneConverter : EditorWindow
{
	private const string PREFS_PREFAB_PATH = "PrefabSceneConverterPrefabPath";
	private const string PREFS_SCENE_PATH = "PrefabSceneConverterScenePath";
	private const string PREFS_OVERRIDE_EXIST = "PrefabSceneConverterOverrideExist";
	private const string PREFS_ADD_TO_BUILD_SCENES = "PrefabSceneConverterAddToBuildScenes";

	private string _prefabPath = "Prefabs/Levels/";
	private string _scenePath = "Scenes/Levels/";
	private string _prefabNames;
	private bool _overrideExist = true;
	private bool _addToBuildScenes = true;

	private Vector2 _scroll;

	[MenuItem("RF Games/Prefab Scene Converter")]
	private static void Init()
	{
		PrefabSceneConverter window = (PrefabSceneConverter)GetWindow(typeof(PrefabSceneConverter));
		window.Show();
	}

	private void Awake()
	{
		_prefabPath = EditorPrefs.GetString(PREFS_PREFAB_PATH, _prefabPath);
		_scenePath = EditorPrefs.GetString(PREFS_SCENE_PATH, _scenePath);
		_overrideExist = EditorPrefs.GetBool(PREFS_OVERRIDE_EXIST, _overrideExist);
		_addToBuildScenes = EditorPrefs.GetBool(PREFS_ADD_TO_BUILD_SCENES, _addToBuildScenes);
	}

	private void OnGUI()
	{
		string prefabPath = EditorGUILayout.TextField("Prefab Path:", _prefabPath);
		if (prefabPath != _prefabPath) {
			EditorPrefs.SetString(PREFS_PREFAB_PATH, _prefabPath = prefabPath);
		}

		string scenePath = EditorGUILayout.TextField("Scene Path:", _scenePath);
		if (scenePath != _scenePath) {
			EditorPrefs.SetString(PREFS_SCENE_PATH, _scenePath = scenePath);
		}

		EditorGUILayout.LabelField("Prefab Names: (separate by new line)");

		_scroll = EditorGUILayout.BeginScrollView(_scroll);
		_prefabNames = EditorGUILayout.TextArea(_prefabNames, GUILayout.Height(position.height - 30));
		EditorGUILayout.EndScrollView();

		bool overrideExist = EditorGUILayout.Toggle("Override Exist:", _overrideExist);
		if (overrideExist != _overrideExist) {
			EditorPrefs.SetBool(PREFS_OVERRIDE_EXIST, _overrideExist = overrideExist);
		}

		bool addToBuildScenes = EditorGUILayout.Toggle("Add Scenes to Build Scenes", _addToBuildScenes);
		if (addToBuildScenes != _addToBuildScenes) {
			EditorPrefs.SetBool(PREFS_ADD_TO_BUILD_SCENES, _addToBuildScenes = addToBuildScenes);
		}

		if (GUILayout.Button("Convert")) {
			string[] prefabNames = _prefabNames.Split('\n');
			foreach (string prefabName in prefabNames) {

				string newSceneAssetPath = Path.Combine("Assets/", _scenePath, prefabName + ".unity");
				string newSceneFullPath = Path.Combine(Application.dataPath, _scenePath, prefabName + ".unity");

				string prefabAssetPath = Path.Combine("Assets/", _prefabPath, prefabName + ".prefab");

				bool newSceneExist = File.Exists(newSceneFullPath);

				if (_overrideExist || !newSceneExist) {

					Scene scene = EditorSceneManager.NewScene(NewSceneSetup.EmptyScene, NewSceneMode.Single);
					GameObject prefab = (GameObject)AssetDatabase.LoadAssetAtPath(prefabAssetPath, typeof(GameObject));

					if (prefab == null) {
						Debug.LogError($"Prefab doesn't exist at {prefabAssetPath}, skipping...");
						continue;
					}

					GameObject go = (GameObject)PrefabUtility.InstantiatePrefab(prefab);
					Debug.Log(go.name);

					bool succeed = EditorSceneManager.SaveScene(scene, newSceneAssetPath);
					Debug.Log($"Save scene {(succeed ? "succeed" : "failed")} at {newSceneAssetPath}");

					if (succeed && _addToBuildScenes && !newSceneExist) {
						EditorBuildSettingsScene[] buildScenes = EditorBuildSettings.scenes;
						EditorBuildSettingsScene[] newBuildScenes = new EditorBuildSettingsScene[buildScenes.Length + 1];
						System.Array.Copy(buildScenes, newBuildScenes, buildScenes.Length);
						EditorBuildSettingsScene newbuildScene = new EditorBuildSettingsScene(newSceneAssetPath, true);
						newBuildScenes[newBuildScenes.Length - 1] = newbuildScene;
						EditorBuildSettings.scenes = newBuildScenes;
					}

				} else {

					Debug.Log($"Scene already exists at {newSceneAssetPath}, skipping...");

				}
			}
		}
	}
}