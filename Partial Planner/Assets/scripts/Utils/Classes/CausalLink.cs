using UnityEngine;
using System.Collections;

namespace POPL.Planner
{
	public class CausalLink {
		
		public Affordance act1;
		public Affordance act2;
		public Condition p;
		
		public CausalLink(Affordance actor1, Condition preCond, Affordance actor2) {
			
			act1 = actor1;
			act2 = actor2;
			p = preCond;
		}
		
		public void disp() {
			act1.disp ();
			Debug.Log (p.condition);
			act2.disp();
		}
	}
}