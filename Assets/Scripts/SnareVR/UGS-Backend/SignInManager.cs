using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.Services.Authentication;
using Unity.Services.CloudSave;
using Unity.Services.Core;
using UnityEngine;
//using GooglePlayGames;
//using GooglePlayGames.BasicApi;
//using Facebook.Unity;
using System;
using UnityEngine.UI;


public class SignInManager : MonoBehaviour
{
    public enum AppState { This, That, Other }


    public Text successText;
    public bool testFBLogin = false;
    public List<GameObject> buttons;
    public void OnClickSignInFacebook(GameObject textObj) => CallFBLoginManual(textObj); //replacing the dynamic fb sdk login with manual 


    private IDictionary<string, string> players;
    private bool isAnonymousLogin = true; //always true on start as we use Anon login to get FB Access Tokens, then we login to FB using select AT
    
    //Note: Used for when need to setup FB SDK for first time in Unity Project
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
        //depreciate FB login init
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

            string successMessage = "";
            
            if(isAnonymousLogin)
            {
                successMessage = "Anonymous sign in succeeded";
                GrabFBAccessTokens();
            }
            //logged in via FB Access Token
            else
            {
                successMessage = "FB sign in succeeded";
                successText.text = "Success - Press X to open map, Grab location and Pull to Load";
                ScriptsConnector.Instance.OnCacheHealthFromUGS();
            }

            Debug.Log(successMessage);
        };

        AuthenticationService.Instance.SignedOut += () =>
        {
            string successMessage = "";

            if (isAnonymousLogin)
            {
                successMessage = "Anonymous loggout succeeded";
            }
            else
            {
                successMessage = "FB loggout succeeded";
            }

            Debug.Log(successMessage);
        };
        //You can listen to events to display custom messages
        AuthenticationService.Instance.SignInFailed += errorResponse =>
        {
            if (isAnonymousLogin)
            {
                successText.text = "Anonymous login fail";
            }
            else
            {
                successText.text = "FB login fail";
            }

            Debug.LogError($"Sign in failed with error code: {errorResponse.ErrorCode}");
        };

        await AuthenticationService.Instance.SignInAnonymouslyAsync();

        #region depreciatedLoginMethods
        //SignInWithGoogleAsync(string idToken);

        //PlayGamesPlatform.Activate();
        //PlayGamesPlatform.Instance.Authenticate(OnSignInResult); //ManuallyAuthenticate?
        //PlayGamesPlatform.Instance.RequestServerSideAccess(true, thisAction);

        //LoginGooglePlayGames();
        #endregion
    }

    public void Update()
    {
        if(testFBLogin)
        {
            GameObject basic = new GameObject();
            basic.AddComponent<Text>();

            CallFBLoginManual(basic);
            //InitializeFBLogin(); //the actual sdk login flow, which we are no longer doing 4.25.22
            testFBLogin = false;
        }
    }

    private async void GrabFBAccessTokens()
    {
        //call Cloud Script to get list of access token - adds them to a dictionary
        if (Unity.Services.Authentication.AuthenticationService.Instance.IsSignedIn)
        {
            players = await CloudCodeManager.instance.CallGetFBAccessTokensEndpoint();

            //activate player select buttons based on dictonary items
            int i = 0;

            foreach(string name in players.Keys)
            {
                //to prevent a dictionary exceeding buttons 10 (can refactor if we ever branch out from 10)
                if(i <= buttons.Count)
                {
                    //do activate
                    buttons[i].GetComponentInChildren<Text>().text = name;
                    buttons[i].SetActive(true);
                }

                i++;
            }
        }

        //signs out of the anonymous account we are using to grab valid Facebook Access Tokens
        AuthenticationService.Instance.SignOut();
    }

    //called on event of pointer click when user selects a profile
    private async void CallFBLoginManual(GameObject objUserKey)
    {
        string tokenKey = objUserKey.GetComponentInChildren<Text>().text; 
        string accessToken = players[tokenKey];

        isAnonymousLogin = false;
        await SignInFacebook(accessToken);
    }

    //note: a UGS specific method (does not require FB SDK)
    private async Task SignInFacebook(string accessTokenStr)
    {
        try
        {
            await AuthenticationService.Instance.SignInWithFacebookAsync(accessTokenStr); ;
        }
        catch (RequestFailedException ex)
        {
            Debug.LogException(ex);
            Debug.Log("Failed to sign in with Facebook!");
            //SetException(ex);
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

    //depreciated Facebook and Google Play login methods
    /*
    private void InitializeFBLogin()
    {
        if(Application.isEditor)
        {
            GameObject basic = new GameObject();
            basic.AddComponent<Text>();

            CallFBLoginManual(basic);
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


    /*
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
