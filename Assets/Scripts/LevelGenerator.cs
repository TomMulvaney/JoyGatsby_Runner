using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Wingrove;

public class LevelGenerator : MonoBehaviour 
{
	[SerializeField]
	private Transform m_levelParent;
	[SerializeField]
	private Transform m_target;
	[SerializeField]
	private int m_distanceThreshold = 100;
	[SerializeField]
	private GameObject[] m_chunkPrefabs;

	int m_totalNumSpawned = 0;

	float m_distanceTravelled = 0;
	public float distanceTravelled
	{
		get
		{
			return m_distanceTravelled;
		}
	}
	
	float m_lastTargetPosX;

	List<Transform> m_spawnedChunks = new List<Transform>(); // TODO: This should be transform, not game object

	void Awake()
	{
		m_lastTargetPosX = m_target.position.x;
	}

	void Start()
	{
		for (int i = 0; i < 3; ++i) 
		{
			SpawnChunk ();
		}

		StartCoroutine (DestroyChunks ());
	}

	bool ShouldDestroy(Transform chunk)
	{
		return chunk.position.x < (m_target.position.x - (m_distanceThreshold * 3));
	}

	IEnumerator DestroyChunks() // Coroutine is more efficient than Update because this operation does not need to be done every frame
	{
		yield return new WaitForSeconds (3f);

		List<Transform> chunksToDestroy = m_spawnedChunks.FindAll (ShouldDestroy);

		for (int i = chunksToDestroy.Count - 1; i > -1; --i) 
		{
			m_spawnedChunks.Remove(chunksToDestroy[i]);
			Destroy(chunksToDestroy[i].transform);
		}

		StartCoroutine (DestroyChunks ());
	}

	void Update()
	{
		m_distanceTravelled += m_target.localPosition.x - m_lastTargetPosX;

		if (m_distanceTravelled >= m_distanceThreshold) 
		{
			SpawnChunk();

			m_distanceTravelled = 0;
		}

		m_lastTargetPosX = m_target.localPosition.x;
	}

	void SpawnChunk()
	{
		// Spawn next level prefab
		GameObject newChunk = SpawningHelpers.InstantiateUnderWithIdentityTransforms(m_chunkPrefabs[Random.Range(0, m_chunkPrefabs.Length)], m_levelParent);

		ColorChunk (newChunk);
		
		Vector3 pos = newChunk.transform.localPosition;
		pos.x =  m_totalNumSpawned * m_distanceThreshold; // TODO: There needs to be a precise x-position
		newChunk.transform.localPosition = pos;

		Debug.Log (newChunk.name + " - " + pos.x);
		
		m_spawnedChunks.Add(newChunk.transform);

		++m_totalNumSpawned;
	}

	void ColorChunk(GameObject chunk)
	{
		Color col = m_totalNumSpawned % 2 == 0 ? Color.blue : Color.red;
		Renderer[] rens = chunk.GetComponentsInChildren<Renderer> () as Renderer[];
		foreach (Renderer ren in rens) 
		{
			ren.material.color = col;
		}
	}
}