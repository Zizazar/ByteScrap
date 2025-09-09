using System;
using DG.Tweening;
using Unity.VisualScripting;
using UnityEngine;

public class LampComponent : CircuitComponent
{
    [SerializeField] private Light bulbLight;
    [SerializeField] private Renderer lampRenderer;
    
    private float intensity = 0;
    
    private void Start() => UpdateVisual();

    public override void UpdateState()
    {
        base.UpdateState();
        UpdateVisual();
    }

    private void UpdateVisual()
    {
        bulbLight.enabled = currentState;
        
        
    }

    private void Update()
    {
        if (currentState)
        {
            var randomIntensity = Mathf.PingPong(Time.time * 4, 5f);
            intensity = 8 + randomIntensity;
        }
        else
        {
            intensity = 0;
        }
        
        lampRenderer.material.SetColor("_EmissionColor", bulbLight.color * intensity);
        
    }

    public override void ReceiveSignal(Direction fromDirection, bool signal)
    {
        if (signal) newState = true;
    }

    public override bool GetOutput(Direction direction)
    {
        return false; // Лампа не передает сигнал
    }
}