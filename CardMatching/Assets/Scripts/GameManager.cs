using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private Queue<Card> flipQueue = new Queue<Card>();
    private bool isChecking = false;

    private float lastMatchTime;
    private float comboWindow = 6f;

    [Header("UI")]
    public TMP_Text scoreText;
    public TMP_Text comboText;

    [Header("Level Data")]
    public LevelDataSO[] levelDatas;

    [Header("Player Data")]
    public PlayerData currentPlayerData;

    public int totalPairs;
    public int matchedPairs = 0;
    public int numOfTurns = 0;
    public int score = 0;
    public int comboCount = 0;

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
        CheckPlayerData();
    }
    public void UpdatePlayerlevelData(int level_id)
    {
        if (currentPlayerData != null)
        {
            currentPlayerData.last_level_id = level_id;
        }
    }


    void CheckPlayerData()
    {
        if(LoadPlayerData() != null)
        {
            currentPlayerData = LoadPlayerData();
        }
        else
        {
            currentPlayerData = new PlayerData();
            currentPlayerData.user_id = SystemInfo.deviceUniqueIdentifier;
            currentPlayerData.last_level_id = 0;
            currentPlayerData.levels = new List<PlayerLevelData>();
            currentPlayerData.levels.Add(new PlayerLevelData() { level = 0,score = 0});
            SavePlayerData(currentPlayerData);
        }

        numOfTurns = levelDatas[currentPlayerData.last_level_id].levelData.numOfTurns;

        EventHandler.ExecuteEvent<GameManager>(GameStaticEvents.OnGameManagerDataUpdate, this);
        EventHandler.ExecuteEvent(GameStaticEvents.OnUpdateScore);

    }

    public void TriggerRestartGame()
    {
        score = 0;
        matchedPairs = 0;
        comboCount = 0;
        numOfTurns = levelDatas[currentPlayerData.last_level_id].levelData.numOfTurns;
        EventHandler.ExecuteEvent(GameStaticEvents.OnRestartGame);
        EventHandler.ExecuteEvent(GameStaticEvents.OnStartGame);
    }

    public void TriggerNextRound()
    {
        score = 0;
        comboCount = 0;
        currentPlayerData.last_level_id++;
        currentPlayerData.levels.Add(new PlayerLevelData() { level = currentPlayerData.last_level_id, score = 0 });
        numOfTurns = levelDatas[currentPlayerData.last_level_id].levelData.numOfTurns;
        matchedPairs = 0;
        SavePlayerData(currentPlayerData);
        EventHandler.ExecuteEvent(GameStaticEvents.OnStartGame);
    }

    public void OnCardFlipped(Card card)
    {
        flipQueue.Enqueue(card);
        if (!isChecking)
            _ = ProcessFlips();
    }

    void UpdateTurns()
    {
        numOfTurns--;
        if(numOfTurns <= 0 && matchedPairs < totalPairs)
        {
            Debug.Log("Game Over");
            EventHandler.ExecuteEvent<string>(GameStaticEvents.OnAudioUpdate, "gameOver");
            EventHandler.ExecuteEvent<ModalUIWindowtype, bool>(GameStaticEvents.OnTogglePanel, ModalUIWindowtype.Gameover, true);
        }
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
                EventHandler.ExecuteEvent<string>(GameStaticEvents.OnAudioUpdate, "match");
                await Task.WhenAll(first.HideCard(), second.HideCard());

                if (Time.time - lastMatchTime <= comboWindow)
                    comboCount++;
                else
                    comboCount = 1;

                lastMatchTime = Time.time;

                int points = 100 * comboCount;
                score += points;

                Debug.Log($"Combo x{comboCount} + {points} points");

                UpdateTurns();
                EventHandler.ExecuteEvent(GameStaticEvents.OnUpdateScore);
                matchedPairs++;


                if(matchedPairs >= totalPairs)
                {
                    Debug.Log("Round Complete");
                    EventHandler.ExecuteEvent<string>(GameStaticEvents.OnAudioUpdate, "gameOver");
                    currentPlayerData.levels[currentPlayerData.last_level_id].score = score;
                    SavePlayerData(currentPlayerData);
                    EventHandler.ExecuteEvent<ModalUIWindowtype, bool>(GameStaticEvents.OnTogglePanel, ModalUIWindowtype.Gameover, true);
                    break;
                }
            }
            else
            {
                EventHandler.ExecuteEvent<string>(GameStaticEvents.OnAudioUpdate, "mismatch");
                await Task.WhenAll(first.ShakeCard(), second.ShakeCard());

                await Utils.WaitForSeconds(0.5f);

                await Task.WhenAll(first.FlipToBack(), second.FlipToBack());

                comboCount = 0;
                UpdateTurns();
                EventHandler.ExecuteEvent(GameStaticEvents.OnUpdateScore);
            }
        }

        isChecking = false;
    }

    private PlayerData LoadPlayerData()
    {
        PlayerData playerData;
        Utils.TryLoad<PlayerData>($"{SystemInfo.deviceUniqueIdentifier}.json", out playerData);
        if(playerData != null)
            return playerData;
        return null;
    }

    private void SavePlayerData(PlayerData playerData)
    {
        if(playerData != null)
            Utils.TrySave<PlayerData>(playerData, $"{playerData.user_id}.json");
    }
}

[Serializable]
public class PlayerData
{
    public string user_id;
    public int last_level_id;
    public List<PlayerLevelData> levels;
}

[Serializable]
public class PlayerLevelData
{
    public int level;
    public int score;
}
