using UnityEngine;
using System.Collections;

public class ActorManager : MonoBehaviour
{
    private Vector3 _despawnPosition;
    private GameManager _gameManager;

	private void Start()
	{
        _gameManager = GameManager.Instance;
	}

	public void SetDespawnBoundary(Vector3 position) {
        _despawnPosition = position;
    }

	// Update is called once per frame
	void Update()
	{
        if(transform.position.y >= _despawnPosition.y) {
            Despawn();
            return;
        }

        // move the actor "up" the screen by their speed
        var newPosition = new Vector3(_gameManager.GameSpeedX, _gameManager.GameSpeedY, 0) * Time.deltaTime;
        //var newPosition = new Vector3(0, _gameManager.BaseSpeed * Time.deltaTime, 0) ;
        transform.Translate(newPosition,Space.World);
	}

    private void Despawn() {
        Destroy(this.gameObject);
    }
}
