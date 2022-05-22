using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Unity.Services.Authentication;
using Unity.Services.CloudCode;
using UnityEngine;
using System.Collections.Generic;

public class CloudCodeManager : MonoBehaviour
{
    // Cloud Code SDK exceptions.
    const int k_CloudCodeUnprocessableEntityExceptionErrorCode = 9009;
    const int k_CloudCodeRateLimitExceptionErrorCode = 50;
    const int k_CloudCodeMissingScriptExceptionErrorCode = 9002;

    // Cloud Code script errors.
    const int k_UntypedCustomScriptError = 0;
    const int k_ValidationScriptError = 400;
    const int k_RateLimitScriptError = 429;

    public static CloudCodeManager instance { get; private set; }


    void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this);
        }
        else
        {
            instance = this;
        }
    }

    void OnDestroy()
    {
        if (instance == this)
        {
            instance = null;
        }
    }

    public async Task<IDictionary<string, string>> CallGetFBAccessTokensEndpoint()
    {
        IDictionary<string, string> testPlayers = new Dictionary<string, string>(); ;

        try
        {
            if (!AuthenticationService.Instance.IsSignedIn)
            {
                Debug.LogError("Cloud Code can't be called because you're not logged in.");
                throw new CloudCodeResultUnavailableException(null,
                    "Not logged in to authentication in CallGetTrapTimeRemainingEndpoint.");
            }

            Debug.Log("Calling Cloud Code 'GetTrapTimeRemaining'.");

            var testTokensObj = await CloudCode.CallEndpointAsync<TestPlayers>(
                "GetFBAccessTokens", new object());

            testPlayers = testTokensObj.testPlayers;

            // Check that scene has not been unloaded while processing async wait to prevent throw.
            if (this == null) return testPlayers;

            Debug.Log("CloudCode script got player Dictionary");
        }
        catch (CloudCodeException e)
        {
            HandleCloudCodeException(e);
            throw new CloudCodeResultUnavailableException(e,
                "Handled exception in CheckTrap.");
        }

        return testPlayers;
    }

    public async Task<float> CallGetTrapTimeRemainingEndpoint()
    {
        float timeRemaining;

        try
        {
            if (!AuthenticationService.Instance.IsSignedIn)
            {
                Debug.LogError("Cloud Code can't be called because you're not logged in.");
                throw new CloudCodeResultUnavailableException(null,
                    "Not logged in to authentication in CallGetTrapTimeRemainingEndpoint.");
            }

            Debug.Log("Calling Cloud Code 'GetTrapTimeRemaining'.");

            var timeResult = await CloudCode.CallEndpointAsync<TrapTimeRemainingResult>(
                "GetTrapTimeRemaining", new object());

            // Check that scene has not been unloaded while processing async wait to prevent throw.
            if (this == null) return timeResult.timeRemaining;

            if(timeResult.trapExists)
            {
                timeRemaining = timeResult.timeRemaining;
            }
            else
            {
                //denotes that there is no time remaining data available
                timeRemaining = -1.0f;
            }

            Debug.Log("CloudCode script for time time remaining: " + timeRemaining);
        }
        catch (CloudCodeException e)
        {
            HandleCloudCodeException(e);
            throw new CloudCodeResultUnavailableException(e,
                "Handled exception in CheckTrap.");
        }

        return timeRemaining;
    }

    public async Task CallSetTrapTriggeredTimeEndpoint()
    {
        try
        {
            if (!AuthenticationService.Instance.IsSignedIn)
            {
                Debug.LogError("Cloud Code can't be called to grant random currency because you're not logged in.");
                throw new CloudCodeResultUnavailableException(null,
                    "Not logged in to authentication in CallSetTrapTriggeredTimeEndpoint.");
            }

            Debug.Log("Calling Cloud Code 'SetTrapTriggerTime'.");

            string timeResult = await CloudCode.CallEndpointAsync(
                "SetTrapTriggerTime", new object());

            // Check that scene has not been unloaded while processing async wait to prevent throw.
            if (this == null) return;

            Debug.Log("CloudCode script for trap trigger time: " + timeResult);
        }
        catch (CloudCodeException e)
        {
            HandleCloudCodeException(e);
            throw new CloudCodeResultUnavailableException(e,
                "Handled exception in SetTrapTriggerTime.");
        }
    }

    public async Task<bool> CallCheckTrapEndpoint()
    {
        bool caught = false;

        try
        {
            if (!AuthenticationService.Instance.IsSignedIn)
            {
                Debug.LogError("Cloud Code can't be called because you're not logged in.");
                throw new CloudCodeResultUnavailableException(null,
                    "Not logged in to authentication in CallCheckTrapEndpoint.");
            }

            Debug.Log("Calling Cloud Code 'CheckTrap'.");

            var trapResult = await CloudCode.CallEndpointAsync<CheckTrapResult>(
                "CheckTrap", new object());

            // Check that scene has not been unloaded while processing async wait to prevent throw.
            if (this == null) return trapResult.caught;

            if(trapResult.caught == true)
            {
                caught = trapResult.caught;
            }

            Debug.Log("CloudCode script for check trap caught: " + trapResult.caught);
        }
        catch (CloudCodeException e)
        {
            HandleCloudCodeException(e);
            throw new CloudCodeResultUnavailableException(e,
                "Handled exception in CheckTrap.");
        }

        return caught; //TODO - GG - test if problems this could cause
    }

    public async Task<float> CallGetHealthRemainingEndpoint()
    {
        float healthRemaining;

        try
        {
            if (!AuthenticationService.Instance.IsSignedIn)
            {
                Debug.LogError("Cloud Code can't be called because you're not logged in.");
                throw new CloudCodeResultUnavailableException(null,
                    "Not logged in to authentication in CallGetHealthRemainingEndpoint.");
            }

            Debug.Log("Calling Cloud Code 'GetHealthRemaining'.");

            var healthResult = await CloudCode.CallEndpointAsync<HealthRemainingResult>(
                "GetHealthRemaining", new object());

            // Check that scene has not been unloaded while processing async wait to prevent throw.
            if (this == null) return healthResult.healthFloat;

            healthRemaining = healthResult.healthFloat;

            Debug.Log("CloudCode script for time time remaining: " + healthRemaining);
        }
        catch (CloudCodeException e)
        {
            HandleCloudCodeException(e);
            throw new CloudCodeResultUnavailableException(e,
                "Handled exception in CheckTrap.");
        }

        return healthRemaining;
    }

    void HandleCloudCodeException(CloudCodeException e)
    {
        switch (e.ErrorCode)
        {
            case k_CloudCodeUnprocessableEntityExceptionErrorCode:
                var cloudCodeCustomError = ConvertToActionableError(e);
                HandleCloudCodeScriptError(cloudCodeCustomError);
                break;

            case k_CloudCodeRateLimitExceptionErrorCode:
                Debug.Log("Rate Limit Exceeded. Try Again.");
                break;

            case k_CloudCodeMissingScriptExceptionErrorCode:
                Debug.Log("Couldn't find requested Cloud Code Script");
                break;

            default:
                Debug.Log(e);
                break;
        }
    }

    CloudCodeCustomError ConvertToActionableError(CloudCodeException e)
    {
        // trim the text that's in front of the valid JSON
        var trimmedExceptionMessage = Regex.Replace(
            e.Message, @"^[^\{]*", "", RegexOptions.IgnorePatternWhitespace);

        if (string.IsNullOrEmpty(trimmedExceptionMessage))
        {
            return new CloudCodeCustomError("Could not parse CloudCodeException.");
        }

        // Convert the message string ultimately into the Cloud Code Custom Error object which has a
        // standard structure for all errors.
        var parsedMessage = JsonUtility.FromJson<CloudCodeExceptionParsedMessage>(trimmedExceptionMessage);
        return JsonUtility.FromJson<CloudCodeCustomError>(parsedMessage.message);
    }

    // This method does whatever handling is appropriate given the specific error. So for example for an invalid
    // play in the Cloud Ai Mini Game, it shows a popup in the scene to explain the error.
    void HandleCloudCodeScriptError(CloudCodeCustomError cloudCodeCustomError)
    {
        switch (cloudCodeCustomError.status)
        {
            case k_UntypedCustomScriptError:
                Debug.Log($"Cloud code returned error: {cloudCodeCustomError.status}: " +
                            $"{cloudCodeCustomError.title}: {cloudCodeCustomError.message}");
                break;

            case k_ValidationScriptError:
                Debug.Log($"{cloudCodeCustomError.title}: {cloudCodeCustomError.message} : " +
                            $"{cloudCodeCustomError.additionalDetails[0]}");
                break;

            case k_RateLimitScriptError:
                Debug.Log($"Rate Limit has been exceeded. Wait {cloudCodeCustomError.retryAfter} " +
                            $"seconds and try again.");
                break;

            default:
                Debug.Log($"Cloud code returned error: {cloudCodeCustomError.status}: " +
                            $"{cloudCodeCustomError.title}: {cloudCodeCustomError.message}");
                break;
        }
    }

    public struct CheckTrapResult
    {
        public bool caught;
    }

    public struct TrapTimeRemainingResult
    {
        public float timeRemaining; //milliseconds
        public bool trapExists;
        public bool hasExpired;
    }
    public struct TestPlayers
    {
        public IDictionary<string,string> testPlayers; //10 max
    }

    public struct HealthRemainingResult
    {
        public float healthFloat;
    }

    // Struct used to receive result from Cloud Code.
    public struct GrantRandomCurrencyResult
    {
        public string currencyId;
        public int amount;
    }

    public struct CloudCodeExceptionParsedMessage
    {
        public string message;
    }

    public struct CloudCodeCustomError
    {
        public int status;
        public string title;
        public string message;
        public string retryAfter;
        public string[] additionalDetails;

        public CloudCodeCustomError(string title)
        {
            this.title = title;
            status = 0;
            message = null;
            retryAfter = null;
            additionalDetails = new string[] { };
        }
    }
}