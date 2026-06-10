using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class TutorialManager : MonoBehaviour
{
    [Header("UI References")]
    public GameObject tutorialPanel;   // 玩法说明面板
    public Button startButton;         // 你的Start按钮
    public Button continueButton;      // 继续游戏按钮

    [Header("Scene Settings")]
    public string gameSceneName = "Scene2"; // 要跳转到的场景名（必须正确）

    void Start()
    {
        // 运行开局强制隐藏面板，双重保险
        if (tutorialPanel != null)
            tutorialPanel.SetActive(false);

        // 清空按钮所有旧绑定，避免Inspector手动绑定和代码冲突
        startButton.onClick.RemoveAllListeners();
        continueButton.onClick.RemoveAllListeners();

        // 保留你原本的绑定代码，完全不删除
        startButton.onClick.AddListener(ShowTutorial);
        continueButton.onClick.AddListener(GoToGameScene);
        BGMManager.Instance.PlayGlobalBGM();
    }

    // 点击Start → 显示规则面板
    public void ShowTutorial()
    {
        Debug.Log("Start按钮已点击，打开教程面板");
        tutorialPanel.SetActive(true);
    }

    // 点击Continue → 跳转到游戏场景 Scene2
    public void GoToGameScene()
    {
        SceneManager.LoadScene(gameSceneName);
    }
}