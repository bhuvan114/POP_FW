using UnityEngine;
using System.Collections;
using TreeSharpPlus;

using POPL.Planner;

public class OpenDoor : Affordance {

	DoorScript doorScript;
	Transform openPos;

	public OpenDoor(SmartDoor afdnt, SmartCharacter afdee) {
		
		affodant = afdnt;
		affordee = afdee;
		openPos = afdnt.openPoint;
		initialize ();
	}
	
	void initialize() {
		
		base.initialize ();
		
		preconditions.Add(new Condition(affordeeName, "HandsFree", true));
		preconditions.Add(new Condition(affordantName, "IsOpen", false));
		
		effects.Add(new Condition(affordantName, "IsOpen", true));
		
	}
	
	//Behaviour Tree here
	public Node execute() {
		//Debug.LogError ("Execute");

		return new Sequence (HelperFunctions.ST_ApproachAndWait(affordee.gameObject, openPos), HelperFunctions.ST_Turn(affordee.gameObject, openPos), affordee.gameObject.GetComponent<BehaviorMecanim> ().Node_HandAnimation ("GRAB", true), new LeafWait (2000), this.OpenAnimation());
	}

	Node OpenAnimation() {
		return new LeafInvoke(
			() => this.affodant.gameObject.GetComponent<DoorScript> ().OpenDoor());
	}
}