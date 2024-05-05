#if AR51_LeapMotionHands
using System;
using System.Linq;
using AR51.Unity.SDK.Utilities;
using UnityEngine;
using Leap.Unity.Attributes;
using Leap.Unity;


namespace AR51.Unity.SDK.HandsAdapters
{
    public class UltraleapHandsAdapter
        : IHandsAdapter
    {

        public const int FINGER_COUNT = 5;
        public const int FINGER_PART_COUNT = 4;

        public const float MinConfidence = 0f;
        public string SourceDevice => "Ultraleap";
        public HandJointInfo[] GetRightJointInfos() => GetJointsInfo(HandType.RightHand);
        public HandJointInfo[] GetLeftJointInfos() => GetJointsInfo(HandType.LeftHand);

        private LeapServiceProvider _leapSericeProvider;
        private readonly static HandJointInfo[] _emptyLeft = Enumerable.Range(0, Enum.GetValues(typeof(HandJointType)).Length).Select(i => new HandJointInfo(HandType.LeftHand, (HandJointType)i)).ToArray();
        private readonly static HandJointInfo[] _emptyRight = Enumerable.Range(0, Enum.GetValues(typeof(HandJointType)).Length).Select(i => new HandJointInfo(HandType.RightHand, (HandJointType)i)).ToArray();

        //public UltraleapHandsAdapter()
        //{
        //    Debug.Log("UltraleapHandsAdapter ctor");
        //}

        private LeapServiceProvider GetLeapServiceProvider() =>
            _leapSericeProvider == null ? (_leapSericeProvider = UnityEngine.Object.FindObjectOfType<LeapXRServiceProvider>()) : _leapSericeProvider;

        private HandJointInfo[] GetJointsInfo(HandType handType)
        {
            //Debug.Log($"GetJointsInfo(HandType: {handType}");
            var info = handType == HandType.LeftHand ? _emptyLeft : _emptyRight;
            var provider = GetLeapServiceProvider();

            // validate that we have a unltraleap service provider in the scene, otherwise, return empty handed :)
            if (provider == null)
            {
                Debug.LogError($"Failed in {GetType().Name}.{nameof(GetJointsInfo)}. Error: {nameof(LeapXRServiceProvider)} is null.");
                return info;
            }
            Leap.Hand hand = provider.CurrentFrame.Hands.FirstOrDefault(h => (h.IsLeft == (handType == HandType.LeftHand)));

            //Debug.Log($"hand == null: {hand == null}");
            //Debug.Log($"|| hand.Confidence < MinConfidence: {hand == null || hand.Confidence < MinConfidence}");

            // validate that we have a detected the specified handness in the scene, otherwise, return empty handed :)
            if (hand == null || hand.Confidence < MinConfidence) return info;

            info = new HandJointInfo[info.Length];
            var confidence = hand.Confidence;

            // set wrist, thumb trapezium and pinky meta carpal to the wrist rotation & position.
            var wristPos = hand.WristPosition.ToVector3();
            var wristRot = hand.Rotation.ToQuaternion();
            foreach (var jointType in new[] { HandJointType.Wrist, HandJointType.ThumbTrapezium, HandJointType.LittleMetaCarpal })
                info[(int)jointType] = new HandJointInfo(handType, jointType, confidence, wristPos, wristRot);

            for (int fingerIndex = 0; fingerIndex < FINGER_COUNT; fingerIndex++)
            {
                for (int fingerPartIndex = 0; fingerPartIndex < FINGER_PART_COUNT; fingerPartIndex++)
                {
                    var jointType = (HandJointType)(fingerIndex * FINGER_PART_COUNT + fingerPartIndex + 2);

                    // pinky, i.e. finger index == 4, has an offset because of the pinky metacarpal
                    if (fingerIndex == (int)Leap.Finger.FingerType.TYPE_PINKY)
                        jointType++;

                    var bone = hand.Fingers[fingerIndex].Bone((Leap.Bone.BoneType)fingerPartIndex);
                    var position = bone.NextJoint.ToVector3();
                    var rotation = Quaternion.identity;
                    info[(int)jointType] = new HandJointInfo(handType, jointType, confidence, position, rotation);
                }
            }

            return info;
        }
    }
}

#endif