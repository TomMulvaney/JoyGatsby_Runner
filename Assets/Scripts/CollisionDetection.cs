using UnityEngine;
using System.Collections;
using System;

public class CollisionDetection : MonoBehaviour 
{
	[SerializeField]
	private float m_castDistance = 0.05f;
	[SerializeField]
	private Transform[] m_horizontalOrigins;
	[SerializeField]
	private Transform[] m_verticalOriginsBot;
	[SerializeField]
	private Transform[] m_verticalOriginsTop;
	
	Vector3 m_debugLineOffset = new Vector3 (0, 0, -1);
	
	void Start()
	{
		StateMachine.Instance.OnStateChange += OnStateChange;
		
		CheckState ();
	}
	
	void FixedUpdate ()
	{
		HandleHorizontalCollision(CheckHit (Vector3.right, m_horizontalOrigins));	
		
		Vector3 gravityDirection = Vector3.up;
		gravityDirection.y *= PlayerMove.Instance.gravityDirection;

		if (gravityDirection.y > 0) 
		{
			HandleVerticalCollision (CheckHit (gravityDirection, m_verticalOriginsTop));
		} 
		else 
		{
			HandleVerticalCollision (CheckHit (gravityDirection, m_verticalOriginsBot));
		}
	}
	
	void OnStateChange(StateMachine.State newState)
	{
		if (newState == StateMachine.State.Transition) 
		{
			CheckState();
		}
	}
	
	void CheckState()
	{
		RaycastHit upHit = CheckHit (Vector3.up, m_verticalOriginsTop);
		RaycastHit downHit = CheckHit (Vector3.down, m_verticalOriginsBot);

		bool isGrounded = ((upHit.collider != null && !String.IsNullOrEmpty (upHit.collider.gameObject.tag)) || ( downHit.collider != null && !String.IsNullOrEmpty (downHit.collider.gameObject.tag)));
		StateMachine.Instance.RequestChange ( isGrounded ? StateMachine.State.Grounded : StateMachine.State.Falling) ;
	}
	
	RaycastHit CheckHit(Vector3 direction, Transform[] origins)
	{
		RaycastHit hit = new RaycastHit();

		foreach (Transform origin in origins) 
		{
			if(Physics.Raycast(origin.position, direction, out hit, m_castDistance))
			{
				Debug.DrawLine(origin.position + m_debugLineOffset, origin.position + m_debugLineOffset + (direction * m_castDistance), Color.green);
				break;
			}
			else
			{
				Debug.DrawLine(origin.position + m_debugLineOffset, origin.position + m_debugLineOffset + (direction * m_castDistance), Color.red);
			}
		}
		
		return hit;
	}

	void HandleVerticalCollision(RaycastHit hit)
	{
		if (hit.collider != null) 
		{
			switch(hit.collider.tag)
			{
			case "Untagged":
				StateMachine.Instance.RequestChange (StateMachine.State.Grounded);
				break;
			case "Death":
				StateMachine.Instance.RequestChange (StateMachine.State.Death, true);
				break;
			case "PowerUp":
				break;
			default:
				break;
			}
		}
		else
		{
			StateMachine.Instance.RequestChange (StateMachine.State.Falling);
		}
	}

	void HandleHorizontalCollision(RaycastHit hit)
	{
		if (hit.collider != null) 
		{
			switch(hit.collider.tag)
			{
			case "PowerUp":
				break;
			default:
				StateMachine.Instance.RequestChange (StateMachine.State.Death, true);
				break;
			}
		}
	}
}