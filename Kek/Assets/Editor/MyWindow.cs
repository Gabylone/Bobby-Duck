using UnityEngine;
using UnityEditor;
public class MyWindow : EditorWindow
{
    // Add menu named "My Window" to the Window menu
    [MenuItem("Window/My Window")]
    static void Init()
    {
        // Get existing open window or if none, make a new one:
        MyWindow window = (MyWindow)EditorWindow.GetWindow(typeof(MyWindow));
        window.Show();
    }
    /*
    void OnGUI()
    {
        if (GUILayout.Button("Create Regions"))
        {
            GameObject.FindObjectOfType< WorldTexture >().RecreateRegions();
        }

        if (GUILayout.Button("Create Textures"))
        {
            GameObject.FindObjectOfType< WorldTexture >().CreateRegionTextures();
        }

        if (GUILayout.Button("Apply Textures"))
        {
            GameObject.FindObjectOfType<WorldTexture>().ApplyTextures();
        }

    }

    */
}
