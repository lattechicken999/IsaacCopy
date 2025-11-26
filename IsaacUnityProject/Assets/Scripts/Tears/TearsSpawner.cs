using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TearsSpawner : MonoBehaviour,IStatus
{
    [SerializeField] GameObject _tearsPrefeb;
    [SerializeField] PlayerModel _playerModel;
    private Queue<GameObject> _tearsPool;

    private bool _isAttack = false;
    private WaitForSeconds _attDelay;
    private sStatus _status;
    private Coroutine _attCoroutine;

    //눈물 인스턴스 임시 저장용 공간
    private GameObject _tempTears;

    public float TearRange
    {
        get {return _status._range; }
    }
    public float ShotSpeed
    {
        get { return _status._shotSpeed; }
    }
    public float Damage
    {
        get { return _status._damage; }
    }
    private void Awake()
    {
        //오브젝트 풀 초기 생성
        _tearsPool = new Queue<GameObject>();
        for(int i=0; i< 10;i++)
        {
            var temp = Instantiate(_tearsPrefeb);
            temp.SetActive(false);
            temp.GetComponent<TearsAction>().SetParent(this);
            _tearsPool.Enqueue(temp);
        }
    }

    private void Start()
    {
        _playerModel.RegistSubscripber(this);
    }

    private void Update()
    {
        if (_isAttack)
        {
            if (_attCoroutine == null)
            {
                _attCoroutine = StartCoroutine(AttackCoroutine());
            }
        }
        else
        {
            if (_attCoroutine != null)
                StopCoroutine(_attCoroutine);
            _attCoroutine = null;
        }
    }
    private void OnDisable()
    {
        _playerModel.UnregistSubscriber(this);
    }

    public void SetAttackTrigger(Vector2 attDir)
    {
        if(attDir == Vector2.zero)
        {
            _isAttack = false;
            return;
        }
        _isAttack = true;
        var lookDir = GetLookAxis(attDir);
        //2d 환경에서는 Lookat이 안통함
        //transform.rotation = Quaternion.LookRotation(new Vector3(lookDir.x, lookDir.y, 0));

        if(lookDir == Vector2.left) transform.rotation = Quaternion.Euler(0, 0, 90);//left
        if(lookDir == Vector2.up)transform.rotation = Quaternion.Euler(0, 0, 0);//up
        if(lookDir == Vector2.down)transform.rotation = Quaternion.Euler(0, 0, 180);//Down
        if (lookDir == Vector2.right) transform.rotation = Quaternion.Euler(0, 0, 270);//right

    }

    public void NotifyStatus(sStatus stats)
    {
        _status = stats;

        _attDelay = new WaitForSeconds(1f / _status._tears);
    }
    public void RechargeTears(GameObject tears)
    {
        _tearsPool.Enqueue(tears);
    }
    private Vector2 GetLookAxis(Vector2 dir)
    {
        if (Mathf.Abs(dir.x) > Mathf.Abs(dir.y)) return new Vector2(dir.x, 0).normalized;
        else return new Vector2(0, dir.y).normalized;
    }

    private IEnumerator AttackCoroutine()
    {
        while(true)
        {
            FireTears();
            yield return _attDelay;
        }
    }
    private void FireTears()
    {
        if(_tearsPool.Count == 0)
            _tempTears = Instantiate(_tearsPrefeb);
        else
            _tempTears = _tearsPool.Dequeue();

        _tempTears.GetComponent<TearsAction>().SetParent(this);
        _tempTears.transform.position = transform.position;
        _tempTears.transform.rotation = transform.rotation;
        _tempTears.gameObject.SetActive(true);

    }

}
