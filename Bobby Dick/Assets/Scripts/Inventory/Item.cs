using UnityEngine;
using System.Collections;

[System.Serializable]
public class Item {

	public int ID = 0;

	public string 	name 		= "";
	public int 		value 		= 0;
	public int 		price 		= 0;
	public int 		weight 		= 0;
	public int 		level 		= 0;
	[System.NonSerialized]
	public int		spriteID 	= 0;
	[System.NonSerialized]
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
	}
}
