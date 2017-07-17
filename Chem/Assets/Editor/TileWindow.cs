using UnityEngine;
using UnityEditor;
public class TileWindow : EditorWindow
{
	string myString = "Hello World";

	bool showDesertTiles = true;
	bool showCaveTiles = true;

	public static Array caveArray;
	public static Array desertArray;

	// Add menu named "My Window" to the Window menu
	[MenuItem("Window/Tile Window")]
	static void Init()
	{
		// Get existing open window or if none, make a new one:s
		TileWindow window = (TileWindow)EditorWindow.GetWindow(typeof(TileWindow));
		window.Show();

	}

	void OnGUI()
	{


		GUILayout.Label("Tile Settings", EditorStyles.boldLabel);

		if (caveArray == null) {
			caveArray = GameObject.Find ("Array (cave)").GetComponent<Array> ();
		}
		if (desertArray == null) {
			desertArray = GameObject.Find ("Array (desert)").GetComponent<Array> ();
		}

		showCaveTiles = EditorGUILayout.BeginToggleGroup("Cave", showCaveTiles);
		caveArray.TileOverall.SetActive (showCaveTiles);
		if ( GUILayout.Button("Update Cave Tiles") ) {
			caveArray.LoadTiles ();
		}
		if ( GUILayout.Button("Generate Cave Props") ) {
			caveArray.GenerateProps ();
		}
		if ( GUILayout.Button("Clear Cave Props") ) {
			caveArray.ClearProps ();
		}
		EditorGUILayout.EndToggleGroup();

		showDesertTiles = EditorGUILayout.BeginToggleGroup("Desert", showDesertTiles);
		desertArray.TileOverall.SetActive (showDesertTiles);
		if ( GUILayout.Button("Update Desert Tiles") ) {
			desertArray.LoadTiles ();
		}
		if ( GUILayout.Button("Generate Desert Props") ) {
			desertArray.GenerateProps ();
		}
		if ( GUILayout.Button("Clear Desert Props") ) {
			desertArray.ClearProps ();
		}
		EditorGUILayout.EndToggleGroup();



	}
}