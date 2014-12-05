/*******************************************************************
 * Tweeter for Unity
 * by Deozaan
 * URL: https://gist.github.com/Deozaan/87e90c6679240f81743f
 * Questions or Feedback? http://twitter.com/Deozaan
 *******************************************************************/
 
using System.Collections;
using System.Security.Cryptography;
using System.Text.RegularExpressions;
using UnityEngine;
 
public class Tweeter : MonoBehaviour {
	[SerializeField]
	private string accessTokenSecret = ""; // LEAVE THIS BLANK IN PRODUCTION!	
	private const string tweeterURL = "www.doubledashgames.com/radioai/" + "tweeter.php?status=";
	public bool tweetInEditor = false;
 
	void Start() {
		DontDestroyOnLoad(gameObject);
		if (string.IsNullOrEmpty(accessTokenSecret)) {
			Debug.LogError("No access token assigned. Things won't work.");
		}
 
		if (tweeterURL == "URL_PATH_HEREtweeter.php?status=") {
			Debug.LogError("You forgot to change the tweeter URL. Things won't work.");
		}
	}
 
	/// <summary>Sends a tweet.</summary>
	/// <param name="tweet">The status/tweet message.</param>
	public static void Send(string tweet) {
		if (Application.isEditor && !Instance.tweetInEditor) { return; }
		if (!string.IsNullOrEmpty(Instance.accessTokenSecret)) {
			Instance.StartCoroutine(Instance.SendTweetCoroutine(tweet));
		}
	}
 
	private IEnumerator SendTweetCoroutine(string tweet) {
		string signature = CreateSignature(tweet);
		string fullURL = tweeterURL + WWW.EscapeURL(tweet) + "&hash=" + WWW.EscapeURL(signature);
		//Debug.Log(fullURL);
		WWW w = new WWW(fullURL);
		yield return w;
		if (!string.IsNullOrEmpty(w.error)) {
			Debug.LogWarning("Tweet failed: " + w.error);
		} else {
			if (w.text.StartsWith("{\"errors\"")) {
				Debug.LogWarning("Tweet failed: " + w.text);
			} else {
#if UNITY_EDITOR
				Debug.Log("Tweet Succeeded? " + w.text);
#endif
			}
		}
	}
 
	private byte[] String2Bytes(string myString) {
		return System.Text.Encoding.UTF8.GetBytes(myString);
	}
 
	private string CreateSignature(string signMe) {
		return CreateSignature(signMe, accessTokenSecret);
	}
 
	private string CreateSignature(string signMe, string token) {
		//Debug.Log("Hashing the following text: \n" + signMe);
		HMACSHA1 signature = new HMACSHA1(String2Bytes(token));
		byte[] hash = signature.ComputeHash(String2Bytes(signMe));
		return System.Convert.ToBase64String(hash);
	}
 
	#region Singleton Behaviour
	private static Tweeter _mInstance;
	public static Tweeter Instance {
		get {
			if (!_mInstance) {
				Tweeter[] managers = GameObject.FindObjectsOfType(typeof(Tweeter)) as Tweeter[];
				if (managers.Length != 0) {
					if (managers.Length == 1) {
						_mInstance = managers[0];
						_mInstance.gameObject.name = typeof(Tweeter).Name;
						return _mInstance;
					} else {
						Debug.LogError("You have more than one Tweeter in the scene. You only need 1, it's a singleton!");
						// delete all instances, saving only one (if any) if it has the secret
						bool secretFound = false;
						for (int i = 0; i < managers.Length; i++) {
							if (secretFound || string.IsNullOrEmpty(managers[i].accessTokenSecret)) {
								// and instance with the secret already exists or the secret is null/empty
								Destroy(managers[i].gameObject);
							} else {
								// this is the first one we've found that has a non-null/empty secret. Keep it!
								secretFound = true;
							}
						}
					}
				} else {
					Debug.LogWarning("There were no Tweeters in the scene. Creating one with default accessToken.\n" +
						"If default token is blank (as it should be) then tweeting won't work.");
					GameObject gO = new GameObject(typeof(Tweeter).Name, typeof(Tweeter));
					_mInstance = gO.GetComponent<Tweeter>();
					DontDestroyOnLoad(gO);
				}
			}
 
			return _mInstance;
		}
		set {
			_mInstance = value as Tweeter;
		}
	}
	#endregion
}
 
public static class TweeterExtensions {
	/// <summary>Truncates a string if it is longer than length.
	/// This also removes the @ symbol at the beginning of the string since truncation will break twitter username link.</summary>
	/// <param name="length">The max length of the string before truncation occurs.</param>
	public static string Truncate(this string mString, int length) {
		return (mString.Length > length) ?
				mString.RemoveAtSymbol().Substring(0, length) : // remove the @ sign since truncating the name will break link to Twitter username
				mString; // name is <= length so no changes necessary
	}
 
	/// <summary>Removes @ from the beginning of a string, if one is present</summary>
	public static string RemoveAtSymbol(this string mString) {
		return (mString.StartsWith("@")) ? mString.Substring(1) : mString;
	}
 
	/// <summary>Prefixes a string with a period if it would otherwise start with an @ symbol.
	/// This is useful to make messages show up in all feeds instead of being a pseudo-direct message.</summary>
	public static string PrefixPeriod(this string mString) {
		return (mString.StartsWith("@")) ? "." + mString : mString;
	}
 
	/// <summary>Gives the first name from a string where words are separated by a space.</summary>
	public static string GetFirstName(this string mString) {
		return mString.Split(' ')[0];
	}
}