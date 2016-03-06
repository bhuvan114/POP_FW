using UnityEngine;
using System.Collections;
using POPL.Planner;

public class main : MonoBehaviour {

	// Use this for initialization
	void Start () {
		HelperFunctions.initiatePlanSpace ();
		foreach (System.Type typesl in Constants.characterTypes.Keys)
			Debug.Log (typesl.ToString ());
		foreach (System.Type typesl in Constants.availableAffordances)
			Debug.Log (typesl.ToString ());
		foreach (Affordance aff in Constants.allPossibleAffordances)
			aff.disp ();

		Affordance start = new Affordance ();
		start.setStart ();
		Affordance goal = new Affordance ();
		goal.setGoal ();
		start.addEffects (new Condition("Person1", "Person2", "InScene", true));
		start.addEffects (new Condition("Person2", "Person1", "InScene", true));
		goal.addPrecondition (new Condition("Person1", "Person2", "Knows", true));
		Planner planner = new Planner ();
		planner.computePlan (start, goal);

		planner.showActions ();
		planner.showOrderingConstraints ();
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
