using UnityEngine;

public class ColorCodeUtils
{
    public static string GenerateColorCode()
    {
        string result = "";
        for (int i = 0; i < 4; i++)
        {
            int color = Random.Range(0, 5);
            result += color;
        }
        if (result == "0000")
        {
            return GenerateColorCode();
        }
        StandardlizeColorCode(ref result);
        return result;
    }

    public static void StandardlizeColorCode(ref string colorCode)
    {
        char[] chars = colorCode.ToCharArray();
        int count = 0;
        char lastColor = '0';

        foreach (char color in colorCode)
        {
            if (color != '0' && color != lastColor)
            {
                count++;
                lastColor = color;
            }
        }

        if (count == 1)
        {
            for (int i = 0; i < 4; i++)
            {
                chars[i] = lastColor;
            }
            colorCode = new string(chars);
            return;
        }

        for (int i = 0; i < 4; i++)
        {
            if (colorCode[i] == '0')
            {
                int prevIndex = i == 0 ? 3 : i - 1;
                int nextIndex = i == 3 ? 0 : i + 1;

                if (colorCode[prevIndex] == colorCode[nextIndex])
                {
                    chars[i] = colorCode[prevIndex];
                    char differenceColor = colorCode[prevIndex];
                    for (int j = 0; j < 4; j++)
                    {
                        if (colorCode[j] != colorCode[prevIndex])
                        {
                            differenceColor = colorCode[j];
                        }
                    }
                    chars[nextIndex] = differenceColor;
                }
                else if (colorCode[prevIndex] != '0' && colorCode[nextIndex] == '0')
                {
                    chars[i] = colorCode[prevIndex];
                }
                else if (colorCode[prevIndex] == '0' && colorCode[nextIndex] != '0')
                {
                    chars[i] = colorCode[nextIndex];
                }
                else
                {
                    int nextOfNext = nextIndex == 3 ? 0 : nextIndex + 1;
                    int preOfPre = prevIndex == 0 ? 3 : prevIndex - 1;

                    if (colorCode[preOfPre] == colorCode[prevIndex])
                    {
                        chars[i] = colorCode[nextIndex];
                    }
                    else if (colorCode[nextOfNext] == colorCode[nextIndex])
                    {
                        chars[i] = colorCode[prevIndex];
                    }
                    else
                    {
                        chars[i] =
                            Random.Range(0, 1) % 2 == 0
                                ? colorCode[prevIndex]
                                : colorCode[nextIndex];
                    }
                }
            }
        }

        colorCode = new string(chars);

        chars = colorCode.ToCharArray();
        for (int i = 0; i < 4; i++)
        {
            char value = colorCode[i];
            bool valid = true;

            for (int j = 0; j < 3; j++)
            {
                int index = i + j > 3 ? i + j - 4 : i + j;
                if (colorCode[index] != value)
                {
                    break;
                }
                if (j == 2)
                {
                    valid = false;
                }
            }

            if (!valid)
            {
                int index = i + 3 > 3 ? i - 1 : i + 3;
                chars[i] = colorCode[index];
                break;
            }
        }
        colorCode = new string(chars);
    }

    public static Color RenderColor(char color)
    {
        Color result = Color.gray;

        switch (color)
        {
            case '1':
                ColorUtility.TryParseHtmlString("#33BC62", out result);
                break;
            case '2':
                ColorUtility.TryParseHtmlString("#EFD12C", out result);
                break;
            case '3':
                ColorUtility.TryParseHtmlString("#B133FF", out result);
                break;
            case '4':
                ColorUtility.TryParseHtmlString("#22C7D4", out result);
                break;
        }

        return result;
    }
}
