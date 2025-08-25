using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private Queue<Card> flipQueue = new Queue<Card>();
    private bool isChecking = false;

    private int comboCount = 0;
    private float lastMatchTime;
    private float comboWindow = 10f;

    private int score = 0;

    [Header("UI")]
    public TMP_Text scoreText;
    public TMP_Text comboText;

    void Start()
    {
        UpdateScoreUI();
    }

    public void OnCardFlipped(Card card)
    {
        flipQueue.Enqueue(card);
        if (!isChecking)
            StartCoroutine(ProcessFlips());
    }

    private IEnumerator ProcessFlips()
    {
        isChecking = true;

        while (flipQueue.Count >= 2)
        {
            Card first = flipQueue.Dequeue();
            Card second = flipQueue.Dequeue();

            yield return new WaitForSeconds(0.5f);

            if (first.cardId == second.cardId)
            {
                first.HideCard();
                second.HideCard();

                if (Time.time - lastMatchTime <= comboWindow)
                {
                    comboCount++;
                }
                else
                {
                    comboCount = 1;
                }
                lastMatchTime = Time.time;

                int points = 100 * comboCount;
                score += points;

                Debug.Log("Match! Combo x" + comboCount + " → +" + points + " points");

                UpdateScoreUI();
            }
            else
            {
                first.ShakeCard();
                second.ShakeCard();

                yield return new WaitForSeconds(0.5f);

                StartCoroutine(first.FlipToBack());
                StartCoroutine(second.FlipToBack());

                comboCount = 0;
                UpdateScoreUI();
            }
        }

        isChecking = false;
    }

    private void UpdateScoreUI()
    {
        if (scoreText != null)
            scoreText.text = "Score: " + score;

        if (comboText != null)
        {
            if (comboCount > 1)
                comboText.text = "Combo x" + comboCount + "!";
            else
                comboText.text = "";
        }
    }
}
