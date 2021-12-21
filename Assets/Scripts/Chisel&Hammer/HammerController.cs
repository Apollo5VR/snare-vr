using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HammerController : MonoBehaviour
{
    public BNG.GrabbableHaptics hammerHandHaptic;
    public ChiselController controller;

    void OnCollisionEnter(Collision collisionInfo)
    {
        if (collisionInfo.gameObject.name == "Chisel")
        {
            if (controller.touchingStoneShard == true)
            {
                controller.DeactivateShard();
            }
        }

        //adding this haptic call because oncollision premade does not do the job
        if (hammerHandHaptic.currentGrabber)
        {
            hammerHandHaptic.doHaptics(hammerHandHaptic.currentGrabber.HandSide);
        }
    }
}
