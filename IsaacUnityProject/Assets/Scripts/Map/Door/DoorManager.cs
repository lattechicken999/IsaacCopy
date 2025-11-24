using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class DoorManager : Singleton<DoorManager>,IRoomStatus
{
    private MapNode _curNode;
    private Transform _doors;
    private Transform _doorBackground;
    private Transform _noDoors;
    private List<DoorView> _doorList;

    public void SetNode(MapNode cur)
    {
        _curNode = cur;
        _doors = _curNode.RoomInstance.transform.Find("Doors");
        _noDoors = _curNode.RoomInstance.transform.Find("NoDoor");
        SetRoomDoor();
    }
    private void SetRoomDoor()
    {
        _doorList = new List<DoorView>();
        if (_curNode == null) return;
        for(int i = 0; i<4;i++)
        {
            if(_curNode._doors[i] != null)
            {
                var temp = _doors.Find(((DoorDirection)i).ToString());
                if (temp != null)
                {
                    _doorList.Add(temp.gameObject.GetComponent<DoorView>());
                    temp.gameObject.SetActive(true);
                }
                _noDoors.Find(((DoorDirection)i).ToString())?.gameObject.SetActive(false);
            }
            else
            {
                _doors.Find(((DoorDirection)i).ToString())?.gameObject.SetActive(false);
                _noDoors.Find(((DoorDirection)i).ToString())?.gameObject.SetActive(true);
            }
        }
    }
    private void ChangeDoorStatus(DoorStatus status)
    {
        foreach(var door in _doorList)
        {
            door.SetDoorStatus(status);
        }
    }

    public void NotifyRoomStatus(RoomStatus roomStatus)
    {
        switch (roomStatus)
        {
            case RoomStatus.Ready:
                ChangeDoorStatus(DoorStatus.Open); break;
            case RoomStatus.Cleared:
                ChangeDoorStatus(DoorStatus.Open); break ;
            case RoomStatus.Challenging:
                ChangeDoorStatus(DoorStatus.Close); break ;
        }
    }


}
