using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Threading.Tasks;

public class Card : MonoBehaviour
{
    public int cardId;
    public Sprite frontSprite;
    public Sprite backSprite;

    private Image image;
    private bool isFlipped = false;

    void Start()
    {
        image = GetComponent<Image>();
        image.sprite = backSprite;
        InitCard();
    }

    public async void InitCard()
    {
        await FlipToFront();
        await Utils.WaitForSeconds(2);
        await FlipToBack();
    }

    public async void OnCardClicked()
    {
        if (!isFlipped && gameObject.activeSelf)
        {
            await FlipToFront();
            EventHandler.ExecuteEvent(GameStaticEvents.OnCardFlippedUpdate, this);
        }
    }

    public async Task FlipToFront()
    {
        isFlipped = true;
        await AnimateFlip(backSprite, frontSprite);
    }

    public async Task FlipToBack()
    {
        isFlipped = false;
        await AnimateFlip(frontSprite, backSprite);
    }

    public async Task HideCard()
    {
        await FadeOut();
    }

    public async Task ShakeCard()
    {
        await Shake();
    }

    private async Task AnimateFlip(Sprite fromSprite, Sprite toSprite)
    {
        for (float t = 0; t < 1f; t += Time.deltaTime * 4)
        {
            transform.localScale = new Vector3(Mathf.Lerp(1, 0, t), 1, 1);
            await Task.Yield();
        }

        image.sprite = toSprite;

        for (float t = 0; t < 1f; t += Time.deltaTime * 4)
        {
            transform.localScale = new Vector3(Mathf.Lerp(0, 1, t), 1, 1);
            await Task.Yield();
        }
    }

    private async Task FadeOut()
    {
        CanvasGroup cg = gameObject.AddComponent<CanvasGroup>();
        for (float t = 0; t < 1; t += Time.deltaTime)
        {
            cg.alpha = Mathf.Lerp(1, 0, t);
            await Task.Yield();
        }
        //gameObject.SetActive(false);
    }

    private async Task Shake()
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
            await Task.Yield();
        }

        transform.localPosition = startPos;
    }
}
