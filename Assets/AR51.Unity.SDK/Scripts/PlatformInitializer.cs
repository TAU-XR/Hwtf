using AR51.Core;
using AR51.Unity.SDK.BoundaryAdapters;
using AR51.Unity.SDK.CameraAdapters;
using AR51.Unity.SDK.ControllerAdapters;
using AR51.Unity.SDK.HandsAdapters;
using System;
using System.Linq;
using UnityEngine;

namespace AR51.Unity.SDK
{
    /// <summary>
    /// Responsible for platform-specific initialization requirements.
    /// </summary>
    public class PlatformInitializer
        : MonoBehaviour
    {
        private void Awake()
        {
            CameraAdapterFactory.Register<AndroidCameraAdapter>(PlatformType.Android);
            CameraAdapterFactory.Register<AndroidCameraAdapter>(PlatformType.OculusQuest);
            CameraAdapterFactory.Register<DesktopCameraAdapter>(PlatformType.HtcVive);
            CameraAdapterFactory.Register<DesktopCameraAdapter>(PlatformType.Pc);
            CameraAdapterFactory.Register<DesktopCameraAdapter>(PlatformType.OpenXrTethered);
            CameraAdapterFactory.Register<DesktopCameraAdapter>(PlatformType.OvrTethered);
            CameraAdapterFactory.Register<AndroidCameraAdapter>(PlatformType.HtcFocus3);

#if AR51_OculusQuest || AR51_OpenXRTethered || AR51_OVRHands || AR51_OvrTethered
            OVRManager.TrackingLost += ServiceManager.Instance.Disconnect;
            OVRManager.TrackingAcquired += ServiceManager.Instance.Reconnect;
            OVRManager.HMDLost += ServiceManager.Instance.Disconnect;
            //OVRManager.InputFocusLost += ServiceManager.Instance.Disconnect;
            //OVRManager.VrFocusLost += ServiceManager.Instance.Disconnect;
#endif

#if AR51_OculusQuest
            ControllerAdapterFactory.Register<OVRControllerAdapter>(PlatformType.OculusQuest);
            BoundaryAdapterFactory.Register<OVRBoundaryAdapter>(PlatformType.OculusQuest);
            var ovrManager = FindObjectOfType<OVRManager>();
            if (ovrManager != null)
                ovrManager.trackingOriginType = OVRManager.TrackingOrigin.Stage;
#elif AR51_HtcVive
            BoundaryAdapterFactory.Register<ViveBoundaryAdapter>(PlatformType.HtcVive);
#elif AR51_HtcFocus3
            BoundaryAdapterFactory.Register<XRBoundaryAdapter>(PlatformType.HtcFocus3);            
#elif AR51_OvrTethered
            ControllerAdapterFactory.Register<OVRControllerAdapter>(PlatformType.OvrTethered);
            BoundaryAdapterFactory.Register<OVRBoundaryAdapter>(PlatformType.OvrTethered);
            var ovrManager = FindObjectOfType<OVRManager>();
            if (ovrManager != null)
                ovrManager.trackingOriginType = OVRManager.TrackingOrigin.Stage;
#elif AR51_OpenXRTethered
            //BoundaryAdapterFactory.Register<OpenXrBoundaryAdapter>(PlatformType.OpenXrTethered);
            ControllerAdapterFactory.Register<OVRControllerAdapter>(PlatformType.OpenXrTethered);
            BoundaryAdapterFactory.Register<OVRBoundaryAdapter>(PlatformType.OpenXrTethered);
            var ovrManager = FindObjectOfType<OVRManager>();
            if (ovrManager != null)
                ovrManager.trackingOriginType = OVRManager.TrackingOrigin.Stage;
#elif AR51_Hololens2
            HandsAdapterFactory.Register<HololensHandsAdapter>(PlatformType.HoloLens2);
            BoundaryAdapterFactory.Register<HololensBoundaryAdapter>(PlatformType.HoloLens2);
            CameraAdapterFactory.Register<HololensCameraAdapter>(PlatformType.HoloLens);
            CameraAdapterFactory.Register<HololensCameraAdapter>(PlatformType.HoloLens2);
#endif


#if AR51_LeapMotionHands
            HandsAdapterFactory.Register<UltraleapHandsAdapter>(PlatformType.HtcVive);
            HandsAdapterFactory.Register<UltraleapHandsAdapter>(PlatformType.Pc);
            HandsAdapterFactory.Register<UltraleapHandsAdapter>(PlatformType.OpenXrTethered);
            HandsAdapterFactory.Register<UltraleapHandsAdapter>(PlatformType.OvrTethered);
#elif AR51_OVRHands
            HandsAdapterFactory.Register<OVRHandsAdapter>(PlatformType.OculusQuest);
            HandsAdapterFactory.Register<OVRHandsAdapter>(PlatformType.Pc);
            HandsAdapterFactory.Register<OVRHandsAdapter>(PlatformType.OpenXrTethered);
            HandsAdapterFactory.Register<OVRHandsAdapter>(PlatformType.OvrTethered);
#elif AR51_HTCHands
            HandsAdapterFactory.Register<ViveHandsAdapter>(PlatformType.HtcVive);
            HandsAdapterFactory.Register<ViveHandsAdapter>(PlatformType.HtcFocus3);
#endif
        }
    }
}
