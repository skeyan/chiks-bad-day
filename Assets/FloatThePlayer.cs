using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatThePlayer : MonoBehaviour
{
    public float speedOfFloating;
    float distanceFromCenterOfFloating;
    Vector3 newPlayerPos;
    Vector3 originalPlayerPos;

    // Start is called before the first frame update
    void Start()
    {
        originalPlayerPos = transform.position;

        // Adjust these values to alter feel
        speedOfFloating = 3.5f;
        distanceFromCenterOfFloating = 0.1f;
    }

    // Update is called once per frame
    void Update()
    {
        // Get current position
        newPlayerPos = transform.position;

        // Smoothly fluctuate up and down using sine
        // Must add originalPlayerPos.y, not newPlayerPos.y, because we always want it centered around the original position
        newPlayerPos.y = originalPlayerPos.y + distanceFromCenterOfFloating * Mathf.Sin(Time.time * speedOfFloating);

        // Set player position equal to fluctuating position to achieve basic floating effect
        transform.position = newPlayerPos;
    }
}
