using UnityEngine;
using System.Collections;
using System.Collections.Generic;


namespace POPL.Planner
{
	public class SmartObject : MonoBehaviour {

		public string name;
		// Use this for initialization
		void Start () {
		
		}
		
		// Update is called once per frame
		void Update () {
		
		}

		public List<System.Type> getAllAffordances() {

			List<System.Type> affordances = new List<System.Type> ();
			System.Reflection.FieldInfo[] members = this.GetType ().GetFields ();
			foreach (System.Reflection.FieldInfo member in members) {
				if (member.FieldType.BaseType == typeof(POPL.Planner.Affordance)) {
					affordances.Add(member.FieldType);
				}
			}
			return affordances;
		}

		public SmartObject(string objName) {

			name = objName;
		}

		public SmartObject(){

			name = "Dummy";
		}

		public bool Equals(SmartObject obj) {

			return name.Equals (obj.name);
		}
	}
}