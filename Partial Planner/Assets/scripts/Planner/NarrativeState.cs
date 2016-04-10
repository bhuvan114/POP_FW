using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using TreeSharpPlus;

using POPL.Planner;

public static class NarrativeState {

	private static List<Condition> conditions =  new List<Condition>();
	private static List<Condition> conditions_NoPlayer =  new List<Condition>();
	private static List<Affordance> actions = new List<Affordance>();
	private static List<CausalLink> causalLinks = new List<CausalLink>();
	private static Dictionary<Affordance, List<Affordance>> constraints = new Dictionary<Affordance, List<Affordance>> ();
	private static Dictionary<Affordance, int> affOrder = new Dictionary<Affordance, int> ();
	private static string journalMessage;
	public static bool terminated = false;
	//private static Dictionary<int, List<Affordance>> affOrder = new Dictionary<int, List<Affordance>> ();

	public static Node root = null;
	public static bool recomputePlan = false;

	public static void AddCondition(Condition cond) {

		//cond.disp ();
		//foreach (Condition effect in conditions) {
		for(int ind = 0; ind < conditions.Count; ind++) {
			//if(effect.isNegation(cond)) {
			if(conditions[ind].isNegation(cond)) {
				//Debug.Log ("Negation");
				//effect.disp ();
				//cond.disp ();
				conditions[ind] = cond;
				//conditions.Remove(effect);
				//conditions.Add(cond);
				/*if (cond.actor1 != "Player" && cond.actor2 != "Player") {
					conditions_NoPlayer.Remove(effect);
					conditions_NoPlayer.Add(cond);
				}*/
				return;
			} else if (conditions[ind].Equals(cond)) {

				return;
			}
		}
		//if (cond.actor1 != "Player" && cond.actor2 != "Player")
		//	conditions_NoPlayer.Add(cond);
		Debug.Log("New Affordance");
		cond.disp ();
		conditions.Add (cond);
	}

	public static List<Condition> GetNarrativeState() {

		return conditions;
	}

	public static List<Condition> GetNarrativeStateWithoutPlayer() {

		return conditions_NoPlayer;
	}

	public static void SetAffordances(List<Affordance> acts) {

		actions = acts;
	}

	public static void SetCausalLinks(List<CausalLink> cLinks) {

		causalLinks = cLinks;
	}

	public static void SetOrderingConstraints (Dictionary<Affordance, List<Affordance>> consts) {

		constraints = consts;
	}

	public static List<Affordance> GetAffordances() {

		return actions;
	}
	
	public static List<CausalLink> GetCausalLinks() {

		return causalLinks;
	}
	
	public static Dictionary<Affordance, List<Affordance>> GetOrderingConstraints () {

		return constraints;
	}

	public static void GenerateNarrative() {

		List<Node> affSTs = new List<Node>();
		foreach (Affordance aff in actions) {
			if(!aff.isGoal() && !aff.isStart()) {

				affSTs.Add(new Sequence(aff.GetSubTree(), aff.UpdateState ()));
			}
		}
		//Debug.Log (actions.Count.ToString () + " , " + affSTs.Count.ToString ());
		affSTs.Reverse ();
		root = new Sequence(affSTs.ToArray());
	}

	public static void GenerateNarrative_V2() {

		affOrder = new Dictionary<Affordance, int> ();
		Debug.LogWarning ("GenerateNarrative_V2");
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
		journalMessage = "- Day started\n";
		Debug.Log ("Indx = " + indx);
		for (int i=1; i<indx; i++) {
			//Debug.Log ("i = " + i);
			//affs = (from aff in affOrder where aff.Value == indx select aff.Key);
			affs = affOrder.Keys.Where (t => affOrder [t] == i);
			foreach(Affordance act in affs) {
				if(!act.isGoal() && !act.isStart()) {

					//act.disp();
					affSTs.Add(new Sequence(act.GetSubTree(), act.UpdateState ()));

					journalMessage = journalMessage + "- " + act.name + "\n";
				}
			}
		}
		root = new Sequence(affSTs.ToArray());
		//UpdateJournal (journalMessage);
	}

	public static void UpdateJournal() {

		Text journal = GameObject.Find ("JournalText").GetComponent<Text>();
		journal.text = journalMessage;
	}
}
