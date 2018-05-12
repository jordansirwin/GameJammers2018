using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AvalancheVisual : MonoBehaviour
{
    [SerializeField] private AvalancheLayer[] _layers;
    //[SerializeField] private float _size = 0;

    private void Start()
    {
        foreach (var layer in _layers)
        {
            layer.Particles.Stop();
            SetEmissionRate(layer.Particles, 0);
        }
    }

    private void Update()
    {
        var size = AvalancheManager.Instance.Size;

        foreach (var layer in _layers)
        {
            if(size >= layer.StartAtSize)
            {
                if (!layer.Particles.isPlaying)
                {
                    //Debug.Log("Start playing " + layer.Particles);
                    layer.Particles.Play();
                }

                if (layer.Particles.emission.rateOverTime.constant < layer.MaxEmission)
                {
                    //float step = (layer.MaxAtSize - _size) / (layer.StartAtSize - layer.MaxAtSize);
                    if (size >= layer.MaxAtSize)
                    {
                        SetEmissionRate(layer.Particles, layer.MaxEmission);
                    }
                    else
                    {
                        float step = (size - layer.StartAtSize) / (layer.MaxAtSize - layer.StartAtSize);
                        SetEmissionRate(layer.Particles, Mathf.Lerp(0, layer.MaxEmission, step));
                    }
                }
            }
            if(layer.UseStopAtSize && size >= layer.StopAtSize && layer.Particles.isPlaying)
            {
                layer.Particles.Stop();
            }
        }
    }

    private void SetEmissionRate(ParticleSystem system, float rateAmount)
    {
        var emission = system.emission;
        var rate = emission.rateOverTime;
        rate.constant = rateAmount;
        emission.rateOverTime = rate;
    }

    [Serializable]
    public class AvalancheLayer
    {
        [SerializeField] public ParticleSystem Particles;
        [SerializeField] public float StartAtSize;
        [SerializeField] public float MaxAtSize;
        [SerializeField] public float MaxEmission;
        [SerializeField] public bool UseStopAtSize;
        [SerializeField] public float StopAtSize;
    }
}
