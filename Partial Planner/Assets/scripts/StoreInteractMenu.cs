using UnityEngine;
using System.Collections;

public class StoreInteractMenu : MonoBehaviour {

	StoreTrigger storeTrigger;
	
	void Update () {


	}

	public void BuyWeapon(){

		storeTrigger.BuyWeapon ();
	}

	public void BuyStore()
	{
		storeTrigger.BuyStore();
	}

	public void setStore(GameObject obj) {

		storeTrigger = obj.GetComponent<StoreTrigger>();
	}
}
