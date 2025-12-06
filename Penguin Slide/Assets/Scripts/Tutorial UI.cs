using UnityEngine;
using TMPro;

public class TriggerMessageFade : MonoBehaviour
{
    [Header("UI Text to show for this trigger")]
    public GameObject textObject;

    [Header("Next trigger to enable")]
    public GameObject nextTrigger;

    [Header("Timing")]
    public float totalDuration = 15f;

    public float fadeDuration = 2f;

    [Header("Player settings")]
    public string playerTag = "Player";

    private bool hasTriggered = false;
    private CanvasGroup canvasGroup;

    private void Awake()
    {
        if (textObject != null)
        {
            canvasGroup = textObject.GetComponent<CanvasGroup>();
            if (canvasGroup == null)
            {
                canvasGroup = textObject.AddComponent<CanvasGroup>();
            }
        }
    }

    private void Start()
    {
        if (textObject != null)
        {
            textObject.SetActive(false);
            if (canvasGroup != null)
                canvasGroup.alpha = 0f;
        }

        if (nextTrigger != null)
            nextTrigger.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (hasTriggered) return;
        if (!other.CompareTag(playerTag)) return;

        hasTriggered = true;
        StartCoroutine(ShowAndFadeRoutine());
    }

    // Show the text and fade it out
    private System.Collections.IEnumerator ShowAndFadeRoutine()
    {
        if (textObject != null)
        {
            textObject.SetActive(true);
            if (canvasGroup != null)
                canvasGroup.alpha = 1f;
        }

        float stayTime = Mathf.Max(0f, totalDuration - fadeDuration);

        if (stayTime > 0f)
            yield return new WaitForSeconds(stayTime);

        // Fade out
        if (canvasGroup != null && fadeDuration > 0f)
        {
            float t = 0f;
            float startAlpha = canvasGroup.alpha;

            while (t < fadeDuration)
            {
                t += Time.deltaTime;
                float normalized = Mathf.Clamp01(t / fadeDuration);
                canvasGroup.alpha = Mathf.Lerp(startAlpha, 0f, normalized);
                yield return null;
            }

            canvasGroup.alpha = 0f;
        }

        if (textObject != null)
            textObject.SetActive(false);

        if (nextTrigger != null)
            nextTrigger.SetActive(true);

        Destroy(gameObject);
    }
}
