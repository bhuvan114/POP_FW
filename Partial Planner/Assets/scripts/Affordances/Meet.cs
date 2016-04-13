using UnityEngine;
using System.Collections;

using POPL.Planner;

public class Meet : Affordance {

	public Meet(SmartCharacter afdnt, SmartCharacter afdee) {
		
		affodant = afdnt;
		affordee = afdee;
		initialize ();
	}
	
	void initialize() {
		
		base.initialize ();
		preconditions.Add (new Condition(affordantName, "InScene", true));
		preconditions.Add (new Condition(affordeeName, "InScene", true));
		effects.Add(new Condition(affordeeName, affordantName, "Infront", true));
		effects.Add(new Condition(affordantName, affordeeName, "Infront", true));
	}
	
	//Behaviour Tree here
	public void execute() {
	}
}
