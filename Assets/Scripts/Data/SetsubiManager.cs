using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetsubiManager
{
    public List<Setsubi> GetAllSetsubi()
    {
        return new List<Setsubi>()
        {
            new GijutsuBookTe(),
            new GijutsuBookKoshi(),
            new GijutsuBookAshi()
        };
    }

    public Setsubi GetSetsubi(string setsubiNo)
    {
        foreach (Setsubi setsubi in this.GetAllSetsubi())
        {
            if (setsubi.no == setsubiNo)
            {
                return setsubi;
            }
        }
        return new Setsubi();
    }
}


public class Setsubi
{
    public string no;
    public string name;
    public int value;
}

public class GijutsuBookTe: Setsubi
{
    public GijutsuBookTe()
    {
        this.no = "1";
        this.name = "技術本(手)";
        this.value = 5;
    }
}

public class GijutsuBookKoshi: Setsubi
{
    public GijutsuBookKoshi()
    {
        this.no = "2";
        this.name = "技術本(腰)";
        this.value = 5;
    }
}

public class GijutsuBookAshi: Setsubi
{
    public GijutsuBookAshi()
    {
        this.no = "3";
        this.name = "技術本(足)";
        this.value = 5;
    }
}