
using UnityEngine;
[CreateAssetMenu(fileName ="MapManagerSO",menuName = "ScripableObject/MapManagerSO")]
public class MapManagerSO : ScriptableObject
{
    [Header("Prefebs")]
    public GameObject[] StartRooms;
    public GameObject[] NormalRooms;
    public GameObject[] BigRooms;
    public GameObject[] VerticalRooms;
    public GameObject[] HorizontalRooms;
    public GameObject[] BossRoom;
    [Header("Map Setting")]
    public int MapSize;
}
