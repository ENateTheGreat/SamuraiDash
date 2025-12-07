/* Author: E. Nathan Lee
 * Date: 12/7/2025
 * Description: The Music Manager handles background music and soundeffects
 *              in a singleton pattern, persisting across scenes.
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MusicManager : MonoBehaviour
{
    //===================
    // Singleton Instance
    //===================
    public static MusicManager Instance;

    //============
    // Music Clips
    //============
    [Header("Clips By Music Scene")]
    [SerializeField] private AudioClip mainMenuMusic;
    [SerializeField] private AudioClip level1Music;
    [SerializeField] private AudioClip level2Music;

    //==============
    // Audio Sources
    //==============
    public AudioSource musicSource;
    public AudioSource sfxSource;

    //========================================
    // Singleton management with music changes
    //========================================
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        if (musicSource == null)
        {
            musicSource = gameObject.AddComponent<AudioSource>();
        }
        musicSource.loop = true;
        musicSource.playOnAwake = false;
        musicSource.spatialBlend = 0f;

        if (sfxSource == null)
        {
            sfxSource = gameObject.AddComponent<AudioSource>();
        }
        sfxSource.loop = false;
        sfxSource.playOnAwake = false;
        sfxSource.spatialBlend = 0f;

        SceneManager.sceneLoaded += OnSceneLoaded;
        OnSceneLoaded(SceneManager.GetActiveScene(), LoadSceneMode.Single);
    }

    // Cleanup
    private void OnDestroy()
    {
        if (Instance == this)
        {
            SceneManager.sceneLoaded -= OnSceneLoaded;
        }
    }

    // Start playing music for the loaded scene
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (sfxSource != null)
        {
            sfxSource.Stop();
        }
        AudioClip clipToPlay = GetClipForScene(scene.name);
        if (clipToPlay != null && musicSource.clip != clipToPlay)
        {
            musicSource.clip = clipToPlay;
            musicSource.Play();
        }
    }

    // Audio clup selection based on scene name
    private AudioClip GetClipForScene(string sceneName)
    {
        switch (sceneName)
        {
            case "MainMenu":
                return mainMenuMusic;
            case "Level_1":
                return level1Music;
            case "Level_2":
                return level2Music;
            default:
                return mainMenuMusic; // Level 3 music
        }
    }

    // Once shot logic for sfx
    public void PlaySfx(AudioClip clip)
    {
               sfxSource.PlayOneShot(clip);
    }

    // Pasue and resume music
    public void PauseMusic()
    {
        musicSource.Pause();
    }

    public void ResumeMusic()
    {
        musicSource.UnPause();
    }
}
