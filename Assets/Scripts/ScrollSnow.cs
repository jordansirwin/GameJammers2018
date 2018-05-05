using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrollSnow : MonoBehaviour {

    [Tooltip("Link to the GameManager")]
    [SerializeField]
    private GameManager gameManager;

    private Renderer _renderer;

    public float ScrollX = 0.5f;
    //public float ScrollY = 0.5f;

	private void Start()
	{
        _renderer = GetComponent<Renderer>();
	}


	// Update is called once per frame
	void Update () {
        float OffsetX = Time.time * ScrollX;
        float OffsetY = Time.time * -gameManager.BaseSpeed/30;
        _renderer.material.mainTextureOffset = new Vector2(OffsetX, OffsetY);
    }
}
