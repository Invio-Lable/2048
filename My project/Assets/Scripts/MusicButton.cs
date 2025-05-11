using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MusicButton : MonoBehaviour
{
    [SerializeField] private MusicManager musicManager;
    [SerializeField] private Image iconImage; // Компонент для іконки
    [SerializeField] private Sprite musicOnSprite; // Спрайт для увімкненої музики
    [SerializeField] private Sprite musicOffSprite; // Спрайт для вимкненої музики
    private TextMeshProUGUI buttonText; // Залишаємо для тексту (опціонально)

    private void Awake()
    {
        buttonText = GetComponentInChildren<TextMeshProUGUI>(); // Може бути null, якщо текст видалено
        iconImage = transform.Find("Icon")?.GetComponent<Image>();
        if (iconImage == null)
        {
            Debug.LogError("Icon Image not found in MusicButton!");
        }
        UpdateButtonDisplay();
    }

    public void OnButtonClick()
    {
        musicManager.ToggleMusic();
        UpdateButtonDisplay();
    }

    private void UpdateButtonDisplay()
    {
        bool isMusicEnabled = musicManager.IsMusicEnabled();
        iconImage.sprite = isMusicEnabled ? musicOnSprite : musicOffSprite;
        if (buttonText != null)
        {
            buttonText.text = isMusicEnabled ? "Mute Music" : "Unmute Music";
        }
    }
}