using UnityEngine;
using System.Collections;

using POPL.Planner;

public class BuyWeapon : Affordance {
	
	
	public BuyWeapon(SmartStore afdnt, SmartCharacter afdee) {
		
		affodant = afdnt;
		affordee = afdee;
		initialize ();
	}
	
	void initialize() {
		
		base.initialize ();
		
		preconditions.Add(new Condition(affordantName, "HasGun", true));
		preconditions.Add(new Condition(affordeeName, "HasMoney", true));
		preconditions.Add(new Condition(affordeeName, affordantName, "InStore", true));
		
		effects.Add(new Condition(affordeeName, "HasMoney", false));
		effects.Add (new Condition(affordeeName, "HasGun", true));
		
	}
	
	//Behaviour Tree here
	public void execute() {
	}
}