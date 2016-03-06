using UnityEngine;
using System.Linq;
using System.Reflection;
using System.Collections;
using System.Collections.Generic;

namespace POPL.Planner
{
	public static class HelperFunctions {

		public static void initiatePlanSpace() {

			populateCharactersInScene ();
			populateAllPossibleActions ();
		}

		static void populateCharactersInScene () {

			IEnumerable<System.Type> characterTypes = System.Reflection.Assembly.GetExecutingAssembly ().GetTypes ()
				.Where (t => t.BaseType != null && t.BaseType == typeof(POPL.Planner.SmartObject));
			GameObject[] smartObjects = GameObject.FindGameObjectsWithTag ("SmartObject");
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
		}

		static void addAffordances(List<System.Type> affordances) {
			foreach (System.Type affordanceType in affordances) {
				if(!Constants.availableAffordances.Contains(affordanceType))
					Constants.availableAffordances.Add(affordanceType);
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
	}
}