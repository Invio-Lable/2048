using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Tile : MonoBehaviour
{
    public TileState state { get; private set; }
    public TileCell cell { get; private set; }
    public int number { get; private set; }
    public bool locked { get; set; }

    private Image background;
    private TextMeshProUGUI text;
    private Image animalImage; // Новий компонент для спрайту тварини
    private bool useAnimalMode = false; // Локальний стан режиму (буде синхронізуватися з глобальним)

    private void Awake() 
    {
        background = GetComponent<Image>();
        text = GetComponentInChildren<TextMeshProUGUI>();
        animalImage = transform.Find("AnimalImage")?.GetComponent<Image>(); // Знаходимо Image для тварини
        if (animalImage == null)
        {
            Debug.LogError("AnimalImage not found in Tile prefab!");
        }
    }

    public void SetState(TileState state, int number)
    {
        this.state = state;
        this.number = number;

        background.color = state.backgroundColor;
        UpdateDisplay(); // Оновлюємо відображення залежно від режиму
    }

    public void SetAnimalMode(bool useAnimals)
    {
        useAnimalMode = useAnimals;
        UpdateDisplay(); // Оновлюємо відображення при зміні режиму
    }

    private void UpdateDisplay()
    {
        if (useAnimalMode && state.animalSprite != null)
        {
            // Режим тварин: показуємо спрайт, ховаємо текст
            text.gameObject.SetActive(false);
            animalImage.gameObject.SetActive(true);
            animalImage.sprite = state.animalSprite;
            animalImage.color = Color.white; // Переконайся, що спрайт видимий
        }
        else
        {
            // Режим чисел: показуємо текст, ховаємо спрайт
            text.gameObject.SetActive(true);
            animalImage.gameObject.SetActive(false);
            text.color = state.textColor;
            text.text = number.ToString();
        }
    }

    public void Spawn(TileCell cell)
    {
        if (this.cell != null)
        {
            this.cell.tile = null;
        }

        this.cell = cell;
        this.cell.tile = this;

        transform.position = cell.transform.position;
    }

    public void MoveTo(TileCell cell) 
    {
        if (this.cell != null)
        {
            this.cell.tile = null;
        }

        this.cell = cell;
        this.cell.tile = this;

        StartCoroutine(Animate(cell.transform.position, false));
    }

    public void Merge(TileCell cell)
    {
        if (this.cell != null)
        {
            this.cell.tile = null;
        }

        this.cell = null;
        cell.tile.locked = true;

        StartCoroutine(Animate(cell.transform.position, true));
    }

    private IEnumerator Animate(Vector3 to, bool merging) 
    {
        float elapsed = 0f;
        float duration = 0.1f;

        Vector3 from = transform.position;

        while (elapsed < duration)
        {
            transform.position = Vector3.Lerp(from, to, elapsed / duration);
            elapsed += Time.deltaTime;
            yield return null;
        }

        transform.position = to;

        if (merging)
        {
            Destroy(gameObject);
        }
    }
}