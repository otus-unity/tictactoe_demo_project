using System;
using UnityEngine;

public static class CheckPoints
{
    private static int currentValue;
    private static Byte[,] currentField;
    
    public static int CheckGettingPoints(Cell cell, byte[,] field)
    {
        // значение текущей ячейки
        int getPoints = 0;
        int xValue = cell.FieldPos.x;
        int yValue = cell.FieldPos.y;
        currentField = field;
        currentValue = currentField[xValue, yValue];

        // горизонтальная
        bool m1 = IsThreeInRow(xValue, yValue-2) && IsThreeInRow(xValue, yValue-1);
        bool m2 = IsThreeInRow(xValue, yValue-1) && IsThreeInRow(xValue, yValue+1);
        bool m3 = IsThreeInRow(xValue, yValue+1) && IsThreeInRow(xValue, yValue+2);
        // вертикаль
        bool m4 = IsThreeInRow(xValue-2, yValue) && IsThreeInRow(xValue-1, yValue);
        bool m5 = IsThreeInRow(xValue-1, yValue) && IsThreeInRow(xValue+1, yValue);
        bool m6 = IsThreeInRow(xValue+1, yValue) && IsThreeInRow(xValue+2, yValue);
        // диагональ 1
        bool m7 = IsThreeInRow(xValue-2, yValue-2) && IsThreeInRow(xValue-1, yValue-1);
        bool m8 = IsThreeInRow(xValue-1, yValue-1) && IsThreeInRow(xValue+1, yValue+1);
        bool m9 = IsThreeInRow(xValue+1, yValue+1) && IsThreeInRow(xValue+2, yValue+2);
        // даигональ 2
        bool m10 = IsThreeInRow(xValue-2, yValue+2) && IsThreeInRow(xValue-1, yValue+1);
        bool m11 = IsThreeInRow(xValue-1, yValue+1) && IsThreeInRow(xValue+1, yValue-1);
        bool m12 = IsThreeInRow(xValue+1, yValue-1) && IsThreeInRow(xValue+2, yValue-2);

        bool[] matches = { m1, m2, m3, m4, m5, m6, m7, m8, m9, m10, m11, m12 };
        for (int i=0; i<matches.Length; i++)
        {
            if (matches[i]) getPoints++;
        }

        return getPoints;
    }

    private static bool IsThreeInRow(int x, int y)
    {
        return (IsCellExisted(x, y) && (currentField[x, y] == currentValue));
    }

    private static bool IsCellExisted(int x, int y)
    {
        return (x < currentField.GetLength(0) &&
                y < currentField.GetLength(0) &&
                x >= 0 &&
                y >= 0);
    }
}
