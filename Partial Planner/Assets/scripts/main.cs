using UnityEngine;
using System.Collections;
using POPL.Planner;
using TreeSharpPlus;

public class main : MonoBehaviour {

	public GameObject ObjectiveUIPanel;
	public GameObject objectiveFailure;
    public DataCollection userMetrics;
    private bool hasPlan;
	private BehaviorAgent behaviorAgent;

	// Use this for initialization
	void Start () {

		HelperFunctions.initiatePlanSpace_v2 ();
		hasPlan = false;

		NarrativeState.AddCondition (new Condition("Assasin", "InScene", true));
		NarrativeState.AddCondition(new Condition("Assasin", "HandsFree", true));
		NarrativeState.AddCondition(new Condition("StoreDoor", "IsOpen", false));
		NarrativeState.AddCondition(new Condition("GunStore", "HasGun", true));
		NarrativeState.AddCondition(new Condition("Assasin", "HasMoney", true));

		NarrativeState.AddCondition (new Condition("Dealer", "InScene", true));
		NarrativeState.AddCondition(new Condition("Dealer", "HandsFree", true));
		NarrativeState.AddCondition(new Condition("Dealer", "HasMoney", true));

		NarrativeState.AddCondition (new Condition("Player", "InScene", true));
		NarrativeState.AddCondition(new Condition("Player", "HandsFree", true));
		NarrativeState.AddCondition(new Condition("Player", "HasMoney", true));
		this.computePlan ();
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
		//goal.addPrecondition (new Condition("Assasin", "HasGun", true));
		goal.addPrecondition (new Condition("Assasin", "Gun", "IsDrawn", true));
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
            userMetrics.setIsRunning(false);
			hasPlan = false;
			Debug.Log("hasPlan false");
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
	}

	public Node TeriminatePlan () {
		
		return new LeafInvoke(
			() => this.TerimateNarrative());
	}	
	
	public void TerimateNarrative() {
        userMetrics.setIsRunning(false);
		objectiveFailure.SetActive (true);
		Cursor.visible = true;
		Cursor.lockState = CursorLockMode.None;
		Time.timeScale = 0f;
	}

}
