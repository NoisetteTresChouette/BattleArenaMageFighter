using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class DeathScreen : MonoBehaviour
{

    [SerializeField]
    [Tooltip("The block on which to idsplay the game reuslts")]
    private TextMeshProUGUI _resultTextZone;

    [SerializeField]
    [Tooltip("The text to write before the score")]
    private string _resultText;

    private string FormatTime(int seconds)
    {
        return $"{seconds/60}:{seconds%60}";
    }

    private void OnEnable()
    {
        _resultTextZone.text = _resultText + FormatTime(Mathf.FloorToInt(Time.timeSinceLevelLoad));
    }

    public void RetryButtonAction()
    {
        SceneManager.LoadSceneAsync("Main");
    }

    public void QuitButtonAction()
    {
        Debug.Log("Quitting application...");
        Application.Quit();
    }

}
