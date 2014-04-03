using UnityEngine;
using System.Collections;

public class PlayerMove : Singleton<PlayerMove> 
{
	private Phys.Direction m_gravityDirection;
	public int gravityDirection
	{
		get
		{
			return (int)m_gravityDirection;
		}
	}

	[SerializeField]
	float m_horizontalSpeedStart = 0.5f;
	[SerializeField]
	float m_horizontalSpeedIncrement = 0.15f;
	[SerializeField]
	float m_verticalSpeed = 2f;
	[SerializeField]
	private Phys.Direction m_startGravityDirection = Phys.Direction.Down;

	bool m_applyCounterForce = false;

	float m_horizontalSpeed;

	float m_targetGroundedPosY;

	void Awake()
	{
		m_horizontalSpeed = m_horizontalSpeedStart;
	}

	void OnStageUp(int stageNum)
	{
		m_horizontalSpeed += m_horizontalSpeedIncrement;
	}

	public void ReverseGravity()
	{
		m_gravityDirection = m_gravityDirection == Phys.Direction.Down ? Phys.Direction.Up : Phys.Direction.Down;

		m_applyCounterForce = StateMachine.Instance.state == StateMachine.State.Falling;
	}

	public bool ReverseGravity(float deltaY)
	{
		bool dragIsDown = deltaY < 0;
		bool gravIsDown = (int)m_gravityDirection == -1;

		bool shouldReverse = dragIsDown != gravIsDown;

		if (shouldReverse) 
		{
			m_gravityDirection = m_gravityDirection == Phys.Direction.Down ? Phys.Direction.Up : Phys.Direction.Down;

			m_applyCounterForce = StateMachine.Instance.state == StateMachine.State.Falling;
		}

		return shouldReverse;
	}

	void Start()
	{
		m_gravityDirection = m_startGravityDirection;

		StateMachine.Instance.OnStateChange += OnStateChange;
		StageManager.Instance.OnStageUp += OnStageUp;
	}

	void FixedUpdate () 
	{
		Vector3 velocity = rigidbody.velocity;
		velocity.x = m_horizontalSpeed;
		rigidbody.velocity = velocity;

		if (StateMachine.Instance.state == StateMachine.State.Falling) 
		{
			if(m_applyCounterForce)
			{
				Vector3 counterVelocity = rigidbody.velocity;
				counterVelocity.y = 0;
				rigidbody.velocity = counterVelocity;
				m_applyCounterForce = false;
			}

			rigidbody.AddForce (new Vector3 (0, m_verticalSpeed, 0) * (int)m_gravityDirection);		
		} 
	}

	void OnStateChange(StateMachine.State newState)
	{
		switch (newState) 
		{
		case StateMachine.State.Grounded:
			Vector3 newVelocity = rigidbody.velocity;
			newVelocity.y = 0;
			rigidbody.velocity = newVelocity;
			break;
		case StateMachine.State.Death:
			rigidbody.velocity = Vector3.zero;
			m_gravityDirection = m_startGravityDirection;
			break;
		default:
			break;
		}
	}
}
