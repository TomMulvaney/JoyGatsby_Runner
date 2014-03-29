using UnityEngine;
using System.Collections;

public class PlayerInput : MonoBehaviour 
{
	[SerializeField]
	private float m_dragThreshold;
	[SerializeField]
	private float m_canChangeWhileFalling;

	float m_totalDeltaY;

	void OnPress(bool pressed)
	{
		m_totalDeltaY = 0;
	}

	void OnDrag(Vector2 delta)
	{
		/*
		bool dragIsDown = delta.y < 0;
		bool gravityIsDown = PlayerMove.Instance.gravityDirection == -1;

		if (StateMachine.Instance.state == StateMachine.State.Grounded && dragIsDown != gravityIsDown) 
		{
			m_totalDeltaY += delta.y;

			if (Mathf.Abs (m_totalDeltaY) >= m_dragThreshold) 
			{
				StateMachine.Instance.RequestChange(StateMachine.State.Falling);		
			}
		}
		*/

		m_totalDeltaY += delta.y;

		if (Mathf.Abs (m_totalDeltaY) >= m_dragThreshold) 
		{
			bool gravReversed = PlayerMove.Instance.ReverseGravity(m_totalDeltaY);

			if(gravReversed && StateMachine.Instance.state == StateMachine.State.Grounded)
			{
				StateMachine.Instance.RequestChange(StateMachine.State.Falling);
			}
		}
	}
}
