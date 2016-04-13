using UnityEngine;
using System.Linq;
using System.Reflection;
using System.Collections;
using System.Collections.Generic;
using TreeSharpPlus;

namespace POPL.Planner
{
	public static class HelperFunctions {

		public static void initiatePlanSpace() {

			//populateCharactersInScene ();
			populateAllPossibleActions ();
		}

		public static void initiatePlanSpace_v2(GameObject[] smartObjects) {
			
			populateCharactersInScene (smartObjects);
			NarrativeStateManager.debugLog = "Before populateAllActionsMap!!";
			populateAllActionsMap ();
			NarrativeStateManager.debugLog = "Before populateAllActionsMap!!";
			updateActionRelations ();
		}

		static void populateCharactersInScene (GameObject[] smartObjects) {

			NarrativeStateManager.debugLog = "populateCharactersInScene 1";
			IEnumerable<System.Type> characterTypes = System.Reflection.Assembly.GetExecutingAssembly ().GetTypes ()
				.Where (t => t.BaseType != null && t.BaseType == typeof(POPL.Planner.SmartObject));
			NarrativeStateManager.debugLog = "populateCharactersInScene 1.5";
			//GameObject[] smartObjects = GameObject.FindGameObjectsWithTag ("SmartObject");
			NarrativeStateManager.debugLog = "populateCharactersInScene 2";
			foreach (GameObject smartObject in smartObjects) {
				foreach (System.Type charType in characterTypes) {

					SmartObject obj = (SmartObject)smartObject.GetComponent(charType);
					if(obj != null) {
						if(!Constants.characterTypes.ContainsKey(charType)) {
							Constants.characterTypes.Add(charType, new List<GameObject>());
							addAffordances(obj.getAllAffordances());
						}
						Constants.characterTypes[charType].Add(smartObject);
					}
				}
			}
			NarrativeStateManager.debugLog = "populateCharactersInScene 3";
		}

		static void addAffordances(List<System.Type> affordances) {
			foreach (System.Type affordanceType in affordances) {
				if(!Constants.availableAffordances.Contains(affordanceType)) {
					Constants.availableAffordances.Add(affordanceType);
				}
			}
		}

		static void populateAllPossibleActions () {

			foreach (System.Type affordanceType in Constants.availableAffordances) {

				ConstructorInfo[] constructors = affordanceType.GetConstructors();
				foreach(ConstructorInfo consInfo in constructors) {
					ParameterInfo[] info =  consInfo.GetParameters();
					System.Type typ1 = info[0].ParameterType;
					System.Type typ2 = info[1].ParameterType;
					List<GameObject> objs_1 = Constants.characterTypes[typ1];
					List<GameObject> objs_2 = Constants.characterTypes[typ2];
						foreach (GameObject obj1 in objs_1) {
							foreach (GameObject obj2 in objs_2) {
								SmartObject smObj1 = (SmartObject) obj1.GetComponent(typ1);
								SmartObject smObj2 = (SmartObject) obj2.GetComponent(typ2);
								if(!smObj1.Equals(smObj2)) {
									Affordance aff = (Affordance) System.Activator.CreateInstance(affordanceType, smObj1, smObj2);
									Constants.allPossibleAffordances.Add(aff);
								}
							}
						}
				}
			}
		}

		static void populateAllActionsMap() {

			foreach (System.Type affordanceType in Constants.availableAffordances) {
				
				ConstructorInfo[] constructors = affordanceType.GetConstructors();
				foreach(ConstructorInfo consInfo in constructors) {
					ParameterInfo[] info =  consInfo.GetParameters();
					System.Type typ1 = info[0].ParameterType;
					System.Type typ2 = info[1].ParameterType;
					Debug.Log(typ1.ToString() + " - " + typ2.ToString());
					List<GameObject> objs_1 = Constants.characterTypes[typ1];
					List<GameObject> objs_2 = Constants.characterTypes[typ2];
					foreach (GameObject obj1 in objs_1) {
						foreach (GameObject obj2 in objs_2) {
							SmartObject smObj1 = (SmartObject) obj1.GetComponent(typ1);
							SmartObject smObj2 = (SmartObject) obj2.GetComponent(typ2);
							if(!smObj1.Equals(smObj2)) {
								Affordance aff = (Affordance) System.Activator.CreateInstance(affordanceType, smObj1, smObj2);
								if(!Constants.possibleActionsMap.ContainsKey(affordanceType.ToString()))
									Constants.possibleActionsMap.Add(affordanceType.ToString(), new List<Affordance> ());
								Constants.possibleActionsMap[affordanceType.ToString()].Add(aff);
							}
						}
					}
				}
			}
		}

		static void updateActionRelations() {
			/*
			SmartObject smObj = new SmartObject ();
			List<Affordance> affds = new List<Affordance>();
			Dictionary<string, List<string>> affdRelns = new Dictionary<string, List<string>> ();
			foreach (System.Type affordanceType in Constants.availableAffordances) {

				ConstructorInfo[] constructors = affordanceType.GetConstructors();
				System.Type typ1;
				System.Type typ2;
				foreach(ConstructorInfo consInfo in constructors) {
					ParameterInfo[] info =  consInfo.GetParameters();
					typ1 = info[0].ParameterType;
					typ2 = info[1].ParameterType;
				}

				affds.Add((Affordance) System.Activator.CreateInstance(affordanceType, smObj, smObj));
			}

			foreach (Affordance affd in affds) {

				foreach(Condition preCond in affd.getPreconditions()) {

					Tuple<string, bool> cond = new Tuple<string, bool>(preCond.condition, preCond.status);
					if(!Constants.affordanceRelations.ContainsKey(cond)) {
						Constants.affordanceRelations.Add(cond, getActionRelations (preCond, affds));
					}
				}
			}
			*/
			foreach (System.Type affType in Constants.availableAffordances) {
				Debug.Log(affType.ToString());
				Affordance affd = Constants.possibleActionsMap[affType.ToString()].ElementAt(0);
				foreach(Condition effect in affd.getEffects()) {
					
					//Tuple<string, bool> cond = new Tuple<string, bool>(effect.condition, effect.status);
					if(!Constants.affordanceRelations.ContainsKey(effect.condition)) 
						Constants.affordanceRelations.Add(effect.condition, new Dictionary<bool, List<string>>());
					
					if(!Constants.affordanceRelations[effect.condition].ContainsKey(effect.status))
						Constants.affordanceRelations[effect.condition].Add(effect.status, new List<string>());

					if(!Constants.affordanceRelations[effect.condition][effect.status].Contains(affType.ToString()))
						Constants.affordanceRelations[effect.condition][effect.status].Add(affType.ToString());
				}
			}
		}

		static List<string> getActionRelations(Condition preCond) {

			List<string> actions = new List<string> ();
			foreach(System.Type affType in Constants.availableAffordances){

				Affordance aff = Constants.possibleActionsMap[affType.ToString()].ElementAt(0);
				foreach(Condition effect in aff.getEffects()){
					if(effect.isSimilar(preCond)) {
						if(!actions.Contains(affType.ToString()))
							actions.Add(affType.ToString());
					}
				}
			}
			return actions;
		}

		public static Node ST_ApproachAndWait(GameObject participant, Transform target)
		{
			Val<Vector3> position = Val.V (() => target.position);
			return new Sequence( participant.GetComponent<BehaviorMecanim>().Node_GoTo(position), new LeafWait(500));
		}

		public static Node ST_Turn(GameObject participant, Transform target)
		{
			/*Vector3 ang = target.position;
			//ang.x = ang.x - 1.1f;
			Val<Vector3> ornt = Val.V (() => (target.position));
			//ornt.Value = -ornt.Value;
			return new Sequence( participant.GetComponent<BehaviorMecanim>().Node_OrientTowards(ornt));*/

			Val<Quaternion> ornt = Val.V (() => (target.rotation));
			return new Sequence (participant.GetComponent<BehaviorMecanim>().Node_Orient(ornt));
		}

		public static GameObject GetChildGameObjectByName(GameObject parent, string name) {

			Transform ts = parent.gameObject.GetComponentInChildren<Transform> ();
			foreach (Transform t in ts) {

				if(t.gameObject.name == name)
					return t.gameObject;
			}

			Debug.LogWarning("No Object " + name + " found in " + parent.name);
			return null;

		}

		public static List<CausalLink> GetCLsByAction1(List<CausalLink> cls, Affordance act) {

			List<CausalLink> searchResults = new List<CausalLink>();
			foreach (CausalLink cl in cls)
				if (cl.act1.Equals (act))
					searchResults.Add (cl);
			return searchResults;
		}

		public static List<CausalLink> GetCLsByAction2(List<CausalLink> cls, Affordance act) {

			List<CausalLink> searchResults = new List<CausalLink>();
			foreach (CausalLink cl in cls)
				if (cl.act2.Equals (act))
					searchResults.Add (cl);
			return searchResults;
		}
	}
}