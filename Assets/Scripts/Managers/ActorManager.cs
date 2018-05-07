using UnityEngine;
using System.Collections;

public class ActorManager : MonoBehaviour
{
    private bool _isCastOfActors;
    private Vector3 _despawnPosition;

    private PlayerCharacter _playerCharacter;
    private AvalancheManager _avalancheManager;
    private GameManager _gameManager;

    [Tooltip("Animation to play when a collision with this object happens")]
    [SerializeField]
    private Animation _onCollisionAnimation;

    [Tooltip("Particle system to play when a collision with this object happens")]
    [SerializeField]
    private ParticleSystem _onCollisionParticleSystem;

    [Tooltip("Audio to play when a collision with this object happens")]
    [SerializeField]
    private AudioClip _onCollisionAudioClip;

    [Tooltip("True if this is a hazard, false if it's a bonus")]
    [SerializeField]
    private bool _isHazard;

    [Tooltip("If player hits this hazard, how much should the avalanche move (postive numbers move avalanche towards player, negative away")]
    [SerializeField]
    private float _avalancheEncroachmentAmount = -2f;

    [Tooltip("If player hits this hazard, how much should the player move (postive numbers move player away from avalanche, negative towards")]
    [SerializeField]
    private float _playerFallbackAmount = 2f;

	private void Start()
	{
        _gameManager = FindObjectOfType<GameManager>();
        _playerCharacter = FindObjectOfType<PlayerCharacter>();
        _avalancheManager = FindObjectOfType<AvalancheManager>();
	}

	public void SetAsCastOfActors() {
        _isCastOfActors = true;
    }

	public void SetDespawnBoundary(Vector3 position) {
        _despawnPosition = position;
    }

	// Update is called once per frame
	void Update()
	{
        if(transform.position.y >= _despawnPosition.y + 10) {
            Despawn();
            return;
        }

        // represents a collection of actors that will handle their own translations
        if (_isCastOfActors) return;

        // move the actor "up" the screen by their speed
        var newPosition = new Vector3(_gameManager.GameSpeedX, _gameManager.GameSpeedY, 0) * Time.deltaTime;
        transform.Translate(newPosition,Space.World);
	}

    private void Despawn() {
        Debug.Log("Despawning " + gameObject.name);
        Destroy(this.gameObject);
    }

    private void HandleHazardCollision() {

        if (_onCollisionAnimation != null)
        {
            // clone it so we can destroy our object without stopping the pfx
            var afx = Instantiate(_onCollisionAnimation);
            afx.Play();
            Destroy(afx, 5f);
        }

        if (_onCollisionParticleSystem != null)
        {
            // clone it so we can destroy our object without stopping the pfx
            var pfx = Instantiate(_onCollisionParticleSystem);
            pfx.Play();
            Destroy(pfx, 5f);
        }

        if (_onCollisionAudioClip != null)
        {
            // clone it so we can destroy our object without stopping the pfx
            var sfx = Instantiate(_onCollisionParticleSystem);
            sfx.Play();
            Destroy(sfx, 5f);
        }

        Despawn();

        // if player hit a hazard, make adjustments based on its properties
        if (_isHazard)
        {
            _avalancheManager.ModifyEncroachment(_avalancheEncroachmentAmount);
            _playerCharacter.ModifyFallback(_playerFallbackAmount);
        }
    }

	private void OnTriggerEnter(Collider other)
	{
        // only trigger if player
        if (other.tag != "Player") return;

        Debug.Log("Trigger: " + gameObject.name + " triggered by " + other.gameObject.name);

        HandleHazardCollision();
	}
}
