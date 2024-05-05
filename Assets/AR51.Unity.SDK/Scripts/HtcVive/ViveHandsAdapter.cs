#if AR51_HTCHands
using System;
using System.Linq;
using UnityEngine;
using UnityEngine.Assertions;
using ViveHandTracking;
using Object = UnityEngine.Object;

namespace AR51.Unity.SDK.HandsAdapters
{
    public class ViveHandsAdapter
        : IHandsAdapter
    {
        private GestureProvider _provider;
        private HandJointType[] _jointTypes;
        private HandJointInfo[] _leftJointsInfo;
        private HandJointInfo[] _rightJointInfo;        
        private HandJointInfo[] GetJoints(HandType type) => type == HandType.LeftHand ? _leftJointsInfo : _rightJointInfo;
        public string SourceDevice => "HtcVive";
        public HandJointInfo[] GetRightJointInfos() => RecalculateHandJointsInfo(HandType.RightHand);
        public HandJointInfo[] GetLeftJointInfos() => RecalculateHandJointsInfo(HandType.LeftHand);

        public ViveHandsAdapter()
        {
            InitializeGestureProvider();
            InitializeHandsJointsInfo();
//#if UNITY_EDITOR
//            _provider.gameObject.AddComponent<ViveGestureProviderEditorGUI>();
//#endif
        }

        private void InitializeHandsJointsInfo()
        {
            _jointTypes = Enum.GetValues(typeof(HandJointType)).Cast<HandJointType>().ToArray();
            _leftJointsInfo = new HandJointInfo[_jointTypes.Length];
            _rightJointInfo = new HandJointInfo[_jointTypes.Length];
        }

        private void InitializeGestureProvider()
        {
            Assert.IsNull(_provider);
            _provider = Object.FindObjectOfType<GestureProvider>(true);
            if (_provider != null) return;

            var providerOwner = Camera.main != null ? Camera.main.gameObject : null;
            if (providerOwner == null)
            {
                Debug.LogWarning("ViveHandsAdapter: failed to locate main camera in scene (generating gesture provider on new game object)");
                providerOwner = new GameObject("ViveGestureProvider");
            }

            _provider = providerOwner.AddComponent<GestureProvider>();
        }

        private HandJointInfo[] RecalculateHandJointsInfo(HandType handType)
        {
            var hand = handType == HandType.LeftHand ? GestureProvider.LeftHand : GestureProvider.RightHand;
            if (hand == null)
                return ResetHand(handType);

            var jointPositions = hand.points;
            var jointRotations = hand.rotations;
            var confidence = hand.confidence;
            var jointsInfo = GetJoints(handType);
            for (var i = 0; i < jointsInfo.Length; i++)
            {
                var jointType = _jointTypes[i];
                var boneId = ToViveBoneId(jointType);
                jointsInfo[i] = new HandJointInfo(
                    handType, 
                    jointType,
                    confidence,
                    jointPositions[boneId],
                    jointRotations[boneId]);
            }

            return jointsInfo;
        }

        private HandJointInfo[] ResetHand(HandType handType)
        {
            var jointsInfo = GetJoints(handType);
            for (var i = 0; i < jointsInfo.Length; i++)
                jointsInfo[i] = new HandJointInfo(handType, _jointTypes[i]);

            return jointsInfo;
        }

        private static int ToViveBoneId(HandJointType jointType)
        {
            switch (jointType)
            {
                case HandJointType.Wrist: return 0;

                case HandJointType.ThumbTrapezium: return 0;
                case HandJointType.ThumbMetaCarpal: return 1;
                case HandJointType.ThumbProximal: return 2;
                case HandJointType.ThumbDistal: return 3;
                case HandJointType.ThumbTip: return 4;

                case HandJointType.IndexProximal: return 5;
                case HandJointType.IndexIntermediate: return 6;
                case HandJointType.IndexDistal: return 7;
                case HandJointType.IndexTip: return 8;

                case HandJointType.MiddleProximal: return 9;
                case HandJointType.MiddleIntermediate: return 10;
                case HandJointType.MiddleDistal: return 11;
                case HandJointType.MiddleTip: return 12;

                case HandJointType.RingProximal: return 13;
                case HandJointType.RingIntermediate: return 14;
                case HandJointType.RingDistal: return 15;
                case HandJointType.RingTip: return 16;

                case HandJointType.LittleMetaCarpal: return 0;
                case HandJointType.LittleProximal: return 17;
                case HandJointType.LittleIntermediate: return 18;
                case HandJointType.LittleDistal: return 19;
                case HandJointType.LittleTip: return 20;
                default: throw new ArgumentOutOfRangeException(nameof(jointType), jointType, null);
            }
        }
    }
}
#endif//AR51_HtcVive