using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class TitleScreenManager : MonoBehaviour
{
    [Header("Splash Screens")]
    public Image SplashScreen1, SplashScreen2;
    public GameObject TitleMenu, SplashScreenMask, BGM, BGS, ClockBGS, CardOrbitHolder;
    public GameObject TrickScreen, ToriaPre, RitualMass, RitualGarden, OrbitDealer, RitualMass2, ToriaPost;

    [Header("Fade Settings")]
    [Tooltip("Time taken for the splash screen to fully appear.")]
    public float fadeInDuration = 1f;
    [Tooltip("Time taken for the splash screen to fully disappear.")]
    public float fadeOutDuration = 1f;
    [Tooltip("How long to wait on each splash screen before starting the fade out.")]
    public float displayDuration = 2f;

    public float fadeSpeed = 1.0f;
    public float waitTimeBetweenSplash = 1.0f;
    public float shortFlickerDuration = 0.2f;  // You can adjust this for ToriaPre & ToriaPost flickering
    
    private void Start()
    {
        StartCoroutine(FadeInAndOutSplashScreen(SplashScreen1));
        StartCoroutine(FadeInAndOutSplashScreen(SplashScreen2, fadeInDuration + displayDuration + fadeOutDuration));
    }

    private IEnumerator FadeInAndOutSplashScreen(Image splashImage, float delay = 0f)
    {
        if (delay > 0f)
            yield return new WaitForSeconds(delay);

        // Fade in.
        float timer = 0;
        while (timer <= fadeInDuration)
        {
            timer += Time.deltaTime;
            splashImage.color = new Color(1, 1, 1, Mathf.Lerp(0, 1, timer / fadeInDuration));
            yield return null;
        }

        // Wait for displayDuration.
        yield return new WaitForSeconds(displayDuration);

        // Fade out.
        timer = 0;
        while (timer <= fadeOutDuration)
        {
            timer += Time.deltaTime;
            splashImage.color = new Color(1, 1, 1, Mathf.Lerp(1, 0, timer / fadeOutDuration));
            yield return null;
        }

        // Check if it's the last splash screen to continue with the other instructions.
        if(splashImage == SplashScreen2)
        {
            ContinueWithOtherTasks();
        }
    }

    private void ContinueWithOtherTasks()
    {
        TitleMenu.SetActive(true);  // Activate Title Menu.
        
        // Assuming SplashScreenMask has an Image component, we fade it out.
        StartCoroutine(FadeOutMask());

        // Activate BGM
        BGM.SetActive(true);
    }

    private IEnumerator FadeOutMask()
    {
        Image maskImage = SplashScreenMask.GetComponent<Image>();
        float timer = 0;
        while (timer <= fadeOutDuration)
        {
            timer += Time.deltaTime;
            maskImage.color = new Color(1, 1, 1, Mathf.Lerp(1, 0, timer / fadeOutDuration));
            yield return null;
        }
        SplashScreenMask.SetActive(false);
    }

    public void BeginAnew()
    {
        StartCoroutine(PlaySequence());
    }

    private IEnumerator PlaySequence()
    {
        // 1. Deactivate BGM & BGS
        BGM.SetActive(false);
        BGS.SetActive(false);

        // Activate ClockBGS
        ClockBGS.SetActive(true);

        // 2. Activate TrickScreen
        TrickScreen.SetActive(true);

        // 3. Wait for seven seconds
        yield return new WaitForSeconds(7f);

        // 4. Activate ToriaPre for a fraction of a second, then Deactivate
        ToriaPre.SetActive(true);
        yield return new WaitForSeconds(shortFlickerDuration);
        ToriaPre.SetActive(false);

        // 5. Wait for two seconds
        yield return new WaitForSeconds(2f);

        // 6. Activate RitualMass
        RitualMass.SetActive(true);

        // 7. Wait two seconds
        yield return new WaitForSeconds(2f);

        // 8. Activate Ritual Garden
        RitualGarden.SetActive(true);

        // 9. Activate Orbit Dealer
        OrbitDealer.SetActive(true);

        // 10. Wait four seconds
        yield return new WaitForSeconds(4f);

        // 11. Activate RitualMass2 for one second, then deactivate
        RitualMass2.SetActive(true);
        yield return new WaitForSeconds(1f);
        RitualMass2.SetActive(false);

        // 12. Wait two seconds
        yield return new WaitForSeconds(2f);

        // Deactivate CardOrbitHolder
        CardOrbitHolder.SetActive(false);

        // 13. Activate Toria Pre again, hold for two seconds
        ToriaPre.SetActive(true);
        yield return new WaitForSeconds(2f);

        // 14. Activate Toria Post for a fraction of a second, then deactivate, then repeat for a flickering effect
        for (int i = 0; i < 5; i++) // Assuming you want it to flicker 5 times
        {
            ToriaPost.SetActive(true);
            yield return new WaitForSeconds(shortFlickerDuration);
            ToriaPost.SetActive(false);
            yield return new WaitForSeconds(shortFlickerDuration);
        }

        // 15. Deactivate ALL Game Objects in reference
        BGM.SetActive(false);
        BGS.SetActive(false);
        TrickScreen.SetActive(false);
        ToriaPre.SetActive(false);
        RitualMass.SetActive(false);
        RitualGarden.SetActive(false);
        OrbitDealer.SetActive(false);
        RitualMass2.SetActive(false);
        ToriaPost.SetActive(false);
        ClockBGS.SetActive(false);
    }

    private IEnumerator FirstTime()
    {
	// Activate Resolution Selector...
	yield return null;
    }
}
