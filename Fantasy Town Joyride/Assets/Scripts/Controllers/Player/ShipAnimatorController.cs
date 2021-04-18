using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Spacecraft.Controllers.Player
{
    public class ShipAnimatorController : MonoBehaviour
    {
        private static readonly int MoveUp = Animator.StringToHash("MoveUp");
        private static readonly int MoveDown = Animator.StringToHash("MoveDown");
        private Animator shipAnimator { get; set; }
        
        private void Start()
        {
            shipAnimator = this.GetComponent<Animator>();
        }
        
        public void TriggerMoveUp()
        {
            shipAnimator.SetTrigger(MoveUp);
        }
        
        public void TriggerMoveDown()
        {
            shipAnimator.SetTrigger(MoveDown);
        }
    }
}
