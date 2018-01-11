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
	public static SaveTool Instance;
	private const string dataPathSave = "/GameSaveData";

	void Awake () {
		Instance = this;
	}

	/// <summary>
	/// save
	/// </summary>
	#region Save gen data
	public void Save()
    {
		string path = getPath ();

		byte[] bytes = Encoding.Unicode.GetBytes(path);
		path = Encoding.Unicode.GetString(bytes);

		File.Delete(path);

		FileStream file = File.Open(path, FileMode.CreateNew);
		XmlSerializer serializer = new XmlSerializer(typeof(GameData));

//		file = file.
		serializer.Serialize(file, SaveManager.Instance.CurrentData);

		file.Close();
    }
	#endregion

//	#region Save gen data
//	public void SaveCurrentChunks() {
//		
//		string path = getChunkPath(Coords.current);
//
//		byte[] bytes = Encoding.Unicode.GetBytes(path);
//		path = Encoding.Unicode.GetString(bytes);
//
//		File.Delete(path);
//
//		FileStream file = File.Open(path, FileMode.CreateNew);
//		XmlSerializer serializer = new XmlSerializer(typeof(ChunkGroupData));
//
//
//
//		//		file = file.
//		serializer.Serialize(file, chunks);
//
//		file.Close();
//	}
//	public void Update()
//	{
//		if ( Input.GetKeyDown(KeyCode.K) ) {
//			int overallX = 0;
//			while (overallX <= MapGenerator.Instance.MapScale) {
//				
//				int overallY = 0;
//				while (overallY <= MapGenerator.Instance.MapScale) {
//
////					print (new Coords (overallX , overallY).ToString ());
//					print (getChunkPath (new Coords(overallX,overallY)));
//
//
//					overallY += chunkLimit;
//
//
//				}
//
//				overallX += chunkLimit;
//
//			}
////
////			for (int chunkX = x; chunkX < x + chunkLimit; chunkX++) {
////
////				for (int chunkY = y; chunkY < y + chunkLimit; chunkY++) {
////
////					currentChunks.Add ( Chunk.GetChunk(new Coords(chunkX,chunkY) ) );
////					print ("adding chunk : " + new Coords (chunkX, chunkY));
////				}
////			}
//		}
//
//
//	}
//	#endregion

	//// <summary>
	/// path
	/// </summary>

	#region path
	public string getPath () {
		string path = Application.dataPath + dataPathSave + ".xml";
		if ( Application.isMobilePlatform )
			path = Application.persistentDataPath + dataPathSave + ".xml";

		return path;
	}
	#endregion

	/// <summary>
	/// load
	/// </summary>
	#region Load

	public GameData Load()
    {
		GameData gameSaveData = new GameData();

		string path = getPath ();

		byte[] bytes = Encoding.Unicode.GetBytes(path);
		path = Encoding.Unicode.GetString(bytes);


		FileStream file = File.Open(path, FileMode.OpenOrCreate);
		XmlSerializer serializer = new XmlSerializer(typeof(GameData));
		gameSaveData = (GameData)serializer.Deserialize(file);

		file.Close();

		return gameSaveData;
	}
	#endregion

	public bool FileExists()
    {
		string path = getPath ();

        byte[] bytes = Encoding.Unicode.GetBytes(path);
        path = Encoding.Unicode.GetString(bytes);

		bool exists = (File.Exists(path));

		return exists;
    }


	#region chunk
	int chunkLimit = 10;
	public class ChunkGroupData
	{
		public Chunk[][] chunks;

		public ChunkGroupData()
		{
			// islands ids
		}
	}

//	public ChunkGroupData GetChunkGroupData ( Coords c ) {
//		int x = 0;
//		int y = 0;
//
//		while ( c.x >= x+chunkLimit ) {
//			x += chunkLimit;
//		}
//		while ( c.y >= y+chunkLimit ) {
//			y += chunkLimit;
//		}
//
//		Chunk[][] chunks = new Chunk[chunkLimit-1][chunkLimit-1];
//
//		for (int chunkX = 0; chunkX < x+chunkLimit; chunkX++) {
//
//			for (int chunkY = 0; chunkY < y+chunkLimit; chunkY++) {
//
//				chunks [chunkX] [chunkY] = Chunk.GetChunk (new Coords(chunkX , chunkY) );
//
//
//
//			}
//
//		}
//	}

	public string getChunkPath ( Coords c ) {

		int x = 0;
		int y = 0;

		while ( c.x >= x+chunkLimit ) {
			x += chunkLimit;
		}
		while ( c.y >= y+chunkLimit ) {
			y += chunkLimit;
		}

		string chunkString = "Chunk_X_" + x + "_" + (x + chunkLimit) + "_" + "Y_" + y + "_" + (y + chunkLimit);

		string path = "/Chunks/" + chunkString + ".xml";

		if (Application.isMobilePlatform)
			return Application.persistentDataPath + path;

		return Application.dataPath + path;
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
