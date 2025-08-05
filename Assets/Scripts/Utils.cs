using UnityEngine;

public class SmoothFloat
{
    public float currentValue;
    float velocity;
    float duration;

    public SmoothFloat(float _duration)
    {
        duration = _duration;
        currentValue = 0f;
        velocity = 0f;
    }

    public float GetAndUpdateValue(float targetValue)
    {
        currentValue = Mathf.SmoothDamp(currentValue, targetValue, ref velocity, duration);
        return currentValue;
    }
}





