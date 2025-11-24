using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class DoorManager : Singleton<DoorManager>
{
    private MapNode _curNode;
    private Transform _doors;
    private Transform _doorBackground;
    private Transform _noDoors;
    public void SetNode(MapNode cur)
    {
        _curNode = cur;
        _doors = _curNode.RoomInstance.transform.Find("Doors");
        _noDoors = _curNode.RoomInstance.transform.Find("NoDoor");
        SetRoomDoor();
    }
    private void SetRoomDoor()
    {
        if (_curNode == null) return;
        for(int i = 0; i<4;i++)
        {
            if(_curNode._doors[i] != null)
            {
                _doors.Find(((DoorDirection)i).ToString())?.gameObject.SetActive(true);
                _noDoors.Find(((DoorDirection)i).ToString())?.gameObject.SetActive(false);
            }
            else
            {
                _doors.Find(((DoorDirection)i).ToString())?.gameObject.SetActive(false);
                _noDoors.Find(((DoorDirection)i).ToString())?.gameObject.SetActive(true);
            }
        }
    }
}
