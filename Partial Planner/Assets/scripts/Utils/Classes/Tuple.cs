using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace POPL.Planner
{
	public class Tuple<T1, T2>
	{
		public T1 First { get; private set; }
		public T2 Second { get; private set; }
		internal Tuple(T1 first, T2 second)
		{
			First = first;
			Second = second;
		}

		public bool Equals (Tuple<T1, T2> obj)
		{
			return (First.Equals(obj.First) && Second.Equals(obj.Second));
		}
	}
	
	public static class Tuple
	{
		public static Tuple<T1, T2> New<T1, T2>(T1 first, T2 second)
		{
			var tuple = new Tuple<T1, T2>(first, second);
			return tuple;
		}
	}
}