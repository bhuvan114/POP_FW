using UnityEngine;
using System.Collections;
using TreeSharpPlus;

using POPL.Planner;

public class EnterStore : Affordance {

	private Transform interactionPoint;
	
	public EnterStore(SmartStore afdnt, SmartCharacter afdee) {

		interactionPoint = HelperFunctions.GetChildGameObjectByName(afdnt.gameObject, "InteractionPoint").transform;
		affodant = afdnt;
		affordee = afdee;
		initialize ();
		preconditions.Add(new Condition(afdnt.storeDoor.name, "IsOpen", true));
	}
	
	void initialize() {
		
		base.initialize ();

		effects.Add(new Condition(affordeeName, affordantName, "InStore", true));

		treeRoot = this.execute ();
		
	}
	
	//Behaviour Tree here
	public Node execute() {

		return new Sequence (HelperFunctions.ST_ApproachAndWait (affordee.gameObject, interactionPoint), HelperFunctions.ST_Turn (affordee.gameObject, interactionPoint));
	}
}