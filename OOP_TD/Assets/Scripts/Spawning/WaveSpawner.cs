using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveSpawner : MonoBehaviour
{
    public Wave[] waves;
    public Transform spawnLocation;
    public float timeBetweenWaves = 0f;
    private int waveIndex = 0;
    public int pathnumb;

    void FixedUpdate()
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
        for (int i = 0; i < wave.count; i++) //Spawns all the enemies in current wave with 0.5 second interval
        {
            SpawnEnemy(wave.enemies[i], wave.paths[i]); //Enemy prefabs and their path numbers will be chosen from lists in Wave class
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

    void SpawnEnemy(GameObject enemy, int path)
    {
        pathnumb = path; //Enemies will get this value in the Start method in EnemyMovement
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
