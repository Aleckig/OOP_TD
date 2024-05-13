using UnityEngine;
using TMPro;
using Sirenix.OdinInspector;
using Michsky.UI.Heat;
public class LevelSelectionManager : MonoBehaviour
{
    [Title("Reference")]
    public GameData gameData;
    [SerializeField] private TMP_Text levelNameText;
    [SerializeField] private UnityEngine.UI.Image levelimage;
    [SerializeField] private GameObject levelSelectionBlock;
    [SerializeField] private GameObject levelList;
    [SerializeField] private GameObject levelGrid;
    [SerializeField] private ButtonManager easyButton;
    [SerializeField] private ButtonManager hardButton;
    [Title("Prefabs")]
    [SerializeField] private GameObject levelSelectionButtonPrefab;

    private void Start()
    {
        levelSelectionBlock.SetActive(false);

        CreateLevelButtons();
    }

    private void CreateLevelButtons()
    {
        foreach (var item in gameData.levelsDataList)
        {
            GameObject levelButton = Instantiate(levelSelectionButtonPrefab, levelGrid.transform);

            BoxButtonManager boxButtonManager = levelButton.GetComponent<BoxButtonManager>();
            boxButtonManager.buttonTitle = $"Level {item.levelId}";
            boxButtonManager.onClick.AddListener(() => { OpenLevelSelection(item.levelId); });
            boxButtonManager.UpdateUI();
        }
    }

    public void ReturnBack()
    {
        levelSelectionBlock.SetActive(false);
        levelList.SetActive(true);

        gameData.activeLevelId = -1;
    }

    public void OpenLevelSelection(int levelId)
    {
        LevelSettingsItem levelDataItem = gameData.levelsDataList.Find(item => item.levelId == levelId);

        levelNameText.text = $"Level {levelDataItem.levelId}";

        levelimage.overrideSprite = levelDataItem.levelImage;

        easyButton.onClick.AddListener(() => { levelDataItem.LoadEasyLevel(); });
        hardButton.onClick.AddListener(() => { levelDataItem.LoadHardLevel(); });

        levelList.SetActive(false);
        levelSelectionBlock.SetActive(true);

        gameData.activeLevelId = levelId;
    }
}
