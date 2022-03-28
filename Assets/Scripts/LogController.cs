using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LogController : MonoBehaviour
{
    [HideInInspector] private float rotationVelocity;
    [HideInInspector] private float timeTillRotation;

    [Header("Behavior")]
    [SerializeField] private float difficulty = 1f;
    [SerializeField] private float stopChance = 0.5f;
    [SerializeField] private float reverseChance = 0.1f;

    [Header("Rotation")]
    [SerializeField] private Transform rotatingObject;
    [SerializeField] private float maxRotationVelocity = 1f;
    [SerializeField] private float accelerationSpeed = 1f;
    [SerializeField] private float rotationCooldown = 2f;
    [SerializeField] private bool shouldRotate = true;
    [SerializeField] private bool shouldReverse = false;

    private void Start()
    {
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

        rotatingObject.Rotate(Vector3.down, rotationVelocity, Space.Self);
    }

    private void RandomizeBehavior()
    {
        float randomSeed = Random.Range(0f, 100f);

        if (shouldRotate && randomSeed < stopChance * difficulty)
        {
            shouldRotate = false;
            timeTillRotation = rotationCooldown;
        }

        if (randomSeed < reverseChance * difficulty)
        {
            shouldReverse = !shouldReverse;
        }
    }
}
