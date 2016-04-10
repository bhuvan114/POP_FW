﻿using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using TreeSharpPlus;

using POPL.Planner;

public static class NarrativeStateManager {

	/* Functions

		1. InitiateNarrativeStateManager - Initiates current state, assigns that as start state to the planner
		2. SetGoalState - Sets the goal state for the planner
		3. UpdateAffordanceStatus - If affordance is executed, move affordance from actions to executedActions list
		4. UpdateCausalLinksStatus - If act0 is executed, move to runningCLs list; if act1 is executed, move to executedCLs list
		5. CheckInconsistencies -
			UnderConsistent - If a causal link from runningCLs is negated, act1 is UnderConsistent
			OverConsistent - If a causal link from causalLinks is already executed, ac1 is OverConsistent
		6. PropogateConsistency - For an over consistent state, propagate consestency upward in the causal link heirarchy
		7. AddToAgenda - Add a <condition, affordance> to the planner
		8. ConstructBehaviourTree
		9. UpdatePlanSpace - calls UpdateNarrativeState, UpdateAffordanceStatus, UpdateCausalLinksStatus
		10.UpdateNarrativeState - Updates the current state of the narrative
		11.AddConditionToNarrativeState - Adds a condition to narrative state
	*/

	/*
	private static List<Condition> conditions_NoPlayer =  new List<Condition>();
	private static Dictionary<Affordance, List<Affordance>> constraints = new Dictionary<Affordance, List<Affordance>> ();
	private static Dictionary<Affordance, int> affOrder = new Dictionary<Affordance, int> ();
	private static string journalMessage;
	public static bool terminated = false;
	*/

	static Affordance start, goal;
	static bool isConsistent, isReplanRequired, hasPlan;
	private static DynamicPlanner planner;

	private static List<Condition> currentState =  new List<Condition>();
	public static Dictionary<Affordance, List<Affordance>> constraints = new Dictionary<Affordance, List<Affordance>> ();

	public static List<Affordance> actions = new List<Affordance>();
	private static List<Affordance> executedActions = new List<Affordance>();

	public static List<CausalLink> causalLinks = new List<CausalLink>();
	private static List<CausalLink> runningCLs = new List<CausalLink>();
	private static List<CausalLink> executedCLs = new List<CausalLink>();

	public static Node root = null;

	static void ResetStartState() {

		start = new Affordance();
		start.setStart ();
		foreach(Condition cond in currentState)
			start.addEffects(cond);
	}

	static void ResetRunningCLs () {

		for(int i = 0; i < causalLinks.Count; i++) {
			if (causalLinks[i].act1.Equals (start)) {

				runningCLs.Add (causalLinks [i]);
				causalLinks.RemoveAt (i);
				i--;
			}
		}
	}

	static void AddConditionToNarrativeState(Condition cond) {

		for(int ind = 0; ind < currentState.Count; ind++) {
			if(currentState[ind].isNegation(cond)) {
				currentState[ind] = cond;
				return;
			} else if (currentState[ind].Equals(cond)) {
				return;
			}
		}
		currentState.Add (cond);
	}

	static void UpdateNarrativeState(Affordance action) {

		foreach (Condition effect in action.getEffects())
			AddConditionToNarrativeState (effect);
	}

	static void UpdateAffordanceStatus(Affordance action) {

		actions.Remove (action);
		executedActions.Add (action);
	}

	static void UpdateCausalLinksStatus(Affordance action) {

		for(int i = 0; i < causalLinks.Count; i++) {
			if (causalLinks[i].act1.Equals (action)) {

				runningCLs.Add (causalLinks [i]);
				causalLinks.RemoveAt (i);
				i--;
			}
		}

		for(int i = 0; i < runningCLs.Count; i++) {
			if (runningCLs[i].act2.Equals (action)) {

				executedCLs.Add (runningCLs [i]);
				runningCLs.RemoveAt (i);
				i--;
			}
		}
	}

	static void RemoveActionFromActionsAndOrderingConstraints(Affordance action) {

		if (actions.Contains (action))
			actions.Remove (action);

		if (constraints.ContainsKey (action))
			constraints.Remove (action);

		foreach (Affordance key in constraints.Keys) {
			if (constraints [key].Contains (action))
				constraints [key].Remove (action);
		}
	}

	static void PropogateConsistency(List<Affordance> affs) {

		List<Affordance> overConsistentActions = new List<Affordance> ();
		foreach(Affordance act in affs) {
			int count = 0;
			for (int i = 0; i < causalLinks.Count (); i++) {
				
				if (causalLinks [i].act1.Equals (act))
					count++;
			}

			/*
			if (count == 0) {
				for (int i = 0; i < runningCLs.Count (); i++) {

					if (causalLinks [i].act2.Equals (act))
						count++;
				}
			}
*/
			//If there are no causal links dependent on act
			if (count == 0) {
				for (int i = 0; i < causalLinks.Count (); i++) {

					if (causalLinks [i].act2.Equals (act)) {
						overConsistentActions.Add (causalLinks [i].act1);
						causalLinks.RemoveAt (i);
						i--;
					}
				}

				for (int i = 0; i < runningCLs.Count (); i++) {

					if (runningCLs [i].act2.Equals (act)) {
						//overConsistentActions.Add (causalLinks [i].act1);
						runningCLs.RemoveAt (i);
						i--;
					}
				}

				RemoveActionFromActionsAndOrderingConstraints(act);
			}
		}

		if (overConsistentActions.Count () > 0)
			PropogateConsistency (overConsistentActions);
	}

	static void CheckInconsistencies(Affordance action) {

		List<POPL.Planner.Tuple<Condition, Affordance>> inconsistencies = new List<POPL.Planner.Tuple<Condition, Affordance>> ();
		List<Affordance> overConsistentActions = new List<Affordance> ();
		isConsistent = true;
		isReplanRequired = false;

		//Check for OverConsistency
		foreach (Condition effect in action.getEffects()) {
			for (int i = 0; i < causalLinks.Count (); i++) {
				if (causalLinks [i].p.Equals (effect)) {

					//causalLinks.Add (new CausalLink(start, causalLinks [i].p, causalLinks [i].act2));
					//planner.AddOrderingConstraint (start, causalLinks [i].act2);
					inconsistencies.Add (new POPL.Planner.Tuple<Condition, Affordance> (causalLinks [i].p, causalLinks [i].act2));
					overConsistentActions.Add (causalLinks [i].act1);
					causalLinks.RemoveAt (i);
					i--;
					isConsistent = false;
				}
			}
		}

		if (overConsistentActions.Count() > 0)
			PropogateConsistency (overConsistentActions);

		//Check for UnderConsistency
		foreach (Condition effect in action.getEffects()) {
			for (int i = 0; i < runningCLs.Count (); i++) {

				if (runningCLs [i].p.isNegation (effect)) {
					inconsistencies.Add (new POPL.Planner.Tuple<Condition, Affordance> (runningCLs [i].p, runningCLs [i].act2));
					runningCLs.RemoveAt (i);
				}
			}
		}

		if (inconsistencies.Count () > 0) {
			foreach (CausalLink cl in runningCLs)
				inconsistencies.Add (new POPL.Planner.Tuple<Condition, Affordance> (cl.p, cl.act2));
			runningCLs = new List<CausalLink> ();
			isConsistent = false;
			isReplanRequired = true;
			root = null;
		}

		if (isReplanRequired) {
			if (planner.AddToAgenda (inconsistencies)) {
				hasPlan = true;
				ResetRunningCLs ();
			} else {
				hasPlan = false;
			}
		}
	}

	public static void ConstructBehaviourTree () {

		Dictionary<Affordance, int> affOrder = new Dictionary<Affordance, int> ();
		int indx = actions.Count;
		foreach (Affordance act in actions) {
			if(act.isStart()) {
				affOrder.Add(act, 1);
			} else {
				affOrder.Add(act, indx);
			}
		}

		indx = 1;
		//IEnumerable<Affordance> affs = (from aff in affOrder where aff.Value == indx select aff.Key);
		IEnumerable<Affordance> affs = affOrder.Keys.Where (t => affOrder [t] == indx);
		List<Affordance> afds;
		while(affs.Count() != 0) {
			//Debug.Log(indx);
			afds = new List<Affordance>();
			foreach (Affordance act in affs) {
				if (constraints.ContainsKey(act)) {
					foreach(Affordance child in constraints[act])
						afds.Add(child);
				}
			}

			foreach(Affordance afd in afds)
				affOrder[afd] = indx + 1;

			indx = indx + 1;
			affs = (from aff in affOrder where aff.Value == indx select aff.Key);
		}
		/*
		foreach(Affordance act in affOrder.Keys) {

			act.disp();
			Debug.LogWarning(affOrder[act]);
		}*/
		List<Node> affSTs = new List<Node>();
		//journalMessage = "- Day started\n";
		Debug.Log ("Indx = " + indx);
		for (int i=1; i<indx; i++) {
			//Debug.Log ("i = " + i);
			//affs = (from aff in affOrder where aff.Value == indx select aff.Key);
			affs = affOrder.Keys.Where (t => affOrder [t] == i);
			foreach(Affordance act in affs) {
				if(!act.isGoal() && !act.isStart()) {

					//act.disp();
					affSTs.Add(new Sequence(act.GetSubTree(), act.UpdateState ()));

					//journalMessage = journalMessage + "- " + act.name + "\n";
				}
			}
		}
		root = new Sequence(affSTs.ToArray());
		//UpdateJournal (journalMessage);
	}

	static void InitiateNarrativeStateManager() {

		AddConditionToNarrativeState(new Condition("Assasin", "InScene", true));
		AddConditionToNarrativeState(new Condition("Assasin", "HandsFree", true));
		AddConditionToNarrativeState(new Condition("StoreDoor", "IsOpen", false));
		AddConditionToNarrativeState(new Condition("GunStore", "HasGun", true));
		AddConditionToNarrativeState(new Condition("Assasin", "HasMoney", true));

		AddConditionToNarrativeState(new Condition("Dealer", "InScene", true));
		AddConditionToNarrativeState(new Condition("Dealer", "HandsFree", true));
		AddConditionToNarrativeState(new Condition("Dealer", "HasMoney", true));

		AddConditionToNarrativeState(new Condition("Player", "InScene", true));
		AddConditionToNarrativeState(new Condition("Player", "HandsFree", true));
		AddConditionToNarrativeState(new Condition("Player", "HasMoney", true));

		ResetStartState ();

		Affordance goal = new Affordance ();
		goal.setGoal ();
		goal.addPrecondition (new Condition("Assasin", "Gun", "IsDrawn", true));

		hasPlan = false;
		List<POPL.Planner.Tuple<Condition, Affordance>> inconsistencies = new List<POPL.Planner.Tuple<Condition, Affordance>> ();
		foreach (Condition cond in goal.getPreconditions())
			inconsistencies.Add (new POPL.Planner.Tuple<Condition, Affordance> (cond, goal));

		planner = new DynamicPlanner ();

		if (planner.AddToAgenda (inconsistencies)) {
			hasPlan = true;
			ResetRunningCLs ();
		}
	}

	public static void UpdatePlanSpace(Affordance action) {

		UpdateNarrativeState (action);
		UpdateAffordanceStatus (action);
		UpdateCausalLinksStatus (action);
	}

	public static void UpdatePlanSpaceForUserAction (Affordance action) {

		UpdateNarrativeState (action);
		CheckInconsistencies (action);
		if (isReplanRequired && hasPlan) {
			ConstructBehaviourTree ();
		}
	}

	public static bool IsPlanExists () {
		
		return hasPlan;
	}

	public static bool IsPlanRecomputed () {

		return isReplanRequired;
	}

	public static void ResetReplan () {

		isReplanRequired = false;
	}
}