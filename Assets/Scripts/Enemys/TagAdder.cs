using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TagAdder : MonoBehaviour
{
    void Start()
    {
        transform.parent.tag = "Untagged";
    }
    [SerializeField] private string tagToAdd; // Der Tag, der hinzugefügt werden soll

    // Wird aufgerufen, wenn eine Kollision mit einem Collider stattfindet
    private void OnCollisionEnter(Collision collision)
    {
        // Überprüfen, ob das übergeordnete Objekt existiert
        if (transform.parent != null)
        {
            // Den angegebenen Tag dem übergeordneten Objekt hinzufügen
            transform.parent.tag = tagToAdd;
        }
        else
        {
            Debug.LogWarning("Kein übergeordnetes Objekt vorhanden, Tag konnte nicht hinzugefügt werden.");
        }
    }
}
