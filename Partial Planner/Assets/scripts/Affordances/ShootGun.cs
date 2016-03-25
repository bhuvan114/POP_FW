using UnityEngine;
using System.Collections;
using TreeSharpPlus;

using POPL.Planner;

public class ShootGun : Affordance {
	
	
	public ShootGun(SmartCharacter afdnt, SmartGun afdee) {
		
		affodant = afdnt;
		affordee = afdee;
		initialize ();
	}
	
	void initialize() {
		
		base.initialize ();
		
		preconditions.Add (new Condition(affordeeName, "IsDrawn", true));
        //preconditions.Add (new Condition(affordeeName, "HasAmmo", true));

		effects.Add(new Condition(affordantName, affordeeName, "HasFired", true));
		//effects.Add(new Condition(affordantName, affordeeName, "IsDrawn", true));
		
	}

    //Behaviour Tree here
    public Node execute()
    {
        Debug.Log("ShootGun execute");
        return new Sequence(this.ShootGunAnimation());
    }

    Node ShootGunAnimation()
    {
        Debug.Log("ShootGunAnimation");
        return new Sequence(
            affodant.GetComponent<BehaviorMecanim>().Node_HandAnimation("PISTOLFIRE", true), new LeafWait(500),
            affodant.GetComponent<BehaviorMecanim>().Node_HandAnimation("PISTOLFIRE", false), new LeafWait(500),
            affodant.GetComponent<BehaviorMecanim>().Node_HandAnimation("PISTOLAIM", true), new LeafWait(500)
            );
    }
}