using UnityEngine;
using UnityEngine.UI;

public class FloatingHealthBar : MonoBehaviour
{
    [SerializeField] private GameObject healthBarPrefab; // перетащи префаб сюда
    [SerializeField] private Vector3 offset = new Vector3(0, -2f, 0); // смещение

    private GameObject healthBarInstance;
    private Image fillImage;
    private RectTransform rectTransform;

    void Start()
    {
        Canvas mainCanvas = FindFirstObjectByType<Canvas>();
        if (mainCanvas != null && healthBarPrefab != null)
        {
            healthBarInstance = Instantiate(healthBarPrefab, mainCanvas.transform);
            rectTransform = healthBarInstance.GetComponent<RectTransform>();
            fillImage = healthBarInstance.GetComponent<Image>();
        }
    }

    void Update()
    {
        if (healthBarInstance != null)
        {
            Vector3 worldPos = transform.position + offset;
            Vector2 screenPos = Camera.main.WorldToScreenPoint(worldPos);
            rectTransform.position = screenPos;
        }
    }

    // Этот метод вызывай из твоего скрипта статов, когда персонаж получает урон
    public void SetHealthPercent(float percent)
    {
        if (fillImage != null)
        {
            fillImage.fillAmount = percent;
        }
    }

    void OnDestroy()
    {
        if (healthBarInstance != null)
            Destroy(healthBarInstance);
    }
}