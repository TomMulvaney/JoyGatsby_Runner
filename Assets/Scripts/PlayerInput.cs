using UnityEngine;
using System.Collections;

public class PlayerInput : MonoBehaviour 
{
	[SerializeField]
	private float m_dragThreshold;
	[SerializeField]
	private InputType m_inputType;

	enum InputType
	{
		Flick,
		Tap
	}

	float m_totalDeltaY;

	void OnPress(bool pressed)
	{
		if (m_inputType == InputType.Flick) 
		{
			m_totalDeltaY = 0;
		} 
		else if (m_inputType == InputType.Tap && pressed) 
		{
			if(StateMachine.Instance.state == StateMachine.State.Falling)
			{
				LewisAudio.Instance.PlayFall();
			}

			PlayerMove.Instance.ReverseGravity();
			StateMachine.Instance.RequestChange(StateMachine.State.Falling);
		}
	}

	void OnDrag(Vector2 delta)
	{
		if (m_inputType == InputType.Flick) 
		{
			m_totalDeltaY += delta.y;

			if (Mathf.Abs (m_totalDeltaY) >= m_dragThreshold) 
			{
				bool gravReversed = PlayerMove.Instance.ReverseGravity (m_totalDeltaY);

				if (gravReversed) 
				{
					if(StateMachine.Instance.state == StateMachine.State.Falling)
					{
						LewisAudio.Instance.PlayFall();
					}

					StateMachine.Instance.RequestChange (StateMachine.State.Falling);
				}
			}
		}
	}
}
