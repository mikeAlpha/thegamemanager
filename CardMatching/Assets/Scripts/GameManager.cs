using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private Queue<Card> flipQueue = new Queue<Card>();
    private bool isChecking = false;

    private int comboCount = 0;
    private float lastMatchTime;
    private float comboWindow = 3f;

    private int score = 0;

    [Header("UI")]
    public TMP_Text scoreText;
    public TMP_Text comboText;

    [Header("Level Data")]
    public LevelDataSO[] levelDatas;

    private void OnEnable()
    {
        EventHandler.RegisterEvent<Card>(GameStaticEvents.OnCardFlippedUpdate, OnCardFlipped);
    }

    private void OnDisable()
    {
        EventHandler.UnregisterEvent<Card>(GameStaticEvents.OnCardFlippedUpdate, OnCardFlipped);
    }

    void Start()
    {
        UpdateScoreUI();
        EventHandler.ExecuteEvent(GameStaticEvents.OnBoardManagerDataUpdate, levelDatas[0].levelData);
    }

    public void OnCardFlipped(Card card)
    {
        flipQueue.Enqueue(card);
        if (!isChecking)
            _ = ProcessFlips();
    }

    private async Task ProcessFlips()
    {
        isChecking = true;

        while (flipQueue.Count >= 2)
        {
            Card first = flipQueue.Dequeue();
            Card second = flipQueue.Dequeue();

            await Utils.WaitForSeconds(0.5f);

            if (first.cardId == second.cardId)
            {
                await Task.WhenAll(first.HideCard(), second.HideCard());

                if (Time.time - lastMatchTime <= comboWindow)
                    comboCount++;
                else
                    comboCount = 1;

                lastMatchTime = Time.time;

                int points = 100 * comboCount;
                score += points;

                Debug.Log($"Match! Combo x{comboCount} → +{points} points");

                UpdateScoreUI();
            }
            else
            {
                await Task.WhenAll(first.ShakeCard(), second.ShakeCard());

                await Utils.WaitForSeconds(0.5f);

                await Task.WhenAll(first.FlipToBack(), second.FlipToBack());

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
            comboText.text = comboCount > 1 ? $"Combo x{comboCount}!" : "";
    }
}
