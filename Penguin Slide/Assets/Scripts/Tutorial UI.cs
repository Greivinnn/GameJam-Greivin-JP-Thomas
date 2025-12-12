using UnityEngine;
using TMPro;

public class TriggerMessageFade : MonoBehaviour
{
    public GameObject textObject;
    public GameObject nextTrigger;

    public float totalDuration = 6f;
    public float fadeDuration = 2f;
    public string tagObject = "Player";

    bool hasTriggered = false;
    CanvasGroup canvasGroup;

    void Awake()
    {
        if (textObject != null)
        {
            canvasGroup = textObject.GetComponent<CanvasGroup>();
            if (canvasGroup == null)
                canvasGroup = textObject.AddComponent<CanvasGroup>();
        }
    }

    void Start()
    {
        hasTriggered = false;

        if (textObject != null)
        {
            textObject.SetActive(false);
            if (canvasGroup != null)
                canvasGroup.alpha = 0f;
        }

        if (nextTrigger != null)
            nextTrigger.SetActive(false);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (hasTriggered) return;
        if (!other.CompareTag(tagObject))
        {
            return;
        }

        hasTriggered = true;
        StartCoroutine(ShowAndFadeRoutine());
    }

    System.Collections.IEnumerator ShowAndFadeRoutine()
    {
        if (textObject != null)
            textObject.SetActive(true);

        if (canvasGroup != null)
            canvasGroup.alpha = 1f;

        float stayTime = Mathf.Max(0f, totalDuration - fadeDuration);

        if (stayTime > 0f)
            yield return new WaitForSeconds(stayTime);

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