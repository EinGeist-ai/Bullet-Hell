using UnityEngine;
using TMPro;
using System.Linq; // LINQ für Select hinzufügen

public class Console : MonoBehaviour
{
    [SerializeField] private TMP_Text textObject;

    public TMP_InputField tMP_InputField;
    public GameObject ConsoleObject;
    public bool ConsoleActive = false;

    public TMP_Text tMP_Text

    [SerializeField] private SpawnEnemys spawnScript; // Reference to SpawnEnemys script

    private void Start()
    {

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
        checkCommand(text); // Überprüfe den Befehl
        tMP_InputField.text = ""; // Leere das Eingabefeld nach dem Senden des Befehls
        ConsoleObject.SetActive(false); // Deaktiviere das Eingabefeld nach dem Senden des Befehls
        ConsoleActive = false; // Setze ConsoleActive auf false, um die Konsole zu schließen
    }


    private void checkCommand(string command)
    {
        // Entferne unsichtbare Zeichen wie Zero-Width Space
        command = command.Replace("\u200B", "").Trim();

        string firstWord = command.Split(' ')[0];

        switch (firstWord)
        {
            case "spawn":
                Spawn(command);
                break;
            case "help":
                Debug.Log("Available commands: clear, help"); // Zeigt verfügbare Befehle an
                break;
            case "spawnToggle":
                spawnScript.ToggleSpawning();
                Debug.Log("Spawn toggled."); // Beispielausgabe
                break;
            default:
                Debug.Log("Unknown command: '" + firstWord + "'"); // Zeigt unbekannte Befehle an
                break;
        }
    }


    private void Spawn(string command)
    {
        string secondWord = command.Split(' ')[1];

        switch (secondWord)
        {
            case "meele":
                // Hier den Code zum Spawnen eines Meele-Gegners einfügen
                Debug.Log("Spawning Meele enemy..."); // Beispielausgabe
                break;
            case "ranged":
                // Hier den Code zum Spawnen eines Ranged-Gegners einfügen
                Debug.Log("Spawning Ranged enemy..."); // Beispielausgabe
                break;
            case "boss":
                // Hier den Code zum Spawnen eines Boss-Gegners einfügen
                Debug.Log("Spawning Boss enemy..."); // Beispielausgabe
                break;
            default:
                Debug.Log("Unknown spawn type: " + secondWord); // Zeigt unbekannte Typen an
                break;
        }

    }
}