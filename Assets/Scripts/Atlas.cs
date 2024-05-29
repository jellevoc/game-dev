using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Atlas : MonoBehaviour
{

    [Header("Attributes")]
    [SerializeField] private CircleCollider2D circleCollider;
    [SerializeField] private GameObject bitcoinCrate;

    [Header("Events")]
    public static UnityEvent onWaveEnd = new UnityEvent();

    private float radius;

    void Start()
    {
        radius = circleCollider.radius;
        onWaveEnd.AddListener(DropBitcoinCrate);
    }

    // Gets called when the wave ends
    private void DropBitcoinCrate()
    {
        Vector2 randomPos = GetRandomPositionInCircle();
        Instantiate(bitcoinCrate, randomPos, Quaternion.identity);
    }


    Vector2 GetRandomPositionInCircle()
    {
        // (0, 360)
        float randomAngle = Random.Range(0f, Mathf.PI * 2);
        // (0, 5)
        float randomRadius = Mathf.Sqrt(Random.Range(0f, 1f)) * radius;

        // Calculate X and Y axis
        float xPos = Mathf.Cos(randomAngle) * randomRadius;
        float yPos = Mathf.Sin(randomAngle) * randomRadius;

        // Change pos to include offset
        Vector2 offset = circleCollider.offset;
        Vector2 randomPosition = new Vector2(xPos, yPos) + offset + (Vector2)gameObject.transform.position;

        return randomPosition;
    }
}
