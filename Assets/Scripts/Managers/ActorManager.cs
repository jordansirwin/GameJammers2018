using UnityEngine;
using System.Collections;

public class ActorManager : MonoBehaviour
{
    private float _speed;
    private Vector3 _despawnPosition;

    public void SetSpeed(float speed) {
        _speed = speed;
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
        transform.Translate(Vector3.up * _speed * Time.deltaTime);
	}

    private void Despawn() {
        Destroy(this.gameObject);
    }
}
