using UnityEngine;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

namespace POPL.Planner {
	public class DynamicPlanner {

		Affordance start, goal;
		Stack<Tuple<Condition, Affordance>> agenda = new Stack<Tuple<Condition, Affordance>>();
		/*
		Dictionary<Affordance, List<Affordance>> orderingConsts = new Dictionary<Affordance, List<Affordance>>();
		List<Affordance> actions = new List<Affordance>();
		List<CausalLink> causalLinks = new List<CausalLink>();
		List<Affordance> allPossibleActions = new List<Affordance>();
		Dictionary<string, List<Affordance>> possibleActions = Constants.possibleActionsMap;

		void instantiatePlan(Affordance start, Affordance goal) {

			//start = strt;
			//goal = gl;
			//addToOrderingConstraints (start, goal);
			actions.Add (start);
			actions.Add (goal);
			//addActionToAgenda (goal);
			allPossibleActions = Constants.allPossibleAffordances;

		}
		*/
		void InitiatePlan(){

			Debug.LogError ("Instantiate Plan!!");
			NarrativeStateManager.debugLog = "Initiating planner!!";
			start = NarrativeStateManager.GetStartState ();
			goal = NarrativeStateManager.GetGoalState ();
			//NarrativeStateManager.GetGoalState ().disp ();
			if (agenda.Count () == 0)
				AddActionToAgenda (goal);
			NarrativeStateManager.affordances.Add (start);
			if(!NarrativeStateManager.affordances.Contains(goal))
				NarrativeStateManager.affordances.Add (goal);

			foreach(Affordance act in NarrativeStateManager.affordances)
				if(!act.isStart())
					AddToOrderingConstraints (start, act);
			
		}

		void AddToOrderingConstraints(Affordance key, Affordance value) {

			if (NarrativeStateManager.constraints.ContainsKey(key)){
				if(!NarrativeStateManager.constraints[key].Contains(value))
					NarrativeStateManager.constraints[key].Add(value);
			} else {
				List<Affordance> listActions = new List<Affordance> ();
				listActions.Add (value);
				NarrativeStateManager.constraints.Add (key, listActions);
			}
		}

		void AddActionToAgenda(Affordance act) {

			foreach (Condition p in act.getPreconditions()) {

				agenda.Push(Tuple.New(p, act));
			}
		}

		bool PreConditionAlreadySatisfied(Tuple<Condition, Affordance> g, out Affordance satAct) {

			satAct = null;

			foreach(Affordance act in NarrativeStateManager.affordances) {
				List<Condition> actEffects = act.getEffects();
				foreach(Condition effect in actEffects) {
					if(effect.Equals(g.First) && !(NarrativeStateManager.constraints.ContainsKey(g.Second) && NarrativeStateManager.constraints[g.Second].Contains(act))) {
						satAct = act;
						return true;
					}
				}
			}
			return false;
		}

		bool CheckActionsForPrecondition(Tuple<Condition, Affordance> g, out Affordance satAct) {

			satAct = null;

			foreach (string affType in Constants.affordanceRelations[g.First.condition][g.First.status]) {

				foreach(Affordance act in Constants.possibleActionsMap[affType]) {
					List<Condition> actEffects = act.getEffects();
					foreach(Condition effect in actEffects) {
						if(effect.Equals(g.First)) {
							satAct = act;
							return true;
						}
					}
				}
			}
			/*
			foreach(Affordance act in allPossibleActions) {
				List<Condition> actEffects = act.getEffects();
				foreach(Condition effect in actEffects) {
					//Debug.Log ("allPossibleActions - " + effect.condition);
					//effect.disp();
					if(effect.Equals(g.First)) {
						//Debug.LogError ("true that -- " + effect.condition);
						//effect.disp();
						satAct = act;
						allPossibleActions.Remove(act);
						return true;
					}
				}
			}*/
			return false;
		}

		void ResolveConflicts(CausalLink cl, Affordance act) {

			foreach (Condition effect in act.getEffects()) {
				if(!(cl.act1.Equals(act)) && !(cl.act2.Equals(act)) && (effect.isNegation(cl.p))) {

					//Debug.Log("resolveConflicts : ");
					//act.disp();
					//cl.disp();
					if ((NarrativeStateManager.constraints.ContainsKey (cl.act2) && NarrativeStateManager.constraints [cl.act2].Contains (act))
					    || (NarrativeStateManager.constraints.ContainsKey (act) && NarrativeStateManager.constraints [act].Contains (cl.act1))) {
						//continue;
					} else {
						Debug.LogError ("Conflict - ");
						cl.disp ();
						act.disp ();
						Debug.LogError ("End Conflict - ");
						if (cl.act2.isGoal ())
							AddToOrderingConstraints (act, cl.act1);
						else if (!cl.act1.isStart ())
							AddToOrderingConstraints (cl.act2, act);
					}
				}
			}
		}

		public void AddInconsistenciesToAgenda (List<POPL.Planner.Tuple<Condition, Affordance>> inconsistencies) {
			agenda = new Stack<Tuple<Condition, Affordance>> ();
			foreach (POPL.Planner.Tuple<Condition, Affordance> inconsistency in inconsistencies)
				agenda.Push (inconsistency);
		}

		public bool ComputePlan(List<POPL.Planner.Tuple<Condition, Affordance>> inconsistencies) {

			AddInconsistenciesToAgenda (inconsistencies);
			return ComputePlan ();
		}

		public bool ComputePlan() {

			InitiatePlan ();
			//Debug.Log (agenda.Count);
			do {
				Tuple<Condition, Affordance> subG = agenda.Pop();
				Affordance act;
				if(!PreConditionAlreadySatisfied(subG, out act)) {

					//if (checkActionsForPrecondition(subG, out act)) {
					if(CheckActionsForPrecondition(subG, out act)) {
						NarrativeStateManager.affordances.Add(act);
						AddToOrderingConstraints(start, act);
						AddToOrderingConstraints(act, subG.Second);
						foreach(CausalLink cl in NarrativeStateManager.causalLinks) {

							ResolveConflicts(cl, act);
						}

						AddActionToAgenda(act);

					} else {
						Debug.LogError("Plan Failed for condition - ");
						subG.First.disp();
						//showActions ();
						return false;
					}
				} else {

					AddToOrderingConstraints(act, subG.Second);
				}
				CausalLink cLink = new CausalLink(act, subG.First, subG.Second);
				NarrativeStateManager.causalLinks.Add(cLink);

				foreach(Affordance a in NarrativeStateManager.affordances) {

					if (!a.isStart())
						ResolveConflicts(cLink, a);
				}

				//Debug.Log("In While loop!!");
			} while (agenda.Count() != 0);

			Debug.LogError("Goal Reached!!");
			return true;
		}

		public void ShowOrderingConstraints() {

			foreach (Affordance key in NarrativeStateManager.constraints.Keys) {
				Debug.LogWarning("Constraint : ");
				key.disp();
				List<Affordance> values = NarrativeStateManager.constraints[key];
				Debug.Log("Happens before : ");
				foreach(Affordance act in values)
					act.disp();
			}
		}

		public DynamicPlanner () { }

	}
}