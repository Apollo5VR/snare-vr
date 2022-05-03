using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.Services.Authentication;
using Unity.Services.CloudSave;
using Unity.Services.Core;
using UnityEngine;
//using GooglePlayGames;
//using GooglePlayGames.BasicApi;
using Facebook.Unity;
using System;
using UnityEngine.UI;


public class SignIn : MonoBehaviour
{
    public Text successText;
    public bool testFBLogin = false;
    public void OnClickSignInFacebook() => CallFBLoginManual(); //replacing the dynamic fb sdk login with manual 

    //TODO - potentially put in appropriate OnCreate location and try, else delete
    /*
@Override
public void onCreate()
{
super.onCreate();
FacebookSdk.sdkInitialize(getApplicationContext());
AppEventsLogger.activateApp(this);
}
*/

    private async void Awake()
    {
        /*
        if (!FB.IsInitialized)
        {
            // Initialize the Facebook SDK
            FB.Init(InitCallback, OnHideUnity);
        }
        else
        {
            // Already initialized, signal an app activation App Event
            FB.ActivateApp();
        }
        */

        await UnityServices.InitializeAsync();

        AuthenticationService.Instance.SignedIn += () =>
        {
            //Shows how to get a playerID
            Debug.Log($"PlayedID: {AuthenticationService.Instance.PlayerId}");

            //Shows how to get an access token
            Debug.Log($"Access Token: {AuthenticationService.Instance.AccessToken}");

            successText.text = "Success - Press X to open map, Grab location and Pull to Load";

            const string successMessage = "Sign in w FB succeeded!";
            Debug.Log(successMessage);
        };

        AuthenticationService.Instance.SignedOut += () =>
        {
            Debug.Log("Signed Out!");
        };
        //You can listen to events to display custom messages
        AuthenticationService.Instance.SignInFailed += errorResponse =>
        {
            Debug.LogError($"Sign in w FB failed with error code: {errorResponse.ErrorCode}");
        };

        //SignInWithGoogleAsync(string idToken);

        //PlayGamesPlatform.Activate();
        //PlayGamesPlatform.Instance.Authenticate(OnSignInResult); //ManuallyAuthenticate?
        //PlayGamesPlatform.Instance.RequestServerSideAccess(true, thisAction);

        //LoginGooglePlayGames();
    }

    public void Update()
    {
        if(testFBLogin)
        {
            CallFBLoginManual();
            //InitializeFBLogin(); //the actual sdk login flow, which we are no longer doing 4.25.22
            testFBLogin = false;
        }
    }

    private async void CallFBLoginManual()
    {
        await AuthenticationService.Instance.SignInAnonymouslyAsync();

        //await SignInFacebook("EAAEjPF8T9tMBAOhVIdGwg4cAZAu5oeqxk8joRVf8v9qqh5AVjZAZAfTyOmkOZBQZAMx3IWHyVq5WnZBY8AuMWjLFgyd7ZBBBd9W2f8alUMirglqP72j91IqnKPDh6T1kAvD8sk3JQsX3aeFjb6Y0xOP27lFayDQLOPOtHRQQBvo0GZAsjsshmiBoIZAN0vwREbUMOBTeAvCyIGMnNeIYLU6Cr");
    }

    private void InitializeFBLogin()
    {
        if(Application.isEditor)
        {
            CallFBLoginManual();
        }
        else
        {
            var perms = new List<string>() { "public_profile", "email" };
            FB.LogInWithReadPermissions(perms, AuthCallback);
        }
    }

    private async void AuthCallback(ILoginResult result)
    {
        if (FB.IsLoggedIn)
        {
            // AccessToken class will have session details
            var aToken = Facebook.Unity.AccessToken.CurrentAccessToken;
            // Print current access token's User ID
            Debug.Log(aToken.UserId);
            // Print current access token's granted permissions
            foreach (string perm in aToken.Permissions)
            {
                Debug.Log(perm);
            }

            //connect player to UGS
            await SignInFacebook(aToken.TokenString);
        }
        else
        {
            Debug.Log("User cancelled login");
        }
    }

    //unused - for connecting anonymous users
    async Task LinkWithFacebookAsync(string accessToken)
    {
        try
        {
            await AuthenticationService.Instance.LinkWithFacebookAsync(accessToken);
            Debug.Log("Link is successful.");
        }
        catch (AuthenticationException ex) when (ex.ErrorCode == AuthenticationErrorCodes.AccountAlreadyLinked)
        {
            // Prompt the player with an error message.
            Debug.LogError("This user is already linked with another account. Log in instead.");
        }
        catch (AuthenticationException ex)
        {
            // Compare error code to AuthenticationErrorCodes
            // Notify the player with the proper error message
            Debug.LogException(ex);
        }
        catch (RequestFailedException ex)
        {
            // Compare error code to CommonErrorCodes
            // Notify the player with the proper error message
            Debug.LogException(ex);
        }
    }

    private async Task SignInFacebook(string accessTokenStr)
    {
        try
        {
            await AuthenticationService.Instance.SignInWithFacebookAsync(accessTokenStr);
            Debug.Log("Signed in with Facebook!");
            Debug.Log($"PlayedID: {AuthenticationService.Instance.PlayerId}");
            //UpdateUI();
        }
        catch (RequestFailedException ex)
        {
            Debug.LogException(ex);
            Debug.Log("Failed to sign in with Facebook!");
            //SetException(ex);
        }
    }

    private void InitCallback()
    {
        if (FB.IsInitialized)
        {
            // Signal an app activation App Event
            FB.ActivateApp();
            // Continue with Facebook SDK
            // ...
        }
        else
        {
            Debug.Log("Failed to Initialize the Facebook SDK");
        }
    }

    private void OnHideUnity(bool isGameShown)
    {
        if (!isGameShown)
        {
            // Pause the game - we will need to hide
            Time.timeScale = 0;
        }
        else
        {
            // Resume the game - we're getting focus again
            Time.timeScale = 1;
        }
    }

    /*
    private void OnSignInResult(SignInStatus signInStatus)
    {
        if (signInStatus == SignInStatus.Success)
        {
            Debug.Log("Authenticated. Hello, " + Social.localUser.userName + " (" + Social.localUser.id + ")");
        }
        else
        {
            Debug.Log("*** Failed to authenticate with " + signInStatus);
        }
    }
    */
    

    async Task SignInWithGoogleAsync(string idToken)
    {
        try
        {
            await AuthenticationService.Instance.SignInWithGoogleAsync(idToken);
            Debug.Log("SignIn is successful.");
        }
        catch (AuthenticationException ex)
        {
            // Compare error code to AuthenticationErrorCodes
            // Notify the player with the proper error message
            Debug.LogException(ex);
        }
        catch (RequestFailedException ex)
        {
            // Compare error code to CommonErrorCodes
            // Notify the player with the proper error message
            Debug.LogException(ex);
        }
    }

    void LoginGooglePlayGames()
    {
        //PlayGamesPlatform.DebugLogEnabled = true;
        //PlayGamesPlatform.Activate();
        //-PlayGamesPlatform.Instance.Authenticate(OnSignInResult);//ManuallyAuthenticate?
        //Social.localUser.Authenticate(OnGooglePlayGamesLogin);
    }

    void OnGooglePlayGamesLogin(bool success)
    {
        if (success)
        {
            // Call Unity Authentication SDK to sign in or link with Google.
            Debug.Log("Google login success");
            Debug.Log("Login with Google Play Games done. IdToken: " + Social.localUser.id);
        }
        else
        {
            Debug.Log("Unsuccessful login");
        }
    }



    /*
    private async void Awake()
    {
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
    }
    */
}
