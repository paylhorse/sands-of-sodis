using System.Collections;
using UnityEngine;

public class CardOrbit : MonoBehaviour
{
    public int numberOfCards = 5;
    public GameObject cardPrefab;
    public float orbitRadius = 5f;
    public float cardDelay = 0.5f; 
    public float moveSpeed = 2f; 
    public float rotationSpeed = 30f;
    public float localRotationSpeed = 60f;  // Local y-axis rotation speed for each card

    private GameObject[] cards;

    // Bezier curve control points
    private Vector3 startPoint = new Vector3(-10f, 0, -5f);
    private Vector3 controlPoint = new Vector3(-5f, 0, 0);
    private Vector3 orbitStartPoint;

    void Start()
    {
        cards = new GameObject[numberOfCards];
        orbitStartPoint = new Vector3(0, 0, orbitRadius); // Start of the orbit

        for (int i = 0; i < numberOfCards; i++)
        {
            GameObject card = Instantiate(cardPrefab, startPoint, Quaternion.identity);
            cards[i] = card;
            StartCoroutine(MoveCard(card, i));
        }
    }

    IEnumerator MoveCard(GameObject card, int index)
    {
        yield return new WaitForSeconds(cardDelay * index);

        float t = 0;
        while (t < 1)
        {
            t += Time.deltaTime * moveSpeed;

            // Get the current position on the Bezier curve
            Vector3 cardPosition = BezierCurve(t, startPoint, controlPoint, orbitStartPoint);
            card.transform.position = cardPosition;

            card.transform.Rotate(Vector3.up, rotationSpeed * Time.deltaTime);
            card.transform.Rotate(Vector3.up * localRotationSpeed * Time.deltaTime, Space.Self);

            yield return null;
        }

        // Once the card reaches its endpoint, transition to orbit
        float orbitAngle = 0f;
        while (true)
        {
            orbitAngle += rotationSpeed * Time.deltaTime * Mathf.Deg2Rad;
            Vector3 orbitPos = new Vector3(orbitRadius * Mathf.Sin(orbitAngle), 0, orbitRadius * Mathf.Cos(orbitAngle));
            card.transform.position = orbitPos;
            card.transform.Rotate(Vector3.up, rotationSpeed * Time.deltaTime);
            card.transform.Rotate(Vector3.up * localRotationSpeed * Time.deltaTime, Space.Self);

            yield return null;
        }
    }

    Vector3 BezierCurve(float t, Vector3 start, Vector3 control, Vector3 end)
    {
        float u = 1 - t;
        Vector3 point = (u * u * start) + (2 * u * t * control) + (t * t * end);
        return point;
    }
}

