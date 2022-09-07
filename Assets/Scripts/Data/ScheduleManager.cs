using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class ScheduleManager
{
    public Schedule[] scheduleArray;

    public Schedule GetSchedule(DateTime dt)
    {
        Schedule target = new Schedule();

        List<Schedule> thisMonthSchedhule = new List<Schedule>();
        foreach(Schedule item in scheduleArray)
        {
            if (dt.Month == item.month) {
                thisMonthSchedhule.Add(item);
            }
        }

        // 同じ月のがなければ空を返す
        if(thisMonthSchedhule.Count == 0)
        {
            return target;
        }

        // 日付が一致するか検索
        foreach(Schedule item in thisMonthSchedhule)
        {
            if(dt.Day == item.day) {
                return item;
            }
        }

        // 週が一致するか検索
        int thisWeekNum = CountWeekNum(dt);
        List<Schedule> thisWeekSchedhule = new List<Schedule>();
        foreach(Schedule item in thisMonthSchedhule)
        {
            if(thisWeekNum == item.week) {
                thisWeekSchedhule.Add(item);
            }
        }

        // 曜日が一致するか検索
        int thisDayOfWeek = (int)dt.DayOfWeek;
        foreach(Schedule item in thisWeekSchedhule)
        {
            if(thisDayOfWeek == item.dayOfWeek)
            {
                return item;
            }
        }

        return target;
    }

    private int CountWeekNum(DateTime dt)
    {
        int countWeekNum = 1;
        for(int i = 1; i < DateTime.DaysInMonth(dt.Year, dt.Month); i++)
        {
            int checkDayOfWeek =  (int)new DateTime(dt.Year, dt.Month, i).DayOfWeek;
            if (dt.Day == i) {
                break;
            }
            if (checkDayOfWeek == 0)
            {
                countWeekNum ++;
            }
        }
        return countWeekNum;
    }
}
[Serializable]
public class Schedule
{
    public int month;
    public int day;
    public int week;
    public int dayOfWeek;
    public string eventName;
    public string eventId;
    public string eventType;
    public string filterEventId;
    public int filterValue;

    public List<string> GetTournamentIdList()
    {
        List<string> idList = new List<string>();
        switch (this.eventId)
        {
            case "01":
                idList = new List<string>()
                {
                    "073101",
                    "073102",
                    "073103",
                    "073201",
                    "073202",
                    "073203",
                    "073301",
                    "073302",
                    "073303",
                    "073304",
                    "073401",
                    "073402",
                    "073403",
                    "073404",
                    "073405",
                    "073501",
                    "073502",
                    "073503",
                    "073504",
                    "073505"
                };
                break;
            case "02":
                idList = new List<string>()
                {
                    "073100",
                    "073200",
                    "073300",
                    "073400",
                    "073500"
                };
                break;
            case "03":
                idList = new List<string>()
                {
                    "070000",
                };
                break;
            case "04":
                idList = new List<string>()
                {
                    "000000",
                };
                break;
            case "05":
                idList = new List<string>()
                {
                    "000000",
                };
                break;
            default:
                idList = new List<string>();
                break;
        }
        return idList;
    }
}