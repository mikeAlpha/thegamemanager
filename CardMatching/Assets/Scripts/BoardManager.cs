using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

[RequireComponent(typeof(GridLayoutGroup))]
public class BoardManager : MonoBehaviour
{
    public GameObject cardPrefab;
    public Sprite[] cardSprites;

    [Header("Grid Settings")]
    public int rows = 4;
    public int columns = 4;
    public Vector2 spacing = new Vector2(10, 10);

    private GridLayoutGroup gridLayout;

    void Start()
    {
        gridLayout = GetComponent<GridLayoutGroup>();
        gridLayout.spacing = spacing;

        SetupBoard();
    }

    //void Update()
    //{
    //    AdjustCellSize();
    //}

void AdjustCellSize()
{
    RectTransform rt = GetComponent<RectTransform>();
    float cellWidth = (rt.rect.width - (spacing.x * (columns - 1))) / columns;
    float cellHeight = (rt.rect.height - (spacing.y * (rows - 1))) / rows;
    gridLayout.cellSize = new Vector2(cellWidth, cellHeight);
}

    void SetupBoard()
    {
        int totalCards = rows * columns;
        List<int> cardIds = new List<int>();

        for (int i = 0; i < totalCards / 2; i++)
        {
            int spriteIndex = i % cardSprites.Length;
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

        gridLayout.constraint = GridLayoutGroup.Constraint.FixedColumnCount;
        gridLayout.constraintCount = columns;

        RectTransform rt = GetComponent<RectTransform>();
        float cellWidth = (rt.rect.width - (spacing.x * (columns - 1))) / columns;
        float cellHeight = (rt.rect.height - (spacing.y * (rows - 1))) / rows;
        gridLayout.cellSize = new Vector2(cellWidth, cellHeight);

        foreach (int id in cardIds)
        {
            GameObject cardObj = Instantiate(cardPrefab, transform);
            Card card = cardObj.GetComponent<Card>();
            card.cardId = id;
            card.frontSprite = cardSprites[id];
        }
    }
}
