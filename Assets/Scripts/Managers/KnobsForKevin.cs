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

    [Tooltip("Default rate (in seconds) to increase the game speed")]
    public float speedIncreaseRate = 1f;

    [Tooltip("How much the game speed should increase at each speedIncreaseRate tick")]
    public float speedIncreaseAmount = 0.5f;

    [Tooltip("Default rate (in seconds) to increase the avalanche size")]
    public float avalancheSizeIncreaseRate = 1f;

    [Tooltip("How much the avalanche size should increase at each avalancheSizeIncreaseRate tick")]
    public float avalancheSizeIncreaseAmount = 0.5f;

    [Header("Spawn Object Knobs")]

    [Tooltip("To force player to turn left/right, spawn items at center this percent of time")]
    [Range(1, 100)]
    public int percentChanceForCenterSpawn = 10;

    [Tooltip("Default rate (in seconds) to spawn hazards")]
    public float defaultObjectSpawnRate = 10f;

    [Tooltip("Prefabs of objects to spawn")]
    public ActorManager[] spawnableObjectPrefabs;


    [Header("Score Knobs")]

    [Tooltip("Array of scores to reach for mighty bonus hottness")]
    public int[] goalScores;
}
