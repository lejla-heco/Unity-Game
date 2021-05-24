using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

namespace Spacecraft.Controllers.Core.LevelGenerator
{
	public class ObjectPool<T>
	{
		private int Index { get; set; } = 0;
		private List<T> ObjectsList { get; set; }

		public ObjectPool()
		{
			ObjectsList = new List<T>();
		}

		public ObjectPool(List<T> objects)
		{
			ObjectsList = objects;
		}

		public void Add(T gameObject)
		{
			this.ObjectsList.Add(gameObject);
		}

		public T PickChunkFromPool()
		{
			if (Index >= ObjectsList.Count) Index = 0;
			Debug.Log(Index);
			return ObjectsList[Index++];
		}
	}
}
