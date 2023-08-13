using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance;

    public AudioSource audioSource;
    public List<AudioClip> clips;
    private Dictionary<string, AudioClip> clipDictionary;

    void Awake()
    {
        // Ensure only one instance of SoundManager exists
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        // Initialize the audio source
        audioSource = GetComponent<AudioSource>();

        // Initialize the dictionary and add clips to it
        clipDictionary = new Dictionary<string, AudioClip>();
        foreach (var clip in clips)
        {
            clipDictionary.Add(clip.name, clip);
        }
    }

    public void PlaySound(string clipName)
    {
        if (clipDictionary.ContainsKey(clipName))
        {
            audioSource.PlayOneShot(clipDictionary[clipName]);
        }
        else
        {
            Debug.LogError("No clip with the name " + clipName + " found!");
        }
    }
}
