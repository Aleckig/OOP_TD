using System;
using System.Collections.Generic;
using UnityEngine.Rendering;

//That custom dicitonaries only for pure c# Classes wich is used as data type. For other inherit from Odin Serialized... class

[Serializable]
public class DictionaryStrInt : SerializedDictionary<string, int> { }
[Serializable]
public class DictionaryStrFloat : SerializedDictionary<string, float> { }
[Serializable]
public class LevelsDataSaveDict : SerializedDictionary<string, List<LevelDataSaveItem>> { }
[Serializable]
public class LevelsDataDict : SerializedDictionary<int, LevelDataSaveSlot> { }
