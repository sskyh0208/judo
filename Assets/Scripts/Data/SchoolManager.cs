using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;



[Serializable]
public class SchoolManager
{
    public School[] schoolArray;

    public School getSchool(string placeId, string schoolId)
    {
        School target = new School();
        foreach(School school in schoolArray)
        {
            if(school.placeId == placeId && school.id == schoolId)
            {
                target = school;
            }
        }
        return target;
    }

    public List<School> getPlaceAllSchool(string placeId)
    {
        List<School> targetPlaceSchools = new List<School>();
        foreach(School school in schoolArray)
        {
            if(school.placeId == placeId)
            {
                targetPlaceSchools.Add(school);
            }
        }
        return targetPlaceSchools;
    }

    // 監督を設定する
    public void setSuperVoisor(string placeId, string schoolId, PlayerManager supervisor)
    {
        foreach(School school in schoolArray)
        {
            if(school.placeId == placeId && school.id == schoolId)
            {
                school.supervisor = supervisor;
                break;
            }
        }
    }
}

[Serializable]
public class School
{
    public string id;
    public string placeId;
    public string name;
    public int rank;

    // 顧問
    public PlayerManager supervisor;

    // 部員
    public List<PlayerManager> members;


    public int GenerateThisYearLimitMembersNum()
    {
        float membersDefaultNum = 10.0f;
        float membersMaxWeight;
        System.Random r = new System.Random();
        switch (rank)
        {
            default:
            case 1:
                membersMaxWeight = r.Next(0, 3) * 0.1f;
                break;
            case 2:
                membersMaxWeight = r.Next(3, 5) * 0.1f;
                break;
            case 3:
                membersMaxWeight = r.Next(5, 7) * 0.1f;
                break;
            case 4:
                membersMaxWeight = r.Next(7, 9) * 0.1f;
                break;
            case 5:
                membersMaxWeight = r.Next(9, 11) * 0.1f;
                break;
            case 6:
                membersMaxWeight = r.Next(11, 13) * 0.1f;
                break;

        }
        return (int)(membersDefaultNum * membersMaxWeight);
    }

    public List<PlayerManager> GetSortDescMembers()
    {
        return members.OrderByDescending(member => member.positionId).ToList();
    }
}
