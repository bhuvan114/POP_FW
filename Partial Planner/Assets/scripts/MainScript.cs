using UnityEngine;
using System.Collections;
using POPL.Planner;
using TreeSharpPlus;

public class MainScript : MonoBehaviour {

	public GameObject ObjectiveUIPanel;
	public GameObject objectiveFailure;
	public DataCollection userMetrics;
	private bool hasPlan;
	private BehaviorAgent behaviorAgent;

	// Use this for initialization
	void Start () {
		
		GameObject[] smartObjects;
		NarrativeStateManager.debugLog = "Before calling FindGameObjects11";

		smartObjects = GameObject.FindGameObjectsWithTag ("SmartObject");
		NarrativeStateManager.debugLog = "Before calling helper!!";
		HelperFunctions.initiatePlanSpace_v2 (smartObjects);
		NarrativeStateManager.debugLog = "Before calling NSM!!";
		NarrativeStateManager.InitiateNarrativeStateManager ();

		//this.computePlan ();
	}

	public void computePlan(){

		Debug.Log ("Compute Plan Called");
		Affordance start = new Affordance ();
		start.setStart ();
		Affordance goal = new Affordance ();
		goal.setGoal ();

		//START STATE
		Debug.Log ("In compute plan");
		foreach (Condition cond in NarrativeState.GetNarrativeState()) {
			cond.disp();
			start.addEffects (cond);
		}
		Debug.Log ("current state!!");
		/*start.addEffects (new Condition("Assasin", "InScene", true));
		start.addEffects(new Condition("Assasin", "HandsFree", true));
		start.addEffects(new Condition("StoreDoor", "IsOpen", false));
		start.addEffects(new Condition("GunStore", "HasGun", true));
		start.addEffects(new Condition("Assasin", "HasMoney", true));
		start.addEffects (new Condition("Dealer", "InScene", true));
		start.addEffects(new Condition("Dealer", "HandsFree", true));
		start.addEffects(new Condition("Dealer", "HasMoney", true));
		*/
		//goal.addPrecondition (new Condition("Person1", "Person2", "Knows", true));
		goal.addPrecondition (new Condition("Assasin", "HasGun", true));
		//goal.addPrecondition (new Condition("Assasin", "Gun", "IsDrawn", true));
		Planner planner = new Planner ();



		System.Diagnostics.Stopwatch swatch = new System.Diagnostics.Stopwatch ();


		//HelperFunctions.initiatePlanSpace ();


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
		if (!planner.computePlan (start, goal)) {
			//userMetrics.setIsRunning(false);
			hasPlan = false;
			//Debug.Log("hasPlan false");
			ObjectiveUIPanel.SetActive(true);
			NarrativeState.terminated = true;
			Cursor.visible = true;
			Cursor.lockState = CursorLockMode.None;
			Time.timeScale = 0f;
		} else {

			hasPlan = true;
			NarrativeState.SetAffordances(planner.getActions());
			NarrativeState.SetCausalLinks(planner.getCausalLinks());
			NarrativeState.SetOrderingConstraints(planner.getConstarints());
			//NarrativeState.GenerateNarrative();
			NarrativeState.GenerateNarrative_V2();
			Time.timeScale = 1f;
		}
		swatch.Stop ();

		planner.showActions ();
		planner.showOrderingConstraints ();


	}

	// Update is called once per frame
	void Update () {
		/*
		if (hasPlan && !NarrativeState.root.IsRunning) {

			NarrativeState.root = new Sequence(NarrativeState.root , this.TeriminatePlan());
			behaviorAgent = new BehaviorAgent (NarrativeState.root);
			BehaviorManager.Instance.Register (behaviorAgent);
			behaviorAgent.StartBehavior ();
		}

		if (NarrativeState.root != null) {
			if(!NarrativeState.root.IsRunning) {
				Time.timeScale = 0f;
				Debug.Log (NarrativeState.root.LastStatus.ToString ());
			}
		}

		if (NarrativeState.recomputePlan) {
			userMetrics.incrementActions();
			NarrativeState.recomputePlan = false;
			this.computePlan();
		}
		*/
		if (NarrativeStateManager.hasPlan == false && NarrativeStateManager.isPlanning == false) {
			Debug.LogWarning ("Planner Failed!!");
			ObjectiveUIPanel.SetActive(true);
			Cursor.visible = true;
			Cursor.lockState = CursorLockMode.None;
			Time.timeScale = 0f;
			//userMetrics.setIsRunning(false);
			userMetrics.publishData();
		}

		if (NarrativeStateManager.hasPlan == true && !NarrativeStateManager.root.IsRunning) {

			NarrativeStateManager.root = new Sequence (NarrativeStateManager.root, this.TeriminatePlan ());
			behaviorAgent = new BehaviorAgent (NarrativeStateManager.root);
			BehaviorManager.Instance.Register (behaviorAgent);
			behaviorAgent.StartBehavior ();
		}
	}

	public Node TeriminatePlan () {

		return new LeafInvoke(
			() => this.TerimateNarrative());
	}	

	public void TerimateNarrative() {
		//userMetrics.setIsRunning(false);
		userMetrics.publishData();
		objectiveFailure.SetActive (true);
		Cursor.visible = true;
		Cursor.lockState = CursorLockMode.None;
		Time.timeScale = 0f;
	}


	void OnGUI() {
		/*if (NarrativeStateManager.hasPlan == false) {
			GUI.color = Color.white;
			GUI.Box(new Rect(20, 20, 200, 25), NarrativeStateManager.debugLog);
		}*/
		if (NarrativeStateManager.isJournalUpdated)
		{
			GUI.color = Color.white;
			if (NarrativeStateManager.first) {
				GUI.Box (new Rect (40, Screen.height - 30, 250, 25), "Press 'J' to view the journal!!");
			} else {
				GUI.Box (new Rect (40, Screen.height - 30, 250, 25), "Ughh!! You changed the past!! ('J')");
			}
		}
	}
}
