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

    public GameObject dontDestoyOnLoadGroup;

	void Awake () {
		Instance = this;
		CreateDirectories ();
	}

	#region directories
	void CreateDirectories ()
	{
		if ( DirectoryExists(GetSaveFolderPath()) == false ) {
//			Debug.Log ("BYTES SaveData folder doesnt exist, creating it");
			Directory.CreateDirectory (GetSaveFolderPath ());
		}
	}
    public void EraseSave()
    {
        File.Delete(GetSaveFolderPath() + "/inventory.xml");
        File.Delete(GetSaveFolderPath() + "/regions.xml");
    }
    #endregion


    #region save & load
    public void SaveToPath ( string path , object o) {

		path = GetSaveFolderPath () + "/" + path + ".xml";

		byte[] bytes = Encoding.Unicode.GetBytes(path);
		path = Encoding.Unicode.GetString(bytes);

		File.Delete(path);

		FileStream file = File.Open(path, FileMode.CreateNew);
		XmlSerializer serializer = new XmlSerializer(o.GetType ());

		//		file = file.
		serializer.Serialize(file, o);

		file.Close();
	}

	public object LoadFromPath(string path, string className)
	{
		path = GetSaveFolderPath () + "/" + path + ".xml";

		byte[] bytes = Encoding.Unicode.GetBytes(path);
		path = Encoding.Unicode.GetString(bytes);

//		FileStream file = File.Open(path, FileMode.OpenOrCreate);
		FileStream file = File.Open(path, FileMode.Open);
		XmlSerializer serializer = new XmlSerializer( Type.GetType(className) );

		object o = serializer.Deserialize(file);

		file.Close();

		return o;
	}
	#endregion

	public bool FileExists(string path)
    {
		path = GetSaveFolderPath () + "/" + path + ".xml";

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

	#region paths
	public string GetSaveFolderPath () {
		string path = Application.dataPath + "/SaveData";
		if ( Application.isMobilePlatform )
			path = Application.persistentDataPath + "/SaveData";

		return path;
	}
	#endregion

}
