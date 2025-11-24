using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapManager :Singleton<MapManager>
{
    [SerializeField] MapManagerSO _prefebs;
    private void Start()
    {
        MapCreate mc = new MapCreate(_prefebs);
        var rooms = mc.CreateMap(10);
        foreach(var r in rooms)
        {
            var temp = Instantiate(r.GetPrefeb);
            temp.transform.position = new Vector2(r.MapXIndex * 20, r.MapYIndex * 14);
        }
    }
}
