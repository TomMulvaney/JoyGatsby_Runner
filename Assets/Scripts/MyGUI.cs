using UnityEngine;
using System.Collections;

public class MyGUI : MonoBehaviour 
{
	void OnGUI()
	{
		GUILayout.Label("State: " + StateMachine.Instance.state);
		GUILayout.Label ("NextState: " + StateMachine.Instance.nextState);
		GUILayout.Label ("GravityDirection: " + PlayerMove.Instance.gravityDirection);
		GUILayout.Label("Score: " + ScoreKeeper.Instance.score);
	}
}
