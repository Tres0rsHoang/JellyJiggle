using UnityEngine;

public class PersistentUI : MonoBehaviour
{
    public static PersistentUI Instance;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void ShowWinDialog()
    {
        transform.Find("WinDialog").gameObject.SetActive(true);
    }

    public void HideWinDialog()
    {
        transform.Find("WinDialog").gameObject.SetActive(false);
    }

    public void NextLevel()
    {
        GameManager.Instance.NextScene();
    }

    public void ReloadLevel()
    {
        GameManager.Instance.ReloadScene();
    }

    public void ShowLoseDialog()
    {
        transform.Find("LoseDialog").gameObject.SetActive(true);
    }

    public void HideLoseDialog()
    {
        transform.Find("LoseDialog").gameObject.SetActive(false);
    }
}
