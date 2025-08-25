using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Card : MonoBehaviour
{
    public int cardId;
    public Sprite frontSprite;
    public Sprite backSprite;

    private Image image;
    private bool isFlipped = false;
    private GameManager gameManager;

    void Start()
    {
        image = GetComponent<Image>();
        image.sprite = backSprite;
        gameManager = FindObjectOfType<GameManager>();
        StartCoroutine(InitCard());
    }

    IEnumerator InitCard()
    {
        StartCoroutine(FlipToFront());
        yield return new WaitForSeconds(2);
        StartCoroutine(FlipToBack());
    }

    public void OnCardClicked()
    {
        if (!isFlipped && gameObject.activeSelf)
        {
            StartCoroutine(FlipToFront());
            gameManager.OnCardFlipped(this);
        }
    }

    public IEnumerator FlipToFront()
    {
        isFlipped = true;
        yield return AnimateFlip(backSprite, frontSprite);
    }

    public IEnumerator FlipToBack()
    {
        isFlipped = false;
        yield return AnimateFlip(frontSprite, backSprite);
    }

    public void HideCard()
    {
        StartCoroutine(FadeOut());
    }

    public void ShakeCard()
    {
        StartCoroutine(Shake());
    }

    private IEnumerator AnimateFlip(Sprite fromSprite, Sprite toSprite)
    {
        for (float t = 0; t < 0.5f; t += Time.deltaTime * 4)
        {
            transform.localScale = new Vector3(Mathf.Lerp(1, 0, t), 1, 1);
            yield return null;
        }

        image.sprite = toSprite;

        for (float t = 0; t < 1f; t += Time.deltaTime * 4)
        {
            transform.localScale = new Vector3(Mathf.Lerp(0, 1, t), 1, 1);
            yield return null;
        }
    }

    private IEnumerator FadeOut()
    {
        CanvasGroup cg = gameObject.AddComponent<CanvasGroup>();
        for (float t = 0; t < 1; t += Time.deltaTime)
        {
            cg.alpha = Mathf.Lerp(1, 0, t);
            yield return null;
        }
        //gameObject.SetActive(false);
    }

    private IEnumerator Shake()
    {
        Vector3 startPos = transform.localPosition;
        float shakeTime = 0.3f;
        float elapsed = 0f;

        while (elapsed < shakeTime)
        {
            float x = Random.Range(-5f, 5f);
            float y = Random.Range(-5f, 5f);
            transform.localPosition = startPos + new Vector3(x, y, 0);

            elapsed += Time.deltaTime;
            yield return null;
        }

        transform.localPosition = startPos;
    }
}
