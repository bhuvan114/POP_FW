using UnityEngine;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

namespace POPL.Planner
{
	public class Planner {

		Dictionary<Affordance, List<Affordance>> orderingConsts = new Dictionary<Affordance, List<Affordance>>();
		List<Affordance> actions = new List<Affordance>();
		List<CausalLink> causalLinks = new List<CausalLink>();
		Stack<Tuple<Condition, Affordance>> agenda = new Stack<Tuple<Condition, Affordance>>();
		List<Affordance> allPossibleActions = new List<Affordance>();

		void instantiatePlan(Affordance start, Affordance goal) {

			addToOrderingConstraints (start, goal);
			actions.Add (start);
			actions.Add (goal);
			addActionToAgenda (goal);
			allPossibleActions = Constants.allPossibleAffordances;

		}

		void addToOrderingConstraints(Affordance key, Affordance value) {
			
			if (orderingConsts.ContainsKey(key)){
				if(!orderingConsts[key].Contains(value))
					orderingConsts[key].Add(value);
			} else {
				List<Affordance> listActions = new List<Affordance> ();
				listActions.Add (value);
				orderingConsts.Add (key, listActions);
			}
		}

		void addActionToAgenda(Affordance act) {

			foreach (Condition p in act.getPreconditions()) {
				
				agenda.Push(Tuple.New(p, act));
			}
		}

		bool preConditionAlreadySatisfied(Tuple<Condition, Affordance> g, out Affordance satAct) {
			
			satAct = null;
			
			foreach(Affordance act in actions) {
				List<Condition> actEffects = act.getEffects();
				foreach(Condition effect in actEffects) {
					if(effect.Equals(g.First)) {
						satAct = act;
						return true;
					}
				}
			}
			return false;
		}

		bool checkActionsForPrecondition(Tuple<Condition, Affordance> g, out Affordance satAct) {
			
			satAct = null;
			
			//g.First.disp ();
			
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
			}
			return false;
		}

		void resolveConflicts(CausalLink cl, Affordance act) {
			
			foreach (Condition effect in act.getEffects()) {
				if(!(cl.act1.Equals(act)) && !(cl.act2.Equals(act)) && (effect.isNegation(cl.p))) {
					
					//Debug.Log("resolveConflicts : ");
					//act.disp();
					//cl.disp();
					if(cl.act2.isGoal())
						addToOrderingConstraints(act, cl.act1);
					else if(!cl.act1.isStart()) 
						addToOrderingConstraints(cl.act2, act);

				}
			}
		}

		public /*bool*/ void computePlan(Affordance start, Affordance goal) {

			instantiatePlan (start, goal);
			Debug.Log (agenda.Count);
			do {
				Tuple<Condition, Affordance> subG = agenda.Pop();
				Affordance act;
				if(!preConditionAlreadySatisfied(subG, out act)) {
					
					if (checkActionsForPrecondition(subG, out act)) {
						
						actions.Add(act);
						addToOrderingConstraints(start, act);
						foreach(CausalLink cl in causalLinks) {
							
							resolveConflicts(cl, act);
						}
						
						addActionToAgenda(act);
						
					} else {
						Debug.LogError("Plan Failed for condition - ");
						subG.First.disp();
						//showActions ();
						//return false;
					}
				}

				addToOrderingConstraints(act, subG.Second);
				CausalLink cLink = new CausalLink(act, subG.First, subG.Second);
				causalLinks.Add(cLink);
				
				foreach(Affordance a in actions) {
					
					if (!a.isStart())
						resolveConflicts(cLink, a);
				}
				
				//Debug.Log("In While loop!!");
			} while (agenda.Count() != 0);

			Debug.LogError("Goal Reached!!");
			//return true;
		}

		public Planner() { }

		public void showActions() {
			
			foreach (Affordance act in actions)
				act.disp ();
		}

		public void showOrderingConstraints() {
			
			foreach (Affordance key in orderingConsts.Keys) {
				Debug.LogWarning("Constraint : ");
				key.disp();
				List<Affordance> values = orderingConsts[key];
				Debug.Log("Happens before : ");
				foreach(Affordance act in values)
					act.disp();
			}
		}
	}
}