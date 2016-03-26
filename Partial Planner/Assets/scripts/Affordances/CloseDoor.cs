using UnityEngine;
using System.Collections;
using TreeSharpPlus;

using POPL.Planner;

public class CloseDoor : Affordance {
	
	
	public CloseDoor(SmartDoor afdnt, SmartCharacter afdee) {
		
		affodant = afdnt;
		affordee = afdee;
		initialize ();
	}
	
	void initialize() {
		
		base.initialize ();

		name = affordeeName + " closes " + affordantName + " door";
		preconditions.Add(new Condition(affordeeName, "HandsFree", true));
		preconditions.Add(new Condition(affordantName, "IsOpen", true));
		
		effects.Add(new Condition(affordantName, "IsOpen", false));
		
	}
	
	//Behaviour Tree here
	public Node execute() {

		Debug.Log ("Close execute");
		return new Sequence (this.CloseAnimation());
	}

	Node CloseAnimation() {
		Debug.Log ("CloseAnimation");
		return new LeafInvoke(
			() => this.affodant.gameObject.GetComponent<DoorScript> ().OpenDoor());
	}
}