using System.Collections.Generic;
using Sirenix.OdinInspector;
using Tymski;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameLoader : MonoBehaviour
{
    [SerializeField] private SceneReference mainScene;
    [ShowInInspector]
    public List<ScriptableObject> savedScriptableObjectsList = new();
    private void Awake()
    {
        DontDestroyOnLoad(transform.gameObject);
    }
    private void Start()
    {
        if (SaveManager.CheckFolderExist("Saves"))
        {
            foreach (var item in savedScriptableObjectsList)
            {
                string jsonData = SaveManager.LoadData("Saves", $"{item}");
                JsonUtility.FromJsonOverwrite(jsonData, item);
            }
        }

        SceneManager.LoadScene(mainScene);
    }

    private void OnApplicationQuit()
    {
        foreach (var item in savedScriptableObjectsList)
        {
            string jsonData = JsonUtility.ToJson(item);
            SaveManager.SaveData("Saves", $"{item}", jsonData);
        }
    }
}
