#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine.SceneManagement;
using System.IO;

[InitializeOnLoad]
public class SceneBuilder
{
    static SceneBuilder()
    {
        // Run once per project open to create the demo scene if it doesn't exist.
        EditorApplication.delayCall += TryBuildOnOpen;
    }

    static void TryBuildOnOpen()
    {
        // Avoid running in batchmode or in play mode
        if (EditorApplication.isPlayingOrWillChangePlaymode) return;

        string scenePath = "Assets/Scenes/DemoScene.unity";
        if (!File.Exists(scenePath))
        {
            BuildDemoScene();
        }
    }

    [MenuItem("Tools/Build Demo Scene (manual)")]
    public static void BuildDemoScene()
    {
        // Ensure folders exist
        Directory.CreateDirectory("Assets/Scenes");
        Directory.CreateDirectory("Assets/Prefabs");

        // Create new scene
        var scene = EditorSceneManager.NewScene(NewSceneSetup.DefaultGameObjects, NewSceneMode.Single);

        // Ground plane
        GameObject ground = GameObject.CreatePrimitive(PrimitiveType.Plane);
        ground.transform.position = Vector3.zero;
        ground.name = "Ground";
        ground.transform.localScale = Vector3.one * 5f;

        // Directional light (already exists in default, but ensure settings)
        var lightGO = GameObject.FindObjectOfType<Light>();
        if (lightGO != null)
        {
            lightGO.type = LightType.Directional;
            lightGO.transform.rotation = Quaternion.Euler(50f, -30f, 0f);
        }

        // Create Balloon prefab
        GameObject balloon = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        balloon.name = "Balloon_Prefab_Temp";
        var rb = balloon.AddComponent<Rigidbody>();
        rb.mass = 0.2f;
        balloon.AddComponent<BoxCollider>().isTrigger = false;
        var wb = balloon.AddComponent<WaterBalloon>();
        wb.splashRadius = 3f;
        wb.soakAmount = 25f;

        // Save balloon prefab
        string balloonPrefabPath = "Assets/Prefabs/Balloon.prefab";
        PrefabUtility.SaveAsPrefabAsset(balloon, balloonPrefabPath);
        Object.DestroyImmediate(balloon);

        // Create Player prefab (capsule + camera + scripts)
        GameObject player = GameObject.CreatePrimitive(PrimitiveType.Capsule);
        player.name = "Player_Prefab_Temp";
        player.transform.position = new Vector3(0f, 1f, 0f);

        // Add camera child
        GameObject camGO = new GameObject("Camera");
        Camera cam = camGO.AddComponent<Camera>();
        camGO.transform.SetParent(player.transform);
        camGO.transform.localPosition = new Vector3(0f, 1.2f, -3f);
        camGO.transform.localEulerAngles = new Vector3(10f, 0f, 0f);

        // Add PlayerThrow
        var pt = player.AddComponent<PlayerThrow>();
        pt.balloonPrefab = AssetDatabase.LoadAssetAtPath<GameObject>(balloonPrefabPath);

        // Create throwOrigin
        GameObject throwOrigin = new GameObject("ThrowOrigin");
        throwOrigin.transform.SetParent(camGO.transform);
        throwOrigin.transform.localPosition = new Vector3(0f, 0f, 1f);
        pt.throwOrigin = throwOrigin.transform;

        // Add Soakable
        var s = player.AddComponent<Soakable>();

        // Save player prefab
        string playerPrefabPath = "Assets/Prefabs/Player.prefab";
        PrefabUtility.SaveAsPrefabAsset(player, playerPrefabPath);

        // Destroy temp player in scene
        Object.DestroyImmediate(player);

        // Instantiate player in scene
        var playerPrefab = AssetDatabase.LoadAssetAtPath<GameObject>(playerPrefabPath);
        GameObject playerInstance = (GameObject)PrefabUtility.InstantiatePrefab(playerPrefab);
        playerInstance.transform.position = new Vector3(0f, 1f, 0f);

        // Add a simple UI Canvas with soak slider placeholder
        GameObject canvasGO = new GameObject("Canvas");
        var canvas = canvasGO.AddComponent<UnityEngine.Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        GameObject sliderGO = new GameObject("SoakSlider");
        sliderGO.transform.SetParent(canvasGO.transform);

        var slider = sliderGO.AddComponent<UnityEngine.UI.Slider>();
        sliderGO.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, -50);
        sliderGO.GetComponent<RectTransform>().sizeDelta = new Vector2(200, 20);

        // Assign slider to player's Soakable (if present)
        var soak = playerInstance.GetComponent<Soakable>();
        if (soak != null)
        {
            soak.soakSlider = slider;
        }

        // Save scene
        string scenePath = "Assets/Scenes/DemoScene.unity";
        EditorSceneManager.SaveScene(SceneManager.GetActiveScene(), scenePath);

        // Refresh database
        AssetDatabase.Refresh();

        Debug.Log("Demo scene and prefabs created at Assets/Scenes/DemoScene.unity and Assets/Prefabs/");
    }
}
#endif
