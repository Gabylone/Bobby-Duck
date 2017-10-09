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

	#region Save
	public void Save()
    {
		string path = getPath ();
//		string path = PathToSave + "/GameSave.xml";

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

	#region Load
	public string getPath () {
		string path = Application.dataPath + dataPathSave + ".xml";
		if ( Application.isMobilePlatform )
			path = Application.persistentDataPath + dataPathSave + ".xml";

		return path;
	}
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
