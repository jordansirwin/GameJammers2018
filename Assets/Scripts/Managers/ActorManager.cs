using UnityEngine;
using System.Collections;

public class ActorManager : MonoBehaviour
{
    private float _speed;

    public void SetSpeed(float speed) {
        _speed = speed;
    }

	// Update is called once per frame
	void Update()
	{
        // move the actor "up" the screen by their speed
        transform.Translate(Vector3.up * _speed * Time.deltaTime);
	}
}
