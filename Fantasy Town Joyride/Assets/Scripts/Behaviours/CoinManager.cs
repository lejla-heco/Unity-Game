using System.Collections;
using Spacecraft.Core.Entities;
using Spacecraft.Core.LevelGenerator;
using UnityEngine;

namespace Spacecraft
{
	public class CoinManager : MonoBehaviour
	{
		private int RotateSpeed;
		private void Start()
		{
			RotateSpeed = 3;
		}
		private void Update()
		{
			transform.Rotate(0, RotateSpeed, 0, Space.World);
		}
	}
}
