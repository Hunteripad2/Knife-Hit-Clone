using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnifeController : MonoBehaviour
{
    [HideInInspector] public bool isLast;
    [HideInInspector] public KnifeThrower knifeThrower;
    [HideInInspector] private LogController logController;
    [HideInInspector] private Rigidbody rb;
    [HideInInspector] public bool readyToThrow;
    [HideInInspector] public bool isStuck;

    [Header("Reloading")]
    [SerializeField] private float reloadSpeed = 10f;

    private void Start()
    {
        logController = GameObject.FindGameObjectWithTag("LogController").GetComponent<LogController>();
        rb = gameObject.GetComponent<Rigidbody>();
    }

    private void Update()
    {
        if (!readyToThrow && transform.position != transform.parent.position)
        {
            transform.position = Vector3.MoveTowards(transform.position, transform.parent.position, reloadSpeed * Time.deltaTime);
        }
        else
        {
            readyToThrow = true;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (!isStuck && collision.transform.CompareTag("Knife"))
        {
            rb.useGravity = true;
            gameObject.GetComponent<BoxCollider>().enabled = false;
            GameObject.FindGameObjectWithTag("UI").GetComponent<UIController>().ShowGameOverScreen();
        }
    }

    private void OnTriggerEnter(Collider collider)
    {
        if (collider.CompareTag("Log"))
        {
            rb.isKinematic = true;
            transform.SetParent(collider.transform);
            isStuck = true;

            if (isLast)
            {
                logController.CompleteLevel();
                knifeThrower.ReloadKnifeAmount();
            }
        }
        else if (collider.CompareTag("Apple"))
        {
            collider.transform.SetParent(null);
            collider.GetComponent<Rigidbody>().useGravity = true;
            //TODO
        }
    }
}
