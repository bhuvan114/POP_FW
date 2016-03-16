using UnityEngine;
using System.Collections;
using POPL.Planner;

public class main : MonoBehaviour {

	// Use this for initialization
	void Start () {

		Affordance start = new Affordance ();
		start.setStart ();
		Affordance goal = new Affordance ();
		goal.setGoal ();

		//START STATE
		start.addEffects (new Condition("Person1", "InScene", true));
		start.addEffects(new Condition("Person1", "HandsFree", true));
		start.addEffects(new Condition("Door", "IsOpen", false));
		start.addEffects(new Condition("Store", "HasGun", true));
		start.addEffects(new Condition("Person1", "HasMoney", true));
		//goal.addPrecondition (new Condition("Person1", "Person2", "Knows", true));
		goal.addPrecondition (new Condition("Person1", "Gun", "IsDrawn", true));
		
		Planner planner = new Planner ();
		


		System.Diagnostics.Stopwatch swatch = new System.Diagnostics.Stopwatch ();


		//HelperFunctions.initiatePlanSpace ();

		HelperFunctions.initiatePlanSpace_v2 ();
		/*foreach (System.Type typesl in Constants.characterTypes.Keys)
			Debug.Log (typesl.ToString ());
		foreach (System.Type typesl in Constants.availableAffordances)
			Debug.Log (typesl.ToString ());
		foreach (string cond in Constants.affordanceRelations.Keys) {
			foreach(bool status in Constants.affordanceRelations[cond].Keys)
				Debug.Log(cond + status.ToString());
		}*/
		//foreach (Affordance aff in Constants.allPossibleAffordances)
		//	aff.disp ();

		swatch.Start ();
		planner.computePlan (start, goal);
		swatch.Stop ();

		planner.showActions ();
		planner.showOrderingConstraints ();

		//Debug.Log (swatch.Elapsed.ToString());
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
