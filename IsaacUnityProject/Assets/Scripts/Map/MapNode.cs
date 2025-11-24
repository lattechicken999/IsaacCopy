using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapNode
{
    //Map Node 들 간 링크용
    public  MapNode[] _doors = new MapNode[4];

    ////JSON 파일에 저장용
    //int _leftIndex;
    //int _rightIndex;
    //int _upIndex;
    //int _downIndex;

    //룸 타입 저장용 

    private Rooms _roomType;
    //설치 좌표
    public int MapXIndex { private set; get; }
    public int MapYIndex{private set; get;}

    public Rooms RoomType
    { private set; get; }

    public int NodeIndex
    {
        private set; get;
    }
    public GameObject GetPrefeb { private set; get; }
    public GameObject RoomInstance { private set; get; }
    public MapNode(Rooms type, GameObject prefeb, int nodeindex,int xindex, int yindex)
    {
        _roomType = type;
        GetPrefeb = prefeb;
        NodeIndex = nodeindex;
        MapXIndex = xindex;
        MapYIndex = yindex;
    }

    public void Instantiate()
    {
        RoomInstance = GameObject.Instantiate(GetPrefeb);
    }
    /// <summary>
    /// Door 중 비어있는 것을 랜덤으로 반환
    /// </summary>
    /// <returns>비어있는 방향을 반환함</returns>
    public DoorDirection GetEmptyRandomDoorIndex()
    {
        List<DoorDirection> returnList = new List<DoorDirection>();
        for(int i = 0; i<_doors.Length;i++)
        {
            if(_roomType == Rooms.VerticalRoom)
            {
                if (i ==(int) DoorDirection.Left || i == (int)DoorDirection.Right) continue;
            }
            if (_roomType == Rooms.HorizontalRoom)
            {
                if (i == (int)DoorDirection.Up || i == (int)DoorDirection.Down) continue;
            }
            if (_doors[i] == null) returnList.Add((DoorDirection)i);
        }

        if (returnList.Count == 0) return DoorDirection._End;
        return returnList[Random.Range(0, returnList.Count)];
    }



    /// <summary>
    /// 노드가 가지고 있는 Door 와 받은 Node 를 Link 시킴. 
    /// </summary>
    /// <param name="linkedNode">호출자 Node</param>
    /// <param name="doorDir">문의 방향</param>
    /// <returns>성공하면 자기자신을 return, 실패하면 null </returns>
    public MapNode LinkDoor(MapNode linkedNode, DoorDirection doorDir)
    {
        if (_roomType == Rooms.VerticalRoom)
        {
            if (doorDir == DoorDirection.Left || doorDir == DoorDirection.Right) return null;
        }
        if(_roomType == Rooms.HorizontalRoom)
        {
            if(doorDir == DoorDirection.Up || doorDir == DoorDirection.Down) return null;
        }

        //이미 연결된 것이 있다면 실패 (사실 나오면 안됨)
        if (_doors[(int)doorDir] != linkedNode && _doors[(int)doorDir] != null) return null;

        _doors[(int)doorDir] = linkedNode;
        return this;
    }
}
