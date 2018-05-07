using UnityEngine;
using System.Collections;

public class KnobsForKevin : MonoSingleton<KnobsForKevin>
{
    [Header("Speed")]

    [Tooltip("Initial speed the game starts with. All objects derive their speeds from this base.")]
    public float baseSpeed = 5;

    [Tooltip("The avalanche and player move speeds will come from the base speed multiplied by this value.")]
    public float encroachSpeedMultiplier = 0.1f;

    [Tooltip("Speed adjustements when the player turnes. 1f is equal to current baseSpeed.")]
    public Vector2 playerTurnSpeedModifier = new Vector2(0.5f, 1f);

    [Header("Spawn Object Knobs")]

    [Tooltip("Default rate (in seconds) to spawn hazards")]
    public float defaultHazardSpawnRate = 10f;

    [Tooltip("Default rate (in seconds) to spawn bonuses")]
    public float defaultBonusSpawnRate = 10f;

    [Tooltip("Prefabs of hazards to spawn")]
    public ActorManager[] hazardPrefabs;

    [Tooltip("Prefabs of bonuses to spawn")]
    public ActorManager[] bonusPrefabs;


    [Header("Score Knobs")]

    [Tooltip("Array of scores to reach for mighty bonus hottness")]
    public int[] goalScores;
}
