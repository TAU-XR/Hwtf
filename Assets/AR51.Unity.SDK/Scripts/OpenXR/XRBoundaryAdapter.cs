using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

namespace AR51.Unity.SDK.BoundaryAdapters
{
    public class XRBoundaryAdapter
        : IBoundaryAdapter
    {
        private List<Vector3> boundaryPoints = new List<Vector3>();

        public GameObject BoundaryPoint;
        public const string WaveInputSubsystemId = "WVR Input Provider";
        public string BoundaryName => "Native XR Chaperone";

        public Vector3[] GetPoints()
        {
            XRInputSubsystem XRIS = GetInputSubsystem();
            XRIS.TryGetBoundaryPoints(boundaryPoints);

            return boundaryPoints.ToArray();
        }

        static XRInputSubsystem GetInputSubsystem()
        {
            var subsystems = new List<XRInputSubsystem>();
            SubsystemManager.GetInstances(subsystems);
            foreach (var subsystem in subsystems)
            {
                if (subsystem.SubsystemDescriptor.id == WaveInputSubsystemId)
                {
                    return subsystem;
                }
            }
            return null;
        }

    }

}
