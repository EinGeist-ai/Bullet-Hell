using UnityEngine;
using TMPro;
using System.Linq;
public class Console : MonoBehaviour
{

    [Header("TMP Objects")]
    public TMP_Text textObject;
    public TMP_InputField tMP_InputField;
    public TMP_Text tMP_Text;

    [Header("Console Variables")]
    public GameObject ConsoleObject;
    public bool ConsoleActive = false;
    public GameObject EnemySpawner; 
    public SpawnEnemys spawnScript;
    public GameObject levelup;
    public LevelUP levelupscript;

    private void Start()
    {

        EnemySpawner = GameObject.Find("ENEMY SPAWNER"); 
        spawnScript = EnemySpawner.GetComponent<SpawnEnemys>(); 
        levelup = GameObject.Find("LevelUp");



        if (ConsoleObject == null)
        {
            Debug.LogError("ConsoleObject ist nicht zugewiesen!");
            return;
        }
        else
        {
            ConsoleObject.SetActive(false);
            ConsoleActive = false;

        }

        if (textObject == null)
        {
            Debug.LogError("TMP_Text ist nicht zugewiesen!");
            return;
        }

        if (spawnScript == null)
        {
            spawnScript = GameObject.Find("EnemySpawner").GetComponent<SpawnEnemys>();
            if (spawnScript == null)
            {
                Debug.LogError("SpawnEnemys-Skript konnte nicht gefunden werden!");
            }
        }
    }

    private void Update()
    {

        SendCommand();

    }

    public void SendCommand()
    {
        if (Input.GetKeyDown(KeyCode.Return) && ConsoleActive)
        {
            LogTextObjectContent();
        }
        else if (Input.GetKeyDown(KeyCode.Return) && !ConsoleActive)
        {
            ConsoleObject.SetActive(true);
            ConsoleActive = true;
            tMP_InputField.ActivateInputField();
        }

    }


    private void LogTextObjectContent()
    {
        string text = textObject.text;
        checkCommand(text);
        tMP_InputField.text = "";
        ConsoleObject.SetActive(false);
        ConsoleActive = false;
    }

    private void checkCommand(string command)
    {
        
        command = command.Replace("\u200B", "").Trim();

        string firstWord = command.Split(' ')[0];

        switch (firstWord)
        {
            case "spawn":
                Spawn(command);
                break;
            case "help":
                Debug.Log("Available commands: spawn <type>, help, spawnToggle, kill, killself, levelup"); 
                break;
            case "spawnToggle":
                spawnScript.ToggleSpawning();
                Debug.Log("Spawn toggled.");
                break;
            case "kill":
                KillAllEnemies();
                break;
            case "killself":
                KillSelf();
                break;
            case "levelup":
                levelupscript = levelup.GetComponent<LevelUP>();
                levelupscript.TriggerLevelUp();
                break;
            default:
                Debug.Log("Unknown command: '" + firstWord + "'"); 
                break;
        }
    }

    private void KillSelf()
    {
        PlayerHealth playerHealth = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerHealth>();
        if (playerHealth != null)
        {
            playerHealth.TakeDamage(69420);
        }
        else
        {
            Debug.LogError("PlayerHealth component not found on Player object.");
        }
    }

    private void KillAllEnemies()
    {
        
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");

        foreach (GameObject enemy in enemies)
        {
            EnemyHealth enemyHealth = enemy.GetComponent<EnemyHealth>();
            if (enemyHealth != null)
            {
                enemyHealth.TakeDamage(69420);
            }
        }
        GameObject[] enemiesBoss = GameObject.FindGameObjectsWithTag("EnemyBoss");

        foreach (GameObject enemyBoss in enemiesBoss)
        {
            EnemyHealth enemyHealth = enemyBoss.GetComponent<EnemyHealth>();
            if (enemyHealth != null)
            {
                enemyHealth.TakeDamage(69420);
            }
        }

        Debug.Log("All enemies have been killed.");
    }

    private void Spawn(string command)
    {
        string secondWord = command.Split(' ')[1];

        switch (secondWord)
        {
            case "meele":
                
                spawnScript.ManualSpawn("meele"); 
                Debug.Log("Spawning Meele enemy..."); 
                break;
            case "ranged":
                
                spawnScript.ManualSpawn("ranged"); 
                Debug.Log("Spawning Ranged enemy..."); 
                break;
            case "boss":
                
                spawnScript.ManualSpawn("boss"); 
                Debug.Log("Spawning Boss enemy...");
                break;
            default:
                Debug.Log("Unknown spawn type: " + secondWord); 
                break;
        }

    }
}