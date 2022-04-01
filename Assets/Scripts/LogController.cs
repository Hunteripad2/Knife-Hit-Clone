using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LogController : MonoBehaviour
{
    [HideInInspector] private float rotationVelocity;
    [HideInInspector] private float timeTillRotation;

    [Header("Log")]
    [SerializeField] private Transform currentLog;
    [SerializeField] private Vector3 newLogPosition;

    [Header("Starting Items")]
    [SerializeField] private GameObject knifePrefab;
    [SerializeField] private GameObject applePrefab;
    [SerializeField] private float appleChance = 0.25f;
    [SerializeField] private int minKnifes = 1;
    [SerializeField] private int maxKnifes = 3;

    [Header("Behavior")]
    [SerializeField] private float difficulty = 1f;
    [SerializeField] private float stopChance = 0.5f;
    [SerializeField] private float reverseChance = 0.1f;

    [Header("Rotation")]
    [SerializeField] private float maxRotationVelocity = 1f;
    [SerializeField] private float accelerationSpeed = 1f;
    [SerializeField] private float rotationCooldown = 2f;
    [SerializeField] private bool shouldRotate = true;
    [SerializeField] private bool shouldReverse = false;

    private void Start()
    {
        GenerateObjects();
    }

    private void Update()
    {
        if (timeTillRotation > 0f)
        {
            timeTillRotation -= Time.deltaTime;
        }
        else if (!shouldRotate)
        {
            shouldRotate = true;
        }

        HandleRotation();
    }

    private void FixedUpdate()
    {
        RandomizeBehavior();
    }

    private void GenerateObjects()
    {
        float randomChance = Random.Range(0f, 1f);
        float appleAngle = Random.Range(-180f, 180f);

        if (randomChance <= appleChance)
        {
            GameObject apple = Instantiate(applePrefab, currentLog.transform.position, Quaternion.Euler(0f, 0f, appleAngle));
            apple.transform.SetParent(currentLog.transform);
        }

        int knifesAmount = Random.Range(minKnifes, maxKnifes);
        List<float> knifeAngles = new List<float>();

        while (knifesAmount > 0)
        {
            float knifeAngle = Random.Range(-180f, 180f);

            if (knifeAngle != appleAngle && !knifeAngles.Contains(knifeAngle))
            {
                GameObject knife = Instantiate(knifePrefab, currentLog.transform.position, Quaternion.Euler(0f, 0f, knifeAngle));
                knife.transform.SetParent(currentLog.transform);
                knifesAmount--;
            }
        }
    }

    private void HandleRotation()
    {
        float targetRotationVelocity = 0f;

        if (shouldRotate)
        {
            if (shouldReverse)
            {
                targetRotationVelocity = -maxRotationVelocity * difficulty;
            }
            else
            {
                targetRotationVelocity = maxRotationVelocity * difficulty;
            }
        }

        rotationVelocity = Mathf.Lerp(rotationVelocity, targetRotationVelocity, accelerationSpeed * Time.deltaTime);

        transform.Rotate(Vector3.back, rotationVelocity, Space.Self);
    }

    private void RandomizeBehavior()
    {
        float randomChance = Random.Range(0f, 100f);

        if (shouldRotate && randomChance < stopChance * difficulty)
        {
            shouldRotate = false;
            timeTillRotation = rotationCooldown;
        }

        if (randomChance < reverseChance * difficulty)
        {
            shouldReverse = !shouldReverse;
        }
    }

    public void CompleteLevel()
    {
        currentLog.transform.SetParent(null);

        Rigidbody rb = currentLog.GetComponent<Rigidbody>();

        rb.isKinematic = false;
        rb.useGravity = true;
    }
}
