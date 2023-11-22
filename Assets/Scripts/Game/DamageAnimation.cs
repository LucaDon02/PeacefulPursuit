using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DamageAnimation : MonoBehaviour
{
    public bool start = false;
    public float animationDuration = 0.125f;

    private Image image;
    private float timer = 0f;
    private bool increasing = true;
    private const float BaseValue = 1000f;

    private void Start() { image = GetComponent<Image>(); }

    private void Update()
    {
        if (!start) return;
        timer += Time.unscaledDeltaTime;
        
        if (timer > animationDuration)
        {
            timer = 0f;
            increasing = !increasing;
            start = !increasing;
        }

        var t = Mathf.Clamp01(timer / animationDuration);
        
        var exponent = increasing ? 1 - t : t;
        var newPixelsPerUnit = Mathf.Pow(BaseValue, exponent);

        image.pixelsPerUnitMultiplier = newPixelsPerUnit;
    }
}
