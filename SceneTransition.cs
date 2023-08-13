using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using UnityEngine.UI;

public class SceneTransition : MonoBehaviour
{
    public Image image;
    public List<Sprite> closeSceneSprites;
    public List<Sprite> openSceneSprites;
    public float frameDuration = 0.1f;
    [SerializeField] private string combatSceneName = "COMBAT_TEST";

    private Camera mainCamera;

    private void Start()
    {
        mainCamera = Camera.main;
    }

    public void CloseScene()
    {
        StartCoroutine(Animate(closeSceneSprites));
    }

    public void OpenScene()
    {
        StartCoroutine(Animate(openSceneSprites));
    }

    public void LoadBattle()
    {
        mainCamera.enabled = false;
        image.canvas.enabled = false;

        SceneManager.LoadScene(combatSceneName, LoadSceneMode.Additive);
    }

    public IEnumerator Animate(List<Sprite> sprites)
    {
        foreach (Sprite sprite in sprites)
        {
            image.sprite = sprite;
            yield return new WaitForSeconds(frameDuration);
        }
    }
}
