using UnityEngine;
using UnityEngine.UI;
using System;
using System.Threading.Tasks;

public class Tile : MonoBehaviour
{
    public int IconID { get; private set; }
    public int Layer { get; private set; }
    public RectTransform RectTransform { get; private set; }

    [SerializeField] private Image background;
    [SerializeField] private Image iconImage;
    [SerializeField] private Button button;

    private bool isExposed = true;
    public Action<Tile> OnTileTapped;

    public void Initialize(int iconID, int layer, Sprite baseSprite, Sprite iconSprite)
    {
        IconID = iconID;
        Layer = layer;
        background.sprite = baseSprite;
        iconImage.sprite = iconSprite;
        RectTransform = GetComponent<RectTransform>();
        
        button.onClick.AddListener(HandleClick);
    }

    public void SetExposed(bool exposed)
    {
        isExposed = exposed;
        Color color = exposed ? Color.white : new Color(0.3f, 0.3f, 0.3f, 1f);
        background.color = color;
        iconImage.color = color;
        button.interactable = exposed;
    }

    private async void HandleClick()
    {
        if (isExposed)
        {
            AudioManager.Instance.PlayTap();
            await AnimateTap();
            OnTileTapped?.Invoke(this);
        }
    }

    private async Task AnimateTap()
    {
        Vector3 originalScale = transform.localScale;
        Vector3 targetScale = originalScale * 0.9f;
        float duration = 0.1f;
        float elapsed = 0;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            transform.localScale = Vector3.Lerp(originalScale, targetScale, elapsed / duration);
            await Task.Yield();
        }
        
        elapsed = 0;
        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            transform.localScale = Vector3.Lerp(targetScale, originalScale, elapsed / duration);
            await Task.Yield();
        }
        transform.localScale = originalScale;
    }

    public async Task MoveToRack(Vector3 targetPosition, float duration)
    {
        button.interactable = false;
        Vector3 startPos = transform.position;
        Vector3 startScale = transform.localScale;
        Vector3 endScale = Vector3.one * 0.8f;
        float elapsed = 0;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / duration;
            t = t * t * (3f - 2f * t);
            transform.position = Vector3.Lerp(startPos, targetPosition, t);
            transform.localScale = Vector3.Lerp(startScale, endScale, t);
            await Task.Yield();
        }

        transform.position = targetPosition;
        transform.localScale = endScale;
    }
}
