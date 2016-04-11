using UnityEngine;
using System.Collections;
using TreeSharpPlus;

using POPL.Planner;

public class BuyStore : Affordance
{


    public BuyStore(SmartStore afdnt, SmartCharacter afdee)
    {

        //storeDealer = HelperFunctions.GetChildGameObjectByName (afdnt.gameObject, "Dealer");
        affodant = afdnt;
        affordee = afdee;
        initialize();
    }

    void initialize()
    {

        base.initialize();

        name = affordeeName + " buys store from " + affordantName;
        //preconditions.Add(new Condition(affordantName, "HasStore", true));
        preconditions.Add(new Condition(affordeeName, "HasMoney", true));
        preconditions.Add(new Condition(affordeeName, affordantName, "InStore", true));

        //effects.Add(new Condition(affordantName, "HasStore", false));
        effects.Add(new Condition(affordeeName, "HasMoney", false));
        //effects.Add(new Condition(affordeeName, "HasStore", true));
        effects.Add(new Condition(affordeeName, "HasKey", true));
        treeRoot = this.execute();

    }

    //Behaviour Tree here
    public Node execute()
    {

        Debug.Log ("Buying Store ");
        return new Sequence(affordee.gameObject.GetComponent<BehaviorMecanim>().Node_HandAnimation("GRAB", true) /*storeDealer.gameObject.GetComponent<BehaviorMecanim> ().Node_HandAnimation ("GRAB", true)*/
                             );
    }
}
