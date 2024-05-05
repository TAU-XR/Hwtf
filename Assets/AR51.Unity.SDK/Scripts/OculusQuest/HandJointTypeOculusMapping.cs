#if AR51_OculusQuest || AR51_OpenXRTethered || AR51_OvrTethered
using System;

namespace AR51.Unity.SDK.Utilities
{
    using BoneId = OVRSkeleton.BoneId;

    public static class HandJointTypeOculusMapping
    {
        public static BoneId ToBoneId(this HandJointType type) => GetBoneId(type);
        public static BoneId GetBoneId(HandJointType type)
        {
            switch (type)
            {
                case HandJointType.Wrist: return BoneId.Hand_ForearmStub;
                case HandJointType.ThumbTrapezium: return BoneId.Hand_Thumb0;
                case HandJointType.ThumbMetaCarpal: return BoneId.Hand_Thumb1;
                case HandJointType.ThumbProximal: return BoneId.Hand_Thumb2;
                case HandJointType.ThumbDistal: return BoneId.Hand_Thumb3;
                case HandJointType.ThumbTip: return BoneId.Hand_ThumbTip;
                case HandJointType.IndexProximal: return BoneId.Hand_Index1;
                case HandJointType.IndexIntermediate: return BoneId.Hand_Index2;
                case HandJointType.IndexDistal: return BoneId.Hand_Index3;
                case HandJointType.IndexTip: return BoneId.Hand_IndexTip;
                case HandJointType.MiddleProximal: return BoneId.Hand_Middle1;
                case HandJointType.MiddleIntermediate: return BoneId.Hand_Middle2;
                case HandJointType.MiddleDistal: return BoneId.Hand_Middle3;
                case HandJointType.MiddleTip: return BoneId.Hand_MiddleTip;
                case HandJointType.RingProximal: return BoneId.Hand_Ring1;
                case HandJointType.RingIntermediate: return BoneId.Hand_Ring2;
                case HandJointType.RingDistal: return BoneId.Hand_Ring3;
                case HandJointType.RingTip: return BoneId.Hand_RingTip;
                case HandJointType.LittleMetaCarpal: return BoneId.Hand_Pinky0;
                case HandJointType.LittleProximal: return BoneId.Hand_Pinky1;
                case HandJointType.LittleIntermediate: return BoneId.Hand_Pinky2;
                case HandJointType.LittleDistal: return BoneId.Hand_Pinky3;
                case HandJointType.LittleTip: return BoneId.Hand_PinkyTip;
                default: throw new ArgumentOutOfRangeException(nameof(type), type, null);
            }
        }
    }
}
#endif