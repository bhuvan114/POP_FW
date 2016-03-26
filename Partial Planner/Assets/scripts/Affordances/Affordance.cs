using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using TreeSharpPlus;

namespace POPL.Planner
{
	public class Affordance {

		public SmartObject affodant, affordee;
		public string name = "";
		protected string affordantName, affordeeName;
		protected List<Condition> preconditions = new List<Condition>();
		protected List<Condition> effects = new List<Condition>();
		protected bool start = false; 
		protected bool goal = false;
		protected Node treeRoot = null;

		public Affordance() {
			//affodant = new SmartObject ("affodant");
			//affordee = new SmartObject ("affordee");
			affodant = GameObject.Find ("affodant").GetComponent<SmartObject> ();
			affordee = GameObject.Find ("affordee").GetComponent<SmartObject> ();
			initialize ();
		}

		protected void initialize() {

			name = this.GetType ().ToString();
			affordantName = affodant.name;
			affordeeName = affordee.name;
		}

		public List<Condition> getPreconditions() {
			
			return preconditions;
		}
		
		public List<Condition> getEffects() {
			
			return effects;
		}

		public void addPrecondition(Condition cond) {
			
			preconditions.Add (cond);
		}
		
		public void addEffects(Condition cond) {
			
			effects.Add (cond);
		}

		public void setStart() {
			
			name = "START";
			start = true;
		}
		
		public void setGoal() {
			
			name = "GOAL";
			goal = true;
		}
		
		public bool isStart() {
			
			return start;
		}
		
		public bool isGoal() {
			
			return goal;
		}

		public bool Equals(Affordance aff) {
			
			return ((affodant.Equals(aff.affodant)) && (affordee.Equals(aff.affordee)) && (name.Equals(aff.name)));
		}

		public virtual void disp() { 
			
			Debug.Log (affordantName + name + affordeeName);
		}



		public Node GetSubTree() {

			return this.treeRoot;
		}

		public Node UpdateState() {
			return new LeafInvoke(
				() => this.UpdateNarrativeState());
		}

		public void UpdateNarrativeState() {
			//Debug.Log ("called for - " + name);
			foreach (Condition effect in effects)
				NarrativeState.AddCondition (effect);
		}

		public bool CheckAffordancePreconditions() {

			List<Condition> conditions = NarrativeState.GetNarrativeState ();
			foreach (Condition cond in preconditions) {

				if(!conditions.Contains(cond)) {
					return false;
				}
			}
			return true;
		}
	}
}