using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public LevelData data;

    public static GameManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void NextScene()
    {
        if (data.GetNextLevel() == null)
        {
            return;
        }
        data = data.GetNextLevel();

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        LevelBoard.Instance.Reload();
        PersistentUI.Instance.HideWinDialog();
    }

    public void ReloadScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        LevelBoard.Instance.Reload();
    }
}
