using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class WaveSpawner : MonoBehaviour
{
    public Wave[] waves;
    public Transform spawnLocation;
    public float timeBetweenWaves = 0f;
    private int waveIndex = 0;
    public int pathnumb;
    public int aliveEnemies;
    public bool lastWave;
    public TMP_Text countDown;
    public GameWinScreen gameWinScreen;
    public bool betweenWaves;

    void Start()
    {
        lastWave = false;
    }
    void FixedUpdate()
    {
        if (aliveEnemies == 0 && lastWave == true)
        {
            gameWinScreen.ShowGameWinScreen();
        }
     if (timeBetweenWaves <= 0f)
        {
            if (lastWave == false && aliveEnemies == 0)
            {
                StartCoroutine(SpawnWave());
            }
            timeBetweenWaves = 8f;
            return;
        }
        if (aliveEnemies == 0)
        {
            betweenWaves = true;
            timeBetweenWaves -= Time.deltaTime;
        }
        else
        {
            betweenWaves = false;
        }
        if (timeBetweenWaves <= 4.0f && timeBetweenWaves >= 0f && lastWave == false)
        {
            countDown.gameObject.SetActive(true);
            countDown.text = "Next wave in: " + Mathf.FloorToInt(timeBetweenWaves);
        }
        else
        {
            countDown.gameObject.SetActive(false);
        }
    }

    IEnumerator SpawnWave()
    {
        Wave wave = waves[waveIndex];
        for (int i = 0; i < wave.count; i++) //Spawns all the enemies in current wave with 0.8 second interval
        {
            SpawnEnemy(wave.enemies[i], wave.paths[i]); //Enemy prefabs and their path numbers will be chosen from lists in Wave class
            yield return new WaitForSeconds(0.8f);
        }
        waveIndex++;
        Debug.Log("Wave spawned");
        if (waveIndex == waves.Length)
        {
            Debug.Log("Final Wave!");
            lastWave = true;
            //this.enabled = false;
        }
    }

    void SpawnEnemy(GameObject enemy, int path)
    {
        pathnumb = path; //Enemies will get this value in the Start method in EnemyMovement
        Instantiate(enemy, spawnLocation.position, spawnLocation.rotation);
        aliveEnemies++;
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
