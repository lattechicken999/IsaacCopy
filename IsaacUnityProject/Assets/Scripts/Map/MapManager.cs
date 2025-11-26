using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapManager :Singleton<MapManager>
{
    [SerializeField] MapManagerSO _prefebs;

    private List<RoomNode> _rooms;
    private RoomNode _currentRoom;
    private PlayerRoomTeleport _currentPRS;

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
            r.RoomInstance.AddComponent<PlayerRoomTeleport>();
            //r.RoomInstance.SetActive(false);
        }
        //시작 방 문 초기화
        RoomManager.Instance.SetNode(_rooms[0]);
        _currentRoom = _rooms[0];
        _currentRoom.roomState = RoomState.Cleared;
        //_currentRoom.RoomInstance.SetActive(true);
        _currentPRS = _currentRoom.RoomInstance.GetComponent<PlayerRoomTeleport>();
        //ActiveAround();
    }
    public void ChangeRoom(DoorDirection dir)
    {
        _currentRoom = _currentRoom._doors[(int)dir];
        _currentPRS = _currentRoom.RoomInstance.GetComponent<PlayerRoomTeleport>();

        RoomManager.Instance.SetNode(_currentRoom);
        DoorDirection revDir = (DoorDirection)(((int)DoorDirection._End - 1) - (int)dir);
        _currentPRS.Teleport(revDir);
    }
    private void ActiveAround()
    {
        foreach(var node in _currentRoom._doors)
        {
            if (node == null) continue;
            if(!node.RoomInstance.activeSelf)
            {
                node.RoomInstance.SetActive(true);
            }
        }
    }
    private void DeactiveAround(RoomNode node)
    {
        foreach (var n in node._doors)
        {
            if (_currentRoom == n) continue;
            if (node == null) continue;
            if (n.RoomInstance.activeSelf)
            {
                n.RoomInstance.SetActive(false);
            }
        }
    }


}

