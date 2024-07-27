using System.IO;

public class SaveManager
{
    static public bool CheckFolderExist(string folderName)
    {
        string documentsPath = System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments);
        string folderPath = System.IO.Path.Combine(documentsPath, $"OOP TowerDefence/{folderName}");
        Directory.CreateDirectory(folderPath);

        return Directory.Exists(folderPath);
    }
    static public void SaveData(string folderName, string fileName, string dataJson)
    {
        string documentsPath = System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments);
        string folderPath = System.IO.Path.Combine(documentsPath, $"OOP TowerDefence/{folderName}");
        Directory.CreateDirectory(folderPath);

        string filePath = System.IO.Path.Combine(folderPath, $"{fileName}.txt");
        // File.WriteAllText(filePath, dataJson);
        // write the serialized data to the file
        using FileStream stream = new(filePath, FileMode.Create);
        using StreamWriter writer = new(stream);
        writer.Write(dataJson);
    }

    static public string LoadData(string folderName, string fileName)
    {
        string documentsPath = System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments);
        string folderPath = System.IO.Path.Combine(documentsPath, $"OOP TowerDefence/{folderName}");

        string filePath = System.IO.Path.Combine(folderPath, $"{fileName}.txt");
        if (File.Exists(filePath))
        {
            // string dataJson = File.ReadAllText(filePath);
            // load the serialized data from the file
            string dataToLoad = "";
            using (FileStream stream = new(filePath, FileMode.Open))
            {
                using StreamReader reader = new(stream);
                dataToLoad = reader.ReadToEnd();
            }

            return dataToLoad;
        }
        return "";
    }
}
