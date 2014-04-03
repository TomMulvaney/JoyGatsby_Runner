using UnityEngine;
using System.Collections;

public class PowerUp : MonoBehaviour 
{
	void OnTriggerEnter(Collider other)
	{
		if (other.tag == "Player") 
		{
			Destroy(gameObject);
		}
	}
}
