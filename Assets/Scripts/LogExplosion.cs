using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LogExplosion : MonoBehaviour
{
    [Header("Parameters")]
    [SerializeField] private float explosionForce = 250f;

    private void Start()
    {
        gameObject.GetComponent<Rigidbody>().AddForce(-transform.up * explosionForce);
        Destroy(gameObject, 2f);
    }
}
