using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace _Project.Scripts
{
    public class Rotator : MonoBehaviour
    {
    public float swayAmplitude = 3.0f; 
    
    public float swayFrequency = 1.5f; 

    
    public float noiseAmplitude = 0.5f; 
    
    public float noiseSpeed = 0.8f; 
    
    private Quaternion initialRotation; 
    
    private float noiseOffset1; 
    private float noiseOffset2;
    private float noiseOffset3;

    private void Start()
    {
        initialRotation = transform.localRotation;

        noiseOffset1 = Random.Range(0f, 100f);
        noiseOffset2 = Random.Range(0f, 100f);
        noiseOffset3 = Random.Range(0f, 100f);
    }

    private void Update()
    {
        float time = Time.time * swayFrequency;
        
        float sinusoidalYaw = Mathf.Sin(time) * swayAmplitude;
        
        float noiseTime = Time.time * noiseSpeed;
        
        float noiseX = (Mathf.PerlinNoise(noiseOffset1 + noiseTime, 0f) * 2f - 1f) * noiseAmplitude;
        float noiseY = (Mathf.PerlinNoise(noiseOffset2 + noiseTime, 0f) * 2f - 1f) * noiseAmplitude;
        float noiseZ = (Mathf.PerlinNoise(noiseOffset3 + noiseTime, 0f) * 2f - 1f) * noiseAmplitude;
        
        float finalYaw = sinusoidalYaw + noiseY; 
        
        Quaternion targetRotation = initialRotation * Quaternion.Euler(noiseX, finalYaw, noiseZ);
        
        transform.localRotation = Quaternion.Slerp(transform.localRotation, targetRotation, Time.deltaTime * 5f);
    }
    }
}