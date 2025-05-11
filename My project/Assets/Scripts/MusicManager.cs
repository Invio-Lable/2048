using UnityEngine;
using System.Collections.Generic;

public class MusicManager : MonoBehaviour
{
    [SerializeField] private List<AudioClip> musicTracks; // Список треків
    [SerializeField] private AudioSource audioSource; // Компонент AudioSource
    private bool isMusicEnabled = true; // Стан музики

    private void Awake()
    {
        // Переконуємося, що об’єкт не знищується при зміні сцени
        DontDestroyOnLoad(gameObject);

        // Завантажуємо стан музики з PlayerPrefs
        isMusicEnabled = PlayerPrefs.GetInt("MusicEnabled", 1) == 1;
        audioSource = GetComponent<AudioSource>();
        audioSource.loop = false; // Вимикаємо зациклення, бо ми керуємо відтворенням вручну
    }

    private void Start()
    {
        if (isMusicEnabled)
        {
            PlayRandomTrack();
        }
    }

    private void Update()
    {
        // Якщо музика увімкнена і поточний трек закінчився, відтворюємо новий
        if (isMusicEnabled && !audioSource.isPlaying)
        {
            PlayRandomTrack();
        }
    }

    private void PlayRandomTrack()
    {
        if (musicTracks.Count == 0) return;

        // Вибираємо випадковий трек
        int randomIndex = Random.Range(0, musicTracks.Count);
        audioSource.clip = musicTracks[randomIndex];
        audioSource.Play();
    }

    public void ToggleMusic()
    {
        isMusicEnabled = !isMusicEnabled;
        PlayerPrefs.SetInt("MusicEnabled", isMusicEnabled ? 1 : 0);
        PlayerPrefs.Save();

        if (isMusicEnabled)
        {
            PlayRandomTrack();
        }
        else
        {
            audioSource.Stop();
        }
    }

    public bool IsMusicEnabled()
    {
        return isMusicEnabled;
    }
}