using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;

public class MapCreate
{
    private MapManagerSO _roomPrefebs;
    private List<RoomNode> _rooms;
    private Queue<RoomNode> _roomQueue;
    private int[,] _mapLayout;

    int roomIndex = 1;
    float mapSizeCount;

    public MapCreate(MapManagerSO prefebs)
    {
        _roomPrefebs = prefebs;
    }
    public List<RoomNode> CreateMap(int mapSize)
    {
        _rooms = new List<RoomNode>();
        _roomQueue = new Queue<RoomNode>();
        //충분한 크기 잡아주기
        _mapLayout = new int[mapSize*2,mapSize*2];

        //방 갯수 카운팅
        //시작방 빼기
        mapSizeCount = mapSize -1;

        _rooms.Add(new RoomNode(Rooms.StartRoom, GetRoomPrefeb(Rooms.StartRoom), _rooms.Count+1, mapSize, mapSize));

        //맵 구조에 기록
        _mapLayout[mapSize, mapSize] = roomIndex;

        //검색 큐에 넣음
        _roomQueue.Enqueue(_rooms[0]);
        while (mapSizeCount > 0 && _roomQueue.Count > 0)
        {
            CreateNewRoom();
        }
        _rooms[_rooms.Count - 1].SetBossRoom();
        return _rooms;
    }

    private void CreateNewRoom()
    {
        if (_roomQueue.Count == 0) return;

        int x;
        int y;
        Rooms type;
        RoomNode newNode = null;
        RoomNode node = _roomQueue.Dequeue();
        int doornum;

        //생성할 문의 갯수
        if (_roomQueue.Count == 0) doornum = GetRandomDoorNum(node.RoomType,1);
        else doornum = GetRandomDoorNum(node.RoomType);

        for (int i = 0; i < doornum; i++)
        {
            //문마다 랜덤하게 방향 결정
            var roomDir = node.GetEmptyRandomDoorIndex();

            //문을 설치 할 수 없다면 종료
            if (roomDir == DoorDirection._End) return;
            if (!CheckCreatable(node, roomDir)) return;
            
            x = node.MapXIndex;
            y = node.MapYIndex;
            MoveIndex(ref x,ref y, roomDir);

            type = GetNewRoomType(roomDir);

            newNode = new RoomNode(type,
                                                   GetRoomPrefeb(type),
                                                   _rooms.Count + 1,
                                                   x,y
                                                   );
            _roomQueue.Enqueue(newNode);
            _rooms.Add(newNode);
            _mapLayout[x, y] = _rooms.Count;
            node._doors[(int)roomDir] = newNode.LinkDoor(node, ReverseDir(roomDir));
            DecreaseCount(type);
        }
    }

    private void DecreaseCount(Rooms r)
    {
        if (r == Rooms.VerticalRoom || r == Rooms.HorizontalRoom)
        {
            mapSizeCount -= 0.5f;
        }
        else
        {
            mapSizeCount--;
        }

    }
    /// <summary>
    /// 방향에 따른 옳바른 방 타입만 가져오게 하는 함수
    /// 확장한다면 다른 방식이 필요해 보임
    /// </summary>
    /// <param name="dir">문 방향</param>
    /// <returns></returns>
    private Rooms GetNewRoomType(DoorDirection dir)
    {
        if(dir == DoorDirection.Up || dir == DoorDirection.Down)
        {
            if(Random.value > 0.2f)
            {
                return Rooms.NormalRoom;
            }
            else
            {
                return Rooms.VerticalRoom;
            }
        }
        else
        {
            if (Random.value > 0.2f)
            {
                return Rooms.NormalRoom;
            }
            else
            {
                return Rooms.HorizontalRoom;
            }
        }
    }
    /// <summary>
    /// 설치하려는 방향이 비어 있는지 확인
    /// </summary>
    /// <param name="node">기준 노드</param>
    /// <param name="dir">방향</param>
    /// <returns>true면 설치 가능</returns>
    private bool CheckCreatable(RoomNode node, DoorDirection dir)
    {
        int x = node.MapXIndex;
        int y = node.MapYIndex;

        MoveIndex(ref x, ref y, dir);

        if (_mapLayout[x, y] != 0)
        {
            //이미 있는 자리라면 서로 연결시켜줌
            

            node._doors[(int)dir] = _rooms[_mapLayout[x, y]-1].LinkDoor(node, ReverseDir(dir));
            return false;
        }
        return true;
    }
    private DoorDirection ReverseDir(DoorDirection dir)
    {
        DoorDirection targetDir;
        if (dir == DoorDirection.Up) targetDir = DoorDirection.Down;
        else if (dir == DoorDirection.Down) targetDir = DoorDirection.Up;
        else if (dir == DoorDirection.Left) targetDir = DoorDirection.Right;
        else targetDir = DoorDirection.Left;
        return targetDir;
    }
    private void MoveIndex(ref int x, ref int y,DoorDirection dir)
    {
        if (dir == DoorDirection.Up) y++;
        if (dir == DoorDirection.Down) y--;
        if (dir == DoorDirection.Left) x--;
        if (dir == DoorDirection.Right) x++;
    }
    private GameObject GetRoomPrefeb(Rooms type)
    {
        switch (type)
        {
            case Rooms.StartRoom:
                return _roomPrefebs.StartRooms[Random.Range(0, _roomPrefebs.StartRooms.Length)];
            case Rooms.NormalRoom:
                return _roomPrefebs.NormalRooms[Random.Range(0, _roomPrefebs.NormalRooms.Length)];
            //case Rooms.BigRoom:
            //    return _roomPrefebs.BigRooms[Random.Range(0, _roomPrefebs.BigRooms.Length - 1)];
            case Rooms.HorizontalRoom:
                return _roomPrefebs.HorizontalRooms[Random.Range(0, _roomPrefebs.HorizontalRooms.Length)];
            case Rooms.VerticalRoom:
                return _roomPrefebs.VerticalRooms[Random.Range(0, _roomPrefebs.VerticalRooms.Length)];
        }
        return null;
    }

    private int GetRandomDoorNum(Rooms type)
    {
        switch (type)
        {
            case Rooms.StartRoom:
                return RandomForStart();
            case Rooms.NormalRoom:
                return RandomForNormalRoom();
            //case Rooms.BigRoom:
            //    return RandomForBigRoom();
            default:
                return Random.Range(0, 2);
        }
    }
    private int GetRandomDoorNum(Rooms type,int min)
    {
        switch (type)
        {
            case Rooms.StartRoom:
                return RandomForStart();
            case Rooms.NormalRoom:
                return RandomForNormalRoom_nozero();
            default:
                return Random.Range(min, 2);
        }
    }
    private int RandomForStart()
    {
        return Random.Range(2, 5);
    }
    private int RandomForNormalRoom()
    {
        float rate = Random.value;
        if (rate <= 0.15f) return 0;
        if (rate <= 0.85f) return 1;
        if (rate <= 0.95f) return 2;
        return 3;
    }
    private int RandomForNormalRoom_nozero()
    {
        float rate = Random.value;
        if (rate <= 0.85f) return 1;
        if (rate <= 0.95f) return 2;
        return 3;
    }
}
