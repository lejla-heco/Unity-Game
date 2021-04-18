using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Spacecraft.Controllers.Player
{
    public class ShipAnimatorController : MonoBehaviour
    {
        private static readonly int TriggerPlayerMoveUp = Animator.StringToHash("TriggerPlayerMoveUp");
        private static readonly int TriggerPlayerMoveDown = Animator.StringToHash("TriggerPlayerMoveDown");

        private Animator shipAnimator { get; set; }
        
        private void Start()
        {
            shipAnimator = this.GetComponent<Animator>();
        }
        
        public void TriggerMoveUp()
        {
            shipAnimator.SetTrigger(TriggerPlayerMoveUp);
        }
        
        public void TriggerMoveDown()
        {
            shipAnimator.SetTrigger(TriggerPlayerMoveDown);
        }
    }
}
