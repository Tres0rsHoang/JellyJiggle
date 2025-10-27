using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridBoard : MonoBehaviour
{
    private static readonly WaitForSeconds _waitForSeconds1 = new(1f);

    private struct Point2D
    {
        public int X;
        public int Y;

        public Point2D(int y, int x)
        {
            X = x;
            Y = y;
        }
    }

    [SerializeField]
    private GameObject slotPrefab;

    public int width;
    public int height;

    public float spacing = 1.3f;
    public static GridBoard Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    void Start()
    {
        width = GameManager.Instance.data.GetBoardWidth();
        height = GameManager.Instance.data.GetBoardHeight();
        GenerateBoard();
    }

    void GenerateBoard()
    {
        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                Vector3 pos = new(x * spacing, 0, -y * spacing);
                GameObject slot = Instantiate(slotPrefab, pos, Quaternion.identity, transform);
                slot.name = $"Slot_{y}_{x}";
            }
        }
    }

    private void Print2DMap(int[,] map)
    {
        string debugString = "";

        for (int y = 0; y < height * 2; y++)
        {
            string row = "";
            for (int x = 0; x < width * 2; x++)
            {
                row += map[y, x] + " ";
            }
            row += " \n";

            debugString += row;
        }
        Debug.Log(debugString);
    }

    private int[,] Get2DMap()
    {
        int[,] result = new int[height * 2, width * 2];

        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                string colorCode = transform
                    .Find($"Slot_{y}_{x}")
                    .GetComponent<Slot>()
                    .GetColorString();

                int i = y * 2;
                int j = x * 2;

                result[i + 1, j] = colorCode[0] - '0';
                result[i + 1, j + 1] = colorCode[1] - '0';
                result[i, j + 1] = colorCode[2] - '0';
                result[i, j] = colorCode[3] - '0';
            }
        }

        return result;
    }

    private void BoardMapping(int[,] board)
    {
        for (int i = 1; i < height * 2; i += 2)
        {
            for (int j = 0; j < width * 2; j += 2)
            {
                int y = (i - 1) / 2;
                int x = j / 2;

                string colorCode =
                    board[i, j].ToString()
                    + board[i, j + 1].ToString()
                    + board[i - 1, j + 1].ToString()
                    + board[i - 1, j].ToString();

                if (transform.Find($"Slot_{y}_{x}").TryGetComponent(out Slot slot))
                {
                    slot.SetColorString(colorCode);
                }
            }
        }
    }

    public IEnumerator Calculate()
    {
        int[,] board = Get2DMap();
        bool[,] checkedPos = new bool[height * 2, width * 2];

        for (int i = 0; i < height * 2; i++)
        {
            for (int j = 0; j < width * 2; j++)
            {
                checkedPos[i, j] = false;
            }
        }
        bool deleted = false;
        for (int i = 0; i < height * 2; i++)
        {
            for (int j = 0; j < width * 2; j++)
            {
                if (!checkedPos[i, j] && board[i, j] != 0)
                {
                    List<Point2D> tempList = new() { new Point2D(i, j) };
                    DFS(i, j, board, ref checkedPos, ref tempList);
                    if (tempList.Count > 1 && IsValidRemoveList(tempList))
                    {
                        foreach (Point2D point in tempList)
                        {
                            LevelBoard.Instance.CountDecrease(board[point.Y, point.X]);
                            board[point.Y, point.X] = 0;
                        }
                        deleted = true;
                    }
                }
            }
        }

        BoardMapping(board);

        if (deleted)
        {
            yield return _waitForSeconds1;

            yield return Calculate();
        }
        else if (CheckLoseCondition(board))
        {
            PersistentUI.Instance.ShowLoseDialog();
        }

        yield return null;
    }

    bool CheckLoseCondition(int[,] board)
    {
        for (int i = 0; i < height * 2; i++)
        for (int j = 0; j < width * 2; j++)
        {
            if (board[i, j] == 0)
                return false;
        }
        return true;
    }

    bool IsValidRemoveList(List<Point2D> list)
    {
        bool[,] exist = new bool[height, width];

        for (int i = 0; i < height; i++)
        {
            for (int j = 0; j < width; j++)
            {
                exist[i, j] = false;
            }
        }

        foreach (Point2D point in list)
        {
            Point2D temp = GetSlotPos(point);
            exist[temp.Y, temp.X] = true;
        }

        int count = 0;
        for (int i = 0; i < height; i++)
        {
            for (int j = 0; j < width; j++)
            {
                if (exist[i, j])
                    count++;
                if (count > 1)
                    return true;
            }
        }

        return false;
    }

    Point2D GetSlotPos(Point2D point)
    {
        int y = point.Y;
        int x = point.X;

        return new Point2D(y / 2, x / 2);
    }

    void DFS(int y, int x, int[,] board, ref bool[,] checkedPos, ref List<Point2D> removeList)
    {
        checkedPos[y, x] = true;
        if (board[y, x] == 0)
        {
            return;
        }

        if (y - 1 >= 0 && !checkedPos[y - 1, x] && board[y - 1, x] == board[y, x])
        {
            removeList.Add(new Point2D(y - 1, x));
            DFS(y - 1, x, board, ref checkedPos, ref removeList);
        }

        if (y + 1 < height * 2 && !checkedPos[y + 1, x] && board[y + 1, x] == board[y, x])
        {
            removeList.Add(new Point2D(y + 1, x));
            DFS(y + 1, x, board, ref checkedPos, ref removeList);
        }

        if (x - 1 >= 0 && !checkedPos[y, x - 1] && board[y, x - 1] == board[y, x])
        {
            removeList.Add(new Point2D(y, x - 1));
            DFS(y, x - 1, board, ref checkedPos, ref removeList);
        }

        if (x + 1 < width * 2 && !checkedPos[y, x + 1] && board[y, x + 1] == board[y, x])
        {
            removeList.Add(new Point2D(y, x + 1));
            DFS(y, x + 1, board, ref checkedPos, ref removeList);
        }
    }
}
