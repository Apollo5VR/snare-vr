using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BNG
{
    public class GrabGameLauncher : GrabbableEvents
    {
        private bool isStart;

        public override void OnGrab(Grabber grabber)
        {
            if (isStart)
            {
                Debug.Log("Mock Wire Grabbed");
                isStart = false;
                ProgressionController.Instance.OnLoadNextScene(1);

                //base.OnGrab(grabber);
            }
        }
    }
}
