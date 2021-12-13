using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BNG
{
    public class WholeStoneManager : GrabbableEvents
    {
        public GameObject[] stoneShards;
        public GameObject reActivateShard;
        //public HammerController hammerController;

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            int count = 0;

            for(int i = 0; i < stoneShards.Length; i++)
            {
                if(stoneShards[i].activeSelf == false)
                {
                    count++;
                }
            }

            if(count == stoneShards.Length)
            {
                if (reActivateShard.activeSelf == false)
                {
                    foreach (GameObject shard in stoneShards)
                    {
                        shard.SetActive(true);
                    }
                }

                reActivateShard.SetActive(true);
            }
        }
    }
}
