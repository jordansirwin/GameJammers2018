using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapManager : MonoBehaviour {

	public GameObject player;
	
	[Tooltip("Default rate (in seconds) to spawn hazards")]
	public float defaultHazardSpawnRate = 10f;
	[Tooltip("Default rate (in seconds) to spawn bonuses")]
	public float defaultBonusSpawnRate = 10f;
	[Tooltip("Prefabs of hazards to spawn")]
	public GameObject[] hazards;
	[Tooltip("Prefabs of bonuses to spawn")]
	public GameObject[] bonuses;

	[Tooltip("Farthest left point to spawn a hazard/bonus")]
	public Transform leftSpawnLimit;
	[Tooltip("Farthest right point to spawn a hazard/bonus")]
	public Transform rightSpawnLimit;

	// private GameManager _gameManager;

	// keep track of next time to spawn things
	private float _timeUntilNextHazardSpawn;
	private float _timeUntilNextBonusSpawn;

	
	// Update is called once per frame
	void Update () {
		// stay aligned with player so we always spawn objects under them
		AlignPositionWithPlayer();

		// spawn hazards
		_timeUntilNextHazardSpawn -= Time.deltaTime;
		if(_timeUntilNextHazardSpawn <= 0)
			SpawnHazard();

		// spawn bonuses
		_timeUntilNextBonusSpawn -= Time.deltaTime;
		if(_timeUntilNextBonusSpawn <= 0)
			SpawnBonus();
	}

	void AlignPositionWithPlayer() {
		var newPosition = new Vector3(player.transform.position.x, this.transform.position.y, this.transform.position.z);
		this.transform.position = newPosition;
	}

	void SpawnHazard() {
		var rndIndex = Random.Range(0, hazards.Length);
		var go = Instantiate(hazards[rndIndex], GetSpawnPosition(), Quaternion.identity);

		// get time to spawn next hazard
		_timeUntilNextHazardSpawn = GetNextSpawnTime(defaultHazardSpawnRate);
	}

	void SpawnBonus() {
		var rndIndex = Random.Range(0, hazards.Length);
		Instantiate(hazards[rndIndex], GetSpawnPosition(), Quaternion.identity);

		// get time to spawn next hazard
		_timeUntilNextBonusSpawn = GetNextSpawnTime(defaultBonusSpawnRate);
	}

	float GetNextSpawnTime(float defaultRate) {
		// set new spawn time
		// as player speed increases, spawn faster
		var playerSpeed = 1f;//_gameManager.BaseSpeed;
		return defaultRate - playerSpeed;
	}

	Vector3 GetSpawnPosition() {
		var rndX = Random.Range(leftSpawnLimit.position.x, rightSpawnLimit.position.x);
		return new Vector3(rndX, transform.position.y, transform.position.z);
	}
}
