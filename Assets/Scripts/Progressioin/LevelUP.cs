using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class LevelUP : MonoBehaviour
{
    public List<string> Upgrades;
    public List<Sprite> UpgradeSprites;

    public GameObject Player;
    public PlayerUpgrades playerUpgrades;
    public GameObject Canvas;
    public Canvas CanvasComponent;
    public GameObject HeadText;
    public Text HeadTextComponent;

    private int upgradeIndex = 0;

    private int left = 0;
    private int right = 0;
    private int middle = 0;

    public GameObject leftButton;
    public GameObject rightButton;
    public GameObject middleButton;

    public Text leftText;
    public Text rightText;
    public Text middleText;

    private Image imageLeft;
    private Image imageRight;
    private Image imageMiddle;

    void Start()
    {
        
        
        if (Upgrades.Count != UpgradeSprites.Count)
        {
            Debug.LogError("Die Listen 'Upgrades' und 'UpgradeSprites' müssen die gleiche Anzahl an Elementen enthalten!");
            return;
        }

        upgradeIndex = Upgrades.Count;

        Player = GameObject.Find("Player");
        playerUpgrades = Player.GetComponent<PlayerUpgrades>();
        Canvas = GameObject.Find("Canvas");
        CanvasComponent = Canvas.GetComponent<Canvas>();
        HeadTextComponent = HeadText.GetComponent<Text>();
        
        // Text-Komponenten abrufen
        leftText = leftButton.GetComponentInChildren<Text>();
        rightText = rightButton.GetComponentInChildren<Text>();
        middleText = middleButton.GetComponentInChildren<Text>();

        // Image-Komponenten abrufen
        imageLeft = leftButton.GetComponentInChildren<Image>();
        imageRight = rightButton.GetComponentInChildren<Image>();
        imageMiddle = middleButton.GetComponentInChildren<Image>();

       
        CanvasComponent.enabled = false; // Canvas wieder deaktivieren


    }

    public void TriggerLevelUp()
    {
        CanvasComponent.enabled = true;
        HeadTextComponent.fontSize = 30; // Schriftgröße des HeadTexts anpassen
        HeadTextComponent.fontSize = 100;
        Time.timeScale = 0f;
        // Zufällige, eindeutige Indizes generieren
        HashSet<int> usedIndices = new HashSet<int>();
        left = GetUniqueRandomIndex(usedIndices);
        middle = GetUniqueRandomIndex(usedIndices);
        right = GetUniqueRandomIndex(usedIndices);

        // Texte und Sprites der Buttons aktualisieren
        UpdateButton(leftButton, leftText, imageLeft, left);
        UpdateButton(rightButton, rightText, imageRight, right);
        UpdateButton(middleButton, middleText, imageMiddle, middle);
    }

    private int GetUniqueRandomIndex(HashSet<int> usedIndices)
    {
        int index;
        do
        {
            index = Random.Range(0, upgradeIndex);
        } while (usedIndices.Contains(index));

        usedIndices.Add(index);
        return index;
    }

    private void UpdateButton(GameObject button, Text textComponent, Image imageComponent, int index)
    {
        if (button == null)
        {
            Debug.LogError("Button ist null!");
            return;
        }
        if (textComponent == null)
        {
            Debug.LogError($"Text ist null für Button: {button.name}");
            return;
        }
        if (imageComponent == null)
        {
            Debug.LogError($"Image ist null für Button: {button.name}");
            return;
        }

        textComponent.text = Upgrades[index];
        imageComponent.sprite = UpgradeSprites[index];
    }

    public void ButtonClicked(string buttonName)
    {
        Debug.Log($"Button geklickt: {buttonName}");
        switch (buttonName)
        {
            case "LeftButton":
                playerUpgrades.ApplyUpgrade(Upgrades[left]);
                Debug.Log($"Linker Button geklickt: {Upgrades[left]}");
                CanvasComponent.enabled = false;
                Time.timeScale = 1f;
                break;
            case "RightButton":
                playerUpgrades.ApplyUpgrade(Upgrades[right]);
                Debug.Log($"Rechter Button geklickt: {Upgrades[right]}");
                CanvasComponent.enabled = false;
                Time.timeScale = 1f;
                break;
            case "MiddleButton":
                playerUpgrades.ApplyUpgrade(Upgrades[middle]);
                Debug.Log($"Mittlerer Button geklickt: {Upgrades[middle]}");
                CanvasComponent.enabled = false;
                Time.timeScale = 1f;
                break;
            default:
                Debug.LogWarning($"Unbekannter Button: {buttonName}");
                break;
        }
    }

    // Update wird hier nicht benötigt, kann aber für zukünftige Erweiterungen genutzt werden
    void Update()
    {
    }
}
