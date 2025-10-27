using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Count : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI count;

    [SerializeField]
    private RawImage image;

    public void SetCount(int number)
    {
        count.text = number.ToString();
    }

    public void SetColor(Color color)
    {
        image.color = color;
    }

    public int GetCount()
    {
        return int.Parse(count.text);
    }
}
