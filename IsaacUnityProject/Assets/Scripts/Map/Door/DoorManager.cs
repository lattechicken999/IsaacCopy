using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class DoorManager : Singleton<DoorManager>,IRoomStatus
{
    private RoomNode _curNode;
    private Transform _doors;
    private Transform _doorBackground;
    private Transform _noDoors;
    private List<DoorView> _doorViewList;

    public void SetNode(RoomNode cur)
    {
        _curNode = cur;
        _doors = _curNode.RoomInstance.transform.Find("Doors");
        _noDoors = _curNode.RoomInstance.transform.Find("NoDoor");
        InitDoorViewComponent();
        SetRoomDoor();
    }
    private void InitDoorViewComponent()
    {
        _doorViewList = new List<DoorView>();

        for (int i =0; i< _doors.childCount;i++)
        {
            DoorView temp;
            if(_doors.GetChild(i).gameObject.TryGetComponent<DoorView>(out temp))
            {
                _doorViewList.Add(temp);
                continue;
            }
            _doorViewList.Add(_doors.GetChild(i).gameObject.AddComponent<DoorView>());
        }
    }
    private void SetRoomDoor()
    {
        _doorViewList = new List<DoorView>();
        if (_curNode == null) return;
        for(int i = 0; i<4;i++)
        {
            if(_curNode._doors[i] != null)
            {
                var temp = _doors.Find(((DoorDirection)i).ToString());
                if (temp != null)
                {
                    _doorViewList.Add(temp.gameObject.GetComponent<DoorView>());
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
        foreach(var door in _doorViewList)
        {
            door.SetDoorStatus(status);
        }
    }

    /// <summary>
    /// Room의 상태가 바뀔 때 마다 알림 받음
    /// </summary>
    /// <param name="roomStatus"></param>
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
