using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class destroyExplosion : MonoBehaviour
{
    private Animator _animator;

    // Start is called before the first frame update
    void Start()
    {
        _animator = GetComponent<Animator>();
        if (_animator != null)
        {
            // Zerstört das GameObject nach Abschluss der Animation
            Destroy(gameObject, _animator.GetCurrentAnimatorStateInfo(0).length);
        }
        else
        {
            Debug.LogWarning("Animator not found on explosion object. Destroying immediately.");
            Destroy(gameObject);
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
