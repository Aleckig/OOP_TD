using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyVisibility : MonoBehaviour
{
     private Renderer enemyRenderer;
    private Coroutine visibilityCoroutine;

    void Start()
    {
        enemyRenderer = GetComponent<Renderer>();
        visibilityCoroutine = StartCoroutine(ToggleVisibility());
    }

    IEnumerator ToggleVisibility()
    {
        while (true)
        {
            // Toggle the visibility of the enemy
            enemyRenderer.enabled = !enemyRenderer.enabled;

            // Wait for a random interval
            yield return new WaitForSeconds(Random.Range(1.0f, 2.5f));

            // Ensure the enemy is visible after the interval
            enemyRenderer.enabled = true;

            // Wait for 2 seconds
            yield return new WaitForSeconds(2.0f);
        }
    }
}
