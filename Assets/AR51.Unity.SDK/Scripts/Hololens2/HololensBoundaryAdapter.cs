#if AR51_Hololens2
using System;
using UnityEngine;

namespace AR51.Unity.SDK.BoundaryAdapters
{
    public class HololensBoundaryAdapter
        : IBoundaryAdapter
    {
        public string BoundaryName => "Native Hololens Boundary";

        public Vector3[] GetPoints()
        {
            return Array.Empty<Vector3>();
        }
    }
}
#endif