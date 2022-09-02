using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;


[Serializable]
public class PlaceManager
{
    public string name;
    public Place[] placeArray;

    // 県の名前一覧取得
    public List<string> GetAllPlaceName()
    {
        List<string> allPlaceName = new List<string>();
        foreach(Place item in placeArray)
        {
            allPlaceName.Add(item.name);
        }
        return allPlaceName;
    }

    public Place getPlaceDataWithName(string name)
    {
        Place target = new Place();
        foreach(Place place in placeArray)
        {
            if(place.name == name)
            {
                target = place;
            }
        }
        return target;
    }

    public Place getPlaceDataWithId(string placeId)
    {
        Place target = new Place();
        foreach(Place place in placeArray)
        {
            if(Regex.IsMatch(place.id, placeId + "$"))
            {
                target = place;
            }
        }
        return target;
    }
}

[Serializable]
public class Place
{
    public string id;
    public string name;
}