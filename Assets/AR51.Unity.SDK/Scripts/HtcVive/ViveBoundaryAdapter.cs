#if AR51_HtcVive
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Valve.VR;

namespace AR51.Unity.SDK.BoundaryAdapters
{
    public class ViveBoundaryAdapter
        : IBoundaryAdapter
    {
        private Matrix4x4 _transformation;
        public string BoundaryName => "Native Vive Chaperone";

        public Vector3[] GetPoints()
        {
            if (OpenVR.ChaperoneSetup == null)
                return Array.Empty<Vector3>();

            var pointsFound = OpenVR.ChaperoneSetup.GetLiveCollisionBoundsInfo(out var quads);
            if (!pointsFound)
                return Array.Empty<Vector3>();

            var boundaryPoints = new HashSet<Vector3>();
            for (var i = 0; i < quads.Length; i++)
            {
                boundaryPoints.Add(_transformation.MultiplyPoint(quads[i].vCorners0.ToVector3()));
                boundaryPoints.Add(_transformation.MultiplyPoint(quads[i].vCorners1.ToVector3()));
                boundaryPoints.Add(_transformation.MultiplyPoint(quads[i].vCorners2.ToVector3()));
                boundaryPoints.Add(_transformation.MultiplyPoint(quads[i].vCorners3.ToVector3()));
            }

            return boundaryPoints.ToArray();
        }

        public ViveBoundaryAdapter()
        {
            var position = Vector3.zero;
            var rotation = Quaternion.Euler(Vector3.up*180.0f);
            var scale = new Vector3(-1.0f, 1.0f, 1.0f);
            _transformation = Matrix4x4.TRS(position, rotation, scale);
        }
    }

    public static class ViveDataExtensions
    {
        public static Vector3 ToVector3(this HmdVector3_t value)
        {
            return new Vector3(value.v0, 0.0f, value.v2);
        }
    }
}
#endif