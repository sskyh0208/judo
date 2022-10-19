using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetsubiManager
{
    public List<Setsubi> GetAllSetsubi()
    {
        return new List<Setsubi>()
        {
            new BookTe(),
            new BookKoshi(),
            new BookAshi(),
            new BookOsae(),
            new BookShime(),
            new BookKansetsu(),
            new DvdTe(),
            new DvdKoshi(),
            new DvdAshi(),
            new DvdOsae(),
            new DvdShime(),
            new DvdKansetsu()
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
    public List<string> coefTargetList;
    public float coef;

    // 設備の係数を取得する
    public float GetSetsubiCoef(string wazaId)
    {
        if (this.coefTargetList.Contains(wazaId))
        {
            return this.coef;
        }
        return 0f;
    }
}

public class BookTe: Setsubi
{
    public BookTe()
    {
        this.no = "1";
        this.name = "技術本(手)";
        this.value = 5;
        this.coefTargetList = new List<string>(){"1", "2", "3", "4"};
        this.coef = 1.1f;
    }
}

public class BookKoshi: Setsubi
{
    public BookKoshi()
    {
        this.no = "2";
        this.name = "技術本(腰)";
        this.value = 5;
        this.coefTargetList = new List<string>(){"5", "6", "7", "8"};
        this.coef = 1.1f;
    }
}

public class BookAshi: Setsubi
{
    public BookAshi()
    {
        this.no = "3";
        this.name = "技術本(足)";
        this.value = 5;
        this.coefTargetList = new List<string>(){"9", "10", "11", "12"};
        this.coef = 1.1f;
    }
}

public class BookOsae: Setsubi
{
    public BookOsae()
    {
        this.no = "4";
        this.name = "技術本(抑)";
        this.value = 5;
        this.coefTargetList = new List<string>(){"21", "22", "23", "24"};
        this.coef = 1.1f;
    }
}

public class BookShime: Setsubi
{
    public BookShime()
    {
        this.no = "5";
        this.name = "技術本(締)";
        this.value = 5;
        this.coefTargetList = new List<string>(){"25", "26", "27", "28"};
        this.coef = 1.1f;
    }
}

public class BookKansetsu: Setsubi
{
    public BookKansetsu()
    {
        this.no = "6";
        this.name = "技術本(関)";
        this.value = 5;
        this.coefTargetList = new List<string>(){"29", "30", "31", "32"};
        this.coef = 1.1f;
    }
}

public class DvdTe: Setsubi
{
    public DvdTe()
    {
        this.no = "7";
        this.name = "技術DVD(手)";
        this.value = 10;
        this.coefTargetList = new List<string>(){"1", "2", "3", "4"};
        this.coef = 1.1f;
    }
}

public class DvdKoshi: Setsubi
{
    public DvdKoshi()
    {
        this.no = "8";
        this.name = "技術DVD(腰)";
        this.value = 10;
        this.coefTargetList = new List<string>(){"5", "6", "7", "8"};
        this.coef = 1.1f;
    }
}

public class DvdAshi: Setsubi
{
    public DvdAshi()
    {
        this.no = "9";
        this.name = "技術DVD(足)";
        this.value = 10;
        this.coefTargetList = new List<string>(){"9", "10", "11", "12"};
        this.coef = 1.1f;
    }
}

public class DvdOsae: Setsubi
{
    public DvdOsae()
    {
        this.no = "10";
        this.name = "技術DVD(抑)";
        this.value = 10;
        this.coefTargetList = new List<string>(){"21", "22", "23", "24"};
        this.coef = 1.1f;
    }
}

public class DvdShime: Setsubi
{
    public DvdShime()
    {
        this.no = "11";
        this.name = "技術DVD(締)";
        this.value = 10;
        this.coefTargetList = new List<string>(){"25", "26", "27", "28"};
        this.coef = 1.1f;
    }
}

public class DvdKansetsu: Setsubi
{
    public DvdKansetsu()
    {
        this.no = "12";
        this.name = "技術DVD(関)";
        this.value = 10;
        this.coefTargetList = new List<string>(){"29", "30", "31", "32"};
        this.coef = 1.1f;
    }
}