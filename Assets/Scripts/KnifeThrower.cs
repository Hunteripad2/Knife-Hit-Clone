using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnifeThrower : MonoBehaviour
{
    [HideInInspector] private int remainingKnifes;

    [Header("Knifes")]
    [SerializeField] private GameObject knifePrefab;
    [SerializeField] private int knifeAmount = 10;
    [SerializeField] private KnifeController currentKnife;
    [SerializeField] private Vector3 newKnifePosition;

    [Header("Throwing")]
    [SerializeField] private float throwForce = 500f;

    private void Start()
    {
        remainingKnifes = knifeAmount;
    }

    private void Update()
    {
        if (remainingKnifes > 0)
        {
            if (currentKnife == null)
            {
                CreateKnife();
            }
            else if (Input.GetButtonDown("Fire1") && currentKnife.readyToThrow)
            {
                ThrowKnife();
            }
        }
    }

    private void CreateKnife()
    {
        GameObject knife = Instantiate(knifePrefab, newKnifePosition, knifePrefab.transform.rotation, transform);
        currentKnife = knife.GetComponent<KnifeController>();

        if (remainingKnifes == 1)
        {
            currentKnife.isLast = true;
        }
    }

    private void ThrowKnife()
    {
        currentKnife.GetComponent<Rigidbody>().AddForce(currentKnife.transform.up * throwForce);
        currentKnife = null;
        remainingKnifes--;
    }
}
