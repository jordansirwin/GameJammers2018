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
    private float _timeUntilNextObjectSpawn;

    private List<ActorManager> _spawnablePool;
    private List<string> _spawnableNames;
    private int _poolSize = 5;

    void Start()
    {
        InitializeSpawnables();
    }

    void InitializeSpawnables()
    {
        var numSpawnablePrefabs = _knobs.spawnableObjectPrefabs.Length;
        if (numSpawnablePrefabs == 0) {
            Debug.LogError("No Spawnable Prefabs set.");
            return;
        }

        _spawnableNames = new List<string>();
        _spawnablePool = new List<ActorManager>();

        foreach(var actor in _knobs.spawnableObjectPrefabs)
        {
            _spawnableNames.Add(actor.ActorName);

            for(var i = 0; i < _poolSize; i++)
            {
                InstantiateInPool(actor);
            }
        }
    }

    ActorManager InstantiateInPool(ActorManager actorPrefab)
    {
        var instance = Instantiate(actorPrefab, _spawnObjectParent.transform);

        if (instance.GetComponentsInChildren<ActorManager>().Length > 1)
            instance.SetAsCastOfActors();
        instance.SetDespawnBoundary(topDespawnBoundary.position);
        instance.gameObject.SetActive(false);

        _spawnablePool.Add(instance);

        return instance;
    }

	void Update () {
        // if not playing the game, do nothing
        if(_gameManager.GetGameState() != GameState.Playing) {
            return;
        }

		// stay aligned with player so we always spawn objects under them
		AlignPositionWithPlayer();

		// spawn objects
		_timeUntilNextObjectSpawn -= Time.deltaTime;
		if(_timeUntilNextObjectSpawn <= 0)
			SpawnObjects();
	}

	void AlignPositionWithPlayer() {
		var newPosition = new Vector3(player.transform.position.x, this.transform.position.y, this.transform.position.z);
		this.transform.position = newPosition;
	}

    void SpawnObjects() {
        if (_knobs.spawnableObjectPrefabs.Length == 0) return;

        // spawn random number up to max
        var numberToSpawn = Random.Range(1, _knobs.maxObjectsToSpawnPerCycle + 1);
        for (int i = 0; i < numberToSpawn; i++) {
            var rndIndex = Random.Range(0, _spawnableNames.Count);
            SpawnObject(GetSpawnableFromPool(_spawnableNames[rndIndex]));
        }

        // get time to spawn next hazard
        _timeUntilNextObjectSpawn = GetNextSpawnTime(_knobs.defaultObjectSpawnRate);
	}

    private ActorManager GetSpawnableFromPool(string name)
    {
        ActorManager poolObject = null;

        // Find eligble object in pool
        for(var i = 0; i < _spawnablePool.Count && poolObject == null; i++)
        {
            if(!_spawnablePool[i].gameObject.activeInHierarchy && _spawnablePool[i].ActorName == name)
                poolObject = _spawnablePool[i];
        }

        // No eligible object, expand the pool with a new one
        if(poolObject == null)
        {
            foreach (var actor in _knobs.spawnableObjectPrefabs)
            {
                if(actor.ActorName == name)
                {
                    poolObject = InstantiateInPool(actor);
                    break;
                }
            }
        }

        return poolObject;
    }

    void SpawnObject(ActorManager actor) {
        actor.transform.position = GetSpawnPosition();
        actor.gameObject.SetActive(true);
    }

	float GetNextSpawnTime(float defaultRate) {
		// set new spawn time
        // as player speed increases, spawn faster
		var playerSpeed = _gameManager.GameSpeedY;
        return Mathf.Max(.2f, defaultRate - playerSpeed/2);
	}

	Vector3 GetSpawnPosition() {
        
        float rndX;
        // spawn stuff in center based on percent
        if (Random.Range(1, 101) <= _knobs.percentChanceForCenterSpawn) {
            rndX = 0;
        }
        else {
            rndX = Random.Range(leftSpawnLimit.position.x, rightSpawnLimit.position.x);
        }

		return new Vector3(rndX, transform.position.y, transform.position.z);
	}
}
