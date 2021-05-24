using System.Collections;
using Spacecraft.Controllers.Core.Entities;
using Spacecraft.Controllers.Core.LevelGenerator;
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
