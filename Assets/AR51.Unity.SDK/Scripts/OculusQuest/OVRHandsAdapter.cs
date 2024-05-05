#if AR51_OVRHands
using System;
using System.Linq;
using AR51.Unity.SDK.Utilities;
using UnityEngine;

namespace AR51.Unity.SDK.HandsAdapters
{
    public class OVRHandsAdapter
        : IHandsAdapter
    {
        public const float LowTrackingConfidence = 0.3f;
        public const float HighTrackingConfidence = 1.0f;
        private readonly OVRTrackedHand[] _tracked;
        private OVRTrackedHand GetTracked(HandType handType) => _tracked[(int)handType];
        public string SourceDevice => "OculusQuest";
        public HandJointInfo[] GetRightJointInfos() => GetJointsInfo(HandType.RightHand);
        public HandJointInfo[] GetLeftJointInfos() => GetJointsInfo(HandType.LeftHand);

        public OVRHandsAdapter()
        {
            var trackedRoot = new GameObject(nameof(OVRHandsAdapter));
            _tracked = new[]
            {
                new OVRTrackedHand(HandType.RightHand, trackedRoot.transform),
                new OVRTrackedHand(HandType.LeftHand, trackedRoot.transform),
            };
        }

        private HandJointInfo[] GetJointsInfo(HandType handType)
        {
            var tracked = GetTracked(handType);
            var boneTransforms = tracked.GetOVRBoneTransforms();
            var confidence = CalculateConfidence(tracked.Hand);
            
            var info = new HandJointInfo[boneTransforms.Length];
            for (var i = 0; i < boneTransforms.Length; i++)
            {
                var jointType = (HandJointType)i;
                var position = boneTransforms[i].position;
                var rotation = boneTransforms[i].rotation;
                info[i] = new HandJointInfo(handType, jointType, confidence, position, rotation);
            }

            return info;
        }

        private static float CalculateConfidence(OVRHand hand)
        {
            if (!hand.IsTracked) return 0.0f;
            return hand.HandConfidence == OVRHand.TrackingConfidence.Low ? LowTrackingConfidence : HighTrackingConfidence;
        }
    }

    /// <summary>
    /// A simple helper wrapper around the OVRSkeleton and OVRHand.
    /// </summary>
    public class OVRTrackedHand
    {
        private Transform[] _boneTransforms;
        public OVRSkeleton Skeleton { get; }
        public OVRHand Hand { get; }
        public HandType Type { get; }
        public bool HasBones => Skeleton.Bones.Count > 0;

        public Transform[] GetOVRBoneTransforms()
        {
            if (_boneTransforms.Length == 0 && Skeleton.Bones.Count > 0)
            {
                var jointTypes = (HandJointType[])Enum.GetValues(typeof(HandJointType));
                _boneTransforms = new Transform[jointTypes.Length];
                _boneTransforms[0] = Skeleton.transform;

                for (var i = 1; i < _boneTransforms.Length; i++)
                    _boneTransforms[i] = Skeleton.Bones.First(b => b.Id == jointTypes[i].ToBoneId()).Transform;
            }

            if (_boneTransforms.Length > 0)
                _boneTransforms[0] = Skeleton.transform;

            return _boneTransforms;
        }

        public OVRTrackedHand(HandType handType, Transform parent)
        {
            _boneTransforms = Array.Empty<Transform>();
            var handRoot = new GameObject(handType.ToString());
            handRoot.transform.SetParent(parent);

            var ovrHandType = handType == HandType.LeftHand ? OVRHand.Hand.HandLeft : OVRHand.Hand.HandRight;
            Hand = handRoot.AddComponent<OVRHand>();
            Hand.SetValue("HandType", ovrHandType);

            var ovrSkeletonType = handType == HandType.LeftHand ? OVRSkeleton.SkeletonType.HandLeft : OVRSkeleton.SkeletonType.HandRight;
            Skeleton = handRoot.AddComponent<OVRSkeleton>();
            Skeleton.SetValue("_skeletonType", ovrSkeletonType);
            Skeleton.SetValue("_updateRootPose", true);

            Type = handType;
        }
    }
}

#endif