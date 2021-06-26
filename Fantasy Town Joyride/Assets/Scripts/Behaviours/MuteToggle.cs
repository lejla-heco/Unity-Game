using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Spacecraft.Behaviours
{
    public class MuteToggle : MonoBehaviour
    {
        private Toggle Toggle1;
        void Start()
        {
            Toggle1 = GetComponent<Toggle>();
            if (AudioListener.volume == 0) Toggle1.isOn = false;
        }

        public void ToggleAudioOnValueChange(bool audioIn)
        {
            if (audioIn)
                AudioListener.volume = 1;
            else
                AudioListener.volume = 0;
        }
    }
}
