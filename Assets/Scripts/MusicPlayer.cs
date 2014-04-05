using UnityEngine;
using System.Collections;
using Wingrove;

public class MusicPlayer : MonoBehaviour 
{
	[SerializeField]
	private string m_eventName;

	void Start()
	{
		Debug.Log ("Posting event: " + m_eventName);
		WingroveAudio.WingroveRoot.Instance.PostEvent (m_eventName);
	}
}
