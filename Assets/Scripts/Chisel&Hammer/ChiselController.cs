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
            //TODO-  confirm ON Collision haptics covers this
            //chiselHandHaptic.doHaptics(chiselHandHaptic.currentGrabber.HandSide);
            stoneShard = collisionInfo.gameObject;
            chiselColor.material.color = Color.green;
            touchingStoneShard = true;
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

            if(shardDestroyedCount == 9)
            {
                if (BNG.WholeStoneController.Instance != null)
                {
                    BNG.WholeStoneController.Instance.OnRockInteraction(CommonEnums.HouseResponses.Gryfindor);
                }
            }
        }
    }
}
