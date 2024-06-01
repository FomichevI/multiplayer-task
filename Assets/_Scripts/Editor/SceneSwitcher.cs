using UnityEditor;
using UnityEditor.Overlays;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.UIElements;

[Overlay(typeof(SceneView), id: "scene-switcher", displayName: "Scene Switcher")]
public class SceneSwitcher : Overlay
{
    public override VisualElement CreatePanelContent()
    {
        var element = new VisualElement();

        var titleLabel = new Label("Scenes:");
        element.Add(titleLabel);

        var button1 = new Button(GoToMenu);
        button1.text = "Menu";
        element.Add(button1);

        var button3 = new Button(GoToGameplay);
        button3.text = "Main";
        element.Add(button3);

        return element;
    }

    private void GoToMenu()
    {
        SceneHelper.OpenSceneAdditionally("MenuScene");
    }

    private void GoToGameplay()
    {
        SceneHelper.OpenSceneAdditionally("MainScene");
    }
}

static class SceneHelper
{
    public static void OpenSceneAdditionally(string sceneName)
    {
        if (EditorApplication.isPlaying)
        {
            EditorApplication.isPlaying = false;
        }

        EditorSceneManager.SaveOpenScenes();

        OpenSceneByName("StartScene");
        OpenSceneByName(sceneName, OpenSceneMode.Additive);
    }

    private static void OpenSceneByName(string sceneName, OpenSceneMode mode = OpenSceneMode.Single)
    {
        string[] guids = AssetDatabase.FindAssets("t:scene " + sceneName, null);
        if (guids.Length == 0)
        {
            Debug.LogWarning("Couldn't find scene file - " + sceneName);
        }
        else
        {
            string scenePath = AssetDatabase.GUIDToAssetPath(guids[0]);
            EditorSceneManager.OpenScene(scenePath, mode);
        }
    }
}
