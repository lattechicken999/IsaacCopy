using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class RoomManager : Singleton<RoomManager>
{
    private RoomNode _curNode;
    private Transform _doors;
    private Transform _noDoors;
    private List<DoorView> _doorViewList;
    private RoomControll _curRoomControll;
    public RoomState _roomState { private set; get; }

    public void SetNode(RoomNode cur)
    {
        _curNode = cur;
        _doors = _curNode.RoomInstance.transform.Find("Doors");
        _noDoors = _curNode.RoomInstance.transform.Find("NoDoor");
        _roomState = _curNode.roomState;
        _curRoomControll = _curNode.RoomInstance.GetComponent<RoomControll>();

        InitDoorViewComponent();
        SetRoomDoor();
        StartRoomControll();
        SetDoorAnimation();
    }
    private void InitDoorViewComponent()
    {
        _doorViewList = new List<DoorView>();

        for (int i =0; i< _doors.childCount;i++)
        {
            DoorView dv;

            if(_doors.GetChild(i).gameObject.TryGetComponent<DoorView>(out dv))
            {
                _doorViewList.Add(dv);
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

    public void SetDoorAnimation()
    {
        switch (_roomState)
        {
            case RoomState.Ready:
                ChangeDoorStatus(DoorStatus.Open); break;
            case RoomState.Cleared:
                ChangeDoorStatus(DoorStatus.Open); break;
            case RoomState.Challenging:
                ChangeDoorStatus(DoorStatus.Close); break;
        }
    }

    private void StartRoomControll()
    {
        switch(_roomState)
        {
            case RoomState.Cleared:
                _curRoomControll.ClearChallenge(); break;
            case RoomState.Ready:
                if (_curNode.BossRoom)
                    _curRoomControll.StartChallenge(true);
                else
                    _curRoomControll.StartChallenge(); break;
            default:
                if(_curNode.BossRoom)
                    _curRoomControll.StartChallenge(true);
                else
                    _curRoomControll.StartChallenge(); break;
        }
    }
    
    public void ClearRoom()
    {
        _roomState = RoomState.Cleared;
        _curNode.roomState = _roomState;
        SetDoorAnimation(); 
    }
    public void StartChallenge()
    {
        _roomState = RoomState.Challenging;
        _curNode.roomState = _roomState;
        SetDoorAnimation();
    }
    


}
