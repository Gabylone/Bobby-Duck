using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Xml.Serialization;
using System.IO;
using System.Xml;
using System;
using System.Text;


//tu peux changer le chemin de sauvegarde il y a troi ligne a changer :
//string path = Application.dataPath + dataPathSave;
public class SaveTool : MonoBehaviour
{
	public int chunkLimit = 10;

	public static SaveTool Instance;

	void Awake () {
		Instance = this;
		CreateDirectories ();
	}

	void CreateDirectories ()
	{
		Debug.Log ("checking directories");

		if ( DirectoryExists(GetGameDataFolderPath()) == false ) {
			Debug.Log ("BYTES SaveData folder doesnt exist, creating it");
			Directory.CreateDirectory (GetGameDataFolderPath ());
		}

		if ( DirectoryExists(GetGameDataFolderPath() + "/Chunks") == false ) {
			Debug.Log ("BYTES SaveData/Chunks folder doesnt exist, creating it");
			Directory.CreateDirectory (GetGameDataFolderPath() + "/Chunks");
		}
	}

	/// <summary>
	/// save
	/// </summary>
	#region Save game data
	public void SaveGameData()
    {
		string path = GetGameDataFilePath ();

		byte[] bytes = Encoding.Unicode.GetBytes(path);
		path = Encoding.Unicode.GetString(bytes);

		File.Delete(path);

		FileStream file = File.Open(path, FileMode.CreateNew);
		XmlSerializer serializer = new XmlSerializer(typeof(GameData));

//		file = file.
		serializer.Serialize(file, SaveManager.Instance.GameData);

		file.Close();
    }
	#endregion

	#region Save chunks
	public void SaveAllChunks () {

		int overallX = 0;

		while (overallX <= MapGenerator.Instance.MapScale) {

			int overallY = 0;
		
			while (overallY <= MapGenerator.Instance.MapScale) {

				Coords coords = new Coords (overallX, overallY);

				SaveSpecificChunks (coords);

				overallY += chunkLimit;

			}

			overallX += chunkLimit;

		}
	}

	public void SaveSpecificChunks(Coords targetCoords) {

		string path = GetChunkPath(targetCoords);

		byte[] bytes = Encoding.Unicode.GetBytes(path);
		path = Encoding.Unicode.GetString(bytes);

		File.Delete(path);

		FileStream file = File.Open(path, FileMode.CreateNew);
		XmlSerializer serializer = new XmlSerializer(typeof(ChunkGroupData));

		ChunkGroupData newChunkGroupData = GetChunkGroupData (targetCoords);
//		foreach (var item in newChunkGroupData.chunks) {
//			foreach (var chunk in item) {
//				if ( chunk.IslandData != null ) {
//					print ("SAUVEGARDE / nombre de content decals : " + chunk.IslandData.storyManager.CurrentStoryHandler.contentDecals.Count);
//				}
//			}
//		}

		serializer.Serialize(file,newChunkGroupData );

		file.Close();
	}
	#endregion

	/// <summary>
	/// load
	/// </summary>
	#region Load game & chunk data
	public GameData LoadGameData()
    {
		GameData gameSaveData = new GameData();

		string path = GetGameDataFilePath ();

		byte[] bytes = Encoding.Unicode.GetBytes(path);
		path = Encoding.Unicode.GetString(bytes);

		FileStream file = File.Open(path, FileMode.OpenOrCreate);
		XmlSerializer serializer = new XmlSerializer(typeof(GameData));
		gameSaveData = (GameData)serializer.Deserialize(file);

		file.Close();

		return gameSaveData;
	}
	public Chunk[][] LoadWorldChunks () {

		Chunk[][] chunks = new Chunk[MapGenerator.Instance.MapScale][];
		for (int i = 0; i < chunks.Length; i++) {
			chunks[i] = new Chunk[MapGenerator.Instance.MapScale];
		}

		int overallX = 0;

		while (overallX <= MapGenerator.Instance.MapScale) {

			int overallY = 0;

			while (overallY <= MapGenerator.Instance.MapScale) {

				Coords coords = new Coords (overallX, overallY);

				ChunkGroupData newChunkGroupData = LoadSpecificChunks(coords);

				for (int chunkX = 0; chunkX < chunkLimit; chunkX++) {

					for (int chunkY = 0; chunkY < chunkLimit; chunkY++) {

						Chunk chunk = newChunkGroupData.chunks [chunkX][chunkY];;

						int x = overallX + chunkX;
						int y = overallY + chunkY;

						if ( x < chunks.Length ) {
							if (y < chunks [x].Length) {

								chunks [x] [y] = chunk;

//								if ( chunk.IslandData != null ) {
//									print ("CHARGEMENT / nombre de content decals : " + chunk.IslandData.storyManager.CurrentStoryHandler.contentDecals.Count);
//								}
							}
						}

					}
				}



				overallY += chunkLimit;

			}

			overallX += chunkLimit;

		}

		return chunks;

	}
	public ChunkGroupData LoadSpecificChunks(Coords targetCoords) {

		string path = GetChunkPath (targetCoords);

		ChunkGroupData newChunkGroupData = new ChunkGroupData ();

		byte[] bytes = Encoding.Unicode.GetBytes(path);
		path = Encoding.Unicode.GetString(bytes);

		FileStream file = File.Open(path, FileMode.OpenOrCreate);

		XmlSerializer serializer = new XmlSerializer(typeof(ChunkGroupData));

		newChunkGroupData = (ChunkGroupData)serializer.Deserialize(file);

		file.Close();

		return newChunkGroupData;
	}
	#endregion

	public bool FileExists(string path)
    {
        byte[] bytes = Encoding.Unicode.GetBytes(path);
        path = Encoding.Unicode.GetString(bytes);

		bool exists = (File.Exists(path));

		return exists;
    }
	public bool DirectoryExists(string path)
	{
		byte[] bytes = Encoding.Unicode.GetBytes(path);
		path = Encoding.Unicode.GetString(bytes);

		bool exists = (Directory.Exists(path));

		return exists;
	}

	#region chunk
	[System.Serializable]
	public class ChunkGroupData
	{
		public Chunk[][] chunks;

		public ChunkGroupData()
		{
			// islands ids
		}
	}

	public ChunkGroupData GetChunkGroupData ( Coords c ) {

		ChunkGroupData chunkGroupData = new ChunkGroupData ();

		int x = 0;
		int y = 0;

		while ( c.x >= x+chunkLimit ) {
			x += chunkLimit;
		}
		while ( c.y >= y+chunkLimit ) {
			y += chunkLimit;
		}

		chunkGroupData.chunks = new Chunk[chunkLimit][];
		for (int i = 0; i < chunkGroupData.chunks.Length; i++) {
			chunkGroupData.chunks[i] = new Chunk[chunkLimit];
		}

		for (int chunkX = 0; chunkX < chunkLimit; chunkX++) {

			for (int chunkY = 0; chunkY < chunkLimit; chunkY++) {
//
				Coords chunkCoords = new Coords (x + chunkX, y + chunkY);

				Chunk newChunk = Chunk.GetChunk (chunkCoords);


				chunkGroupData.chunks [chunkX] [chunkY] = newChunk;

			}

		}

		return chunkGroupData;


	}
	#endregion

	#region paths
	public string GetGameDataFilePath () {
		return GetGameDataFolderPath() + "/GameData.xml";
	}
	public string GetGameDataFolderPath () {
		string path = Application.dataPath + "/SaveData";
		if ( Application.isMobilePlatform )
			path = Application.persistentDataPath + "/SaveData";

		return path;
	}
	public string GetChunkPath ( Coords c ) {

		int x = 0;
		int y = 0;

		while ( c.x >= x+chunkLimit ) {
			x += chunkLimit;
		}
		while ( c.y >= y+chunkLimit ) {
			y += chunkLimit;
		}

		string chunkString = "Chunk_X_" + x + "_" + (x + chunkLimit) + "_" + "Y_" + y + "_" + (y + chunkLimit);

		return GetGameDataFolderPath() + "/Chunks/" + chunkString + ".xml";
	}
	#endregion

}

public static class BytesTools {
	public static byte[] ToBytes<T>(this T[,] array) where T : struct
	{
		var buffer = new byte[array.GetLength(0) * array.GetLength(1) * System.Runtime.InteropServices.Marshal.SizeOf(typeof(T))];
		Buffer.BlockCopy(array, 0, buffer, 0, buffer.Length);
		return buffer;
	}
	public static void FromBytes<T>(this T[,] array, byte[] buffer) where T : struct
	{
		var len = Math.Min(array.GetLength(0) * array.GetLength(1) * System.Runtime.InteropServices.Marshal.SizeOf(typeof(T)), buffer.Length);
		Buffer.BlockCopy(buffer, 0, array, 0, len);
	}
}
