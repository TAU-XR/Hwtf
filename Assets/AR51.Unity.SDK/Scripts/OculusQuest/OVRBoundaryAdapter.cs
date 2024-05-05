#if AR51_OculusQuest || AR51_OpenXRTethered || AR51_OvrTethered
using UnityEngine;
#pragma warning disable 618

namespace AR51.Unity.SDK.BoundaryAdapters
{
    using BoundaryType = OVRBoundary.BoundaryType;

    public class OVRBoundaryAdapter
        : IBoundaryAdapter
    {
        public string BoundaryName => "Native Quest Guardian";

        public Vector3[] GetPoints()
        {
            var boundary = OVRManager.boundary;
            if (boundary == null || !boundary.GetConfigured())
                return AnchorService.Instance.DefaultBoundary.GetChildTransformsAsPoints();

            return boundary.GetGeometry(BoundaryType.OuterBoundary);
        }
    }
}
#endif