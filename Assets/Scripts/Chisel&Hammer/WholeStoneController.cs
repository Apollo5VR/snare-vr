using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;
using Unity.Services.Analytics;

namespace BNG
{
    public class WholeStoneController : GrabbableEvents
    {
        public static WholeStoneController Instance { get; private set; }
        public GameObject[] stoneShards;
        public GameObject reActivateShard;
        public GameObject secretShard;
        public HandleRotator rotator;
        public AudioSource speakAudio;
        public GameObject santaHat;
        //public HammerController hammerController;

        public Action<CommonEnums.HouseResponses> OnRockInteraction;

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            else
            {
                Destroy(gameObject);
            }
        }

        // Start is called before the first frame update
        void Start()
        {
            OnRockInteraction += AlterRock;
        }

        private void AlterRock(CommonEnums.HouseResponses response)
        {
            this.GetComponentInChildren<BoxCollider>().enabled = false;

            //will usually happen only 1x, but can happen more if you "change your mind" last second (part of the test) (still? 12.19.21)
            ResponseCollector.Instance.OnResponseSelected?.Invoke(response);

            if (!Application.isEditor)
            {
                //Analytics Beta
                Dictionary<string, object> parameters = new Dictionary<string, object>()
            {
                { "specificQuestion", "RockCube" },
                { "houseIndex", (int)response },
            };
                AnalyticsService.Instance.CustomData("questionResponse", parameters);
            }


            
            switch ((int)response)
            {
                case 1:
                    //Ravenclaw;
                    //pressed shard
                    ShardsToKinematic();
                    break;
                case 2:
                    //Gryfindor;
                    //done by hand, rock by rock if above 10 broken, have it fall apart
                    ShardsToKinematic();
                    break;
                case 3:
                    //Hufflepuff;
                    //rock animated
                    rotator.enabled = true;
                    speakAudio.Play();
                    StartCoroutine(RockAudioWait());
                    //audio source play
                    break;
                case 4:
                    //Slytherin;
                    //lightning & rock explosion 
                    ShardsToKinematic();
                    break;
                default:
                    break;
            }
            
        }

        private IEnumerator RockAudioWait()
        {
            yield return new WaitForSeconds(10);

            ShardsToKinematic();
        }

        private void ShardsToKinematic()
        {
            for (int i = 0; i < stoneShards.Length; i++)
            {
                if (stoneShards[i].activeSelf == true)
                {
                    Rigidbody shardRb = stoneShards[i].GetComponent<Rigidbody>();
                    shardRb.isKinematic = false;
                    shardRb.useGravity = true;
                }
            }

            santaHat.GetComponent<SphereCollider>().enabled = true;

            ProgressionController.Instance.OnLoadNextScene?.Invoke(15);
        }

        // Update is called once per frame
        /*
        void Update()
        {
            //TODO - remove all this was only for testing
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
        */

        private void OnDestroy()
        {
            OnRockInteraction -= AlterRock;
        }
    }
}
