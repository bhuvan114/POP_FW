using UnityEngine;
using System.Collections;

using POPL.Planner;

public class OpenDoor : Affordance {
	
	
	public OpenDoor(SmartDoor afdnt, SmartCharacter afdee) {
		
		affodant = afdnt;
		affordee = afdee;
		initialize ();
	}
	
	void initialize() {
		
		base.initialize ();
		
		preconditions.Add(new Condition(affordeeName, "HandsFree", true));
		preconditions.Add(new Condition(affordantName, "IsOpen", false));
		
		effects.Add(new Condition(affordantName, "IsOpen", true));
		
	}
	
	//Behaviour Tree here
	public void execute() {
	}
}