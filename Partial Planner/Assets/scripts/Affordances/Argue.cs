using UnityEngine;
using System.Collections;

using POPL.Planner;

public class Argue : Affordance {
	
	
	public Argue(SmartCharacter afdnt, SmartCharacter afdee) {
		
		affodant = afdnt;
		affordee = afdee;
		initialize ();
	}
	
	void initialize() {
		
		base.initialize ();
		
		preconditions.Add(new Condition(affordeeName, affordantName, "Infront", true));
		preconditions.Add(new Condition(affordantName, affordeeName, "Infront", true));
		preconditions.Add(new Condition(affordantName, affordeeName, "Knows", true));

		effects.Add (new Condition (affordantName, affordeeName, "FeelsBad", true));
		effects.Add (new Condition (affordeeName, affordantName, "IsAngry", true));
	}
	
	//Behaviour Tree here
	public void execute() {
	}
}