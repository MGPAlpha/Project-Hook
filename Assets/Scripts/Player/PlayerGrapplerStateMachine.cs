﻿using A2DK.Phys;
using Mechanics;
using MyBox;
using UnityEngine;

namespace Player
{
    [RequireComponent(typeof(PlayerCore))]
    public class PlayerGrapplerStateMachine : GrapplerStateMachine
    {
        private PlayerCore _core;

        protected override void Init()
        {
            base.Init();
            _core = GetComponent<PlayerCore>();
        }
        
        public override Vector2 GetGrappleInputPos() {
            Vector2 rawPos = _core.Input.GetAimPos(MyPhysObj.transform.position);
            Vector2 rawDirection = rawPos - (Vector2) MyPhysObj.transform.position;
            Vector2 transformedDirection = AimAssistSystem.TransformAim(MyPhysObj.transform.position, rawDirection);
            Vector2 transformedPosition = rawDirection.magnitude * transformedDirection.normalized + (Vector2) MyPhysObj.transform.position;
            return transformedPosition;
        }

        protected override Vector2 CollideHorizontalGrapple() {
            return new Vector2(0, Mathf.Abs(_core.Actor.velocityX * _core.HitWallGrappleMult));
        }

        protected override bool GrappleStarted() => _core.Input.GrappleStarted();

        protected override bool GrappleFinished() => _core.Input.GrappleFinished();

        protected override void CollideVerticalGrapple()
        {
            Actor a = _core.Actor;
            a.ApplyVelocity(new Vector2(a.velocityY * _core.HitWallGrappleMult * -Mathf.Sign(a.velocityX), 0));
        }

        protected override Vector2 MoveXGrapple(Vector2 oldV, Vector2 gPos, int direction) {
            Vector2 rawV = gPos - (Vector2) transform.position;
            Vector2 projection = Vector3.Project(oldV, rawV);
            Vector2 ortho = oldV - projection;
            return direction == 0 ? oldV : ortho * _core.MoveXGrappleMult;
        }
    }
}