using System;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

public class CsvImporter
{
    const string inputFolderPath = "Assets/Editor/CsvData"; // csv파일 폴더, 파싱될 클래스 스크립트는 스크립츠의 데이터폴더 아래
    const string outputFolderPath = "Assets/Resources_Address/ScriptableData"; // 파싱 결과 파일 폴더
    
    static Dictionary<string, Action<string>> parserMap = new Dictionary<string, Action<string>>()
    { // ▼ CSV 파일명 → 파싱 함수 매핑 딕셔너리
        { "export_Monster", ConvertNpcCsv },
        { "export_MonsterGroup", ConvertNpcGroupCsv },
        { "export_Stage", ConvertStageCsv },
        { "export_Item", ConvertItemCsv },
        { "export_EquipItem", ConvertEquipItemCsv },
        { "export_DropItemMap", ConvertDropItemMapCsv },
    };

    [MenuItem("Tools/Data Build(Convert csv to so)")]
    public static void ConvertAllCsv()
    {
        if (!Directory.Exists(inputFolderPath))
        {
            Debug.LogError($"CSV 폴더가 없습니다 : {inputFolderPath}");
            return;
        }

        if (!Directory.Exists(outputFolderPath))
        {
            Directory.CreateDirectory(outputFolderPath);
        }

        string[] csvFiles = Directory.GetFiles(inputFolderPath, "*.csv");
        foreach (var csvPath in csvFiles)
        {
            string fileName = Path.GetFileNameWithoutExtension(csvPath);
            if (parserMap.TryGetValue(fileName, out var parserFunc))
            {
                string[] lines = File.ReadAllLines(csvPath);
                if (lines.Length < 3)
                {
                    Debug.LogWarning($"{Path.GetFileName(csvPath)} : No valid data exists.(requires at least 3 rows)");
                    continue;
                }
                parserFunc.Invoke(csvPath);
            }
            else
            {
                Debug.LogWarning($"{fileName}: 매칭되는 파서가 없습니다.");
            }
        }
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
        Debug.Log("모든 데이터 파싱 완료.");
    }
    static void ConvertNpcCsv(string path)
    {
        string[] lines = File.ReadAllLines(path);
        string outputPath = outputFolderPath + "/NpcDB.asset";
        NpcDB output = AssetDatabase.LoadAssetAtPath<NpcDB>(outputPath);
        if (output == null)
        {
            output = ScriptableObject.CreateInstance<NpcDB>();
            AssetDatabase.CreateAsset(output, outputPath);
        }
        output.dataSOs = null;

        string prefabFolderPath = $"Assets/Resources_Address/Monster/Prefab/";
        List<NpcDataSO> dataList = new List<NpcDataSO>();
        string[] header = lines[0].Replace("\r", "").Split('|');
        for (int i = 2; i < lines.Length; i++)
        {
            var line = lines[i].Replace("\r", "");
            if (string.IsNullOrWhiteSpace(line))
            {
                Debug.LogError($"Invalid data line : {lines[i]}");
                continue;
            }
            
            var values = line.Split('|');
            if (values.Length != header.Length)
            {
                Debug.LogError($"{Path.GetFileName(path)} : {i + 1} Mismatch in the number of line fields ({values.Length} != {header.Length})");
                continue;
            }

            try
            {
                NpcData data = new NpcData();
                int dataKey = int.Parse(values[0]);
                string dataName = values[1];
                string dataPrefabRef = values[2];
                int dataHp = int.Parse(values[3]);
                int dataMp = int.Parse(values[4]);
                int dataAtk = int.Parse(values[5]);
                float dataAtkSpeed = float.Parse(values[6]);
                float dataAtkDist = float.Parse(values[7]);
                int dataProjectileType = int.Parse(values[8]);
                int dataDef = int.Parse(values[9]);
                float dataEva = float.Parse(values[10]);
                int dataSkillNo = int.Parse(values[11]);
                data.SetData(dataKey, dataName, dataPrefabRef, dataHp, dataMp, dataAtk, dataAtkSpeed, dataAtkDist, dataProjectileType, dataDef, dataEva, dataSkillNo);

                string prefabFilePath = prefabFolderPath + $"{dataPrefabRef}.prefab";
                GameObject prefabObject = AssetDatabase.LoadAssetAtPath<GameObject>(prefabFilePath);

                NpcDataSO dataSO = ScriptableObject.CreateInstance<NpcDataSO>();
                dataSO.SetDataSO(data, prefabObject);

                string dataPath = outputFolderPath + $"/NpcDatas/Npc{dataKey}.asset";
                AssetDatabase.CreateAsset(dataSO, dataPath);
                dataList.Add(dataSO);
            }
            catch (Exception e)
            {
                Debug.LogError($"{Path.GetFileName(path)} : {i + 1}Line parsing error : {e.Message}");
                return;
            }
        }
        output.dataSOs = dataList.ToArray();
        EditorUtility.SetDirty(output);
        Debug.Log("NpcDB Complete.");
    }
    static void ConvertNpcGroupCsv(string path)
    {
        string[] lines = File.ReadAllLines(path);
        string outputPath = outputFolderPath + "/NpcDB.asset";
        NpcDB output = AssetDatabase.LoadAssetAtPath<NpcDB>(outputPath);
        if (output == null)
        {
            output = ScriptableObject.CreateInstance<NpcDB>();
            AssetDatabase.CreateAsset(output, outputPath);
        }
        output.groupDataSOs = null;

        List<NpcGroupDataSO> dataList = new List<NpcGroupDataSO>();
        string[] header = lines[0].Replace("\r", "").Split('|');
        for (int i = 2; i < lines.Length; i++)
        {
            var line = lines[i].Replace("\r", "");
            if (string.IsNullOrWhiteSpace(line))
            {
                Debug.LogError($"Invalid data line : {lines[i]}");
                continue;
            }

            var values = line.Split('|');
            if (values.Length != header.Length)
            {
                Debug.LogError($"{Path.GetFileName(path)} : {i + 1} Mismatch in the number of line fields ({values.Length} != {header.Length})");
                continue;
            }

            try
            {
                NpcGroupData data = new NpcGroupData();
                int dataKey = int.Parse(values[0]);
                int[] dataUnits = new int[9];
                for (int ii = 1; ii < values.Length; ii++)
                {
                    dataUnits[ii - 1] = int.Parse(values[ii]);
                }
                data.SetData(dataKey, dataUnits);

                NpcGroupDataSO dataSO = ScriptableObject.CreateInstance<NpcGroupDataSO>();
                dataSO.SetDataSO(data);

                string dataPath = outputFolderPath + $"/NpcGroupDatas/NpcGroup{dataKey}.asset";
                AssetDatabase.CreateAsset(dataSO, dataPath);
                dataList.Add(dataSO);
            }
            catch (Exception e)
            {
                Debug.LogError($"{Path.GetFileName(path)} : {i + 1}Line parsing error : {e.Message}");
                return;
            }
        }
        output.groupDataSOs = dataList.ToArray();
        EditorUtility.SetDirty(output);
        Debug.Log("NpcGroupDB Complete.");
    }
    static void ConvertStageCsv(string path)
    {
        string[] lines = File.ReadAllLines(path);
        string outputPath = outputFolderPath + "/StageDB.asset";
        StageDB output = AssetDatabase.LoadAssetAtPath<StageDB>(outputPath);
        if (output == null)
        {
            output = ScriptableObject.CreateInstance<StageDB>();
            AssetDatabase.CreateAsset(output, outputPath);
        }
        output.dataSOs = null;

        string prefabFolderPath = $"Assets/Resources_Address/Environment/Prefab/";
        List<StageDataSO> dataList = new List<StageDataSO>();
        string[] header = lines[0].Replace("\r", "").Split('|');
        for (int i = 2; i < lines.Length; i++)
        {
            var line = lines[i].Replace("\r", "");
            if (string.IsNullOrWhiteSpace(line))
            {
                Debug.LogError($"Invalid data line : {lines[i]}");
                continue;
            }

            var values = line.Split('|');
            if (values.Length != header.Length)
            {
                Debug.LogError($"{Path.GetFileName(path)} : {i + 1} Mismatch in the number of line fields ({values.Length} != {header.Length})");
                continue;
            }

            try
            {
                StageData data = new StageData();
                int dataKey = int.Parse(values[0]);
                string dataFieldName = values[1];
                string dataBattleName = values[2];
                int dataSpawnNormal = int.Parse(values[3]);
                int dataSpawnHard = int.Parse(values[4]);
                int dataBoss = int.Parse(values[5]);
                int datagroupMin = int.Parse(values[6]);
                int datagroupMax = int.Parse(values[7]);
                data.SetData(dataKey, dataFieldName, dataBattleName, dataSpawnNormal, dataSpawnHard, dataBoss, datagroupMin, datagroupMax);

                string fieldPrefabFilePath = prefabFolderPath + $"{dataFieldName}.prefab";
                GameObject fieldPrefabObject = AssetDatabase.LoadAssetAtPath<GameObject>(fieldPrefabFilePath);
                string battlePrefabFilePath = prefabFolderPath + $"{dataBattleName}.prefab";
                GameObject battlePrefabObject = AssetDatabase.LoadAssetAtPath<GameObject>(battlePrefabFilePath);

                StageDataSO dataSO = ScriptableObject.CreateInstance<StageDataSO>();
                dataSO.SetDataSO(data, fieldPrefabObject, battlePrefabObject);

                string dataPath = outputFolderPath + $"/StageDatas/Stage{dataKey}.asset";
                AssetDatabase.CreateAsset(dataSO, dataPath);
                dataList.Add(dataSO);
            }
            catch (Exception e)
            {
                Debug.LogError($"{Path.GetFileName(path)} : {i + 1}Line parsing error : {e.Message}");
                return;
            }
        }
        output.dataSOs = dataList.ToArray();
        EditorUtility.SetDirty(output);
        Debug.Log("StageDB Complete.");
    }
    static void ConvertItemCsv(string path)
    {
        string[] lines = File.ReadAllLines(path);
        string outputPath = outputFolderPath + "/ItemDB.asset";
        ItemDB output = AssetDatabase.LoadAssetAtPath<ItemDB>(outputPath);
        if (output == null)
        {
            output = ScriptableObject.CreateInstance<ItemDB>();
            AssetDatabase.CreateAsset(output, outputPath);
        }
        output.dataSOs = null;

        string normalSpriteFolderPath = $"Assets/Resources_Address/Texture/";
        string weaponSpriteFolderPath = $"Assets/Resources_Address/MainCharacter/Texture/Weapon/";
        string equipSpriteFolderPath = $"Assets/Resources_Address/MainCharacter/Texture/Character/";

        List<ItemDataSO> dataList = new List<ItemDataSO>();
        string[] header = lines[0].Replace("\r", "").Split('|');
        for (int i = 2; i < lines.Length; i++)
        {
            var line = lines[i].Replace("\r", "");
            if (string.IsNullOrWhiteSpace(line))
            {
                Debug.LogError($"Invalid data line : {lines[i]}");
                continue;
            }

            var values = line.Split('|');
            if (values.Length != header.Length)
            {
                Debug.LogError($"{Path.GetFileName(path)} : {i + 1} Mismatch in the number of line fields ({values.Length} != {header.Length})");
                continue;
            }

            try
            {
                Sprite itemIcon = null;
                Sprite subItemIcon = null;

                ItemData data = new ItemData();
                int dataKey = int.Parse(values[0]);
                string dataName = values[1];
                string dataSpriteName = values[2];
                int dataMainType = int.Parse(values[3]);
                int dataSubType = int.Parse(values[4]);
                int dataMaxAmount = int.Parse(values[5]);
                int dataGrade = int.Parse(values[6]);
                string dataInfo = values[7];

                data.SetData(dataKey, dataName, dataSpriteName, dataMainType, dataSubType, dataMaxAmount, dataGrade, dataInfo);

                if (!string.IsNullOrEmpty(dataSpriteName))
                {
                    if(dataMainType == (int)ItemMainType.EquipItem && dataSubType <= (int)EquipItemSubType.Shield)
                    {
                        itemIcon = AssetDatabase.LoadAssetAtPath<Sprite>(weaponSpriteFolderPath + dataSpriteName + ".png");
                    }
                    else if(dataMainType == (int)ItemMainType.EquipItem && dataSubType > (int)EquipItemSubType.Shield)
                    {
                        itemIcon = AssetDatabase.LoadAssetAtPath<Sprite>(equipSpriteFolderPath + dataSpriteName + ".png");
                        if(dataSubType >= (int)EquipItemSubType.Gauntlet)
                        {
                            dataSpriteName = dataSpriteName.Replace("_L_", "_R_");
                            subItemIcon = AssetDatabase.LoadAssetAtPath<Sprite>(equipSpriteFolderPath + dataSpriteName + ".png");
                        }
                    }
                    else
                    {
                        itemIcon = AssetDatabase.LoadAssetAtPath<Sprite>(normalSpriteFolderPath + dataSpriteName + ".png");
                    }
                }

                ItemDataSO dataSO = ScriptableObject.CreateInstance<ItemDataSO>();
                dataSO.SetDataSO(data, itemIcon, subItemIcon);

                string dataPath = outputFolderPath + $"/ItemDatas/Item{dataKey}.asset";
                AssetDatabase.CreateAsset(dataSO, dataPath);
                dataList.Add(dataSO);
            }
            catch (Exception e)
            {
                Debug.LogError($"{Path.GetFileName(path)} : {i + 1}Line parsing error : {e.Message}");
                return;
            }
        }
        output.dataSOs = dataList.ToArray();
        EditorUtility.SetDirty(output);
        Debug.Log("ItemDB Complete.");
    }
    static void ConvertEquipItemCsv(string path)
    {
        string[] lines = File.ReadAllLines(path);
        string outputPath = outputFolderPath + "/ItemDB.asset";
        ItemDB output = AssetDatabase.LoadAssetAtPath<ItemDB>(outputPath);
        if (output == null)
        {
            output = ScriptableObject.CreateInstance<ItemDB>();
            AssetDatabase.CreateAsset(output, outputPath);
        }
        output.equipDataSOs = null;

        List<EquipItemDataSO> dataList = new List<EquipItemDataSO>();
        string[] header = lines[0].Replace("\r", "").Split('|');
        for (int i = 2; i < lines.Length; i++)
        {
            var line = lines[i].Replace("\r", "");
            if (string.IsNullOrWhiteSpace(line))
            {
                Debug.LogError($"Invalid data line : {lines[i]}");
                continue;
            }

            var values = line.Split('|');
            if (values.Length != header.Length)
            {
                Debug.LogError($"{Path.GetFileName(path)} : {i + 1} Mismatch in the number of line fields ({values.Length} != {header.Length})");
                continue;
            }

            try
            {
                EquipItemData data = new EquipItemData();
                int dataKey = int.Parse(values[0]);
                int dataHp = int.Parse(values[1]);
                int dataMp = int.Parse(values[2]);
                int dataAtk = int.Parse(values[3]);
                float dataAtkSpeed = float.Parse(values[4]);
                float dataCri = float.Parse(values[5]);
                int dataDef = int.Parse(values[6]);
                float dataEva = float.Parse(values[7]);
                data.SetData(dataKey, dataHp, dataMp, dataAtk, dataAtkSpeed, dataCri, dataDef, dataEva);

                EquipItemDataSO dataSO = ScriptableObject.CreateInstance<EquipItemDataSO>();
                dataSO.SetDataSO(data);

                string dataPath = outputFolderPath + $"/EquipItemDatas/EquipItem{dataKey}.asset";
                AssetDatabase.CreateAsset(dataSO, dataPath);
                dataList.Add(dataSO);
            }
            catch (Exception e)
            {
                Debug.LogError($"{Path.GetFileName(path)} : {i + 1}Line parsing error : {e.Message}");
                return;
            }
        }
        output.equipDataSOs = dataList.ToArray();
        EditorUtility.SetDirty(output);
        Debug.Log("EquipItemDB Complete.");
    }
    static void ConvertDropItemMapCsv(string path)
    {
        string subPath = path.Replace("DropItemMap", "DropItem");

        string[] sublines = File.ReadAllLines(subPath);
        Dictionary<int, List<DropItemData>> tempDict = new Dictionary<int, List<DropItemData>>();
        string[] subHeader = sublines[0].Replace("\r", "").Split('|');
        for (int i = 2; i < sublines.Length; i++)
        {
            var line = sublines[i].Replace("\r", "");
            if (string.IsNullOrWhiteSpace(line))
            {
                Debug.LogError($"Invalid data line : {sublines[i]}");
                continue;
            }
        
            var values = line.Split('|');
            if (values.Length != subHeader.Length)
            {
                Debug.LogError($"{Path.GetFileName(subPath)} : {i + 1} Mismatch in the number of line fields ({values.Length} != {subHeader.Length})");
                continue;
            }
        
            try
            {
                DropItemData subData = new DropItemData();
                int dataKey = int.Parse(values[0]);
                int dataDropItem = int.Parse(values[1]);
                int dataDropWeight = int.Parse(values[2]);
                int dataMin = int.Parse(values[3]);
                int dataMax = int.Parse(values[4]);
                subData.SetData(dataKey, dataDropItem, dataDropWeight, dataMin, dataMax);

                if (tempDict.ContainsKey(dataKey))
                {
                    tempDict[dataKey].Add(subData);
                }
                else
                {
                    List<DropItemData> tempList = new List<DropItemData>() { subData };
                    tempDict.Add(dataKey, tempList);
                }
            }
            catch (Exception e)
            {
                Debug.LogError($"{Path.GetFileName(subPath)} : {i + 1}Line parsing error : {e.Message}");
                return;
            }
        }

        string[] lines = File.ReadAllLines(path);
        string outputPath = outputFolderPath + "/ItemDB.asset";
        ItemDB output = AssetDatabase.LoadAssetAtPath<ItemDB>(outputPath);
        if (output == null)
        {
            output = ScriptableObject.CreateInstance<ItemDB>();
            AssetDatabase.CreateAsset(output, outputPath);
        }
        output.dropItemMapDataSOs = null;

        DropItemMapDataSO.index = 1;
        List<DropItemMapDataSO> dataList = new List<DropItemMapDataSO>();
        string[] header = lines[0].Replace("\r", "").Split('|');
        for (int i = 2; i < lines.Length; i++)
        {
            var line = lines[i].Replace("\r", "");
            if (string.IsNullOrWhiteSpace(line))
            {
                Debug.LogError($"Invalid data line : {lines[i]}");
                continue;
            }
        
            var values = line.Split('|');
            if (values.Length != header.Length)
            {
                Debug.LogError($"{Path.GetFileName(path)} : {i + 1} Mismatch in the number of line fields ({values.Length} != {header.Length})");
                continue;
            }
        
            try
            {
                DropItemMapData data = new DropItemMapData();
                int dataNpcKey = int.Parse(values[0]);
                int dataDropItemKey = int.Parse(values[1]);

                var dropList = tempDict[dataDropItemKey];
                data.SetData(dataNpcKey, dropList);

                DropItemMapDataSO dataSO = ScriptableObject.CreateInstance<DropItemMapDataSO>();
                dataSO.SetDataSO(data);
        
                string dataPath = outputFolderPath + $"/DropItemMapDatas/DropItemMap{DropItemMapDataSO.index++}.asset";
                AssetDatabase.CreateAsset(dataSO, dataPath);
                dataList.Add(dataSO);
            }
            catch (Exception e)
            {
                Debug.LogError($"{Path.GetFileName(path)} : {i + 1}Line parsing error : {e.Message}");
                return;
            }
        }
        output.dropItemMapDataSOs = dataList.ToArray();
        EditorUtility.SetDirty(output);
        Debug.Log("DropItemDB Complete.");
    }
}
