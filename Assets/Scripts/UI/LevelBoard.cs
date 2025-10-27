using TMPro;
using UnityEngine;

public class LevelBoard : MonoBehaviour
{
    private TextMeshProUGUI levelName;
    private Transform countList;

    [SerializeField]
    private GameObject countPrefab;

    public static LevelBoard Instance { get; private set; }

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
        levelName = transform.Find("LevelName").GetComponent<TextMeshProUGUI>();
        countList = transform.Find("CountList");
        Reload();
    }

    public void Reload()
    {
        levelName.text = GameManager.Instance.data.GetLevelName();
        int[] colorsCount = GameManager.Instance.data.colorsCount;

        for (int i = 0; i < 4; i++)
        {
            if (colorsCount[i] == 0)
            {
                continue;
            }

            Transform countObject = countList.Find($"Count_{i}");
            if (countObject == null)
            {
                countObject = Instantiate(countPrefab, countList).transform;
                countObject.name = $"Count_{i}";
            }
            if (countObject.TryGetComponent(out Count count))
            {
                count.SetColor(ColorCodeUtils.RenderColor((char)(i + 1 + '0')));
                count.SetCount(colorsCount[i]);
            }
        }
        PersistentUI.Instance.HideWinDialog();
        PersistentUI.Instance.HideLoseDialog();
    }

    public void CountDecrease(int colorCode)
    {
        Transform trns = countList.Find($"Count_{colorCode - 1}");

        if (trns != null)
        {
            Count count = trns.GetComponent<Count>();
            count.SetCount(count.GetCount() - 1);
            if (count.GetCount() == 0)
            {
                Destroy(count.gameObject);
            }
        }
    }

    void Update()
    {
        if (countList.childCount == 0)
        {
            PersistentUI.Instance.ShowWinDialog();
        }
    }
}
