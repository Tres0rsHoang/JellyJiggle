using UnityEngine;

public class JellyBlockManager : MonoBehaviour
{
    [SerializeField]
    private string colorString;

    [SerializeField]
    private GameObject pivot0;

    [SerializeField]
    private GameObject pivot1;

    [SerializeField]
    private GameObject pivot2;

    [SerializeField]
    private GameObject pivot3;

    void Start()
    {
        if (colorString == null || colorString == "")
        {
            colorString = ColorCodeUtils.GenerateColorCode();
        }
        ColorCodeUtils.StandardlizeColorCode(ref colorString);

        CreateJelly();
        RenderJelly();
    }

    private void CreateJelly()
    {
        bool isOneCube = true;
        for (int index = 0; index < 3; index++)
        {
            if (colorString[index] != colorString[index + 1])
            {
                isOneCube = false;
                break;
            }
        }

        if (isOneCube)
        {
            GameObject jellyPivot = Instantiate(pivot0, transform);
            jellyPivot.name = "Pivot0";
            if (jellyPivot != null)
            {
                jellyPivot
                    .GetComponentInChildren<JellyColor>()
                    .SetColor(ColorCodeUtils.RenderColor(colorString[0]));
            }
            return;
        }

        int i = 0;
        while (i < 4)
        {
            int nextIndex = i == 3 ? 0 : i + 1;
            GameObject jellyPivot = null;

            if (colorString[i] != colorString[nextIndex])
            {
                switch (i)
                {
                    case 0:
                        jellyPivot = Instantiate(pivot0, transform);
                        jellyPivot.name = "Pivot0";
                        break;
                    case 1:
                        jellyPivot = Instantiate(pivot1, transform);
                        jellyPivot.name = "Pivot1";
                        break;
                    case 2:
                        jellyPivot = Instantiate(pivot2, transform);
                        jellyPivot.name = "Pivot2";
                        break;
                    case 3:
                        jellyPivot = Instantiate(pivot3, transform);
                        jellyPivot.name = "Pivot3";
                        break;
                }
            }

            if (jellyPivot != null)
            {
                jellyPivot
                    .GetComponentInChildren<JellyColor>()
                    .SetColor(ColorCodeUtils.RenderColor(colorString[i]));
            }
            i++;
        }
    }

    private void RenderJelly()
    {
        bool isOneCube = true;
        for (int i = 0; i < 3; i++)
        {
            if (colorString[i] != colorString[i + 1])
            {
                isOneCube = false;
                break;
            }
        }

        if (isOneCube)
        {
            for (int i = 0; i < 4; i++)
            {
                if (transform.childCount == 0)
                    break;
                Transform trs = transform.GetChild(0);
                if (trs != null)
                {
                    Destroy(trs.gameObject);
                }
            }

            GameObject jellyPivot = Instantiate(pivot0, transform);
            jellyPivot.name = "Pivot0";
            if (jellyPivot != null)
            {
                jellyPivot
                    .GetComponentInChildren<JellyColor>()
                    .SetColor(ColorCodeUtils.RenderColor(colorString[0]));

                PivotExpanded pivotExpanded = jellyPivot.GetComponent<PivotExpanded>();
                pivotExpanded.ExpandHorizontal();
                pivotExpanded.ExpandVertical();
            }

            return;
        }

        for (int i = 0; i < 4; i++)
        {
            if (transform.Find($"Pivot{i}") == null)
            {
                continue;
            }

            int preIndex = i == 0 ? 3 : i - 1;
            GameObject jellyPivot = transform.Find($"Pivot{i}").gameObject;
            Transform jellyPivotPrevious = transform.Find($"Pivot{preIndex}");

            if (colorString[i] == colorString[preIndex] && jellyPivotPrevious == null)
            {
                switch (i)
                {
                    case 0:
                        jellyPivot.GetComponent<PivotExpanded>().ExpandVertical();
                        break;
                    case 1:
                        jellyPivot.GetComponent<PivotExpanded>().ExpandHorizontal();
                        break;
                    case 2:
                        jellyPivot.GetComponent<PivotExpanded>().ExpandVertical();
                        break;
                    case 3:
                        jellyPivot.GetComponent<PivotExpanded>().ExpandHorizontal();
                        break;
                }
                continue;
            }

            int nextIndex = i == 3 ? 0 : i + 1;
            Transform jellyPivotNext = transform.Find($"Pivot{nextIndex}");

            if (colorString[i] == colorString[nextIndex] && jellyPivotNext == null)
            {
                switch (i)
                {
                    case 0:
                        jellyPivot.GetComponent<PivotExpanded>().ExpandHorizontal();
                        break;
                    case 1:
                        jellyPivot.GetComponent<PivotExpanded>().ExpandVertical();
                        break;
                    case 2:
                        jellyPivot.GetComponent<PivotExpanded>().ExpandHorizontal();
                        break;
                    case 3:
                        jellyPivot.GetComponent<PivotExpanded>().ExpandVertical();
                        break;
                }
                continue;
            }
        }
    }

    public string GetColorString()
    {
        return colorString;
    }

    public void SetColorString(string color)
    {
        for (int i = 0; i < 4; i++)
        {
            Transform pivot = transform.Find($"Pivot{i}");
            if (color[i] == '0' && pivot != null)
            {
                Destroy(pivot.gameObject);
            }
        }
        ColorCodeUtils.StandardlizeColorCode(ref color);
        colorString = color;
    }

    void Update()
    {
        RenderJelly();
    }
}
