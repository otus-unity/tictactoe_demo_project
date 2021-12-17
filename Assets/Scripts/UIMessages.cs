using TMPro;
using UnityEngine;

public class UIMessages : MonoBehaviour
{
    private const string MY_TURN_RIGHT_NOW = "Мой ход";
    private const string WAITING_FOR_TURN = "Ждем пока сходит другой игрок";
    private const string PLAYER_POINTS = "Счет: ";
    
    public TextMeshProUGUI whoIsNext;
    public TextMeshProUGUI actorPoints;
    
    public void ChangeWhoIsNextMessage(bool myTurn)
    {
        if (myTurn)
        {
            whoIsNext.text = MY_TURN_RIGHT_NOW;
            whoIsNext.color = Color.green;
        }
        else
        {
            whoIsNext.text = WAITING_FOR_TURN;
            whoIsNext.color = Color.red;
        }
    }

    public void UpdatePoints(int points)
    {
        actorPoints.text = PLAYER_POINTS + points;
    }
}