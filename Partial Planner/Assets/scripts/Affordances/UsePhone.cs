﻿using UnityEngine;
using System.Collections;
using TreeSharpPlus;

using POPL.Planner;

public class UsePhone : Affordance
{


    public UsePhone(SmartCharacter afdnt, SmartCharacter afdee)
    {

        affodant = afdnt;
        initialize();
    }

    void initialize()
    {

        base.initialize();

        preconditions.Add(new Condition(affordeeName, "HasPhone", true));
        preconditions.Add(new Condition(affordeeName, "HandsFree", true));

        effects.Add(new Condition(affordantName, "PoliceCalled", true));

    }

    //Behaviour Tree here
    public Node execute()
    {
        Debug.Log("UsePhone execute");
        return new Sequence(this.UsePhoneAnimation());
    }

    Node UsePhoneAnimation()
    {
        Debug.Log("UsePhoneAnimation");
        return new Sequence(
            affodant.GetComponent<BehaviorMecanim>().Node_BodyAnimation("TALKING ON PHONE", true), new LeafWait(500)
            );
    }
}
    