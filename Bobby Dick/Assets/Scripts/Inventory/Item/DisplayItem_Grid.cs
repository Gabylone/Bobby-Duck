using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DisplayItem_Grid: DisplayItem_Loot {

	public override Item HandledItem {
		get {
			return base.HandledItem;
		}
		set {
			
			base.HandledItem = value;

			if ( value == null ) {
				return;
			}

			Tween.Bounce (itemImage.transform);
		}
	}
}
