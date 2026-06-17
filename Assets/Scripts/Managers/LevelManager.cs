using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    public static LevelManager instance;
    private CanvasGroup canvasGroup;
    [SerializeField] private float fadeDuration;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        StartCoroutine(FadeToTransparent());
    }

    public void RestartLevel()
    {
        StartCoroutine(FadeToBlack(SceneManager.GetActiveScene().buildIndex));
    }

    public void LoadLevel(string sceneName)
    {
        StartCoroutine(FadeToBlack(sceneName));
    }

    private IEnumerator FadeToTransparent()
    {
        float time = 0;
        float startAlpha = canvasGroup.alpha;
        float endAlpha = 0;

        while (time < fadeDuration)
        {
            time += Time.deltaTime;
            canvasGroup.alpha = Mathf.Lerp(startAlpha, endAlpha, time / fadeDuration);
            yield return null;
        }
        canvasGroup.alpha = endAlpha;
    }

    private IEnumerator FadeToBlack(string sceneName)
    {
        float time = 0;
        float startAlpha = canvasGroup.alpha;
        float endAlpha = 1;

        while (time < fadeDuration)
        {
            time += Time.deltaTime;
            canvasGroup.alpha = Mathf.Lerp(startAlpha, endAlpha, time / fadeDuration);
            yield return null;
        }
        canvasGroup.alpha = endAlpha;
        SceneManager.LoadScene(sceneName);
    }

    private IEnumerator FadeToBlack(int buildIndex)
    {
        float time = 0;
        float startAlpha = canvasGroup.alpha;
        float endAlpha = 1;

        while (time < fadeDuration)
        {
            time += Time.deltaTime;
            canvasGroup.alpha = Mathf.Lerp(startAlpha, endAlpha, time / fadeDuration);
            yield return null;
        }
        canvasGroup.alpha = endAlpha;
        SceneManager.LoadScene(buildIndex);
    }
}
