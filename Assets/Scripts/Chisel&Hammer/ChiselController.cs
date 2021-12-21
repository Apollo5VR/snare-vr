using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChiselController : MonoBehaviour
{
    public MeshRenderer chiselColor;
    public BNG.GrabbableHaptics chiselHandHaptic;
    public bool touchingStoneShard = false;
    public GameObject stoneShard;

    public int shardDestroyedCount;

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
        //TODO - activate a glow or color change of the chisel to notify user visually contact is made 
        if(collisionInfo.gameObject.tag == "Shard")
        {
            stoneShard = collisionInfo.gameObject;
            chiselColor.material.color = Color.green;
            touchingStoneShard = true;
        }

        //adding this haptic call because oncollision premade does not do the job
        if(chiselHandHaptic.currentGrabber)
        {
            chiselHandHaptic.doHaptics(chiselHandHaptic.currentGrabber.HandSide);
        }
    }

    void OnCollisionExit(Collision collisionInfo)
    {
        //TODO - activate a glow or color change of the chisel to notify user visually contact is made 
        if (collisionInfo.gameObject.tag == "Shard")
        {
            stoneShard = null;
            chiselColor.material.color = Color.red;
            touchingStoneShard = false;
        }
    }

    public void DeactivateShard()
    {
        if(stoneShard != null)
        {
            stoneShard.SetActive(false);
            stoneShard = null;

            shardDestroyedCount++;

            if(shardDestroyedCount == 15)
            {
                if (BNG.WholeStoneController.Instance != null)
                {
                    BNG.WholeStoneController.Instance.OnRockInteraction(CommonEnums.HouseResponses.Gryfindor);
                }
            }
        }
    }
}
