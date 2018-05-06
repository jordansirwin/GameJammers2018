using UnityEngine;
using System.Collections;

public class KnobsForKevin : MonoSingleton<KnobsForKevin>
{
    [Header("Speed")]

    [Tooltip("Initial speed the game starts with. All objects derive their speeds from this base.")]
    public float baseSpeed = 5;

    [Header("Spawn Object Knobs")]

    [Tooltip("Default rate (in seconds) to spawn hazards")]
    public float defaultHazardSpawnRate = 10f;

    [Tooltip("Default rate (in seconds) to spawn bonuses")]
    public float defaultBonusSpawnRate = 10f;

    [Tooltip("Prefabs of hazards to spawn")]
    public ActorManager[] hazardPrefabs;

    [Tooltip("Prefabs of bonuses to spawn")]
    public ActorManager[] bonusPrefabs;


	// Use this for initialization
	void Start()
	{

	}

	// Update is called once per frame
	void Update()
	{
			
	}
}
