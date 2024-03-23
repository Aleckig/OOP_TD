using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RGBColorSwitch : MonoBehaviour
{
    public float switchInterval = 1f; // Interval between color switches
    private Renderer rendererComponent;
    private Color[] colors = new Color[3]; // Array to hold RGB colors
    private int currentIndex = 0;
    private float timer = 0f;

    void Start()
    {
        // Get the Renderer component attached to the GameObject
        rendererComponent = GetComponent<Renderer>();

        // Set RGB colors (Red, Green, Blue)
        colors[0] = Color.red;
        colors[1] = Color.green;
        colors[2] = Color.blue;

        // Set the initial color
        rendererComponent.material.SetColor("_EmissionColor", colors[currentIndex]);
    }

    void Update()
    {
        // Update the timer
        timer += Time.deltaTime;

        // If the timer exceeds the switch interval
        if (timer >= switchInterval)
        {
            // Reset the timer
            timer = 0f;

            // Increment the current index and loop back to 0 if exceeds the array length
            currentIndex = (currentIndex + 1) % colors.Length;

            // Set the new color
            rendererComponent.material.SetColor("_EmissionColor", colors[currentIndex]);
        }
    }
}
