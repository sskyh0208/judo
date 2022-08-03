using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 能力クラス
public class AbillityManager
{
    public Waza[] wazaList;

    public string GetWazaName(string targetId)
    {
        string targetWazaName = "";
        foreach (Waza waza in wazaList)
        {
            if(waza.id == targetId)
            {
                targetWazaName = waza.name;
                break;
            }
        }
        return targetWazaName;
    }

    // 技タイプで技IDをすべて取得する
    public List<string> GetWazaIdArrayWithType(string typeId)
    {
        List<string> resultWazaArray = new List<string>();
        foreach (Waza waza in wazaList)
        {
            if(waza.typeId == typeId)
            {
                resultWazaArray.Add(waza.id);
            }
        }
        return resultWazaArray;
    }

    // 技種類で技IDをすべて取得する
    public List<string> GetWazaIdArrayWithGroup(string groupId)
    {
        List<string> resultWazaArray = new List<string>();
        foreach (Waza waza in wazaList)
        {
            if(waza.groupId == groupId)
            {
                resultWazaArray.Add(waza.id);
            }
        }
        return resultWazaArray;
    }
}

[Serializable]
public class Waza
{
    // 技ID
    public string id;
    // 技のタイプID
    public string typeId;
    // 技の種類ID
    public string groupId;
    // 技名
    public string name;
}