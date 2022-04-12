using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.Services.Authentication;
using Unity.Services.CloudSave;
using Unity.Services.Core;
using UnityEngine;

namespace CloudSaveSample
{
    [Serializable]
    public class StatsObject
    {
        public float healthFloat;
        public float staminaFloat;
    }

    /*
    public class SampleObject
    {
        public string SophisticatedString;
        public int SparklingInt;
        public float AmazingFloat;
    }
    */

    //GG - Used for Health initialization / Health Get (TODO - rename for own project's purposes)
    public class CloudSaveSample : MonoBehaviour
    {
        private async void Awake()
        {
            // Cloud Save needs to be initialized along with the other Unity Services that
            // it depends on (namely, Authentication), and then the user must sign in.
            //TODO - when looking to update to FB Auth - create a UI (otherwise no UI needed)
            await UnityServices.InitializeAsync();
            await AuthenticationService.Instance.SignInAnonymouslyAsync();

            Debug.Log("Signed in? Need to do check?");

            //legacy sample code
            /*
            SampleObject outgoingSample = new SampleObject
            {
                AmazingFloat = 13.37f,
                SparklingInt = 1337,
                SophisticatedString = "hi there!"
            };
            await ForceSaveObjectData("object_key", outgoingSample);
            SampleObject incomingSample = await RetrieveSpecificData<SampleObject>("object_key");
            Debug.Log($"Loaded sample object: {incomingSample.AmazingFloat}, {incomingSample.SparklingInt}, {incomingSample.SophisticatedString}");

            await ForceDeleteSpecificData("object_key");
            */

            
            #region HealthDataLoad
            StatsObject statsObj = await RetrieveSpecificData<StatsObject>("stats");

            if (statsObj == null)
            {
                StatsObject outgoingStats = new StatsObject
                {
                    healthFloat = 100.0f,
                    staminaFloat = 0.0f
                };

                await ForceSaveObjectData("stats", outgoingStats);

                statsObj = await RetrieveSpecificData<StatsObject>("stats");

                ScriptsConnector.Instance?.OnSetHealth("playerId", statsObj.healthFloat); //TODO - V2 - update to actual playerId for multiplayer functionality
                Debug.Log("Loaded sample object: " + statsObj.healthFloat);
            }
            else
            {
                ScriptsConnector.Instance?.OnSetHealth("playerId", statsObj.healthFloat); //TODO - V2 - update to actual playerId for multiplayer functionality
                Debug.Log("Loaded sample object: " + statsObj.healthFloat);
            }
            #endregion

            #region TrapLoadData
            float trapTriggerTime = await RetrieveSpecificData<float>("TRAP_TRIGGER_TIME");

            if (trapTriggerTime == default) 
            {
                //Users first time, load directly into tutorial mode


            }
            else
            {
                DateTimeOffset now = DateTimeOffset.UtcNow;
                long unixTimeMilliseconds = now.ToUnixTimeMilliseconds();

                if(trapTriggerTime == 0)
                {
                    //Load directy into build snare
                }
                else if (unixTimeMilliseconds > trapTriggerTime)
                {
                    //Load directly into check trap mode

                }
                else
                {
                    //Load directly into a "trap not ready" mode
                }
            }
            #endregion


            await ListAllKeys();
            await RetrieveEverything();

            ScriptsConnector.Instance.OnSaveHealthToUGS += SaveHealthToUGS;
        }

        private async void SaveHealthToUGS(string key, string value)
        {
            StatsObject outgoingStats = new StatsObject
            {
                healthFloat = float.Parse(value),
                staminaFloat = 0.0f
            };

            await ForceSaveObjectData(key, outgoingStats); //"stats"
        }

        private async Task ListAllKeys()
        {
            try
            {
                var keys = await SaveData.RetrieveAllKeysAsync();

                Debug.Log($"Keys count: {keys.Count}\n" + 
                          $"Keys: {String.Join(", ", keys)}");
            }
            catch (CloudSaveValidationException e)
            {
                Debug.LogError(e);
            }
            catch (CloudSaveException e)
            {
                Debug.LogError(e);
            }
        }

        private async Task ForceSaveSingleData(string key, string value)
        {
            try
            {
                Dictionary<string, object> oneElement = new Dictionary<string, object>();

                // It's a text input field, but let's see if you actually entered a number.
                if (Int32.TryParse(value, out int wholeNumber))
                {
                    oneElement.Add(key, wholeNumber);
                }
                else if (Single.TryParse(value, out float fractionalNumber))
                {
                    oneElement.Add(key, fractionalNumber);
                }
                else
                {
                    oneElement.Add(key, value);
                }

                await SaveData.ForceSaveAsync(oneElement);

                Debug.Log($"Successfully saved {key}:{value}");
            }
            catch (CloudSaveValidationException e)
            {
                Debug.LogError(e);
            }
            catch (CloudSaveException e)
            {
                Debug.LogError(e);
            }
        }

        private async Task ForceSaveObjectData(string key, StatsObject value)
        {
            try
            {
                // Although we are only saving a single value here, you can save multiple keys
                // and values in a single batch.
                Dictionary<string, object> oneElement = new Dictionary<string, object>
                {
                    { key, value }
                };

                await SaveData.ForceSaveAsync(oneElement);

                Debug.Log($"Successfully saved {key}:{value}");
            }
            catch (CloudSaveValidationException e)
            {
                Debug.LogError(e);
            }
            catch (CloudSaveException e)
            {
                Debug.LogError(e);
            }
        }

        private async Task<T> RetrieveSpecificData<T>(string key)
        {
            try
            {
                var results = await SaveData.LoadAsync(new HashSet<string>{key});

                if (results.TryGetValue(key, out string value))
                {
                    T objType = JsonUtility.FromJson<T>(value);

                    if (objType == null)
                    {
                        //is not obj
                        return (T)Convert.ChangeType(value, typeof(T)); 
                    }
                    else
                    {
                        //is obj
                        return JsonUtility.FromJson<T>(value);
                    } 
                }
                else
                {
                    Debug.Log($"There is no such key as {key}!");
                }
            }
            catch (CloudSaveValidationException e)
            {
                Debug.LogError(e);
            }
            catch (CloudSaveException e)
            {
                Debug.LogError(e);
            }

            return default;
        }

        private async Task RetrieveEverything()
        {
            try
            {
                // If you wish to load only a subset of keys rather than everything, you
                // can call a method LoadAsync and pass a HashSet of keys into it.
                var results = await SaveData.LoadAllAsync();

                Debug.Log($"Elements loaded!");
                
                foreach (var element in results)
                {
                    Debug.Log($"Key: {element.Key}, Value: {element.Value}");
                }
            }
            catch (CloudSaveValidationException e)
            {
                Debug.LogError(e);
            }
            catch (CloudSaveException e)
            {
                Debug.LogError(e);
            }
        }

        private async Task ForceDeleteSpecificData(string key)
        {
            try
            {
                await SaveData.ForceDeleteAsync(key);

                Debug.Log($"Successfully deleted {key}");
            }
            catch (CloudSaveValidationException e)
            {
                Debug.LogError(e);
            }
            catch (CloudSaveException e)
            {
                Debug.LogError(e);
            }
        }
    }
}