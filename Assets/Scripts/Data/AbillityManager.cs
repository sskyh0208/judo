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