using UnityEngine;
using System.Collections;

public class ScoreKeeper : Singleton<ScoreKeeper>
{
	[SerializeField]
	private int m_baseIncreasePerSecond;

	int m_modifier = 1;

	private float m_score = 0;
	public float score
	{
		get
		{
			return Mathf.Round(m_score);
		}
	}

	void Start()
	{
		StateMachine.Instance.OnStateChange += OnStateChange;
	}

	void Update()
	{
		m_score += m_baseIncreasePerSecond * m_modifier * Time.deltaTime;
	}

	public void SetModifier(int modifier, float duration = 5)
	{
		m_modifier = modifier;
		StartCoroutine (ReturnToBase (duration));
	}

	IEnumerator ReturnToBase(float delay)
	{
		yield return new WaitForSeconds (delay);
		m_modifier = 1;
	}

	void OnStateChange(StateMachine.State newState)
	{
		if (newState == StateMachine.State.Death) 
		{
			m_score = 0;
		}
	}
}
