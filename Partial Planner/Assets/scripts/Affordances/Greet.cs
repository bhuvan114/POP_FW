using UnityEngine;
using System.Collections;

using POPL.Planner;

public class Greet : Affordance {


	public Greet(SmartCharacter afdnt, SmartCharacter afdee) {

		affodant = afdnt;
		affordee = afdee;
		initialize ();
	}

	void initialize() {

		base.initialize ();

		preconditions.Add(new Condition(affordeeName, affordantName, "Infront", true));
		preconditions.Add(new Condition(affordantName, affordeeName, "Infront", true));
		
		effects.Add(new Condition(affordeeName, affordantName, "Knows", true));
		effects.Add(new Condition(affordantName, affordeeName, "Knows", true));
	}

	//Behaviour Tree here
	public void execute() {
	}
}
