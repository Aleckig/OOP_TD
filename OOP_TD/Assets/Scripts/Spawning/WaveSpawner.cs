using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveSpawner : MonoBehaviour
{
    public Wave[] waves;
    public Transform spawnLocation;
    private int waveIndex = 0;
    public int pathnumb;

    void FixedUpdate()
    {
        // Remove the auto-spawning logic from FixedUpdate()
        // Auto spawning will be controlled by the SpawnButton method

        // You can remove this line as well
        // if (timeBetweenWaves <= 0f)

        // Remove the Coroutine invocation from here
        // StartCoroutine(SpawnWave());
        // timeBetweenWaves = 10f;
    }

    IEnumerator SpawnWave()
    {
        Wave wave = waves[waveIndex];
        for (int i = 0; i < wave.count; i++)
        {
            SpawnEnemy(wave.enemies[i], wave.paths[i]);
            yield return new WaitForSeconds(1f);
        }
        waveIndex++;
        Debug.Log("Wave spawned");
        if (waveIndex == waves.Length)
        {
            Debug.Log("Final Wave!");
            this.enabled = false;
        }
    }

    void SpawnEnemy(GameObject enemy, int path)
    {
        pathnumb = path;
        Instantiate(enemy, spawnLocation.position, spawnLocation.rotation);
    }

    public void SpawnButton()
    {
        // Call SpawnWave only if there are waves left to spawn
        if (waveIndex < waves.Length)
        {
            StartCoroutine(SpawnWave());
        }
    }
}
