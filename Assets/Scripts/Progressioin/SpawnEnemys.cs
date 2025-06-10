using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnEnemys : MonoBehaviour
{
    // Liste für Gegner-Prefabs für diesen Spawner
    [SerializeField]
    private List<GameObject> enemyPrefabs; // Liste von Gegner-Prefabs für diesen Spawner


    // Wahrscheinlichkeiten für jeden Gegner für diesen Spawner
    [SerializeField]
    private List<float> spawnProbabilities; // Liste von Wahrscheinlichkeiten (muss mit enemyPrefabs übereinstimmen)

    // Spawn-Positionen für diesen Spawner
    [SerializeField]
    private Transform[] spawnPoints; // Array von Spawn-Punkten für diesen Spawner

    public GameObject player;

    // Zeit zwischen Spawns
    [SerializeField]
    private float spawnInterval = 1.5f; // Zeit in Sekunden zwischen Spawns
    private float spawnIntervalFixed = 1.5f; // Timer für den Spawn
    public float bossTimer = 0f;

    public bool spawningEnabled = true; // Flag, um das Spawnen zu aktivieren/deaktivieren

    public GameObject EnemyBoss;

    public float difficulty = 0.02f;

    void Start()
    {
        // Starte die Spawn-Coroutine
        StartCoroutine(SpawnEnemiesRepeatedly());
        StartCoroutine(SpawnBoss());
    }

    void Update()
    {
        
    }
    private IEnumerator SpawnEnemiesRepeatedly()
    {
        while (true)
        {
                SpawnEnemyWithProbability();
                yield return new WaitForSeconds(spawnInterval); // Warte die festgelegte Zeit
                difficulty += 0.01f; // Erhöhe die Schwierigkeit (kann angepasst werden)
                spawnInterval = Mathf.Max(0.0001f, spawnIntervalFixed / difficulty); // Verringere das Intervall, um die Schwierigkeit zu erhöhen
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
                    Instantiate(EnemyBoss, spawnPoint.position, spawnPoint.rotation); // Angenommen, der Boss ist das erste Element in enemyPrefabs
                    bossTimer = 0f;
                }
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

        // Abstand vor dem Spieler, wo der Gegner gespawnt wird
        float spawnDistance = 2.0f;

        // Berechne die Position vor dem Spieler
        Vector3 spawnPosition = player.transform.position + player.transform.forward * spawnDistance;

        switch (enemyType)
        {
            case "Meele":
                if (enemyPrefabs.Count > 0)
                {
                    Instantiate(enemyPrefabs[0], spawnPosition, Quaternion.identity);
                }
                else
                {
                    Debug.LogWarning("Keine Gegner-Prefabs verfügbar für Meele!");
                }
                break;

            case "Ranged":
                if (enemyPrefabs.Count > 1)
                {
                    Instantiate(enemyPrefabs[1], spawnPosition, Quaternion.identity);
                }
                else
                {
                    Debug.LogWarning("Keine Gegner-Prefabs verfügbar für Ranged!");
                }
                break;

            case "Boss":
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
        spawningEnabled = !spawningEnabled; // Umschalten des Spawning-Flags

        if (spawningEnabled)
        {
            StartCoroutine(SpawnEnemiesRepeatedly());
            StartCoroutine(SpawnBoss()); // Starte die Boss-Spawning Coroutine, falls sie nicht bereits läuft
        }
        else
        {
            StopCoroutine(SpawnEnemiesRepeatedly());
            StopCoroutine(SpawnBoss()); // Stoppe die Boss-Spawning Coroutine
        }
    }
}
