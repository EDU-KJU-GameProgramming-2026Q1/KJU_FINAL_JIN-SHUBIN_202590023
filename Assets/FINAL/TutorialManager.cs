using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement; // 必须加这个才能跳转场景

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
        // 绑定按钮事件
        startButton.onClick.AddListener(ShowTutorial);
        continueButton.onClick.AddListener(GoToGameScene);
    }

    // 点击Start → 显示规则
    public void ShowTutorial()
    {
        tutorialPanel.SetActive(true);
    }

    // 点击Continue → 跳转到游戏场景 Scene2
    public void GoToGameScene()
    {
        SceneManager.LoadScene(gameSceneName);
    }
}