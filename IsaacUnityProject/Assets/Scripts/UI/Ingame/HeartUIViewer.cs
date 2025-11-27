using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HeartUIViewer : MonoBehaviour,IStatus
{
    [SerializeField] UIScriptableObj _uiSo;

    private int _heart;
    private int _initHeart;
    private PlayerModel _pm;
    private List<Image> _heartImageComponent;

    private void Start()
    {
        _heartImageComponent = new List<Image>();
        for (int i = 0; i < transform.childCount; i++)
        {
            _heartImageComponent.Add(transform.GetChild(i).gameObject.GetComponent<Image>());
        }
    }
    private void Update()
    {
        //start에 하면 player 생성 전에 불러와 버림..
        Init();
    }
    private void Init()
    {
        if(_pm == null)
        {
            if(GameManager.Instance.Player != null)
            {
                //구독 등록
                _pm = GameManager.Instance.Player.gameObject.GetComponent<PlayerModel>();
                _pm.RegistSubscripber(this);
                DisplayHeart();
            }
        }
    }
    private void DisplayHeart()
    {
        int wholeHeartNum = _heart / 2;
        int halfHeartNum = _heart % 2;
        int emptyHeartNum = (_initHeart - _heart) / 2;
        foreach(Image heart in _heartImageComponent)
        {
            if (wholeHeartNum > 0)
            {
                heart.gameObject.SetActive(true);
                heart.sprite = _uiSo.HeartUiImages[2];
                wholeHeartNum--;
            }
            else if (halfHeartNum > 0)
            {
                heart.gameObject.SetActive(true);
                heart.sprite = _uiSo.HeartUiImages[1];
                halfHeartNum--;
            }
            else if(emptyHeartNum>0)
            {
                heart.gameObject.SetActive(true);
                heart.sprite = _uiSo.HeartUiImages[0];
                emptyHeartNum--;
            }
            else break;
        }
    }
    private void OnDisable()
    {
        _pm?.UnregistSubscriber(this);
    }
    public void NotifyStatus(sStatus stats)
    {
        _heart = stats._heart;
        _initHeart = stats._initHeart;
        DisplayHeart();
    }
}
