using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class BoardDeadLock : MonoBehaviour
{
    int totalPossibleMovesLeft = 0;
    List<GamePiece> GetRowOrColumnList(GamePiece[,] allPieces, int x, int y, int listlength = 3, bool checkRow = true)
    {
        int width = allPieces.GetLength(0);
        int height = allPieces.GetLength(1);

        List<GamePiece> piecesList = new List<GamePiece>();
        for (int i = 0; i < listlength; i++)
        {
            if (checkRow)
            {
                if (x + i < width && y < height)
                {
                    piecesList.Add(allPieces[x + i, y]);
                }
            }
            else
            {
                if (x < width && y + i < height)
                {
                    piecesList.Add(allPieces[x, y + i]);
                }
            }
        }
        return piecesList;
    }

    List<GamePiece> GetMinimumMatches(List<GamePiece> gamePieces, int minForMatch = 2)
    { 
        List<GamePiece> matches = new List<GamePiece>();

        var groups = gamePieces.GroupBy(n => n.matchValue);

        foreach(var group in groups)
        {
            if(group.Count() >= minForMatch && group.Key != GamePiece.MatchValue.None)
            {
                matches = group.ToList();
            }
        }
        return matches;
    }

    List<GamePiece> GetNeigbors(GamePiece[,] allPieces, int x, int y)
    {
        int width = allPieces.GetLength(0);
        int height = allPieces.GetLength(1);

        List<GamePiece> neighbors = new List<GamePiece>();

        Vector2[] searchDirs = new Vector2[4]
        {
            new Vector2(-1f, 0f),
            new Vector2(1f, 0f),
            new Vector2(0f, -1f),
            new Vector2(0f, 1f)
        };

        foreach(Vector2 dir in searchDirs)
        {
            if(x + (int) dir.x >= 0 && x + (int) dir.x < width && y + (int) dir.y >= 0 && y + (int) dir.y < height)
            {
                if(allPieces[x + (int) dir.x, y + (int) dir.y] != null)
                {
                    if (!neighbors.Contains(allPieces[x + (int)dir.x, y + (int)dir.y]))
                    neighbors.Add(allPieces[x + (int)dir.x, y + (int)dir.y]);
                }
            }
        }
        return neighbors;
    }

    bool HasMoveAt(GamePiece[,] allPieces, int x, int y, int listLength = 3, bool checkRow = true) 
    {
        List<GamePiece> pieces = GetRowOrColumnList(allPieces, x, y, listLength, checkRow);
        List<GamePiece> matches = GetMinimumMatches(pieces, listLength - 1);

        GamePiece unmatchedPiece = null;

        if(pieces != null && matches != null)
        {
            if(pieces.Count == listLength && matches.Count == listLength - 1)
            {
                unmatchedPiece = pieces.Except(matches).FirstOrDefault();
            }
            if(unmatchedPiece != null)
            {
                List<GamePiece> neighbors = GetNeigbors(allPieces, unmatchedPiece.xIndex, unmatchedPiece.yIndex);
                neighbors = neighbors.Except(matches).ToList();
                neighbors = neighbors.FindAll(n => n.matchValue == matches[0].matchValue);

                matches = matches.Union(neighbors).ToList();
            }
            if(matches.Count >= listLength)
            {
                return true;
            }

        }
        return false;
    }

    public bool IsDeadlocked(GamePiece [,] allPieces, int listLength = 3)
    {
        int width =allPieces.GetLength(0);
        int height =allPieces.GetLength(1);
        totalPossibleMovesLeft = 0;
        bool isDeadLocked = true;

        for (int i = 0; i< width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                if(HasMoveAt(allPieces, i, j, listLength, true) || HasMoveAt(allPieces, i, j, listLength, false))
                {
                    totalPossibleMovesLeft += 1;
                    isDeadLocked = false;
                }
            }
        }
        Debug.Log("Possible Move Left: " + totalPossibleMovesLeft);
        return isDeadLocked;
    }
}
