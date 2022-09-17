using System;
using System.Collections;

[Serializable]
public class NameManager
{
    public Name[] myoji;
    public Name[] namae;

    public string[] GenarateRandomName()
    {
        System.Random r = new System.Random();
        int uniqueMyojiNum = r.Next(0, 1000);
        int startNum = 0;
        int endNum = 999;
        if (uniqueMyojiNum == 0)
        {
            startNum = 1000;
            endNum = myoji.Length;
        }
        Name genMyoji = myoji[r.Next(startNum, endNum)];
        Name genNamae = namae[r.Next(0, namae.Length)];
        string newKaki = String.Format("{0} {1}", genMyoji.kaki, genNamae.kaki);
        string newyomi = String.Format("{0} {1}", genMyoji.yomi, genNamae.yomi);
        return new string[]{newKaki, newyomi};
    }
}

[Serializable]
public class Name
{
    public string kaki;
    public string yomi;
}