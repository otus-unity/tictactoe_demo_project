using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameStateManager : MonoBehaviour
{
    [Range(10,30)]
    public int fieldSize = 10;

    public Vector2 cellSize = new Vector2(50, 50);
    public GameObject fieldCell;
    public Sprite[] cellImage;



    private Transform fieldCenter;

    void Awake()
    {
        InitGame();
    }

    private void InitGame()
    {
        fieldCenter = transform.GetChild(0);
        GridLayoutGroup gridLayoutGroup = fieldCenter.GetComponent<GridLayoutGroup>();
        gridLayoutGroup.cellSize = cellSize;
        RectTransform rectTransform = fieldCenter.GetComponent<RectTransform>();
        rectTransform.sizeDelta = cellSize * fieldSize;

        for (int i=0; i<fieldSize; i++)
        {
            for (int j=0; j<fieldSize; j++)
            {

                GameObject cellObject = Instantiate(fieldCell, fieldCenter);

                Button cellButton = cellObject.GetComponent<Button>();
                cellButton.onClick.AddListener(() => { PlayerTurn(); });
            }
        }
    }

    private void PlayerTurn()
    {
        Debug.Log("Player Turn!");
    }
}
