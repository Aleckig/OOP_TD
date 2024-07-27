using UnityEngine;
using TMPro;
using Sirenix.OdinInspector;
using Michsky.UI.Heat;
using Unity.VisualScripting;
public class LevelSelectionManager : MonoBehaviour
{
    [Title("Reference")]
    public GameData gameData;
    public PlayerProgressData playerProgressData;
    [SerializeField] private LineManager popUpBlock;
    [SerializeField] private TMP_Text levelNameText;
    [SerializeField] private UnityEngine.UI.Image levelimage;
    [SerializeField] private GameObject levelSelectionBlock;
    [SerializeField] private GameObject levelList;
    [SerializeField] private GameObject levelGrid;
    [SerializeField] private ButtonManager easyButton;
    [SerializeField] private GameObject easyButtonBlocked;
    [SerializeField] private ButtonManager hardButton;
    [SerializeField] private GameObject hardButtonBlocked;
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

        easyButton.onClick = new();
        hardButton.onClick = new();

        if (gameData.towerCardsList.Count == 4)
        {
            easyButton.onClick.AddListener(() => { levelDataItem.LoadEasyLevel(); });
            hardButton.onClick.AddListener(() => { levelDataItem.LoadHardLevel(); });
        }
        else
        {
            easyButton.onClick.AddListener(() => { popUpBlock.TextSet("Please, check that stats for all towers selected in \"Tower Deck\" tab."); popUpBlock.gameObject.SetActive(true); ReturnBack(); });
            hardButton.onClick.AddListener(() => { popUpBlock.TextSet("Please, check that stats for all towers selected in \"Tower Deck\" tab."); popUpBlock.gameObject.SetActive(true); ReturnBack(); });

        }


        levelList.SetActive(false);
        levelSelectionBlock.SetActive(true);

        gameData.activeLevelId = levelId;

        bool[] statusCurrent = playerProgressData.levelDataSaveManager.GetLevelStatus(levelId);
        if (levelId == 1 && statusCurrent[1] == false)
        {
            easyButtonBlocked.SetActive(false);
            easyButton.enabled = true;

            hardButtonBlocked.SetActive(!statusCurrent[0]);
            hardButton.enabled = statusCurrent[0];
        }
        else
        {
            bool[] statusPrev = playerProgressData.levelDataSaveManager.GetLevelStatus(levelId - 1);

            easyButtonBlocked.SetActive(!statusPrev[1]);
            easyButton.enabled = statusPrev[1];

            hardButtonBlocked.SetActive(!statusCurrent[0]);
            hardButton.enabled = statusCurrent[0];
        }

        easyButton.UpdateUI();
        hardButton.UpdateUI();
    }
}
