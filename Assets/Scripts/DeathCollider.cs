using UnityEngine;
using System.Collections;

public class DeathCollider : SpecialCollider 
{
	protected override void UsePower()
	{
		StateMachine.Instance.RequestChange (StateMachine.State.Death, true);
	}
}
