using UnityEngine;
using System.Collections;

using POPL.Planner;

public class DrawGun : Affordance {
	
	
	public DrawGun(SmartCharacter afdnt, SmartGun afdee) {
		
		affodant = afdnt;
		affordee = afdee;
		initialize ();
	}
	
	void initialize() {
		
		base.initialize ();
		
		preconditions.Add(new Condition(affordantName, "HandsFree", true));
		preconditions.Add (new Condition(affordantName, "HasGun", true));

		effects.Add(new Condition(affordantName, "HandsFree", false));
		effects.Add(new Condition(affordantName, affordeeName, "IsDrawn", true));
		
	}
	
	//Behaviour Tree here
	public void execute() {
	}
}