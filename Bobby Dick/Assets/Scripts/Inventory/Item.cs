using UnityEngine;
using System;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

[Serializable()]
public class Item {

	public int ID = 0;

	public string 	name 		= "";
	public int 		value 		= 0;
	public int 		price 		= 0;
	public int 		weight 		= 0;
	public int 		level 		= 0;
	public int		spriteID 	= 0;
	public int 		type 		= 0;

	public ItemCategory category;
		
	public Item () {
		//
	}

	public Item (

		int _id,

		string _name,
		string _description,
		int _value,
		int _price,
		int _weight,
		int _level,
		int _spriteID,
//		int _type,

		ItemCategory _cat
		)
	{
		ID = _id;

		name = _name;
		value = _value;
		price = _price;
		weight = _weight;
		level = _level;

		spriteID = _spriteID;

		category = _cat;
//
//		Debug.Log ("item name : " + name);
//		Debug.Log ("item sprite id : " + spriteID);
	}

	[OnSerialized()]
	internal void OnSerializedMethod(StreamingContext context)
	{
		Debug.Log ("item serialized");
	}

	[OnDeserialized()]
	internal void OnDeserializedMethod(StreamingContext context)
	{
		Debug.Log ("item deserialized");
	}

	public CrewMember.EquipmentPart EquipmentPart {
		get {
			switch (category) {
			case ItemCategory.Weapon:
				return CrewMember.EquipmentPart.Weapon;
				break;
			case ItemCategory.Clothes:
				return CrewMember.EquipmentPart.Clothes;
				break;
			default:
				return CrewMember.EquipmentPart.None;
				break;
			}
		}
	}
}
