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
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
