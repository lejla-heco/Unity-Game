using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Spacecraft.Controllers.Player
{
    public class ShipAnimatorController : MonoBehaviour
    {
        private static readonly int TriggerPlayerMoveUp = Animator.StringToHash("TriggerPlayerMoveUp");
        private static readonly int TriggerPlayerMoveDown = Animator.StringToHash("TriggerPlayerMoveDown");

        private Animator ShipAnimator { get; set; }
        
        private void Start()
        {
            ShipAnimator = this.GetComponent<Animator>();
        }
        
        public void TriggerMoveUp()
        {
            ShipAnimator.SetTrigger(TriggerPlayerMoveUp);
        }
        
        public void TriggerMoveDown()
        {
            ShipAnimator.SetTrigger(TriggerPlayerMoveDown);
        }
    }
}
