using UnityEngine;
using System.Threading.Tasks;

public class GameManager : MonoBehaviour
{
    [SerializeField] private BoardManager boardManager;
    [SerializeField] private RackManager rackManager;
    [SerializeField] private LevelDatabase levelDatabase;
    
    [SerializeField] private GameObject winUI;
    [SerializeField] private GameObject loseUI;

    private LevelData currentLevelData;
    private int triplesMatched = 0;
    private bool isGameOver = false;

    private void Start()
    {
        AudioManager.Instance.PlayGameplayMusic();
        LoadLevel(LevelManager.Instance.TargetLevel);
    }

    public void LoadLevel(int levelNumber)
    {
        currentLevelData = levelDatabase.GetLevel(levelNumber);
        if (currentLevelData == null)
        {
            Debug.LogError("Level " + levelNumber + " not found!");
            return;
        }

        triplesMatched = 0;
        isGameOver = false;
        winUI.SetActive(false);
        loseUI.SetActive(false);

        boardManager.GenerateBoard(currentLevelData, OnTileTapped);
    }

    private async void OnTileTapped(Tile tile)
    {
        if (isGameOver) return;

        boardManager.RemoveTile(tile);
        bool added = await rackManager.AddTile(tile);

        if (!added)
        {
            GameOver(false);
            return;
        }

        CheckGameState();
    }

    private void CheckGameState()
    {
        if (boardManager.IsBoardEmpty() && rackManager.TileCount == 0)
        {
            GameOver(true);
        }
    }

    public void OnTripleMatched()
    {
        triplesMatched++;
        if (boardManager.IsBoardEmpty() && rackManager.TileCount == 0)
        {
            GameOver(true);
        }
    }

    public void NextLevel()
    {
        int next = currentLevelData.levelNumber + 1;
        if (next <= 10)
        {
            LevelManager.Instance.SetTargetLevel(next);
            AudioManager.Instance.PlayGameplayMusic();
            UnityEngine.SceneManagement.SceneManager.LoadScene("Gameplay Scene");
        }
        else
        {
            AudioManager.Instance.PlayMenuMusic();
            UnityEngine.SceneManagement.SceneManager.LoadScene("Home Scene");
        }
    }

    public void RetryLevel()
    {
        AudioManager.Instance.PlayGameplayMusic();
        UnityEngine.SceneManagement.SceneManager.LoadScene("Gameplay Scene");
    }

    public void GoHome()
    {
        AudioManager.Instance.PlayMenuMusic();
        UnityEngine.SceneManagement.SceneManager.LoadScene("Home Scene");
    }

    private void GameOver(bool win)
    {
        if (isGameOver) return;
        isGameOver = true;
        if (win)
        {
            AudioManager.Instance.PlayWin();
            winUI.SetActive(true);
            GameProgress.SetLevelProgress(currentLevelData.levelNumber + 1);
        }
        else
        {
            AudioManager.Instance.PlayLose();
            loseUI.SetActive(true);
        }
    }

    public void CheckLoseCondition()
    {
        if (rackManager.IsFull() && !isGameOver)
        {
            GameOver(false);
        }
    }
}
