using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TagAdder : MonoBehaviour
{
    void Start()
    {
        transform.parent.tag = "Untagged";
    }
    [SerializeField] private string tagToAdd; // Der Tag, der hinzugef�gt werden soll

    // Wird aufgerufen, wenn eine Kollision mit einem Collider stattfindet
    private void OnCollisionEnter(Collision collision)
    {
        // �berpr�fen, ob das �bergeordnete Objekt existiert
        if (transform.parent != null)
        {
            // Den angegebenen Tag dem �bergeordneten Objekt hinzuf�gen
            transform.parent.tag = tagToAdd;
        }
        else
        {
            Debug.LogWarning("Kein �bergeordnetes Objekt vorhanden, Tag konnte nicht hinzugef�gt werden.");
        }
    }
}
