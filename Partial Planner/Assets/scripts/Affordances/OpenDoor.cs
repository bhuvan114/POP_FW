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
		initialize ();
	}
	
	void initialize() {
		
		base.initialize ();

		name = affordeeName + " opens " + affordantName + " door";
		preconditions.Add(new Condition(affordeeName, "HandsFree", true));
		preconditions.Add(new Condition(affordantName, "IsOpen", false));
		
		effects.Add(new Condition(affordantName, "IsOpen", true));
		treeRoot = this.execute ();
		
	}
	
	//Behaviour Tree here
	public Node execute() {
		//Debug.LogError ("Execute");
		openPos = this.affodant.gameObject.GetComponent<DoorScript> ().OpenPoint;
		return new Sequence (HelperFunctions.ST_ApproachAndWait(affordee.gameObject, openPos), HelperFunctions.ST_Turn(affordee.gameObject, openPos), affordee.gameObject.GetComponent<BehaviorMecanim> ().Node_HandAnimation ("GRAB", true), new LeafWait (2000), this.OpenAnimation());
	}

	Node OpenAnimation() {
		return new LeafInvoke(
			() => this.affodant.gameObject.GetComponent<DoorScript> ().OpenDoor());
	}
}