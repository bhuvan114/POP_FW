using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using TreeSharpPlus;

using POPL.Planner;

public static class NarrativeState {

	private static List<Condition> conditions =  new List<Condition>();
	private static List<Affordance> actions = new List<Affordance>();
	private static List<CausalLink> causalLinks = new List<CausalLink>();
	private static Dictionary<Affordance, List<Affordance>> constraints = new Dictionary<Affordance, List<Affordance>> ();
	public static Node root = null;
	public static bool recomputePlan = false;

	public static void AddCondition(Condition cond) {

		//cond.disp ();
		foreach (Condition effect in conditions) {

			if(effect.isNegation(cond)) {

				conditions.Remove(effect);
				conditions.Add(cond);
				return;
			} else if (effect.Equals(cond)) {

				return;
			}
		}

		conditions.Add (cond);
	}

	public static List<Condition> GetNarrativeState() {

		return conditions;
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
}
