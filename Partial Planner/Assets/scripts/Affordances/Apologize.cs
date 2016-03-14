using UnityEngine;
using System.Collections;

using POPL.Planner;

public class Apologize : Affordance {
	
	
	public Apologize(SmartCharacter afdnt, SmartCharacter afdee) {
		
		affodant = afdnt;
		affordee = afdee;
		initialize ();
	}
	
	void initialize() {
		
		base.initialize ();
		
		preconditions.Add(new Condition(affordeeName, affordantName, "Infront", true));
		preconditions.Add(new Condition(affordantName, affordeeName, "Infront", true));
		preconditions.Add(new Condition(affordantName, affordeeName, "Knows", true));
		preconditions.Add (new Condition (affordeeName, affordantName, "IsAngry", true));

		effects.Add (new Condition (affordeeName, affordantName, "IsAngry", false));
		effects.Add (new Condition (affordeeName, affordantName, "IsHappy", true));

	}
	
	//Behaviour Tree here
	public void execute() {
	}
}