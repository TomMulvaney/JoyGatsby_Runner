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
	[SerializeField]
	private string m_landingAudioEvent;
	[SerializeField]
	private string m_fallingAudioEvent;
	
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

	// TODO: Don't do state checks in this method. Find another way, this is messy
	void HandleCollision(RaycastHit hit)
	{
		bool isVertical = Mathf.Approximately (Mathf.Abs (Vector3.Dot (hit.normal, Vector3.right)), 0);

		if (hit.collider != null) 
		{
			string hitTag = hit.collider.gameObject.tag;

			switch (hitTag) 
			{
			case "Untagged":
				Debug.Log("untagged case");
				if (isVertical) 
				{
					if (StateMachine.Instance.state == StateMachine.State.Falling) 
					{
						StateMachine.Instance.RequestChange (StateMachine.State.Grounded);
					}
				} 
				else 
				{
					StateMachine.Instance.RequestChange (StateMachine.State.Death, true);
				}
				break;
			default:
				Debug.Log("default case");
				/*
				if (isVertical && StateMachine.Instance.state == StateMachine.State.Grounded) 
				{
					StateMachine.Instance.RequestChange (StateMachine.State.Falling);
				}
				*/
				break;
			}
		} 
		else if (isVertical && StateMachine.Instance.state == StateMachine.State.Grounded) 
		{
			Debug.Log("hitNothing");
			StateMachine.Instance.RequestChange (StateMachine.State.Falling);
		}
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