using UnityEngine;
using System.Collections;

public class ActorManager : MonoBehaviour
{
    private bool _isCastOfActors;
    private Vector3 _despawnPosition;

    private PlayerCharacter _playerCharacter;
    private AvalancheManager _avalancheManager;
    private GameManager _gameManager;

    [Tooltip("Particle system to play when a collision with this object happens")]
    [SerializeField]
    private ParticleSystem _onCollisionParticleSystem;

    [Tooltip("Audio source with clip to play when a collision with this object happens")]
    [SerializeField]
    private AudioSource _onCollisionAudioSource;

    [SerializeField]
    private Animation _onCollisionAnimation;

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

    IEnumerator HandleObjectCollision() {
        float effectTime = 0;

        if (_onCollisionParticleSystem != null)
        {
            effectTime = Mathf.Max(effectTime, _onCollisionParticleSystem.main.duration);
            _onCollisionParticleSystem.Play();
            // clone it so we can destroy our object without stopping the pfx
            //var pfx = Instantiate(_onCollisionParticleSystem);
            //pfx.Play();
            //Destroy(pfx, 5f);
        }

        if (_onCollisionAudioSource != null)
        {
            if (_onCollisionAudioSource.clip == null)
            {
                Debug.LogError("Spawnable Audio Source has no clip attached!/nRemove Audio Source if audio effect not intended.");
            }
            else
            {
                effectTime = Mathf.Max(effectTime, _onCollisionAudioSource.clip.length);
                _onCollisionAudioSource.Play();
            }
            // clone it so we can destroy our object without stopping the pfx
            //var sfx = Instantiate(_onCollisionParticleSystem);
            //sfx.Play();
            //Destroy(sfx, 5f);
        }

        if(_onCollisionAnimation != null)
        {
            if(_onCollisionAnimation.clip == null)
            {
                Debug.LogError("Spawnable Animation has no clip attached!/nRemove Animation if animation effect not intended.");
            }
            else
            {
                effectTime = Mathf.Max(effectTime, _onCollisionAnimation.clip.length);
                _onCollisionAnimation.Play();
            }
        }

        // move avalanche/player if required
        if (System.Math.Abs(_avalancheEncroachmentAmount) > float.Epsilon)
            _avalancheManager.ModifyEncroachment(_avalancheEncroachmentAmount);
        if (System.Math.Abs(_playerFallbackAmount) > float.Epsilon)
            _playerCharacter.ModifyFallback(_playerFallbackAmount);

        // Wait for effect(s) to finish
        yield return new WaitForSeconds(effectTime);

        Despawn();
    }

    private void OnTriggerEnter(Collider other)
	{
        // only trigger if player
        if (other.tag != "Player") return;

        Debug.Log("Trigger: " + gameObject.name + " triggered by " + other.gameObject.name);

        StartCoroutine(HandleObjectCollision());
	}
}
