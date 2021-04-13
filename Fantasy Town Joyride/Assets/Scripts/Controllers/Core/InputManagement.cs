using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Spacecraft.Controllers.Core
{
public class InputManagement : MonoBehaviour
{
   public Action<float> OnInputChanged;

   private void Update()
   {
      OnInputChanged?.Invoke(Input.GetAxisRaw("Horizontal"));
   }
}
}