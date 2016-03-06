using UnityEngine;
using System.Collections;
using System.Collections.Generic;

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

		public Affordance() {
			affodant = new SmartObject ("affodant");
			affordee = new SmartObject ("affordee");
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

	}
}