using UnityEngine;
using System.Collections;
using TreeSharpPlus;

using POPL.Planner;

public class Die: Affordance {
	
	
	public Die(SmartCharacter afdnt, SmartCharacter afdee) {
		
		affodant = afdnt;
        affordee = afdee;
        initialize ();
	}
	
	void initialize() {
		
		base.initialize ();
		
		preconditions.Add (new Condition(affordeeName, "Shot", true));

		effects.Add(new Condition(affordantName, "Dead", true));
		
	}

    //Behaviour Tree here
    public Node execute()
    {
        Debug.Log("Die execute");
        return new Sequence(this.DieAnimation());
    }

    Node DieAnimation()
    {
        Debug.Log("ShootGunAnimation");
        return new Sequence(
            affodant.GetComponent<BehaviorMecanim>().Node_BodyAnimation("DYING", true), new LeafWait(500)           
            );
    }
}