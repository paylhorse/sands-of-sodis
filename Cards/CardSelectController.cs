using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// **** Backbone of the thought-cards interface

public class CardSelectController : MonoBehaviour
{
    public GameObject pointLight;
    public Vector3[] lightPositions = new Vector3[4];
    public GameObject[] cards;
    public float lerpSpeed = 3.0f;
    public float cardRaiseHeight = 1.0f;
    public float cardMoveBackward = 1.0f;
    public float cardLerpSpeed = 3.0f;

    public Transform leftHand;
    public Transform rightHand;
    public float handBounceDistance = 0.2f;
    public float handBounceSpeed = 3.0f;

    private int currentIndex = 0;
    private bool isMoving = false;
    private Vector3 targetPosition;

    private Vector3 leftHandInitialPos;
    private Vector3 rightHandInitialPos;
    private Vector3[] originalCardPositions;

    private bool isActive = false;

    public GameObject cardInterface;

    // Draw Process

    public GameObject drawCard;
    public GameObject playerCard;

    private List<GameObject> instantiatedObjects = new List<GameObject>();

    public Transform playerHolder;

    public Canvas overlayCanvas;

    // Inspection

    public Transform inspectPosition;

    private enum CardState
    {
        Idle,
        Inspect
    }

    private CardState cardState = CardState.Idle;

    private Vector3[] originalCardScales;

    private Quaternion[] originalCardRotations;

    // Sphere Menu

    public SphereMenu sphereMenu;

    // Tooltip

    public GameObject cardTooltip;

    // Battle Manager

    public BattleManager battleManager;

    public void PlayCard()
    {
        // Card Play Logic Here
        FinishPlay();
    }

    public void FinishPlay()
    {
        Deactivate();
        battleManager.FreezeTime();
        sphereMenu.Activate();
    }

    void Start()
    {
        InitializeLightPositions();
        leftHandInitialPos = leftHand.position;
        rightHandInitialPos = rightHand.position;

        originalCardPositions = new Vector3[cards.Length];
        for (int i = 0; i < cards.Length; i++)
        {
            originalCardPositions[i] = cards[i].transform.position;
        }

        originalCardScales = new Vector3[cards.Length];
        originalCardRotations = new Quaternion[cards.Length];
        for (int i = 0; i < cards.Length; i++)
        {
            originalCardScales[i] = cards[i].transform.localScale;
            originalCardRotations[i] = cards[i].transform.rotation;
        }
    }

    void Update()
    {
        if (isActive)
        {
            if (cardState == CardState.Idle)
            {
                if (!isMoving)
                {
                    if (Input.GetKeyDown(KeyCode.RightArrow))
                    {
                        MoveRight();
                    }
                    else if (Input.GetKeyDown(KeyCode.LeftArrow))
                    {
                        MoveLeft();
                    }
                    else if (Input.GetKeyDown(KeyCode.UpArrow))
                    {
                        InspectCard();
                    }
                    else if (Input.GetKeyDown(KeyCode.Return))
                    {
                        PlayCard();
                    }
                }
                else
                {
                    MoveLight();
                }

                AnimateHands();
                AnimateCards();
            }
            else if (cardState == CardState.Inspect)
            {
                if (Input.GetKeyDown(KeyCode.Return))
                {
                    PlayCard();
                }
                else if (Input.anyKeyDown)
                {
                    UninspectCard();
                }
            }
        }
    }

    void InspectCard()
    {
        cardState = CardState.Inspect;
        GameObject card = cards[currentIndex];
        card.GetComponent<VerticalMeshFloating>().enabled = false;

        // Tooltip Appearance and Fill

        cardTooltip.SetActive(true);
        Card actualCard = card.GetComponent<Card>();
        GameObject[] cardKeywords = actualCard.Keywords;

        for (int i = 0; i < cardKeywords.Length; i++)
        {
            GameObject instance = Instantiate(cardKeywords[i], cardTooltip.transform);
        }

        StartCoroutine(MoveCardToInspectPosition(card, inspectPosition));
    }

    void UninspectCard()
    {
        cardState = CardState.Idle;

        foreach (Transform child in cardTooltip.transform)
        {
            GameObject.Destroy(child.gameObject);
        }
        cardTooltip.SetActive(false);

        GameObject card = cards[currentIndex];
        StartCoroutine(MoveCardToOriginalPosition(card, currentIndex));
    }

    IEnumerator MoveCardToInspectPosition(GameObject card, Transform target)
    {
        float t = 0;
        Vector3 startPosition = card.transform.position;
        Quaternion startRotation = card.transform.rotation;
        Vector3 startScale = card.transform.localScale;

        while (t < 1)
        {
            t += Time.deltaTime * cardLerpSpeed;
            card.transform.position = Vector3.Lerp(startPosition, target.position, t);
            card.transform.rotation = Quaternion.Lerp(startRotation, target.rotation, t);
            card.transform.localScale = Vector3.Lerp(startScale, target.localScale, t);
            yield return null;
        }
    }

    IEnumerator MoveCardToOriginalPosition(GameObject card, int index)
    {
        float t = 0;
        Vector3 startPosition = card.transform.position;
        Quaternion startRotation = card.transform.rotation;
        Vector3 startScale = card.transform.localScale;

        while (t < 1)
        {
            t += Time.deltaTime * cardLerpSpeed;
            card.transform.position = Vector3.Lerp(startPosition, originalCardPositions[index], t);
            card.transform.rotation = Quaternion.Lerp(
                startRotation,
                originalCardRotations[index],
                t
            );
            card.transform.localScale = Vector3.Lerp(startScale, originalCardScales[index], t);
            yield return null;
        }
    }

    void MoveRight()
    {
        currentIndex = (currentIndex + 1) % lightPositions.Length;
        targetPosition = lightPositions[currentIndex];
        isMoving = true;
    }

    void MoveLeft()
    {
        currentIndex = (currentIndex - 1 + lightPositions.Length) % lightPositions.Length;
        targetPosition = lightPositions[currentIndex];
        isMoving = true;
    }

    void MoveLight()
    {
        pointLight.transform.position = Vector3.Lerp(
            pointLight.transform.position,
            targetPosition,
            Time.deltaTime * lerpSpeed
        );
        if (Vector3.Distance(pointLight.transform.position, targetPosition) < 0.01f)
        {
            pointLight.transform.position = targetPosition;
            isMoving = false;
        }
    }

    void AnimateHands()
    {
        float bounce = Mathf.PingPong(Time.time * handBounceSpeed, handBounceDistance);

        if (currentIndex > 0)
        {
            leftHand.position = leftHandInitialPos + Vector3.left * bounce;
        }

        if (currentIndex < lightPositions.Length - 1)
        {
            rightHand.position = rightHandInitialPos + Vector3.right * bounce;
        }
    }

    void AnimateCards()
    {
        if (cardState == CardState.Idle)
        {
            for (int i = 0; i < cards.Length; i++)
            {
                Vector3 targetPosition = originalCardPositions[i];

                if (i == currentIndex && !isMoving)
                {
                    targetPosition += new Vector3(0, cardRaiseHeight, -cardMoveBackward);
                }

                cards[i].transform.position = Vector3.Lerp(
                    cards[i].transform.position,
                    targetPosition,
                    Time.deltaTime * cardLerpSpeed
                );

                if (i == currentIndex)
                {
                    cards[i].GetComponent<VerticalMeshFloating>().enabled = true;
                }
                else
                {
                    cards[i].GetComponent<VerticalMeshFloating>().enabled = false;
                }
            }
        }
    }

    private void InitializeLightPositions()
    {
        lightPositions = new Vector3[cards.Length];
        for (int i = 0; i < cards.Length; i++)
        {
            lightPositions[i] = cards[i].transform.position + new Vector3(0, 3.0f, 1.0f);
        }
    }

    public void Activate()
    {
        // Start the DrawAnimation coroutine
        StartCoroutine(DrawProcessCoroutine());
    }

    private IEnumerator DrawProcessCoroutine()
    {
        // Animations
        // Instantiate 4 drawCards and playerCards each, with 0.5 seconds delay between each
        for (int i = 0; i < 4; i++)
        {
            GameObject newDrawCard = Instantiate(drawCard, overlayCanvas.transform);
            instantiatedObjects.Add(newDrawCard);
            GameObject newPlayerCard = Instantiate(playerCard, playerHolder);
            instantiatedObjects.Add(newPlayerCard);

            yield return new WaitForSeconds(0.2f);
        }

        yield return new WaitForSeconds(0.2f);

        cardInterface.SetActive(true);
        isActive = true;

        // Delete all instantiated objects
        foreach (GameObject obj in instantiatedObjects)
        {
            Destroy(obj);
        }

        // Clear the list
        instantiatedObjects.Clear();
    }

    public void Deactivate()
    {
        cardInterface.SetActive(false);
        isActive = false;
    }
}
