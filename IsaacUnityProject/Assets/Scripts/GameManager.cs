using Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameManager : Singleton<GameManager>
{
    public GameObject Player { private set; get; }
    private CinemachineConfiner2D _playerCineConfinder;
    private Rigidbody2D _rig;
    private GameState _gameState;
    //private float _saveCameraLenzValue = 4.44f;
    private float _targetLenzValue = 2.1f;
    private CinemachineVirtualCamera _vCam;
    private float zoomVelocity = 0f;
    private InputAction _EscAction;

    protected override void init()
    {
        _EscAction = InputSystem.actions["Esc"];
    }

    private void OnEnable()
    {
        _EscAction.performed += ctx => UpdateGameState(GameState.Menu);
    }
    private void OnDisable()
    {
        _EscAction.performed -= ctx => UpdateGameState(GameState.Menu);
    }
    private void Start()
    {
        UpdateGameState(GameState.Menu);
    }
    public void SetPlayer(GameObject player)
    {
        Player = player;
        _playerCineConfinder = Player.transform.Find("Virtual Camera").GetComponent<CinemachineConfiner2D>();
        _rig = Player.GetComponent<Rigidbody2D>();
        _vCam = _playerCineConfinder.gameObject.GetComponent<CinemachineVirtualCamera>();
    }
    public void TeleportPlayer(Vector2 position, PolygonCollider2D bounder)
    {
        _rig.position = position;
        _rig.velocity = Vector2.zero;
        _playerCineConfinder.m_BoundingShape2D = bounder;
    }
    private void Update()
    {

        switch(_gameState)
        {
            case GameState.Playing:
                Playing();
                break;
            case GameState.Menu:
                GameReset();
                break;
            case GameState.GameSuccess:
                GameEnd();
                break;
            case GameState.GameFail:
                GameEnd();
                break;
        }
    }

    private void Playing()
    {
        if (Player == null )
        {
            //게임 시작시 맵 생성
            MapManager.Instance.CreateMap();
        }
    }
    private void GameReset()
    {
        MapManager.Instance.ResetRoon();
        if( Player != null ) Destroy( Player );
        Player = null;
    }
    private void GameEnd()
    {
        if (_playerCineConfinder.m_BoundingShape2D != null)
            _playerCineConfinder.m_BoundingShape2D = null;

        _vCam.m_Lens.OrthographicSize = Mathf.SmoothDamp(_vCam.m_Lens.OrthographicSize, _targetLenzValue, ref zoomVelocity, 0.8f);
    }
    public void UpdateGameState(GameState state)
    {
        _gameState = state;
        UIManager.Instance.SetUI(_gameState);
    }
    
}
