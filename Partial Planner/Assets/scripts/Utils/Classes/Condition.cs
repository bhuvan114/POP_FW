using UnityEngine;
using System.Collections;

namespace POPL.Planner 
{
	public class Condition {
		
		public string actor1;
		public string actor2;
		public string condition;
		public bool status;
		
		public Condition(string actorOneName, string actorTwoName, string conditionName, bool conditionStatus) {
			
			condition = conditionName;
			actor1 = actorOneName;
			actor2 = actorTwoName;
			status = conditionStatus;
		}

		public Condition(string actorOneName, string conditionName, bool conditionStatus) {
			
			condition = conditionName;
			actor1 = actorOneName;
			actor2 = "";
			status = conditionStatus;
		}

		public bool isSimilar(Condition c) {

			return ((condition == c.condition) && (status == c.status));
		}

		public bool Equals(Condition c) {
			
			return ((actor1 == c.actor1) && (actor2 == c.actor2) && (condition == c.condition) && (status == c.status));
		}
		
		public bool isNegation(Condition c){
			
			return ((actor1 == c.actor1) && (actor2 == c.actor2) && (condition == c.condition) && (status != c.status));
		}
		
		public void disp() {
			Debug.LogWarning ("condition - " + condition + ", " + actor1 + ", " + actor2 + ", " + status.ToString());
		}
	}
}