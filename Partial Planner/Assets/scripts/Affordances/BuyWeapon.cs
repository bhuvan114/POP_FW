using UnityEngine;
using System.Collections;
using TreeSharpPlus;

using POPL.Planner;

public class BuyWeapon : Affordance {

	//GameObject storeDealer;
	GameObject hipHolder;
	GameObject gun;

	public BuyWeapon(SmartStore afdnt, SmartCharacter afdee) {

		//storeDealer = HelperFunctions.GetChildGameObjectByName (afdnt.gameObject, "Dealer");
		hipHolder = afdee.hipHolder;
		gun = HelperFunctions.GetChildGameObjectByName (afdnt.gameObject, "Gun");
		affodant = afdnt;
		affordee = afdee;
		initialize ();
	}
	
	void initialize() {
		
		base.initialize ();

		name = affordeeName + " buys gun from " + affordantName;
		preconditions.Add(new Condition(affordantName, "HasGun", true));
		preconditions.Add(new Condition(affordeeName, "HasMoney", true));
		preconditions.Add(new Condition(affordeeName, affordantName, "InStore", true));

		effects.Add (new Condition(affordantName, "HasGun", false));
		effects.Add(new Condition(affordeeName, "HasMoney", false));
		effects.Add (new Condition(affordeeName, "HasGun", true));
		treeRoot = this.execute ();
		
	}
	
	//Behaviour Tree here
	public Node execute() {

		//Debug.Log ("Dealer - " + storeDealer);
		return new Sequence (affordee.gameObject.GetComponent<BehaviorMecanim> ().Node_HandAnimation ("GRAB", true), /*storeDealer.gameObject.GetComponent<BehaviorMecanim> ().Node_HandAnimation ("GRAB", true)*/
		                     new LeafInvoke(() => gun.GetComponent<GunController>().SetHolder(hipHolder)));
	}
}