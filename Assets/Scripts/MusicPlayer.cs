using UnityEngine;
using System.Collections;
using Wingrove;

public class MusicPlayer : MonoBehaviour 
{
	[SerializeField]
	private string[] m_musicEventNames;

	void Start()
	{
		string eventName = m_musicEventNames [Random.Range (0, m_musicEventNames.Length)];
		Debug.Log ("Posting event: " + eventName);
		WingroveAudio.WingroveRoot.Instance.PostEvent (eventName);
	}
}
