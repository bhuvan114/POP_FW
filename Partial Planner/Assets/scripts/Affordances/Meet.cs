using UnityEngine;
using System;
using System.Collections;
using TreeSharpPlus;

using POPL.Planner;

public class Meet : Affordance {

    private BehaviorAgent behaviorAgent;

    public Meet(SmartCharacter afdnt, SmartCharacter afdee) {
		
		affodant = afdnt;
		affordee = afdee;
		initialize ();
	}
	
	void initialize() {
		
		base.initialize ();
		preconditions.Add (new Condition(affordantName, affordeeName, "InScene", true));
		effects.Add(new Condition(affordeeName, affordantName, "Infront", true));
		effects.Add(new Condition(affordantName, affordeeName, "Infront", true));
	}
	
	//Behaviour Tree here
	public Node execute() {
        return BuildTreeRoot();
    }

    public Node ST_Meet()
    {
        Val<Vector3> position1 = Val.V(() => affodant.transform.position);
        Val<Vector3> position2 = Val.V(() => affordee.transform.position);

        return new SelectorShuffle(
            new Sequence(
                affodant.GetComponent<BehaviorMecanim>().Node_GoToUpToRadius(position2, 1),
                ST_Orient()
                ),
            new Sequence(
                affordee.GetComponent<BehaviorMecanim>().Node_GoToUpToRadius(position1, 1),
                ST_Orient()
                )
            );
    }
    protected Node ST_Orient()
    {
        Val<Vector3> p1pos = Val.V(() => affodant.transform.position);
        Val<Vector3> p2pos = Val.V(() => affordee.transform.position);
        return
            new SequenceParallel(
            affodant.GetComponent<BehaviorMecanim>().ST_TurnToFace(p2pos),
            affordee.GetComponent<BehaviorMecanim>().ST_TurnToFace(p1pos),
            new LeafWait(1000));
    }

    protected Node BuildTreeRoot()
    {
        Node meet = new DecoratorLoop(
                            ST_Meet());

        return meet;
    }
}
