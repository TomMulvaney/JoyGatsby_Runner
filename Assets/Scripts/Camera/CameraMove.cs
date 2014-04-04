using UnityEngine;
using System.Collections;

public class CameraMove : MonoBehaviour 
{
	[SerializeField]
	private Transform m_target;
	[SerializeField]
	private float m_offsetX;
	
	// Update is called once per frame
	void Update () 
	{
		//Vector3 pos = transform.position;
		//pos.x = m_target.position.x + m_offsetX;
		//transform.position = pos;

		Vector3 localPos = transform.localPosition;
		localPos.x = m_target.localPosition.x + m_offsetX;
		transform.localPosition = localPos;
	}
}
