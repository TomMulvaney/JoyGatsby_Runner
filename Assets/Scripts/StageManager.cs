using UnityEngine;
using System.Collections;

public class StageManager : Singleton<StageManager> 
{
	public delegate void StageUp(int newStage);
	public event StageUp OnStageUp;

	[SerializeField]
	private float m_startDuration = 15;
	[SerializeField]
	private float m_durationDecrement = 0;
	[SerializeField]
	private float m_minimumStageDuration = 5;

	int m_stage = 0;
	public int stage
	{
		get
		{
			return m_stage;
		}
	}

	float m_stageTimeElapsed = 0;

	float m_stageDuration;

	void Awake()
	{
		m_stageDuration = m_startDuration;
	}

	void Start()
	{
		StateMachine.Instance.OnStateChange += OnStateChange;
	}

	void OnStateChange(StateMachine.State newState)
	{
		if (newState == StateMachine.State.Death) 
		{
			m_stage = 0;
			m_stageTimeElapsed = 0;
			m_stageDuration = m_startDuration;
		}
	}

	void Update()
	{
		m_stageTimeElapsed += Time.deltaTime;

		if (m_stageTimeElapsed >= m_stageDuration) 
		{
			++m_stage;

			m_stageTimeElapsed = 0;

			m_stageDuration -= m_durationDecrement;
			m_stageDuration = Mathf.Clamp(m_stageDuration, m_minimumStageDuration, m_minimumStageDuration + m_stageDuration);

			if(OnStageUp != null)
			{
				OnStageUp(m_stage);
			}
		}
	}
}
