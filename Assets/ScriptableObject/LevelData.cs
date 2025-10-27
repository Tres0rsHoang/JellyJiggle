using UnityEngine;

[CreateAssetMenu(fileName = "LevelData", menuName = "Custom/Level")]
public class LevelData : ScriptableObject
{
    [SerializeField]
    private int boardHeight = 2;

    [SerializeField]
    private int boardWidth = 2;

    [SerializeField]
    private int jellyCreator = 1;

    [SerializeField]
    private string levelName;

    public int[] colorsCount = new int[4];

    [SerializeField]
    private LevelData nextLevel;

    public LevelData GetNextLevel()
    {
        return nextLevel;
    }

    public int GetBoardWidth()
    {
        return boardWidth;
    }

    public int GetBoardHeight()
    {
        return boardHeight;
    }

    public int GetJellyCreator()
    {
        return jellyCreator;
    }

    public string GetLevelName()
    {
        return levelName;
    }
}
