using UnityEngine;
using System.Collections;

public class SpecialCollider : MonoBehaviour 
{
	[SerializeField]
	private bool m_destroyOnCollide;

	protected virtual void UsePower() {}

	void OnTriggerEnter(Collider other)
	{
		if (other.tag == "Player") 
		{
			UsePower();

			if(m_destroyOnCollide)
			{
				Destroy(gameObject);
			}
		}
	}
}
