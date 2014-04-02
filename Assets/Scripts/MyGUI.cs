using UnityEngine;
using System.Collections;

public class MyGUI : MonoBehaviour 
{
	[SerializeField]
	private LevelGenerator m_levelGenerator;

	void OnGUI()
	{
		GUILayout.Label("Stage: " + StageManager.Instance.stage);
		GUILayout.Label("State: " + StateMachine.Instance.state);
		GUILayout.Label ("NextState: " + StateMachine.Instance.nextState);
		GUILayout.Label ("GravityDirection: " + PlayerMove.Instance.gravityDirection);
		GUILayout.Label("Score: " + ScoreKeeper.Instance.score);
		GUILayout.Label("DistanceTravelled: " + m_levelGenerator.distanceTravelled);
	}
}
