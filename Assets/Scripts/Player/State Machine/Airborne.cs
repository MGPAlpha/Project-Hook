﻿using ASK.Core;
using UnityEngine;

namespace Player
{
    public partial class PlayerStateMachine
    {
        public class Airborne : PlayerState
        {
            private GameTimer _jumpCoyoteTimer;

            public override void Enter(PlayerStateInput i)
            {
                // PlayAnimation(PlayerAnimations.JUMP_INIT);
                if (!Input.jumpedFromGround)
                {
                    _jumpCoyoteTimer = GameTimer.StartNewTimer(core.JumpCoyoteTime, "Jump Coyote Timer");
                }
            }

            public override void JumpPressed()
            {
                TimerState coyoteState = GameTimer.GetTimerState(_jumpCoyoteTimer);
                if (coyoteState == TimerState.Running)
                {
                    JumpFromGround();
                    base.JumpPressed();
                    return;
                } else if (Input.canDoubleJump)
                {
                    DoubleJump();
                    return;
                }
                
                else
                {
                    base.JumpPressed();
                }
            }

            public override void JumpReleased()
            {
                base.JumpReleased();
                TryJumpCut();
            }

            public override void DivePressed()
            {
                base.DivePressed();
                if (Input.canDive)
                {
                    MySM.Transition<Diving>();
                }
            }

            public override void SetGrounded(bool isGrounded, bool isMovingUp)
            {
                base.SetGrounded(isGrounded, isMovingUp);
                if (!isMovingUp && isGrounded) {
                    // PlayAnimation(PlayerAnimations.LANDING);
                    MySM.Transition<Grounded>();
                }
            }

            public override void MoveX(int moveDirection)
            {
                // UpdateSpriteFacing(moveDirection);
                // smActor.UpdateMovementX(moveDirection, core.MaxAirAcceleration);
                UpdateSpriteFacing(moveDirection);
                int vxSign = (int) Mathf.Sign(smActor.velocityX);
                int acceleration = moveDirection == vxSign || moveDirection == 0 ? 100 : core.MaxDeceleration;
                smActor.UpdateMovementX(moveDirection, acceleration);
            }

            public override void FixedUpdate()
            {
                smActor.Fall();
                GameTimer.FixedUpdate(_jumpCoyoteTimer);
            }
        }
    }
}