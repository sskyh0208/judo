using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class RankingManager
{
    public Dictionary<int, YearRanking> allYearRanking;

    public RankingManager()
    {
        this.allYearRanking = new Dictionary<int, YearRanking>();
    }

    public YearRanking GetYearRanking(int year)
    {
        return allYearRanking[year];
    }
}

public class YearRanking
{
    public int year;
    public Ranking parameterRanking;
    public Dictionary<string, Ranking> countryRanking;
    public Dictionary<string, Ranking> regionRanking;
    public Dictionary<string, Ranking> placeRanking;
    public Dictionary<string, Ranking> cityRanking;

    public YearRanking(DateTime generateDt)
    {
        this.year = generateDt.Year;
        this.parameterRanking = new Ranking("999", generateDt);
        this.countryRanking = new Dictionary<string, Ranking>();
        this.regionRanking = new Dictionary<string, Ranking>();
        this.placeRanking = new Dictionary<string, Ranking>();
        this.cityRanking = new Dictionary<string, Ranking>();
    }

    public Ranking GetRanking(string id, string filterType)
    {
        switch (filterType)
        {
            default:
            case "countoryRank":
                return countryRanking[id];
            case "regionRank":
                return regionRanking[id];
            case "placeRank":
                return placeRanking[id];
            case "cityRank":
                return cityRanking[id];
        }
    }

    public Ranking GetCountryRanking(string countryId)
    {
        return countryRanking[countryId];
    }
    public Ranking GetRegionRanking(string regionId)
    {
        return regionRanking[regionId];
    }
    public Ranking GetPlaceRanking(string placeId)
    {
        return placeRanking[placeId];
    }
    public Ranking GetCityRanking(string cityId)
    {
        return cityRanking[cityId];
    }
}

public class Ranking
{
    public string id;
    public DateTime date;
    public Dictionary<int, string> school;
    public Dictionary<int, string> members60;
    public Dictionary<int, string> members66;
    public Dictionary<int, string> members73;
    public Dictionary<int, string> members81;
    public Dictionary<int, string> members90;
    public Dictionary<int, string> members100;
    public Dictionary<int, string> membersOver100;

    public Ranking(string id, DateTime generateDt)
    {
        this.id = id;
        this.date = generateDt;
        this.school = new Dictionary<int, string>();
        this.members60 = new Dictionary<int, string>();
        this.members66 = new Dictionary<int, string>();
        this.members73 = new Dictionary<int, string>();
        this.members81 = new Dictionary<int, string>();
        this.members90 = new Dictionary<int, string>();
        this.members100 = new Dictionary<int, string>();
        this.membersOver100 = new Dictionary<int, string>();
    }

    // id の入った配列を順位付けして設定する
    public void SetRankingData(List<string> array, int typeNum)
    {
        array.Reverse();
        Dictionary<int, string> rank = new Dictionary<int, string>();
        int count = 1;
        foreach (string id in array)
        {
            rank[count] = id;
            count ++;
        }
        switch (typeNum)
        {
            default:
            case 0:
                this.school = rank;
                break;
            case 1:
                this.members60 = rank;
                break;
            case 2:
                this.members66 = rank;
                break;
            case 3:
                this.members73 = rank;
                break;
            case 4:
                this.members81 = rank;
                break;
            case 5:
                this.members90 = rank;
                break;
            case 6:
                this.members100 = rank;
                break;
            case 7:
                this.membersOver100 = rank;
                break;
        }
    }
}
