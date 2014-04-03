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
	private Transform[] m_verticalOrigins;
	
	Vector3 m_debugLineOffset = new Vector3 (0, 0, -1);
	
	void Start()
	{
		StateMachine.Instance.OnStateChange += OnStateChange;
		
		CheckState ();
	}
	
	void FixedUpdate ()
	{
		HandleCollision(CheckHit (Vector3.right, m_horizontalOrigins));	
		
		Vector3 gravityDirection = Vector3.up;
		gravityDirection.y *= PlayerMove.Instance.gravityDirection;
		
		HandleCollision(CheckHit(gravityDirection, m_verticalOrigins));
	}
	
	void OnStateChange(StateMachine.State newState)
	{
		Debug.Log ("ColiDet.OSC - " + newState);
		if (newState == StateMachine.State.Transition) 
		{
			CheckState();
		}
	}
	
	void CheckState()
	{
		RaycastHit upHit = CheckHit (Vector3.up, m_verticalOrigins);
		RaycastHit downHit = CheckHit (Vector3.down, m_verticalOrigins);

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
		if (hit.collider != null) 
		{
			bool isVertical = Mathf.Approximately (Mathf.Abs (Vector3.Dot (hit.normal, Vector3.right)), 0);

			string hitTag = hit.collider.gameObject.tag;

			switch (hitTag) 
			{
			case "Untagged":
				if (isVertical) {
					if (StateMachine.Instance.state == StateMachine.State.Falling) {
						StateMachine.Instance.RequestChange (StateMachine.State.Grounded);
					}
				} else {
					StateMachine.Instance.RequestChange (StateMachine.State.Death, true);
				}
				break;
			case "Death":
				StateMachine.Instance.RequestChange (StateMachine.State.Death, true);
				break;
			default:
				if (isVertical && StateMachine.Instance.state == StateMachine.State.Grounded) {
					StateMachine.Instance.RequestChange (StateMachine.State.Falling);
				}
				break;
			}
		}
	}
}

/*
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
	private Transform[] m_verticalOrigins;

	Vector3 m_debugLineOffset = new Vector3 (0, 0, -1);

	void Start()
	{
		StateMachine.Instance.OnStateChange += OnStateChange;

		CheckState ();
	}
	
	void FixedUpdate ()
	{
		HandleCollision(CheckHit (Vector3.right, m_horizontalOrigins), Vector3.right);	

		Vector3 gravityDirection = Vector3.up;
		gravityDirection.y *= PlayerMove.Instance.gravityDirection;

		HandleCollision(CheckHit(gravityDirection, m_verticalOrigins), gravityDirection);
	}

	void OnStateChange(StateMachine.State newState)
	{
		Debug.Log ("ColiDet.OSC - " + newState);
		if (newState == StateMachine.State.Transition) 
		{
			CheckState();
		}
	}
	
	void CheckState()
	{
		bool isGrounded = (!String.IsNullOrEmpty (CheckHit (Vector3.up, m_verticalOrigins)) || !String.IsNullOrEmpty (CheckHit (Vector3.down, m_verticalOrigins)));
		StateMachine.Instance.RequestChange ( isGrounded ? StateMachine.State.Grounded : StateMachine.State.Falling) ;
	}

	string CheckHit(Vector3 direction, Transform[] origins)
	{
		foreach (Transform origin in origins) 
		{
			RaycastHit hit;
			
			if(Physics.Raycast(origin.position, direction, out hit, m_castDistance))
			{
				Debug.DrawLine(origin.position + m_debugLineOffset, origin.position + m_debugLineOffset + (direction * m_castDistance), Color.green);
				return hit.collider.gameObject.tag;
			}
			else
			{
				Debug.DrawLine(origin.position + m_debugLineOffset, origin.position + m_debugLineOffset + (direction * m_castDistance), Color.red);
			}
		}

		return null;
	}

	// TODO: Handle collision according to hit surface normal
	// TODO: Don't do state checks in this method. Find another way, this is messy
	void HandleCollision(string hitTag, Vector3 direction)
	{
		bool isVertical = Mathf.Approximately(Mathf.Abs(Vector3.Dot(direction, Vector3.up)), 1);

		switch(hitTag)
		{
		case "Untagged":
			if(isVertical)
			{
				if(StateMachine.Instance.state == StateMachine.State.Falling)
				{
					StateMachine.Instance.RequestChange(StateMachine.State.Grounded);
				}
			}
			else
			{
				StateMachine.Instance.RequestChange(StateMachine.State.Death, true);
			}
			break;
		case "Death":
			StateMachine.Instance.RequestChange(StateMachine.State.Death, true);
			break;
		default:
			if(isVertical && StateMachine.Instance.state == StateMachine.State.Grounded)
			{
				StateMachine.Instance.RequestChange(StateMachine.State.Falling);
			}
			break;
		}
	}
}
*/