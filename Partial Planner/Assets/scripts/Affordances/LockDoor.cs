using UnityEngine;
using System.Collections;
using TreeSharpPlus;

using POPL.Planner;

public class LockDoor : Affordance
{

    DoorScript doorScript;
    Transform openPos;

    public LockDoor(SmartDoor afdnt, SmartCharacter afdee)
    {

        affodant = afdnt;
        affordee = afdee;
        initialize();
    }

    void initialize()
    {

        base.initialize();

        name = affordeeName + " locks " + affordantName + " door";
        preconditions.Add(new Condition(affordeeName, "HandsFree", true));
        preconditions.Add(new Condition(affordantName, "IsOpen", false));
		preconditions.Add(new Condition(affordeeName, affordantName, "HasKey", true));

        effects.Add(new Condition(affordantName, "IsLocked", true));
        treeRoot = this.execute();

    }

    //Behaviour Tree here
    public Node execute()
    {
        //Debug.LogError ("Execute");
        Debug.Log("Lock execute");
        return new Sequence(this.LockAnimation());
    }

    Node LockAnimation()
    {
        Debug.Log("LockAnimation");
        openPos = this.affodant.gameObject.GetComponent<DoorScript>().OpenPoint;
        return new Sequence(HelperFunctions.ST_ApproachAndWait(affordee.gameObject, openPos), HelperFunctions.ST_Turn(affordee.gameObject, openPos), affordee.gameObject.GetComponent<BehaviorMecanim>().Node_HandAnimation("GRAB", true), new LeafWait(2000));
    }
}
