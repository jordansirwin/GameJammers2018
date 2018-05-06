using UnityEngine;
using System.Collections;

public class ActorManager : MonoBehaviour
{
    private bool _isCastOfActors;
    private Vector3 _despawnPosition;
    private GameManager _gameManager;

	private void Start()
	{
        _gameManager = GameManager.Instance;
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

	private void OnCollisionEnter(Collision collision)
    {
        // represents a collection of actors that will handle their own collisions
        if (_isCastOfActors) return;

        Debug.Log("Collision");
        foreach(var contact in collision.contacts) {
            Debug.Log("Collision with " + contact.thisCollider.name);
        }
	}

	private void OnTriggerEnter(Collider other)
	{
        // only trigger if player
        if (other.tag != "Player") return;

        Debug.Log("Trigger: " + gameObject.name + " triggered by " + other.gameObject.name);
        Despawn();
	}
}
