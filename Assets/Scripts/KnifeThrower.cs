using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class KnifeThrower : MonoBehaviour
{
    [HideInInspector] private int remainingKnifes;
    [HideInInspector] private Color remainingDeltaColor;

    [Header("Knifes")]
    [SerializeField] private GameObject knifePrefab;
    [SerializeField] private int knifesAmount = 10;
    [SerializeField] private KnifeController currentKnife;
    [SerializeField] private Vector3 newKnifePosition;

    [Header("Throwing")]
    [SerializeField] private float throwForce = 500f;

    [Header("UI")]
    [SerializeField] private Text knifesAmountText;
    [SerializeField] private Text remainingKnifesText;
    [SerializeField] private Color remainingMaxColor = Color.red;
    [SerializeField] private Color remainingMinColor = Color.green;

    private void Start()
    {
        remainingKnifes = knifesAmount;
        remainingDeltaColor = remainingMaxColor - remainingMinColor;
        knifesAmountText.text = knifesAmount.ToString();
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

    private void FixedUpdate()
    {
        UpdateKnifeText();
    }

    private void UpdateKnifeText()
    {
        remainingKnifesText.text = remainingKnifes.ToString();

        remainingKnifesText.color = remainingMinColor + remainingDeltaColor / knifesAmount * remainingKnifes;
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
