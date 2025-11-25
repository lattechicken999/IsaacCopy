using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapManager :Singleton<MapManager>
{
    [SerializeField] MapManagerSO _prefebs;

    private List<RoomNode> _rooms;
    private RoomNode _currentRoom;

    private void Start()
    {
        CreateMap(_prefebs);
    }
    private void CreateMap(MapManagerSO prefeb)
    {
        MapCreate mc = new MapCreate(prefeb);
        _rooms = mc.CreateMap(10);
        foreach(var r in _rooms)
        {
            r.Instantiate();
            r.RoomInstance.transform.position = new Vector2(r.MapXIndex * 20, r.MapYIndex * 14);
        }
        //시작 방 문 초기화
        DoorManager.Instance.SetNode(_rooms[0]);
        _currentRoom = _rooms[0];
    }
    public void ChangeRoom(DoorDirection dir)
    {
        _currentRoom = _currentRoom._doors[(int)dir];
        DoorManager.Instance.SetNode(_currentRoom);
    }

}
