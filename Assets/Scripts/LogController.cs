using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LogController : MonoBehaviour
{
    [HideInInspector] private float rotationVelocity;
    [HideInInspector] private float timeTillRotation;
    [HideInInspector] private Transform rotatingObject;

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
        rotatingObject = transform.parent;

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
            GameObject apple = Instantiate(applePrefab, transform.position, Quaternion.Euler(0f, 0f, appleAngle));
            apple.transform.SetParent(transform);
        }

        int knifesAmount = Random.Range(minKnifes, maxKnifes);
        List<float> knifeAngles = new List<float>();

        while (knifesAmount > 0)
        {
            float knifeAngle = Random.Range(-180f, 180f);

            if (knifeAngle != appleAngle && !knifeAngles.Contains(knifeAngle))
            {
                GameObject knife = Instantiate(knifePrefab, transform.position, Quaternion.Euler(0f, 0f, knifeAngle));
                knife.transform.SetParent(transform);
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

        rotatingObject.Rotate(Vector3.back, rotationVelocity, Space.Self);
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
        transform.SetParent(null);
        gameObject.GetComponent<Rigidbody>().useGravity = true;
    }
}
