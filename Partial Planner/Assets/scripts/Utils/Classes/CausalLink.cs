using UnityEngine;
using System.Collections;
using System.Collections.Generic;

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

		public List<CausalLink> GetCLsByAction1(List<CausalLink> cls) {

			List<CausalLink> searchResults = new List<CausalLink>();
			foreach (CausalLink cl in cls)
				if (cl.act1.Equals (act1))
					searchResults.Add (cl);
			return searchResults;
		}

		public List<CausalLink> GetCLsByAction2(List<CausalLink> cls) {

			List<CausalLink> searchResults = new List<CausalLink>();
			foreach (CausalLink cl in cls)
				if (cl.act2.Equals (act1))
					searchResults.Add (cl);
			return searchResults;
		}
	}
}