using UnityEngine;
using System.Collections;

public class DeathHandler : MonoBehaviour 
{
	[SerializeField]
	private Transform m_startLocation;

	void Start()
	{
		StateMachine.Instance.OnStateChange += OnStateChange;
	}

	void OnStateChange(StateMachine.State newState)
	{
		//Debug.Log ("DH.OSC: " + newState);
		if (newState == StateMachine.State.Death) 
		{
			transform.position = m_startLocation.position;
			StateMachine.Instance.RequestChange(StateMachine.State.Transition);
		}
	}
}
