using UnityEngine;
using System.Collections;

public class ScoreKeeper : Singleton<ScoreKeeper>
{
	[SerializeField]
	private int m_baseIncreasePerSecond;
	[SerializeField]
	private int m_increasePerStage = 1;

	private int m_modifier = 1;

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
		StageManager.Instance.OnStageUp += OnStageUp;
	}

	void Update()
	{
		m_score += m_baseIncreasePerSecond * m_modifier * Time.deltaTime;
	}

	void OnStageUp(int stageNum)
	{
		m_baseIncreasePerSecond += m_increasePerStage;
	}

	public void SetModifier(int modifier, float duration)
	{
		m_modifier = modifier;

		duration = Mathf.Clamp (duration, 0, duration);

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
