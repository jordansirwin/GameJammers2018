using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapManager : MonoBehaviour {

	public GameObject player;
	
	[Tooltip("Farthest left point to spawn a hazard/bonus")]
    [SerializeField]
    private Transform leftSpawnLimit;

	[Tooltip("Farthest right point to spawn a hazard/bonus")]
    [SerializeField]
    private Transform rightSpawnLimit;

    [Tooltip("Destroy spawned objects when they exceed this point")]
    [SerializeField]
    private Transform topDespawnBoundary;

    [Tooltip("Link to the GameManager")]
    [SerializeField]
    private GameManager _gameManager;

    [Tooltip("Link to KnobsForKevin for well-tuned properties")]
    [SerializeField]
    private KnobsForKevin _knobs;

    [Tooltip("Link to the parent of spawned objects")]
    [SerializeField]
    private GameObject _spawnObjectParent;

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
        var rndIndex = Random.Range(0, _knobs.hazardPrefabs.Length);
        SpawnObject(_knobs.hazardPrefabs[rndIndex]);

		// get time to spawn next hazard
        _timeUntilNextHazardSpawn = GetNextSpawnTime(_knobs.defaultHazardSpawnRate);
	}

	void SpawnBonus() {
        var rndIndex = Random.Range(0, _knobs.bonusPrefabs.Length);
        SpawnObject(_knobs.bonusPrefabs[rndIndex]);

		// get time to spawn next hazard
        _timeUntilNextBonusSpawn = GetNextSpawnTime(_knobs.defaultBonusSpawnRate);
	}

    void SpawnObject(ActorManager prefab) {
        var actor = Instantiate(prefab, _spawnObjectParent.transform);
        actor.transform.position = GetSpawnPosition();
        actor.SetDespawnBoundary(topDespawnBoundary.position);

        // does this actor act as a container for other actors? (1 for the object itself)
        if (actor.GetComponentsInChildren<ActorManager>().Length > 1)
            actor.SetAsCastOfActors();
    }

	float GetNextSpawnTime(float defaultRate) {
		// set new spawn time
        // as player speed increases, spawn faster
		var playerSpeed = _gameManager.GameSpeedY;
		return defaultRate - playerSpeed;
	}

	Vector3 GetSpawnPosition() {
		var rndX = Random.Range(leftSpawnLimit.position.x, rightSpawnLimit.position.x);
		return new Vector3(rndX, transform.position.y, transform.position.z);
	}
}
