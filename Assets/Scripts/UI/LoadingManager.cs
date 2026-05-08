using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Threading.Tasks;
using TMPro;

public class LoadingManager : MonoBehaviour
{
    [SerializeField] private Image progressBar;
    [SerializeField] private TMP_Text loadingText;
    [SerializeField] private string nextSceneName = "Home Scene";

    private async void Start()
    {
        if (progressBar != null) progressBar.fillAmount = 0;
        await LoadAssetsAndTransition();
    }

    private async Task LoadAssetsAndTransition()
    {
        float targetProgress = 0;
        float currentProgress = 0;
        
        await Task.Delay(500);

        while (currentProgress < 1.0f)
        {
            targetProgress += Random.Range(0.1f, 0.3f);
            if (targetProgress > 1.0f) targetProgress = 1.0f;

            while (currentProgress < targetProgress)
            {
                currentProgress += Time.deltaTime * 0.8f;
                if (currentProgress > targetProgress) currentProgress = targetProgress;
                
                if (progressBar != null) progressBar.fillAmount = currentProgress;
                
                if (loadingText != null)
                {
                    int dots = (int)(Time.time * 4) % 4;
                    loadingText.text = "LOADING" + new string('.', dots);
                }
                
                await Task.Yield();
            }
            await Task.Delay(Random.Range(100, 300)); 
        }

        if (loadingText != null) loadingText.text = "COMPLETE!";
        await Task.Delay(800); 

        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(nextSceneName);
        while (!asyncLoad.isDone)
        {
            await Task.Yield();
        }
    }
}
