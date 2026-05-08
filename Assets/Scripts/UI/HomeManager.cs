using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class HomeManager : MonoBehaviour
{
    [Header("Panels")]
    [SerializeField] private GameObject mainMenuPanel;
    [SerializeField] private GameObject levelSelectionPanel;

    [Header("Main Menu Buttons")]
    [SerializeField] private Button playButton;
    [SerializeField] private Button levelSelectionButton;
    [SerializeField] private Button quitButton;

    [Header("Level Selection")]
    [SerializeField] private GameObject levelSelectorContainer;
    [SerializeField] private GameObject levelButtonPrefab;
    [SerializeField] private Button backButton;

    private void Start()
    {
        AudioManager.Instance.PlayMenuMusic();
        playButton.onClick.AddListener(() => StartLevel(GameProgress.GetCurrentLevel()));
levelSelectionButton.onClick.AddListener(() => ShowLevelSelection(true));
        backButton.onClick.AddListener(() => ShowLevelSelection(false));
        
        if (quitButton != null)
            quitButton.onClick.AddListener(QuitGame);

        GenerateLevelButtons();
        ShowLevelSelection(false);
    }

    private void ShowLevelSelection(bool show)
    {
        mainMenuPanel.SetActive(!show);
        levelSelectionPanel.SetActive(show);
    }

    private void GenerateLevelButtons()
    {
        foreach (Transform child in levelSelectorContainer.transform)
        {
            if (child.gameObject != levelButtonPrefab)
                Destroy(child.gameObject);
        }

        int currentLevel = GameProgress.GetCurrentLevel();
        for (int i = 1; i <= 10; i++)
        {
            GameObject btnObj = Instantiate(levelButtonPrefab, levelSelectorContainer.transform);
            btnObj.SetActive(true);
            Button btn = btnObj.GetComponent<Button>();
            TMP_Text txt = btnObj.GetComponentInChildren<TMP_Text>();
            if (txt != null) txt.text = i.ToString();
            
            int levelNum = i;
            btn.onClick.AddListener(() => StartLevel(levelNum));
            
            if (i > currentLevel)
            {
                btn.interactable = false;
            }
        }
    }

    private void StartLevel(int levelNumber)
    {
        LevelManager.Instance.SetTargetLevel(levelNumber);
        SceneManager.LoadScene("Gameplay Scene");
    }

    private void QuitGame()
    {
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #else
            Application.Quit();
        #endif
    }
}
