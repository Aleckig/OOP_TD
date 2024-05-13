using System;
using System.IO;
using UnityEngine;

public class FeedbacksManager : MonoBehaviour
{
    [SerializeField] private PlayerProgressData playerProgressData;
    [SerializeField] private Transform feedbackLinesBlock;
    [SerializeField] private GameObject feedbackLinePrefab;

    private void Start()
    {
        InstaniateCardButtons();
    }
    private void InstaniateCardButtons()
    {
        foreach (var item in playerProgressData.levelDataSaveManager.levelsDataList)
        {
            GameObject button = Instantiate(feedbackLinePrefab, feedbackLinesBlock);
            LineManager lineManager = button.GetComponent<LineManager>();

            lineManager.TextSet("levelName", $"Level {item.Key}");
            lineManager.TextSet("levelStatus", playerProgressData.levelDataSaveManager.GetLevelStatusDisplay(item.Key));

            lineManager.AddEventOnClick(() => { CreateFeedbackFile(playerProgressData.levelDataSaveManager, item.Key); });
        }
    }

    public void CreateFeedbackFile(LevelDataSaveManager levelDataSaveManager, int levelId)
    {
        string documentsPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
        string folderPath = Path.Combine(documentsPath, "OOP TowerDefence/Feedbacks");
        Directory.CreateDirectory(folderPath);

        string filePath = Path.Combine(folderPath, $"Level {levelId} feedback.txt");
        File.WriteAllText(filePath, levelDataSaveManager.CreateFeedbackForBothLevels(levelId));
    }
}
