using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnEnemys : MonoBehaviour
{
    [Header("Enemy Prefabs")]
    [SerializeField]
    private List<GameObject> enemyPrefabs;


    [Header("Spawn Probabilities")]
    [SerializeField]
    private List<float> spawnProbabilities;

    [Header("Spawn Points")]
    [SerializeField]
    private Transform[] spawnPoints; 

    [Header("Game Objects")]
    public GameObject player;
    public GameObject EnemyBoss;

    [Header("Spawn Settings")]
    [SerializeField]
    private float spawnInterval = 1.5f; 
    private float spawnIntervalFixed = 1.5f; 
    public float bossTimer = 0f;
    public float difficulty = 0.02f;
    public bool spawningEnabled = true;
    
    

    void Start()
    {
        
        StartCoroutine(SpawnEnemiesRepeatedly());
        StartCoroutine(SpawnBoss());
    }


    private IEnumerator SpawnEnemiesRepeatedly()
    {
        while (true)
        {
            SpawnEnemyWithProbability();
            yield return new WaitForSeconds(spawnInterval); 
            difficulty += 0.01f; 
            spawnInterval = Mathf.Max(0.0001f, spawnIntervalFixed / difficulty);
            if (!spawningEnabled)
            {
                break;
            }
        }
    }

    private void SpawnEnemyWithProbability()
    {
        if (enemyPrefabs.Count != spawnProbabilities.Count)
        {
            Debug.LogError("Die Anzahl der Prefabs und Wahrscheinlichkeiten stimmt nicht überein!");
            return;
        }

        // Berechne die Gesamtwahrscheinlichkeit
        float totalProbability = 0f;
        foreach (float probability in spawnProbabilities)
        {
            totalProbability += probability;
        }

        // Wähle einen zufälligen Wert basierend auf der Gesamtwahrscheinlichkeit
        float randomValue = Random.Range(0f, totalProbability);

        // Finde den Gegner basierend auf der Wahrscheinlichkeit
        float cumulativeProbability = 0f;
        for (int i = 0; i < enemyPrefabs.Count; i++)
        {
            cumulativeProbability += spawnProbabilities[i];
            if (randomValue <= cumulativeProbability)
            {
                // Wähle einen zufälligen Spawn-Punkt
                Transform spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];
                Instantiate(enemyPrefabs[i], spawnPoint.position, spawnPoint.rotation);
                return;
            }
        }

    }



    private IEnumerator SpawnBoss()
    {
        while (true)
        {
            if (EnemyBoss != null)
            {
                yield return new WaitForSeconds(1f);
                bossTimer++;

                if (bossTimer >= 25f)
                {

                    Transform spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];
                    Instantiate(EnemyBoss, spawnPoint.position, spawnPoint.rotation);
                    bossTimer = 0f;
                }
            }
            if (!spawningEnabled)
            {
                break;
            }
        }
    }
    public void ManualSpawn(string enemyType)
    {
        if (player == null)
        {
            Debug.LogError("Spieler-Objekt ist nicht gesetzt!");
            return;
        }

        float spawnDistance = 5.0f;

        Vector3 spawnPosition = player.transform.position + player.transform.forward * spawnDistance;
        spawnPosition.z = -1;

        switch (enemyType)
        {
            case "meele":
                if (enemyPrefabs.Count > 0)
                {
                    Instantiate(enemyPrefabs[0], spawnPosition, Quaternion.identity);
                }
                else
                {
                    Debug.LogWarning("Keine Gegner-Prefabs verfügbar für Meele!");
                }
                break;

            case "ranged":
                if (enemyPrefabs.Count > 1)
                {
                    Instantiate(enemyPrefabs[1], spawnPosition, Quaternion.identity);
                }
                else
                {
                    Debug.LogWarning("Keine Gegner-Prefabs verfügbar für Ranged!");
                }
                break;

            case "boss":
                if (EnemyBoss != null)
                {
                    Instantiate(EnemyBoss, spawnPosition, Quaternion.identity);
                }
                else
                {
                    Debug.LogWarning("Kein Boss-Prefab verfügbar!");
                }
                break;

            default:
                Debug.LogWarning($"Unbekannter Spawn-Typ: {enemyType}");
                break;
        }
    }

    public void ToggleSpawning()
    {
        spawningEnabled = !spawningEnabled;

        if (spawningEnabled)
        {
            StartCoroutine(SpawnEnemiesRepeatedly());
            StartCoroutine(SpawnBoss());
        }
        else
        {
            StopCoroutine(SpawnEnemiesRepeatedly());
            StopCoroutine(SpawnBoss());

        }
    }
}
