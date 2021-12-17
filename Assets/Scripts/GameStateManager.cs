using System;
using System.Linq;
using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.UI;
using Newtonsoft.Json;

public class GameStateManager : MonoBehaviour, IOnEventCallback
{
    [Range(10,40)]
    public int fieldSize = 10;
    public Vector2 cellSize = new Vector2(50, 50);
    public GameObject fieldCell;
    public Sprite[] cellImage;

    private Transform _fieldCenter;
    private UIMessages _uiMessages;

    private bool _canTurn;
    private int _actorNumber;
    private int _indexCurrentPlayer;
    private int _actorPoints;
    private GameState _gameState;

    public void Awake()
    {
        InitGame();
    }

    private void InitGame()
    {
        // начальные настройки клетки и поля
        _fieldCenter = transform.GetChild(0);
        GridLayoutGroup gridLayoutGroup = _fieldCenter.GetComponent<GridLayoutGroup>();
        gridLayoutGroup.cellSize = cellSize;
        RectTransform rectTransform = _fieldCenter.GetComponent<RectTransform>();
        rectTransform.sizeDelta = cellSize * fieldSize;
        _uiMessages = GetComponent<UIMessages>();
        
        // состояние игрового поля
        _gameState = new GameState();
        _gameState.fieldState = new Byte[fieldSize,fieldSize];

        for (int i=0; i<fieldSize; i++)
            for (int j=0; j<fieldSize; j++)
            {
                byte defaultState = 0;
                _gameState.fieldState[i,j] = defaultState;
                
                GameObject cellObject = Instantiate(fieldCell, _fieldCenter);

                Cell cell = cellObject.GetComponent<Cell>();
                cell.FieldPos = new Vector2Int(i, j);
                
                Button cellButton = cellObject.GetComponent<Button>();
                cellObject.GetComponent<Button>().onClick.AddListener(() => { PlayerTurn(cellButton, cell); });
            }
        
        // кто первый ходит
        _canTurn = PhotonNetwork.IsMasterClient;
        _uiMessages.ChangeWhoIsNextMessage(_canTurn);
        _uiMessages.UpdatePoints(0);
        _actorNumber = PhotonNetwork.LocalPlayer.ActorNumber;
    }

    // ход игрока
    private void PlayerTurn(Button button, Cell cell)
    {
        if (!_canTurn) return;
        
        // получаем значение текущей ячейки
        Cell currentCell = button.GetComponent<Cell>();
        // устанавливаем новое значение поля в массиве
        _gameState.fieldState[currentCell.FieldPos.x, currentCell.FieldPos.y] = (byte)_actorNumber;
        // устнавливаем новый спрайт
        button.image.sprite = cellImage[_actorNumber];
        button.interactable = false;
        
        // вычисляем кто следующий будет ходить
        int indexNextPlayer = 0;
        for (int index = 0; index < PhotonNetwork.PlayerList.Length; index++)
        {
            if (Equals(PhotonNetwork.PlayerList.ElementAt(index), PhotonNetwork.LocalPlayer))
            {
                indexNextPlayer = ((index + 1) >= PhotonNetwork.PlayerList.Length) ? 0 : index + 1;
                _gameState.playerIndex = indexNextPlayer;
                break;
            }
        }

        // визуал и запрет на ход
        _uiMessages.ChangeWhoIsNextMessage(false);
        _canTurn = false;

        // Photon событие
        RaiseEventOptions options = new RaiseEventOptions { Receivers = ReceiverGroup.Others };
        SendOptions sendOptions = new SendOptions { Reliability = true };
        string data = JsonConvert.SerializeObject(_gameState);
        PhotonNetwork.RaiseEvent(1, data, options, sendOptions);
        
        // проверяем счет на поле
        _actorPoints += CheckPoints.CheckGettingPoints(currentCell, _gameState.fieldState);
        _uiMessages.UpdatePoints(_actorPoints);
    }

    // обновляем игровое поле
    private void UpdateGameField(EventData photonEvent)
    {
        // ищем индекс текущего игрока
        for (int index = 0; index < PhotonNetwork.PlayerList.Length; index++)
        {
            if (Equals(PhotonNetwork.PlayerList.ElementAt(index), PhotonNetwork.LocalPlayer))
            {
                _indexCurrentPlayer = index;
                break;
            }
        }

        // обновить игровое состояние
        switch (photonEvent.Code)
        {
            case 1:
                string data = (string)photonEvent.CustomData;
                GameState newGameState = JsonConvert.DeserializeObject<GameState>(data);
                int getIndex = newGameState.playerIndex;
                _gameState.fieldState = newGameState.fieldState;
                
                // если индекс текущего игрока равен тому кто должен ходить следующий
                if (getIndex == _indexCurrentPlayer)
                {
                    _uiMessages.ChangeWhoIsNextMessage(true);
                    _canTurn = true;
                }

                // синхронизировать визуал
                for (int index = 0; index < _fieldCenter.childCount; index++)
                {
                    Cell cell = _fieldCenter.GetChild(index).GetComponent<Cell>();
                    Button button = _fieldCenter.GetChild(index).GetComponent<Button>();
                    int cellState = _gameState.fieldState[cell.FieldPos.x, cell.FieldPos.y];
                    if (cellState != 0)
                    {
                        button.image.sprite = cellImage[cellState];
                        button.interactable = false;
                    }
                }
                
                break;
        }
    }

    public void OnEvent(EventData photonEvent)
    {
        UpdateGameField(photonEvent);
    }

    public void OnEnable()
    {
        PhotonNetwork.AddCallbackTarget(this);
    }

    public void OnDisable()
    {
        PhotonNetwork.RemoveCallbackTarget(this);
    }
}