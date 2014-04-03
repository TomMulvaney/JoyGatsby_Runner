using UnityEngine;
using System.Collections;

public class ScorePowerUp : SpecialCollider 
{
	protected override void UsePower()
	{
		ScoreKeeper.Instance.DoubleModifier ();
	}
}
