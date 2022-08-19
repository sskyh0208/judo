using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerManager
{
    public string id;
    public string nameKaki;
    public string nameYomi;
    // 誕生日
    public DateTime birthDay;
    // 身長
    public int height;
    // 体重
    public int weight;
    // 出身地（placeIdが入る)
    // public string bone;

    // 属性値 101:監督 1:1年生 2:2年生 3:3年生 4:OB
    public int positionId;
    public string placeId;
    public string schoolId;
    // 才能
    public int sense;
    public List<Abillity> abillities;

    public string powerString;
    public string speedString;
    public string staminaString;
    // 立技
    public string waza0String;
    // 寝技
    public string waza1String;

    // 大会成績
    public int cityRank;
    public int placeRank;
    public int regionRank;
    public int countryRank;

    public int totalStatus;

    public PlayerManager(string id, string nameKaki, string nameYomi, DateTime generateDt, int positionId, string placeId, string schoolId, int sense)
    {
        this.id = id;
        this.nameKaki = nameKaki;
        this.nameYomi = nameYomi;
        this.birthDay = GeneratePlayerBirthDay(generateDt);
        this.height = GeneratePlayerHeight();
        this.weight = GeneratePlayerWeight(this.height);
        this.positionId = positionId;
        this.placeId = placeId;
        this.schoolId = schoolId;
        this.sense = sense;
        this.abillities = GeneratePlayerAbillities(sense);
        this.powerString = GetAbillity("900").displayString;
        this.speedString = GetAbillity("901").displayString;
        this.staminaString = GetAbillity("902").displayString;
        this.waza0String = GetAbillityTypeStatusDisplayString("0");
        this.waza1String = GetAbillityTypeStatusDisplayString("1");
        this.cityRank = 0;
        this.placeRank = 0;
        this.regionRank = 0;
        this.countryRank = 0;
        SetTotalStatus();
        CorrectionAbility();
    }

    private List<Abillity> GeneratePlayerAbillities(int sense)
    {
        Waza[] genWazaList = new Waza[GameData.instance.abillityManager.wazaList.Length];

        genWazaList = GameData.instance.abillityManager.wazaList;
        List<Abillity> playerAbillities = new List<Abillity>();

        // 技の能力値決め
        System.Random r = new System.Random();
        foreach(Waza waza in genWazaList)
        {
            playerAbillities.Add(new Abillity(waza.name, waza.id, waza.typeId, waza.groupId, sense));
        }
        return playerAbillities;
    }

    private DateTime GeneratePlayerBirthDay(DateTime generateYearDt)
    {
        System.Random r = new System.Random();
        int year = generateYearDt.Year;
        int month = r.Next(1, 12);
        // 早生まれは1年追加
        if(month < 4) {
            year ++;
        }
        int date = r.Next(1, DateTime.DaysInMonth(year, month));
        return new DateTime(year, month, date);
    }
    private int GeneratePlayerHeight()
    {
        int height = 0;
        System.Random r = new System.Random();
        int seed = r.Next(0, 1667);
        if(seed < 109)
        {
            height = r.Next(180, 185);
        }
        else if (seed < 126)
        {
            height = r.Next(185, 189);
        }
        else if (seed == 1667)
        {
            height = r.Next(190, 200);
        }
        else
        {
            height = r.Next(minValue: 160, 179);
        }
        return height;
    }

    private int GeneratePlayerWeight(int height)
    {
        int weight = 60;
        System.Random r = new System.Random();
        if(height > 190)
        {
            weight = r.Next(75, 130);
        }
        else if (height > 180)
        {
            weight = r.Next(70, 120);
        }
        else if (height > 170)
        {
            weight = r.Next(65, 110);
        }
        else
        {
            weight = r.Next(minValue: 58, 100);
        }
        return weight;
    }

    // 監督才能欄表示用文字列
    public string GetDisplayPlayerSenseName()
    {
        switch(sense)
        {
            default:
            case 1:
                return "柔道経験者";
            case 2:
                return "県大会入賞";
            case 3:
                return "全国ベスト8";
            case 4:
                return "インターハイ優勝";
            case 5:
                return "日本代表";
            case 6:
                return "金メダリスト";
        }
    }

    // 得意技欄表示用文字列
    public string GetDisplayPlayerTokuiwaza()
    {
        string tokuiwazaId = "";
        int tokuiwazaStatus = 0;
        foreach(Abillity abillity in abillities)
        {
           if(tokuiwazaStatus < abillity.status && abillity.typeId != "9")
           {
                tokuiwazaStatus = abillity.status;
                tokuiwazaId = abillity.id;
           }
        }
        return GameData.instance.abillityManager.GetWazaName(tokuiwazaId);
    }

    public string GetAbillityTypeStatusDisplayString(string typeId)
    {
        int statusSum = 0;
        int wazaCount = 0;
        foreach (Abillity abillity in abillities)
        {
            if(abillity.typeId == typeId)
            {
                statusSum += abillity.status;
                wazaCount ++;
            }
        }
        string displayString = "G";
        if((int)(statusSum / wazaCount) < 2000){displayString = "G";}
        else if ((int)(statusSum / wazaCount) < 3000){displayString = "F";}
        else if ((int)(statusSum / wazaCount) < 4000){displayString = "E";}
        else if ((int)(statusSum / wazaCount) < 5000){displayString = "D";}
        else if ((int)(statusSum / wazaCount) < 6000){displayString = "C";}
        else if ((int)(statusSum / wazaCount) < 7000){displayString = "B";}
        else if ((int)(statusSum / wazaCount) < 8000){displayString = "A";}
        else if ((int)(statusSum / wazaCount) < 9000){displayString = "S";}
        else if ((int)(statusSum / wazaCount) < 10000){displayString = "SS";}
        return displayString;
    }

    public Abillity GetAbillity(string id)
    {
        Abillity target = null;
        foreach (Abillity abillity in abillities)
        {
            if(id == abillity.id)
            {
                target = abillity;
                break;
            }
        }
        return target;
    }

    public string GetBirthdayDisplayString()
    {
        return String.Format("{0}年{1:D2}月{2:D2}日", birthDay.Year, birthDay.Month, birthDay.Day);
    }

    // 階級文字列取得
    public string GetWeightClass()
    {
        string weigthClass = "100kg超級";
        if(weight < 60){weigthClass = "60kg級";}
        else if (weight < 66){weigthClass = "66kg級";}
        else if (weight < 73){weigthClass = "73kg級";}
        else if (weight < 81){weigthClass = "81kg級";}
        else if (weight < 90){weigthClass = "90kg級";}
        else if (weight < 100){weigthClass = "100kg級";}
        return weigthClass;
    }

    private void CorrectionAbility()
    {
        System.Random r = new System.Random();
        // パワーの補正
        Abillity powerAbillity = GetAbillity("900");
        if(weight < 60){powerAbillity.limit = (int)(powerAbillity.limit * (r.Next(8, 11) * 0.1f));}
        else if (weight < 66){powerAbillity.limit = (int)(powerAbillity.limit * (r.Next(9, 11) * 0.1f));}
        else if (weight < 73){powerAbillity.limit = (int)(powerAbillity.limit * 1);}
        else if (weight < 81){powerAbillity.limit = (int)(powerAbillity.limit * (r.Next(10, 12) * 0.1f));}
        else if (weight < 90){powerAbillity.limit = (int)(powerAbillity.limit * (r.Next(10, 13) * 0.1f));}
        else if (weight < 100){powerAbillity.limit = (int)(powerAbillity.limit * (r.Next(10, 14) * 0.1f));}
        if(height < 160){powerAbillity.limit = (int)(powerAbillity.limit * (r.Next(8, 11) * 0.1f));}
        else if (height < 165){powerAbillity.limit = (int)(powerAbillity.limit * (r.Next(9, 11) * 0.1f));}
        else if (height < 170){powerAbillity.limit = (int)(powerAbillity.limit * 1);}
        else if (height < 175){powerAbillity.limit = (int)(powerAbillity.limit * (r.Next(10, 12) * 0.1f));}
        else if (height < 180){powerAbillity.limit = (int)(powerAbillity.limit * (r.Next(10, 13) * 0.1f));}
        else if (height < 185){powerAbillity.limit = (int)(powerAbillity.limit * (r.Next(10, 14) * 0.1f));}
        SetAbillity(powerAbillity);
        // スピードの補正        
        Abillity speedAbillity = GetAbillity("901");
        if(weight < 60){speedAbillity.limit = (int)(speedAbillity.limit * (r.Next(10, 14) * 0.1f));}
        else if (weight < 66){speedAbillity.limit = (int)(speedAbillity.limit * (r.Next(10, 13) * 0.1f));}
        else if (weight < 73){speedAbillity.limit = (int)(speedAbillity.limit * (r.Next(10, 12) * 0.1f));}
        else if (weight < 81){speedAbillity.limit = (int)(speedAbillity.limit * 1);}
        else if (weight < 90){speedAbillity.limit = (int)(speedAbillity.limit * (r.Next(9, 11) * 0.1f));}
        else if (weight < 100){speedAbillity.limit = (int)(speedAbillity.limit * (r.Next(8, 11) * 0.1f));}
        if(height < 160){speedAbillity.limit = (int)(speedAbillity.limit * (r.Next(10, 14) * 0.1f));}
        else if (height < 165){speedAbillity.limit = (int)(speedAbillity.limit * (r.Next(10, 13) * 0.1f));}
        else if (height < 170){speedAbillity.limit = (int)(speedAbillity.limit * (r.Next(10, 12) * 0.1f));}
        else if (height < 175){speedAbillity.limit = (int)(speedAbillity.limit * 1);}
        else if (height < 180){speedAbillity.limit = (int)(speedAbillity.limit * (r.Next(9, 11) * 0.1f));}
        else if (height < 185){speedAbillity.limit = (int)(speedAbillity.limit * (r.Next(8, 11) * 0.1f));}
        SetAbillity(speedAbillity);
    }

    private void SetAbillity(Abillity abillity)
    {
        for(int i = 0; i < abillities.Count; i++)
        {
            if(abillities[i].id == abillity.id)
            {
                abillities[i] = abillity;
                break;
            }
        }
    }

    // 練習の成果を反映
    public void GetTrainingExp(List<Tuple<string, int>> trainingExpArray)
    {
        foreach (Tuple<string, int> exp in trainingExpArray)
        {
            foreach (Abillity abillity in abillities)
            {
                if(exp.Item1 == abillity.id && ! abillity.CheckLimit())
                {   
                    abillity.UpdateExpPoint(exp.Item2);
                }
            }
        }
    }

    public void ClearResultRank()
    {
        this.cityRank = 0;
        this.placeRank = 0;
        this.regionRank = 0;
        this.countryRank = 0;
    }

    public void SetTotalStatus()
    {
        this.totalStatus = 0;
        foreach (Abillity abillity in abillities)
        {
            totalStatus += abillity.status;
        }
    }
}


public class Abillity
{
    static System.Random r = new System.Random();
    public string name;
    // 技ID
    public string id;
    public string typeId;
    public string groupId;
    // 成長力に影響するセンス
    public int sense;
    // 上限
    public int limit;
    // 現在のステータス
    public int status;
    // 経験値
    public int exp;
    public string displayString;

    public Abillity(string name, string id, string typeId, string groupId, int sense)
    {
        this.name = name;
        this.id = id;
        this.typeId = typeId;
        this.groupId = groupId;
        this.sense = sense;
        this.limit = GenerateLimitStatuValue(sense);
        this.status = GenerateDefaultStatuValue(sense);
        if (this.limit < this.status) {this.status = this.limit;}
        this.exp = 0;
        this.displayString = GenerateDisplayString();

    }

    // 上限値生成
    private int GenerateLimitStatuValue(int sense)
    {
        System.Random r = new System.Random();
        int baseStatusValue = 1000;
        float valueWeight;
        switch (sense)
        {
            default:
            case 3:
                valueWeight = GenerateRandomFloatValue(1, 4);
                break;
            case 4:
                valueWeight = GenerateRandomFloatValue(3, 5);
                break;
            case 5:
                valueWeight = GenerateRandomFloatValue(4, 6);
                break;
            case 6:
                valueWeight = GenerateRandomFloatValue(5, 7);
                break;
            case 7:
                valueWeight = GenerateRandomFloatValue(6, 8);
                break;
            case 8:
                valueWeight = GenerateRandomFloatValue(7, 9);
                break;
            case 9:
                valueWeight = GenerateRandomFloatValue(8, 10);
                break;
            case 10:
                valueWeight = GenerateRandomFloatValue(9, 11);
                break;
        }
        return (int)(baseStatusValue * valueWeight);
    }

    // 初期値生成
    private int GenerateDefaultStatuValue(int sense)
    {
        int baseStatusValue = 1000;
        float valueWeight;
        switch (sense - new System.Random().Next(0, 5))
        {
            default:
            case 3:
                valueWeight = GenerateRandomFloatValue(1, 4);
                break;
            case 4:
                valueWeight = GenerateRandomFloatValue(3, 5);
                break;
            case 5:
                valueWeight = GenerateRandomFloatValue(4, 6);
                break;
            case 6:
                valueWeight = GenerateRandomFloatValue(5, 7);
                break;
            case 7:
                valueWeight = GenerateRandomFloatValue(6, 8);
                break;
            case 8:
                valueWeight = GenerateRandomFloatValue(7, 9);
                break;
            case 9:
                valueWeight = GenerateRandomFloatValue(8, 10);
                break;
            case 10:
                valueWeight = GenerateRandomFloatValue(9, 11);
                break;
        }
        return (int)(baseStatusValue * valueWeight);
    }

    private float GenerateRandomFloatValue(int min, int max)
    {
        return (float) r.NextDouble() * (max - min) + min;
    }

    public string GenerateDisplayString()
    {
        string displayString = "G";
        if(status < 2000){displayString = "G";}
        else if (status < 3000){displayString = "F";}
        else if (status < 4000){displayString = "E";}
        else if (status < 5000){displayString = "D";}
        else if (status < 6000){displayString = "C";}
        else if (status < 7000){displayString = "B";}
        else if (status < 8000){displayString = "A";}
        else if (status < 9000){displayString = "S";}
        else if (status < 10000){displayString = "SS";}
        return displayString;
    }

    private void SetDisplayString()
    {
        displayString = GenerateDisplayString();
    }

    // 限界値かどうか判定する
    public bool CheckLimit()
    {   
        return this.status == this.limit;
    }

    public void UpdateExpPoint(int expPoint)
    {
        while (expPoint >= (this.status + 1))
        {
            if (this.status + 1 <= this.limit)
            {
                this.status ++;
                expPoint -= this.status;
                this.exp = expPoint;
            }
            if(CheckLimit())
            {
                break;
            }
        }
        SetDisplayString();
    }

    public float GetUpdateExpSenseCoef()
    {
        switch (sense)
        {
            default:
            case 1:
                return  0.8f;
            case 2:
                return  0.9f;
            case 3:
                return  1.0f;
            case 4:
                return 1.1f;
            case 5:
                return 1.2f;
            case 6:
                return 1.3f;
            case 7:
                return 1.4f;
            case 8:
                return 1.5f;
            case 9:
                return 1.6f;
            case 10:
                return 1.7f;
        }
    }
}