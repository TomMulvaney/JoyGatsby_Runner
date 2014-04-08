using UnityEngine;
using System.Collections;
using Wingrove;

public class LewisAudio : Singleton<LewisAudio> 
{
	[SerializeField]
	private string m_fall;
	[SerializeField]
	private string m_land;
	[SerializeField]
	private string m_death;
	[SerializeField]
	private string[] m_music;

	void Start()
	{
		StateMachine.Instance.OnStateChange += OnStateChange;
	}

	void OnStateChange(StateMachine.State newState)
	{
		switch (newState) 
		{
		case StateMachine.State.Grounded:
			Post(m_land);
			break;
		case StateMachine.State.Falling:
			Post(m_fall);
			break;
		case StateMachine.State.Death:
			Post(m_death);
			break;
		default:
			break;
		}
	}

	public void PlayMusic(int index = -1)
	{
		if (index == -1) 
		{
			index = Random.Range(0, m_music.Length);
		}

		Post (m_music [index]);
	}

	public void PlayFall()
	{
		Post (m_fall);
	}

	void Post(string eventName)
	{
		WingroveAudio.WingroveRoot.Instance.PostEvent (eventName);
	}
}
