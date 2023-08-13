using System.Collections;
using UnityEngine;

public class TitleScreenManager : MonoBehaviour
{
    public GameObject shield;
    public Transform shieldStartPosition;
    public Transform shieldEndPosition;
    public GameObject titleLogo;
    public GameObject titleCommandPanel;
    public GameObject infoText;
    public AudioSource openingBGM;
    public AudioSource mainBGM;
    public float animationDuration = 5f;
    public AnimationCurve positionCurve;
    public AnimationCurve scaleCurve;
    public AnimationCurve rotationCurve;

    private Coroutine openingAnimationCoroutine;

    private bool isOpeningAnimationComplete = false;

    private void Start()
    {
        titleLogo.SetActive(false);
        titleCommandPanel.SetActive(false);
        infoText.SetActive(false);
        mainBGM.Stop();
        openingBGM.Play();

        openingAnimationCoroutine = StartCoroutine(OpeningAnimation());
    }

    private IEnumerator OpeningAnimation()
    {
        float elapsedTime = 0f;

        while (elapsedTime < animationDuration)
        {
            float progress = elapsedTime / animationDuration;

            shield.transform.position = Vector3.Lerp(shieldStartPosition.position, shieldEndPosition.position, positionCurve.Evaluate(progress));
            shield.transform.localScale = Vector3.Lerp(shieldStartPosition.localScale, shieldEndPosition.localScale, scaleCurve.Evaluate(progress));
            shield.transform.localRotation = Quaternion.Lerp(shieldStartPosition.localRotation, shieldEndPosition.localRotation, rotationCurve.Evaluate(progress));

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        shield.transform.position = shieldEndPosition.position;
        shield.transform.localScale = shieldEndPosition.localScale;
        shield.transform.localRotation = shieldEndPosition.localRotation;

        titleLogo.SetActive(true);
        titleCommandPanel.SetActive(true);
        infoText.SetActive(true);

        openingBGM.Stop();
        mainBGM.Play();
    }

    private void Update()
    {
        if (Input.GetButtonDown("Submit"))
        {
            SkipOpeningAnimation();
        }
    }

    private void SkipOpeningAnimation()
    {
        if (!isOpeningAnimationComplete)
        {
            StopCoroutine(openingAnimationCoroutine);

            shield.transform.position = shieldEndPosition.position;
            shield.transform.localScale = shieldEndPosition.localScale;
            shield.transform.localRotation = shieldEndPosition.localRotation;

            titleLogo.SetActive(true);
            titleCommandPanel.SetActive(true);
            infoText.SetActive(true);

            openingBGM.Stop();
            mainBGM.Play();

            isOpeningAnimationComplete = true;
        }
    }
}
