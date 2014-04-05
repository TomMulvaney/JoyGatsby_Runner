using UnityEngine;
using System.Collections;

public class StateMachine : Singleton<StateMachine>
{
	public delegate void StateChange(State newState);
	public event StateChange OnStateChange;
	public event StateChange PreStateChange;

	public enum State
	{
		Transition,
		Grounded,
		Falling,
		Death
	}

	private State m_nextState;
	public State nextState
	{
		get
		{
			return m_nextState;
		}
	}

	private State m_state;
	public State state
	{
		get
		{
			return m_state;
		}
	}

	bool m_locked = false;

	void Update()
	{
		if (m_state != m_nextState)
		{
			//Debug.Log(m_state + " - " + m_nextState);

			m_locked = false;

			if(PreStateChange != null)
			{
				PreStateChange(m_nextState);
			}

			m_state = m_nextState;

			if(OnStateChange != null)
			{
				OnStateChange(m_state);
			}
		}
	}

	public void RequestChange(State requestedState, bool shouldLock = false)
	{
		if (requestedState != m_state) 
		{
			//Debug.Log (System.String.Format ("RequestChange({0}, {1})", requestedState, shouldLock));
			Debug.Log (System.String.Format ("({0} - {1})", m_state, requestedState));
			if (!m_locked || requestedState > m_nextState) {
				m_nextState = requestedState;	
				m_locked = shouldLock;
			} else {
				Debug.Log ("Request denied: " + requestedState);
			}
		}
	}
}
