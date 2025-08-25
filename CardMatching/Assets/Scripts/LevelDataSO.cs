using System;
using UnityEngine;

[CreateAssetMenu(fileName = "New Level Data",menuName = "Level Utils/Create Level Data")]
public class LevelDataSO : ScriptableObject
{
    public LevelData levelData;
}

[Serializable]
public class LevelData
{
    [Header("Sprite refs")]
    public Sprite[] cardSprites;

    [Header("Grid Settings")]
    public int rows = 4;
    public int columns = 4;
    public Vector2 spacing = new Vector2(10, 10);
}