using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveSpawner : MonoBehaviour
{

    //public Transform enemyPrefab;
    public Wave[] waves;
    public Transform spawnLocation;
    public float timeBetweenWaves = 0f;
    private int waveIndex = 0;

    void Update()
    {
     if (timeBetweenWaves <= 0f)
        {
            StartCoroutine(SpawnWave());
            timeBetweenWaves = 8f;
            return;
        }
        timeBetweenWaves -= Time.deltaTime;
    }

    IEnumerator SpawnWave()
    {
        Wave wave = waves[waveIndex];
        for (int i = 0; i < wave.count; i++)
        {
            SpawnEnemy(wave.enemies[i]);
            yield return new WaitForSeconds(0.5f);
        }
        waveIndex++;
        Debug.Log("Wave spawned");
        if (waveIndex == waves.Length)
        {
            Debug.Log("Final Wave!");
            this.enabled = false;
        }
    }

    void SpawnEnemy(GameObject enemy)
    {
        Instantiate(enemy, spawnLocation.position, spawnLocation.rotation);

    }
    public void SpawnButton()
    {
        timeBetweenWaves = 8f;
        if (waveIndex < waves.Length)
        {
            StartCoroutine(SpawnWave());
        }
    }
}