using UnityEngine;
using DG.Tweening;

public class HologramEffect : MonoBehaviour
{
    [Header("Renderer References")]
    public Renderer[] hologramRenderers;
    
    [Header("Materials")]
    public Material hologramMaterial;
    public Material invalidMaterial;
    
    [Header("Animation Settings")]
    public float pulseScale = 1.2f;
    public float pulseDuration = 0.5f;
    
    private Material[] originalMaterials;
    private Tween pulseTween;
    private bool isActive;
    
    private void Awake()
    {
        // Сохраняем оригинальные материалы
        originalMaterials = new Material[hologramRenderers.Length];
        for (int i = 0; i < hologramRenderers.Length; i++)
        {
            originalMaterials[i] = hologramRenderers[i].sharedMaterial;
        }
    }
    
    public void ActivateHologramEffect()
    {
        if (isActive) return;
        isActive = true;
        
        // Применяем материал голограммы
        foreach (Renderer rend in hologramRenderers)
        {
            rend.sharedMaterial = hologramMaterial;
            rend.receiveShadows = false;
        }
    }
    
    public void DeactivateHologramEffect()
    {
        if (!isActive) return;
        isActive = false;
        
        // Восстанавливаем оригинальные материалы
        for (int i = 0; i < hologramRenderers.Length; i++)
        {
            hologramRenderers[i].sharedMaterial = originalMaterials[i];
            hologramRenderers[i].receiveShadows = true;
        }
        
        StopPulseAnimation();
    }
    
    public void SetValidState(bool isValid)
    {
        if (!isActive) return;
        
        Material targetMaterial = isValid ? hologramMaterial : invalidMaterial;
        foreach (Renderer rend in hologramRenderers)
        {
            rend.sharedMaterial = targetMaterial;
        }
        
        if (isValid)
        {
            StopPulseAnimation();
        }
        else
        {
            StartPulseAnimation();
        }
    }
    
    private void StartPulseAnimation()
    {
        if (pulseTween != null && pulseTween.IsActive()) return;
        
        pulseTween = transform.DOScale(transform.localScale * pulseScale, pulseDuration)
            .SetEase(Ease.InOutSine)
            .SetLoops(-1, LoopType.Yoyo);
    }
    
    private void StopPulseAnimation()
    {
        if (pulseTween != null && pulseTween.IsActive())
        {
            pulseTween.Kill();
        }
        transform.localScale = Vector3.one;
    }
    
    private void OnDestroy()
    {
        DeactivateHologramEffect();
        StopPulseAnimation();
    }
}