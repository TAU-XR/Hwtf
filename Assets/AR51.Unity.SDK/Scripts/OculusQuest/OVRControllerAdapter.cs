#if AR51_OculusQuest || AR51_OpenXRTethered || AR51_OvrTethered
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AR51.Unity.SDK.ControllerAdapters
{
    public class OVRControllerAdapter : IControllerAdapter
    {
        public string SourceDevice => "OculusQuest";

        //public ControllerInfo LeftControllerInfo(ControllerType controllerType)
        //{
        //    //var handType = _revolverHand.HandType;
        //    //var triggerInputAxis = handType.GetTriggerInputAxis();
        //    //_revolverHand.CurrentRotation = OVRInput.Get(triggerInputAxis);

        //    // returns true if the primary button (typically “A”) is currently pressed.
        //    var buttonAPressed = OVRInput.Get(OVRInput.Button.One);

        //    // returns true if the primary button (typically “A”) was pressed this frame.
        //    var buttonADown = OVRInput.GetDown(OVRInput.Button.One);

        //    // returns true if the “X” button was released this frame.
        //    var buttonXUp = OVRInput.GetUp(OVRInput.RawButton.X);

        //    return new ControllerInfo(ControllerType.LeftController);
        //}


        public ControllerInfo LeftControllerInfo
        {
            get
            {
                var info = new ControllerInfo(ControllerType.LeftController);

                var activeController = OVRInput.GetActiveController();
                info.IsDetected = activeController == OVRInput.Controller.LTouch ||
                     activeController == OVRInput.Controller.Touch;
                var p = OVRInput.GetLocalControllerPosition(OVRInput.Controller.LTouch);
                var r = OVRInput.GetLocalControllerRotation(OVRInput.Controller.LTouch);
                if (r.x == 0 && r.y == 0 && r.z == 0 && r.w == 0) r = Quaternion.identity;
                info.LocalToWorld = Matrix4x4.TRS(p, r, Vector3.one);

                info.ButtonOneIsPressed = OVRInput.GetDown(OVRInput.Button.One, OVRInput.Controller.LTouch);
                info.ButtonTwoIsPressed = OVRInput.GetDown(OVRInput.Button.Two, OVRInput.Controller.LTouch);
                info.IndexTriggerIsPressed = OVRInput.GetDown(OVRInput.Button.PrimaryIndexTrigger, OVRInput.Controller.LTouch);
                info.HandTriggerIsPressed = OVRInput.GetDown(OVRInput.Button.PrimaryHandTrigger, OVRInput.Controller.LTouch);

                return info;
            }
        }

        public ControllerInfo RightControllerInfo
        {
            get
            {
                var info = new ControllerInfo(ControllerType.RightController);
                var activeController = OVRInput.GetActiveController();
                info.IsDetected = activeController == OVRInput.Controller.RTouch ||
                     activeController == OVRInput.Controller.Touch;
                var p = OVRInput.GetLocalControllerPosition(OVRInput.Controller.RTouch);
                var r = OVRInput.GetLocalControllerRotation(OVRInput.Controller.RTouch);
                if (r.x == 0 && r.y == 0 && r.z == 0 && r.w == 0) r = Quaternion.identity;
                info.LocalToWorld = Matrix4x4.TRS(p, r, Vector3.one);

                info.ButtonOneIsPressed = OVRInput.GetDown(OVRInput.Button.One, OVRInput.Controller.RTouch);
                info.ButtonTwoIsPressed = OVRInput.GetDown(OVRInput.Button.Two, OVRInput.Controller.RTouch);
                info.IndexTriggerIsPressed = OVRInput.GetDown(OVRInput.Button.PrimaryIndexTrigger, OVRInput.Controller.RTouch);
                info.HandTriggerIsPressed = OVRInput.GetDown(OVRInput.Button.PrimaryHandTrigger, OVRInput.Controller.RTouch);

                return info;
            }
        }

    }
}
#endif