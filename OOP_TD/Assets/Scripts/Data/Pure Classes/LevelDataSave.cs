using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using Sirenix.Serialization;

[Serializable]
public class LevelDataSaveManager
{
  [OdinSerialize]
  public Dictionary<int, LevelDataSaveSlot> levelsDataDict;

  public LevelDataSaveManager()
  {
    levelsDataDict = new() { };
  }
  public bool SaveData(int levelId, string difficulty, LevelDataSaveItem item)
  {
    if (levelsDataDict != null)
      if (!levelsDataDict.ContainsKey(levelId))
      {
        levelsDataDict.Add(levelId, new LevelDataSaveSlot());
      }

    LevelDataSaveItem temp = new(item); //For avoid any reference to original object
    return levelsDataDict[levelId].Add(difficulty, temp); //If wrong key(difficulty) than LevelDataSaveSlot.Add(...) return false
  }

  public string CreateFeedbackForLevel(int levelId, string difficulty)
  {
    if (levelsDataDict[levelId][difficulty].Count <= 0) return "";
    //Shortcuts for text formating
    string leftDec = "--- ";
    string rightDec = " ---\n";
    string twoSidesDec = " ---|--- ";
    string oneTab = "\t";
    string twoTab = "\t\t";
    string threeTab = "\t\t\t";

    //Title of the feedback
    string headerStr = leftDec + "Level - " + levelId.ToString() + twoSidesDec + "Level difficulty - " + difficulty + twoSidesDec + "Total attempts - " + levelsDataDict[levelId].CountTotalAttempts(difficulty) + rightDec;

    LevelDataSaveItem tempGeneralData = levelsDataDict[levelId][difficulty][0];

    string generalLevelDataStr = leftDec + "\n" + leftDec + "General level data\n" + oneTab + "Mixing tunnel available - " + (tempGeneralData.mixingTunnelAvailable ? "yes" : "no") + "\n" + oneTab + "Blockable Paths count - " + tempGeneralData.blockablePathsCount.ToString() + "\n" + oneTab + "Tower placement amount - " + tempGeneralData.towerPlacementAmount.ToString() + "\n" + oneTab + "Enemy amount:\n";

    foreach (var item in tempGeneralData.enemiesAmountDict)
    {
      generalLevelDataStr += twoTab + item.Key + " - " + item.Value.ToString() + "\n";
    }

    string attemptsDataStr = "";
    int attemptCount = 1;
    foreach (var item in levelsDataDict[levelId][difficulty])
    {
      //Title
      string titleStr = leftDec + "\n" + leftDec + "\n" + leftDec + "Attempt - " + attemptCount.ToString() + twoSidesDec + "Status - " + (item.completionStatus ? "Victory" : "Defeat") + rightDec;

      //Actions
      string actionsStr = leftDec + "Actions\n" + oneTab + "Power-up used amount:\n";
      foreach (var powerUpItem in item.powerupUsedCountDict)
      {
        actionsStr += twoTab + powerUpItem.Key + " - " + powerUpItem.Value.ToString() + "\n";
      }

      //Towers data
      string towersDataStr = leftDec + "Towers data\n" + oneTab + "Used towers stats:\n";
      foreach (var tower in item.towersList)
      {
        TowerCard towerCard = item.towerCardsList.Find(item => item.towerCardName == tower.towerName);

        string towerStatsStr = twoTab + tower.towerName + ":\n" + threeTab + "Inheritance:\n";
        towerStatsStr += threeTab + "Points for price - " + towerCard.pricePoints + "\n";
        towerStatsStr += threeTab + "Points for damage - " + towerCard.damagePoints + "\n";
        towerStatsStr += threeTab + "Points for damage range - " + towerCard.damageRangePoints + "\n";
        towerStatsStr += threeTab + "Points for attack cooldown - " + towerCard.attackCooldownPoints + "\n";

        towerStatsStr += threeTab + "Polymorphism:\n";
        towerStatsStr += threeTab + "Used damage type names - " + string.Join(", ", tower.towerDamageTypeAdded) + "\n";
        towerStatsStr += threeTab + "Used special methods - " + string.Join(", ", tower.specialMethods) + "\n";

        towersDataStr += towerStatsStr;
      }

      //Enemies data
      string enemiesDataStr = leftDec + "Enemies data\n" + oneTab + "Base total damage received - " + item.baseDamageReceived.ToString() + "\n" + oneTab + "Enemies that dealt damage to base:\n";
      foreach (var enemy in item.enemiesThatDealtDamageDict)
      {
        enemiesDataStr += twoTab + enemy.Key + " - " + enemy.Value.ToString() + "\n";
      }

      //Attempt result string
      attemptsDataStr += titleStr + actionsStr + towersDataStr + enemiesDataStr;

      attemptCount++;
    }

    //All attempts result string
    string result = headerStr + generalLevelDataStr + attemptsDataStr;

    return result;
  }

  public string CreateFeedbackForBothLevels(int levelId)
  {
    return CreateFeedbackForLevel(levelId, "Easy") + "\n\n" + CreateFeedbackForLevel(levelId, "Hard");
  }

  public bool[] GetLevelStatus(int levelId)
  {
    if (!levelsDataDict.ContainsKey(levelId)) return new[] { false, false };

    bool[] result = new bool[2] { false, false };

    foreach (var item in levelsDataDict[levelId]["Easy"])
    {
      if (item.completionStatus == true) { result[0] = true; break; }
    }

    foreach (var item in levelsDataDict[levelId]["Hard"])
    {
      if (item.completionStatus == true) { result[1] = true; break; }
    }
    return result;
  }

  public string GetLevelStatusDisplay(int levelId)
  {
    string levelStatusStr = "Total attempts:\nEasy - " + levelsDataDict[levelId].CountTotalAttempts("Easy") + "\nHard - " + levelsDataDict[levelId].CountTotalAttempts("Hard");

    return levelStatusStr;
  }
}

[Serializable]
public class LevelDataSaveSlot
{
  [OdinSerialize]
  public Dictionary<string, List<LevelDataSaveItem>> levelsDataSaveDict;
  public LevelDataSaveSlot()
  {
    levelsDataSaveDict = new() { { "Easy", new() }, { "Hard", new() } };
  }

  public bool Add(string difficulty, LevelDataSaveItem item)
  {
    if (!levelsDataSaveDict.ContainsKey(difficulty)) return false;

    levelsDataSaveDict[difficulty].Add(item);

    return true;
  }

  public List<LevelDataSaveItem> this[string difficulty]
  {
    get
    {
      if (!levelsDataSaveDict.ContainsKey(difficulty)) return null;
      return levelsDataSaveDict[difficulty];
    }
  }

  public string CountTotalAttempts(string difficulty)
  {
    if (!levelsDataSaveDict.ContainsKey(difficulty)) return "Wrong key";

    int successfulCount = 0;
    int failedCount = 0;

    foreach (var attempt in levelsDataSaveDict[difficulty])
    {
      if (attempt.completionStatus) successfulCount++;
      else failedCount++;
    }

    return $"Victory {successfulCount}/ Defeat {failedCount}";
  }
}

[Serializable]
public class LevelDataSaveItem//Saving information about the Level for building a feedback
{
  public bool completionStatus;
  public bool mixingTunnelAvailable;
  public int blockablePathsCount;
  public int towerPlacementAmount;
  [ShowInInspector]
  [OdinSerialize]
  public Dictionary<string, int> enemiesAmountDict;
  [ShowInInspector]
  [OdinSerialize]
  public Dictionary<string, int> powerupUsedCountDict;
  public List<TowerCard> towerCardsList;
  public List<Tower> towersList;
  public float baseDamageReceived;
  [ShowInInspector]
  [OdinSerialize]
  public Dictionary<string, float> enemiesThatDealtDamageDict;

  public LevelDataSaveItem()
  {
    enemiesAmountDict = new() { };
    powerupUsedCountDict = new() { };
    enemiesThatDealtDamageDict = new() { };
  }
  public LevelDataSaveItem(bool completionStatus, bool mixingTunnelAvailable, int blockablePathsCount, int towerPlacementAmount, Dictionary<string, int> enemiesAmountDict, Dictionary<string, int> powerupUsedCountDict, List<TowerCard> towerCardsList, List<Tower> towersList, float baseDamageReceived, Dictionary<string, float> enemiesThatDealtDamageDict)
  {
    this.completionStatus = completionStatus;
    this.mixingTunnelAvailable = mixingTunnelAvailable;
    this.blockablePathsCount = blockablePathsCount;
    this.towerPlacementAmount = towerPlacementAmount;
    this.enemiesAmountDict = enemiesAmountDict;
    this.powerupUsedCountDict = powerupUsedCountDict;
    this.towerCardsList = towerCardsList;
    this.towersList = towersList;
    this.baseDamageReceived = baseDamageReceived;
    this.enemiesThatDealtDamageDict = enemiesThatDealtDamageDict;
  }

  public LevelDataSaveItem(LevelDataSaveItem levelDataSaveItem)
  {
    completionStatus = levelDataSaveItem.completionStatus;
    mixingTunnelAvailable = levelDataSaveItem.mixingTunnelAvailable;
    blockablePathsCount = levelDataSaveItem.blockablePathsCount;
    towerPlacementAmount = levelDataSaveItem.towerPlacementAmount;
    enemiesAmountDict = levelDataSaveItem.enemiesAmountDict;
    powerupUsedCountDict = levelDataSaveItem.powerupUsedCountDict;
    towerCardsList = levelDataSaveItem.towerCardsList;
    towersList = levelDataSaveItem.towersList;
    baseDamageReceived = levelDataSaveItem.baseDamageReceived;
    enemiesThatDealtDamageDict = levelDataSaveItem.enemiesThatDealtDamageDict;
  }
}