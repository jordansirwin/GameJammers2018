using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrollSnow : MonoBehaviour {

    [Tooltip("Link to the GameManager")]
    [SerializeField]
    private GameManager gameManager;

    private Renderer _renderer;

	private void Start()
	{
        _renderer = GetComponent<Renderer>();
	}


    float OffsetX;
    float OffsetY;
	// Update is called once per frame
    void Update () {
        OffsetX += Time.deltaTime * -gameManager.GameSpeedX / 20;
        OffsetY += Time.deltaTime * -gameManager.GameSpeedY / 30;
        _renderer.material.mainTextureOffset = new Vector2(OffsetX, OffsetY);
    }
}
