using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Spacecraft.Core
{
    public class InputManagement : MonoBehaviour
    {
        public Action OnInputChanged;

        private void Update()
        {
            OnInputChanged?.Invoke();
        }
    }
}