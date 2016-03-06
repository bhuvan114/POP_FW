﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace POPL.Planner
{
	public static class Constants {

		public static Dictionary<System.Type, List<GameObject>> characterTypes = new Dictionary<System.Type, List<GameObject>>();
		public static Dictionary<System.Type, List<string>> characterNames = new Dictionary<System.Type, List<string>>();
		public static List<System.Type> availableAffordances = new List<System.Type>();
		public static List<Affordance> allPossibleAffordances = new List<Affordance>();
	}
}