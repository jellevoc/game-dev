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

    private void DropBitcoinCrate()
    {
        Vector2 randomPos = GetRandomPositionInCircle();
        Instantiate(bitcoinCrate, randomPos, Quaternion.identity);
    }


    Vector2 GetRandomPositionInCircle()
    {
        // Genereer willekeurige coördinaten binnen het bereik van de cirkelstraal
        float randomAngle = Random.Range(0f, Mathf.PI * 2); // Hoek tussen 0 en 2pi (360 graden)
        float randomRadius = Mathf.Sqrt(Random.Range(0f, 1f)) * radius; // Willekeurige straal binnen de cirkel

        // Bereken de x- en y-coördinaten van de gegenereerde positie
        float xPos = Mathf.Cos(randomAngle) * randomRadius;
        float yPos = Mathf.Sin(randomAngle) * randomRadius;

        // Pas de positie aan zodat deze binnen de cirkel is, inclusief de offset
        Vector2 offset = circleCollider.offset;
        Vector2 randomPosition = new Vector2(xPos, yPos) + offset + (Vector2)gameObject.transform.position;

        return randomPosition;
    }
}
