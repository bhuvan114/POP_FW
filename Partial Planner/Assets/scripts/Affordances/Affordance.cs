using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace POPL.Planner
{
	public class Affordance {

		protected SmartObject affodant, affordee;
		protected string affordantName, affordeeName;
		protected string name = "";
		protected List<Condition> preconditions = new List<Condition>();
		protected List<Condition> effects = new List<Condition>();

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

		public virtual void disp() { 
			
			Debug.Log (affordantName + name + affordeeName);
		}
	}
}