using TOM.Common.UI;
using System;
using Microsoft.MixedReality.Toolkit;
using Microsoft.MixedReality.Toolkit.Input;
using Microsoft.MixedReality.Toolkit.Utilities;
using UnityEngine;

namespace TOM.Apps.MartialArts
{
    public class Pad : MonoBehaviour, IMixedRealityPointerHandler, IMixedRealityTouchHandler
    {
        public GameObject cylinder;
        public TextContentUnit padText;

        // determines if this pad is used for training session - if true, cannot grab and drag around pad, only touch interaction
        public bool isTrainingPad;

        // delegate function for collision
        public delegate void CollisionStartDelegate(MaCollisionData collisionData);
        public delegate void CollisionEndDelegate();

        public CollisionStartDelegate onCollisionStart;
        public CollisionEndDelegate onCollisionEnd;

        private MaCollisionData collisionData;
        private MaVector velocityData;
        private MaVector contactData;
        private MaVector targetData;

        public void OnPointerClicked(MixedRealityPointerEventData eventData) { }

        public void OnPointerDown(MixedRealityPointerEventData eventData)
        {
            if (eventData.Pointer is not SpherePointer)
            {
                return;
            }
        }

        public void OnPointerDragged(MixedRealityPointerEventData eventData) { }

        public void OnPointerUp(MixedRealityPointerEventData eventData)
        {
            if (eventData.Pointer is not SpherePointer)
            {
                return;
            }
        }

        public void OnTouchCompleted(HandTrackingInputEventData eventData)
        {
            if (onCollisionEnd == null)
            {
                return;
            }

            onCollisionEnd.Invoke();
        }

        public void OnTouchStarted(HandTrackingInputEventData eventData)
        {
            if (onCollisionStart == null)
            {
                return;
            }

            var hand = eventData.Controller as IMixedRealityHand;
            if (
                hand == null
                || !hand.TryGetJoint(TrackedHandJoint.MiddleKnuckle, out MixedRealityPose jointPose)
                || !isTrainingPad
            )
            {
                return;
            }

            collisionData = getCollisionData(
                hand.ControllerHandedness,
                jointPose.Position,
                hand.Velocity
            );
            onCollisionStart.Invoke(collisionData);
        }

        public void OnTouchUpdated(HandTrackingInputEventData eventData) { }

        // Start is called before the first frame update
        void Start()
        {
            RunInitSequence(InitDraggable, InitCollisionData);
        }

        // Update is called once per frame
        void Update() { }

        public void SetIsTrainingPad(bool isTrainingPad)
        {
            this.isTrainingPad = isTrainingPad;
            if (isTrainingPad)
            {
                RemoveNearDraggable(gameObject);
            }
            else
            {
                MakeNearDraggable(gameObject);
            }
        }

        // https://learn.microsoft.com/en-us/windows/mixed-reality/mrtk-unity/mrtk2/features/input/how-to-add-near-interactivity?view=mrtkunity-2021-05#arbitrary-collider-touch
        public static void MakeNearDraggable(GameObject target)
        {
            // Add ability to drag by re-parenting to pointer object on pointer down
            var pointerHandler = target.AddComponent<PointerHandler>();
            var originalParent = target.transform.parent;
            pointerHandler.OnPointerDown.AddListener(
                (e) =>
                {
                    if (e.Pointer is SpherePointer)
                    {
                        target.transform.parent = ((SpherePointer)(e.Pointer)).transform;
                    }
                }
            );
            pointerHandler.OnPointerUp.AddListener(
                (e) =>
                {
                    if (e.Pointer is SpherePointer)
                    {
                        target.transform.parent = originalParent;
                    }
                }
            );
        }

        public static void RemoveNearDraggable(GameObject target)
        {
            // Remove PointerHandler component
            var pointerHandler = target.GetComponent<PointerHandler>();
            if (pointerHandler != null)
            {
                Destroy(pointerHandler);
            }
        }

        private MaCollisionData getCollisionData(
            Handedness handedness,
            Vector3 collisionPoint,
            Vector3 velocity
        )
        {
            Vector3 targetPos = gameObject.transform.position;
            float angle = Vector3.Angle(velocity, gameObject.transform.forward);
            float distance = (float)
                Math.Sqrt(
                    Math.Pow(contactData.X - targetPos.x, 2)
                        + Math.Pow(contactData.Y - targetPos.y, 2)
                );
            collisionData.DistanceToTarget = distance;
            collisionData.Angle = angle;
            collisionData.Hand = (int)handedness;

            velocityData = MaVectorUtils.convertToMaVector(velocityData, velocity);
            contactData = MaVectorUtils.convertToMaVector(contactData, collisionPoint);
            targetData = MaVectorUtils.convertToMaVector(targetData, targetPos);

            collisionData.Velocity = velocityData;
            collisionData.CollisionPoint = contactData;
            collisionData.PadPosition = targetData;

            return collisionData;
        }

        // Runs init functions sequentially, take in variable number of arguments.
        private void RunInitSequence(params Action[] initFuncs)
        {
            foreach (var initFunc in initFuncs)
            {
                initFunc?.Invoke();
            }
        }

        // sample initialization function
        private void InitDraggable()
        {
            if (isTrainingPad)
            {
                return;
            }
            MakeNearDraggable(gameObject);
        }

        private void InitCollisionData()
        {
            collisionData = new MaCollisionData();
            velocityData = new MaVector();
            contactData = new MaVector();
            targetData = new MaVector();
        }
    }
}
