using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameplayPanel : ModalUIWindow<GameplayPanel>
{
    public Card cardPrefab;
    public GridLayoutGroup gridLayout;

    private LevelData currentLevelData;

    public TMP_Text scoreText;
    public TMP_Text comboText;
    public TMP_Text turnsText;

    public Button settingsBtn;

    private List<Card> cards = new List<Card>();

    private void OnEnable()
    {
        EventHandler.RegisterEvent(GameStaticEvents.OnUpdateScore,UpdateScoreUI);
        EventHandler.RegisterEvent(GameStaticEvents.OnRestartGame, OnRestart);
    }

    private void OnDisable() 
    {
        EventHandler.UnregisterEvent(GameStaticEvents.OnUpdateScore, UpdateScoreUI);
        EventHandler.UnregisterEvent(GameStaticEvents.OnRestartGame, OnRestart);
    }

    public override ModalUIWindowBase Show()
    {
        settingsBtn.onClick.AddListener(() =>
        {
            EventHandler.ExecuteEvent<ModalUIWindowtype, bool>(GameStaticEvents.OnTogglePanel, ModalUIWindowtype.Settings, true);
        });
        InitBoard(gameManager.levelDatas[gameManager.currentPlayerData.last_level_id].levelData);
        return base.Show();
    }

    public override ModalUIWindowBase Close()
    {
        cards.ForEach(x => Destroy(x.gameObject));
        cards.Clear();
        comboText.text = "";
        return base.Close();
    }

    void OnRestart()
    {
        Close();
    }

    void InitBoard(LevelData data)
    {
        currentLevelData = data;
        gridLayout.spacing = currentLevelData.spacing;

        SetupBoard();
    }

    void AdjustCellSize()
    {
        RectTransform rt = gridLayout.GetComponent<RectTransform>();
        float cellWidth = (rt.rect.width - (currentLevelData.spacing.x * (currentLevelData.columns - 1))) / currentLevelData.columns;
        float cellHeight = (rt.rect.height - (currentLevelData.spacing.y * (currentLevelData.rows - 1))) / currentLevelData.rows;
        gridLayout.cellSize = new Vector2(cellWidth, cellHeight);
    }

    void SetupBoard()
    {
        int totalCards = currentLevelData.rows * currentLevelData.columns;

        List<int> cardIds = new List<int>();

        for (int i = 0; i < totalCards / 2; i++)
        {
            int spriteIndex = i % currentLevelData.cardSprites.Length;
            cardIds.Add(spriteIndex);
            cardIds.Add(spriteIndex);
        }

        for (int i = 0; i < cardIds.Count; i++)
        {
            int temp = cardIds[i];
            int randomIndex = Random.Range(i, cardIds.Count);
            cardIds[i] = cardIds[randomIndex];
            cardIds[randomIndex] = temp;
        }

        cardIds.Shuffle();

        gameManager.totalPairs = totalCards / 2;

        gridLayout.constraint = GridLayoutGroup.Constraint.FixedColumnCount;
        gridLayout.constraintCount = currentLevelData.columns;

        AdjustCellSize();

        foreach (int id in cardIds)
        {
            Card card = Instantiate(cardPrefab, gridLayout.transform); ;
            card.cardId = id;
            card.frontSprite = currentLevelData.cardSprites[id];
            cards.Add(card);
        }
        UpdateScoreUI();
    }

    private void UpdateScoreUI()
    {
        if (scoreText != null)
            scoreText.text = "Score: " + gameManager.score;

        if (comboText != null)
            comboText.text = gameManager.comboCount > 1 ? $"Combo x{gameManager.comboCount}!" : "Combo x0";

        if (turnsText != null)
            turnsText.text = $"Turns: {gameManager.numOfTurns}";
    }

   
}
