using System;
using System.Threading.Tasks;
using Unity.Services.Authentication;
using Unity.Services.Core;
using UnityEngine;

public class AuthenticationManager : MonoBehaviour
{
	private void Awake()
	{
		InitialiseAndAuthenticate();
	}

	public static async Task InitialiseAndAuthenticate()
	{
		Debug.Log("Initialising Services and LoginAnon");
		// if (UnityServices.State == ServicesInitializationState.Initialized && AuthenticationService.Instance.IsSignedIn)
		// {
		// Debug.Log("Unity Services already initialized and signed in.");
		// return;
		// }
		Debug.Log("Test");
		// Debug.Log("AuthenticationService.Instance.IsSignedIn = " + AuthenticationService.Instance.IsSignedIn);
		// if (!AuthenticationService.Instance.IsSignedIn)
		// {
		// Optional: Set a unique profile for multi-client testing in the Editor
		InitializationOptions initializationOptions = new InitializationOptions();
#if UNITY_EDITOR || DEVELOPMENT_BUILD
		initializationOptions.SetProfile($"Player{UnityEngine.Random.Range(0, 10000)}");
#endif
		await UnityServices.InitializeAsync(initializationOptions);
		// }


		try
		{
			await AuthenticationService.Instance.SignInAnonymouslyAsync();
			Debug.Log("Sign in anonymously succeeded!");

			// Shows how to get the playerID
			Debug.Log($"PlayerID: {AuthenticationService.Instance.PlayerId}");
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
}