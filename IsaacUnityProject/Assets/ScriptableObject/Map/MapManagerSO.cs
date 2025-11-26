
using UnityEngine;
[CreateAssetMenu(fileName ="MapManagerSO",menuName = "ScripableObject/MapManagerSO")]
public class MapManagerSO : ScriptableObject
{
    public GameObject[] StartRooms;
    public GameObject[] NormalRooms;
    public GameObject[] BigRooms;
    public GameObject[] VerticalRooms;
    public GameObject[] HorizontalRooms;
    public GameObject[] BossRoom;
}
