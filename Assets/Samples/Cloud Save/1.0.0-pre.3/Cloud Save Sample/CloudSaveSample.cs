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

        private Dictionary<string, string> m_CachedCloudData = new Dictionary<string, string>();

        private async void Awake()
        {
            //This section deactivated 4.24 to make room for FB testin login script
            /*
            // Cloud Save needs to be initialized along with the other Unity Services that
            // it depends on (namely, Authentication), and then the user must sign in.
            //TODO - when looking to update to FB Auth - create a UI (otherwise no UI needed)
            await UnityServices.InitializeAsync();

            AuthenticationService.Instance.SignedIn += () =>
            {
                //Shows how to get a playerID
                Debug.Log($"PlayedID: {AuthenticationService.Instance.PlayerId}");

                //Shows how to get an access token
                Debug.Log($"Access Token: {AuthenticationService.Instance.AccessToken}");

                const string successMessage = "Sign in anonymously succeeded!";
                Debug.Log(successMessage);
            };

            AuthenticationService.Instance.SignedOut += () =>
            {
                Debug.Log("Signed Out!");
            };
            //You can listen to events to display custom messages
            AuthenticationService.Instance.SignInFailed += errorResponse =>
            {
                Debug.LogError($"Sign in anonymously failed with error code: {errorResponse.ErrorCode}");
            };

            await AuthenticationService.Instance.SignInAnonymouslyAsync();
            */

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
            //await LoadAndCacheData();

            //TODO - 4.25 reactivate this essential - 
            /*
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
            */

            //await ListAllKeys();
            //await RetrieveEverything();

            ScriptsConnector.Instance.OnSaveHealthToUGS += SaveHealthToUGS;
            ScriptsConnector.Instance.OnDeleteKey += DeleteKey;
            //ScriptsConnector.Instance.OnGetTimerFromUGS += GetTimerFromUGS;
        }

        private void OnDestroy()
        {
            ScriptsConnector.Instance.OnSaveHealthToUGS -= SaveHealthToUGS;
            ScriptsConnector.Instance.OnDeleteKey -= DeleteKey;
        }

        //TODO - utilize this later
        public async Task LoadAndCacheData()
        {
            try
            {
                m_CachedCloudData = await SaveData.LoadAllAsync();

                // Check that scene has not been unloaded while processing async wait to prevent throw.
                if (this == null) return;

                if (m_CachedCloudData == null)
                {
                    m_CachedCloudData = new Dictionary<string, string>();
                }
            }
            catch (Exception e)
            {
                Debug.LogException(e);
            }
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

        private async void DeleteKey(string key)
        {
            await ForceDeleteSpecificData(key);
        }

        //GG depreciated
        /*
        private float GetTimerFromUGS(string key)
        {
            Task<float> thitdd = TGetTimerFromUGS<float>(key);
            float thitdds;
            thitdds = (float)Convert.ChangeType(thitdd, typeof(float));

            return thitdds;
        }
       

        private async Task<T> TGetTimerFromUGS<T>(string key)
        {
            return await RetrieveSpecificData<T>(key);
        }
        */

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

        //TODO use this to cache
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