using UnityEngine;
using System.Collections;
using TreeSharpPlus;

using POPL.Planner;

public class DrawGun : Affordance {
	
	
	public DrawGun(SmartCharacter afdnt, SmartGun afdee) {
		
		affodant = afdnt;
		affordee = afdee;
		initialize ();
	}
	
	void initialize() {
		
		base.initialize ();
		
		preconditions.Add(new Condition(affordantName, "HandsFree", true));
		preconditions.Add (new Condition(affordantName, "HasGun", true));

		effects.Add(new Condition(affordantName, "HandsFree", false));
		effects.Add(new Condition(affordantName, affordeeName, "IsDrawn", true));
		
	}

    //Behaviour Tree here
    public Node execute()
    {
        Debug.Log("DrawGun execute");
        return new Sequence(this.DrawGunAnimation());
    }

    Node DrawGunAnimation()
    {
        Debug.Log("DrawGunAnimation");
        return new Sequence(
            affodant.GetComponent<BehaviorMecanim>().Node_HandAnimation("PISTOLAIM", true), new LeafWait(500),
            new LeafInvoke(() => affordee.gameObject.GetComponent<GunController>().SetIsHolding(true)));
    }
}