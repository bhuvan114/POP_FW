using UnityEngine;
using System.Collections;

using POPL.Planner;

public class EnterStore : Affordance {
	
	
	public EnterStore(SmartStore afdnt, SmartCharacter afdee) {


		affodant = afdnt;
		affordee = afdee;
		initialize ();
		preconditions.Add(new Condition(afdnt.storeDoor.name, "IsOpen", true));
	}
	
	void initialize() {
		
		base.initialize ();

		effects.Add(new Condition(affordeeName, affordantName, "InStore", true));
		
	}
	
	//Behaviour Tree here
	public void execute() {
	}
}