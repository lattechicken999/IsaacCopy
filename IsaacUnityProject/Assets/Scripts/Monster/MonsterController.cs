using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

class SearchNode : IComparable<SearchNode>
{
    private float precision = 3;
    public float _H;//타겟까지 예상 비용
    public float _G;//다음칸 까지의 비용 (거리)
    public float _F;//H+G
    public Vector2 _position;
    public bool _isSearched = false;


    public SearchNode _parent;//이전 노드
    public SearchNode(float H, float G, SearchNode parent,Vector2 position)
    {
        _H= H;
        _G= G;
        _F= H + G;
        _parent = parent;
        _position = new Vector2((float)Math.Round(position.x,3), (float)Math.Round(position.y,3));
    }
    public void ActiveSearch()
    {
        _isSearched = true;
    }
    public int CompareTo(SearchNode other)
    {
        return _F.CompareTo(other._F);
    }
    public Vector2[] GetAdjPositions()
    {
        Vector2[] result = new Vector2[8];
        result[0] = _position - (Vector2.up / precision);
        result[1] = _position - (Vector2.down / precision);
        result[2] = _position - (Vector2.left / precision);
        result[3] = _position - (Vector2.right / precision);
        result[4] = _position - ((Vector2.up + Vector2.left) / precision);
        result[5] = _position - ((Vector2.up + Vector2.right) / precision);
        result[6] = _position - ((Vector2.down + Vector2.left) / precision);
        result[7] = _position - ((Vector2.down + Vector2.right) / precision);
        //int x =(int)(_position.x * 100);
        //int y =(int)(_position.y * 100);

        //result[0] = new Vector2((float)Math.Round((x - 33) / 100f, 3), (float)Math.Round((y) / 100f));
        //result[1] = new Vector2((float)Math.Round((x +33) / 100f, 3), (float)Math.Round((y) / 100f));
        //result[2] = new Vector2((float)Math.Round((x) / 100f, 3), (float)Math.Round((y -33) / 100f));
        //result[3] = new Vector2((float)Math.Round((x) / 100f, 3), (float)Math.Round((y +33) / 100f));
        //result[4] = new Vector2((float)Math.Round((x - 33) / 100f, 3), (float)Math.Round((y - 33) / 100f));
        //result[5] = new Vector2((float)Math.Round((x - 33) / 100f, 3), (float)Math.Round((y + 33) / 100f));
        //result[6] = new Vector2((float)Math.Round((x + 33) / 100f, 3), (float)Math.Round((y-33) / 100f));
        //result[7] = new Vector2((float)Math.Round((x + 33) / 100f, 3), (float)Math.Round((y+33) / 100f));

        return result;
    }
}

[RequireComponent(typeof(Rigidbody2D))]
public partial class MonsterController : MonoBehaviour
{
   
    //몬스터 모델
    [SerializeField] MonsterModel _monsterModel;
    //몬스터 애니메이션 조작용
    [SerializeField] MonsterView _monsterView;
   
    //몬스터 상태
    private MonsterState _state;

    //직렬화
    [SerializeField] LayerMask obstaclrLayers;
    [SerializeField] int _serachMaxCost;
    [SerializeField] Vector2[] _waypoint;

    //Astar 용
    private Dictionary<Vector2, SearchNode> _closeList;
    private Vector2 _targetPosition;
    private Vector2 _startPosition;
    private List<SearchNode> _openList;
    private Vector2[] _AdjPositions;
    private List<SearchNode> _resultPath;
    //장애물 확인용
    private RaycastHit2D _hit;
    //일반 몬스터 랜덤 경로 용
    private int _randomIndex;

    //Move 용
    private int updateIndex = 0;
    private Rigidbody2D _rb;
    Vector2 moveDir;

    //코루틴 용
    private float _searchCooltime = 1.5f;// 알고리즘 동작 쿨타임
    private bool _moveFlag = false; //1.5초 마다 공격, 대기 반복
    private float _TargetDist = 0.2f;
    private WaitForSeconds _searchDelay;
    private WaitForSeconds[] _attackDelay;
    private Coroutine _searchCoroutine;
    private Coroutine _attackCoroutine;

    //private SearchNode _debugNode;
    private void Start()
    {
        _searchDelay = new WaitForSeconds(_searchCooltime);
        _attackDelay = new WaitForSeconds[10];
        for(int i =0;i<10;i++)
        {
            _attackDelay[i] = new WaitForSeconds(UnityEngine.Random.Range(0.3f, 1.5f));
        }
        _rb = GetComponent<Rigidbody2D>();
        _state = MonsterState.Battle;
    }

    private void OnEnable()
    {
        _monsterModel.ResetHp();
    }
    private void FixedUpdate()
    {
        switch(_state)
        {
            case MonsterState.Idle:
                StopAction();
                break;
            case MonsterState.Battle:
                StartAction();
                break;
            case MonsterState.Die:
                StopAction();
                break;
        }

    }
    private void OnMove()
    {
        moveDir = _resultPath[updateIndex]._position - new Vector2(transform.position.x,transform.position.y);
        _rb.MovePosition(new Vector2(transform.position.x,transform.position.y) + moveDir * Time.deltaTime * _monsterModel.Speed/3);
        _monsterView.SetDirection(moveDir);
        
        if (Vector2.Distance(_resultPath[updateIndex]._position, transform.position) < 0.3f)
            updateIndex++;

        if (updateIndex >= _resultPath.Count)
        {
            updateIndex = _resultPath.Count - 1;
        }
    }
    private void OnMoveRandom()
    { 
        Vector2 dir =new Vector2();
        switch(_randomIndex)
        {
            case 0: dir = Vector2.up; break;
            case 1: dir = Vector2.down; break;
            case 2: dir = Vector2.left; break;
            case 3: dir = Vector2.right; break;
            case 4: dir = Vector2.up+Vector2.left; break;
            case 5: dir = Vector2.up + Vector2.right;break;
            case 6: dir = Vector2.down+Vector2.right;break;
            case 7:dir = Vector2.down + Vector2.left;break;
        }
        _rb.MovePosition(new Vector2(transform.position.x, transform.position.y) + dir.normalized * Time.deltaTime * _monsterModel.Speed/3);
        _monsterView.SetDirection(dir);
    }
    private IEnumerator AttackCoolTime()
    {
        while(true)
        {
            _moveFlag = !_moveFlag;
            if (!_monsterModel._isBoos)
                yield return _attackDelay[UnityEngine.Random.Range((int)0, (int)10)];
            else
            {
                _moveFlag = true;
                break;
            }
                
        }
    }
    private IEnumerator DoAttack()
    {
        while (true)
        {
            updateIndex = 0;
            if (_monsterModel._isBoos)
                _resultPath = Astar();
            else
                _randomIndex = UnityEngine.Random.Range(0, 8);
            yield return _searchDelay;
        }
    }
    private void initLists()
    {
        _openList = new List<SearchNode>();
        _closeList = new Dictionary<Vector2, SearchNode>();
    }

    /////////////////////////A Star 알고리즘 ////////////////////////////////
    private List<SearchNode> Astar()
    {
        initLists();
        _startPosition = transform.position;
        _targetPosition = GameManager.Instance.Player.transform.position;

        var newNode = new SearchNode(CreateH(_startPosition, _targetPosition), 0, null, _startPosition);
        _openList.Add(newNode);
        _closeList.Add(_startPosition, newNode);

        while (true)
        {
            if (_openList.Count == 0) break;
            if (_closeList.Count >= _serachMaxCost) break;
            _openList.Sort((a, b) => a.CompareTo(b));

            var curNode = _openList[0];
            _openList.RemoveAt(0);

            if (Vector2.Distance(curNode._position, _targetPosition) < _TargetDist) return ReverseNode(curNode);
            if (!_closeList.ContainsKey(curNode._position)) _closeList.Add(curNode._position, curNode);
            if (curNode._isSearched) continue;

            curNode._isSearched = true;

            _AdjPositions = curNode.GetAdjPositions();

            foreach (var adj in _AdjPositions)
            {

                //_hit = Physics2D.BoxCast(curNode._position, new Vector2(0.1f, 0.1f), 0, adj.normalized, 0.5f, obstaclrLayers);
                _hit = Physics2D.Raycast(curNode._position, adj, 0.5f, obstaclrLayers);
                //_hit = Physics2D.CircleCast(curNode._position, 0.05f, adj,1f, obstaclrLayers);
                if (_hit.collider != null) continue; //장애물 충돌이 나면 넘김


                var curCost = Vector2.Distance(curNode._position, adj) + curNode._G;

                //이미 생성한 노드의 위치라면 G 가 더 작은 값으로 갱신함
                if (_closeList.ContainsKey(adj))
                {
                    if (_closeList[adj]._G > (curCost))
                    {
                        _closeList[adj]._parent = curNode;
                        _closeList[adj]._G = curCost;
                    }
                }
                else
                {
                    var NewNode = new SearchNode(Vector2.Distance(adj, _targetPosition), curCost, curNode, adj);
                    _openList.Add(NewNode);
                    _closeList.Add(adj, NewNode);
                }
            }
        }
        return null;
    }
    private void OnDrawGizmos()
    {
        // 기즈모의 색상을 노란색으로 설정합니다.
        Gizmos.color = Color.yellow;


        if (_hit.collider != null)
        {
            Gizmos.DrawCube(_hit.point, new Vector2(0.1f, 0.1f));
        }
    }
    private float CreateH(Vector2 cur,Vector2 target)
    {
        return Vector2.Distance(cur, target);  
    }
    private List<SearchNode> ReverseNode(SearchNode node)
    {
        var curNode = node;
        List < SearchNode > returnList = new List<SearchNode>();

        while (curNode != null)
        {
            returnList.Add(curNode);
            curNode = curNode._parent;
        }

        returnList.Reverse();

        return returnList;
    }
    //////////////////////////////////////////////////////////////////////////
}
