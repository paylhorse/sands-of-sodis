using UnityEngine;
using System.Collections;

public class TutorialCardManager : MonoBehaviour
{
    public GameObject moveCard1;
    public GameObject moveCard2;
    public GameObject jumpCard;
    public GameObject actionCard;
    public GameObject menuCard;
    public GameObject handCard;
    public GameObject stealthCard;

    private GameObject currentCard1;
    private GameObject currentCard2;

    public void ShowMove()
    {
        StartCoroutine(ShowMoveCoroutine());
    }

    public void ShowJump()
    {
        StartShowCardRoutine(jumpCard);
    }

    public void ShowAction()
    {
        StartShowCardRoutine(actionCard);
    }

    public void ShowMenu()
    {
        StartShowCardRoutine(menuCard);
    }

    public void ShowHand()
    {
        StartShowCardRoutine(handCard);
    }

    public void ShowStealth()
    {
        StartShowCardRoutine(stealthCard);
    }

    public void DeactivateCurrentCards()
    {
        if (currentCard1)
        {
            currentCard1.SetActive(false);
            currentCard1 = null;
        }

        if (currentCard2)
        {
            currentCard2.SetActive(false);
            currentCard2 = null;
        }
    }

    private void StartShowCardRoutine(GameObject card)
    {
        DeactivateCurrentCards();
        currentCard1 = card;
        StartCoroutine(ShowCardForSeconds(card, 10));
    }

    private IEnumerator ShowMoveCoroutine()
    {
        DeactivateCurrentCards();
        currentCard1 = moveCard1;
        currentCard2 = moveCard2;

        moveCard1.SetActive(true);
        yield return new WaitForSeconds(1f);
        moveCard2.SetActive(true);

        yield return new WaitForSeconds(10f);
        DeactivateCurrentCards();
    }

    private IEnumerator ShowCardForSeconds(GameObject card, float seconds)
    {
        card.SetActive(true);
        yield return new WaitForSeconds(seconds);
        card.SetActive(false);
    }
}
