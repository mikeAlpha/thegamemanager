using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

[RequireComponent(typeof(GridLayoutGroup))]
public class BoardManager : MonoBehaviour
{
    public GameObject cardPrefab;

    private LevelData currentLevelData;
    //public Sprite[] cardSprites;

    //[Header("Grid Settings")]
    //public int rows = 4;
    //public int columns = 4;
    //public Vector2 spacing = new Vector2(10, 10);

    private GridLayoutGroup gridLayout;

    private void OnEnable()
    {
        EventHandler.RegisterEvent<LevelData>(GameStaticEvents.OnBoardManagerDataUpdate, InitBoard);
    }

    private void OnDisable()
    {
        EventHandler.UnregisterEvent<LevelData>(GameStaticEvents.OnBoardManagerDataUpdate, InitBoard);
    }

    void InitBoard(LevelData data)
    {
        currentLevelData = data;
        gridLayout = GetComponent<GridLayoutGroup>();
        gridLayout.spacing = currentLevelData.spacing;

        SetupBoard();
    }

    void AdjustCellSize()
    {
        RectTransform rt = GetComponent<RectTransform>();
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

        gridLayout.constraint = GridLayoutGroup.Constraint.FixedColumnCount;
        gridLayout.constraintCount = currentLevelData.columns;

        AdjustCellSize();

        foreach (int id in cardIds)
        {
            GameObject cardObj = Instantiate(cardPrefab, transform);
            Card card = cardObj.GetComponent<Card>();
            card.cardId = id;
            card.frontSprite = currentLevelData.cardSprites[id];
        }
    }
}
