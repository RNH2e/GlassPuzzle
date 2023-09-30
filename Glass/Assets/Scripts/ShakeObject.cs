using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShakeObject : MonoBehaviour
{
    public Transform objectToShake;
    public float shakeAmount = 0.1f;
    public float shakeSpeed = 5.0f;

    private Vector3 originalPosition;

    void Start()
    {
        originalPosition = objectToShake.position;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            StartCoroutine(Shake());
        }
    }

    IEnumerator Shake()
    {
        float elapsedTime = 0f;

        while (elapsedTime < 0.2f) // Toplam sallama süresi
        {
            float offsetX = Mathf.PerlinNoise(Time.time * shakeSpeed, 0) * 2 - 1;
            Vector3 shakeOffset = new Vector3(offsetX, 0, 0) * shakeAmount;

            objectToShake.position = originalPosition + shakeOffset;

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        objectToShake.position = originalPosition; // Sallamayý bitir, objeyi baþlangýç pozisyonuna geri getir
    }
}
