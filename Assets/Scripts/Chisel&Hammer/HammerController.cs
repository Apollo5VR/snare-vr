using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HammerController : MonoBehaviour
{
    public BNG.GrabbableHaptics hammerHandHaptic;
    public ChiselController controller;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnCollisionEnter(Collision collisionInfo)
    {
        if (collisionInfo.gameObject.name == "Chisel")
        {
            //ChiselController controller = gameObject.GetComponent<ChiselController>();

            if (controller.touchingStoneShard == true)
            {
                controller.DeactivateShard();
            }


            //TODO-  confirm ON Collision haptics covers this
            //hammerHandHaptic.doHaptics(hammerHandHaptic.currentGrabber.HandSide);
        }
    }
}
