using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : Singleton<UIManager>
{
    enum enumUI
    {
        Background, Menu, InGame, GameEnd, _End
    }
    [SerializeField] Canvas _canvas;
    [SerializeField] GameObject _menuPrefeb;
    [SerializeField] GameObject _backfroundPrefeb;
    [SerializeField] GameObject _inGamePrefeb;
    [SerializeField] GameObject _gameEndPrefeb;

    private GameObject[] _gameUiObjects;

    protected override void init()
    { 
        _gameUiObjects = new GameObject[4];
        _gameUiObjects[0] = Instantiate(_backfroundPrefeb, _canvas.transform);
        _gameUiObjects[1] = Instantiate(_menuPrefeb, _canvas.transform);
        _gameUiObjects[2] = Instantiate(_inGamePrefeb, _canvas.transform);
        _gameUiObjects[3] = Instantiate(_gameEndPrefeb, _canvas.transform);
        foreach (var go in _gameUiObjects)
        {
            go.SetActive(false);
        }
    }
    
    public void SetUI(GameState gState)
    {
        switch (gState)
        {
            case GameState.Menu:
                ActiveDeactiveUI(enumUI.Menu); break;
            case GameState.Playing:
                ActiveDeactiveUI(enumUI.InGame); break;
            case GameState.GameSuccess:
                ActiveDeactiveUI(enumUI.GameEnd); break;
            case GameState.GameFail:
                ActiveDeactiveUI(enumUI.GameEnd); break;
         }

    }
    private void ActiveDeactiveUI(enumUI activeUI)
    {
        for(int i=0;i<(int)enumUI._End;i++)
        {
            if ((int)activeUI == i)
                _gameUiObjects[i].SetActive(true);
            else
                _gameUiObjects[i].SetActive(false);
        }
    }
}
