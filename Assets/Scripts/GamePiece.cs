using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GamePiece : MonoBehaviour
{
    public int xIndex;
    public int yIndex;
    Board m_board;

    bool m_isMoving = false;

    public enum MatchValue
    {
        Yellow, 
        Blue,
        Green,
        Magenta,
        Indigo,
        Teal,
        Red,
        Cyan,
        Wild,
        None,
    }

    public int scoreValue = 20;


    public MatchValue matchValue;
    // Start is called before the first frame update
    void Start()
    {

    }

    void Update()
    {
        
    }

    public void Init(Board board)
    {
        m_board = board;
    }

    public void SetCoord(int x, int y)
    {
        xIndex = x; yIndex = y;
    }

    public void Move(int destX, int destY, float timeToMove)
    {
        if (!m_isMoving)
        {
            StartCoroutine(MoveRoutine(new Vector3(destX, destY, 0), timeToMove));
        }
    }

    IEnumerator MoveRoutine(Vector3 destination, float timeToMove)
    {
        Vector3 startPos = transform.position;
        bool reachedDest = false;
        float elapsedTime = 0f;
        m_isMoving = true;
        while (!reachedDest)
        {
            //den gan muc tieu
            if (Vector3.Distance(transform.position, destination) < 0.01f)
            {
                reachedDest = true;
                if(m_board != null)
                {
                    m_board.PlaceGamePiece(this, (int) destination.x, (int) destination.y);
                }
                break;
            }

            elapsedTime += Time.deltaTime; // so thoi gian cua frame truoc

            float t = Mathf.Clamp(elapsedTime / timeToMove, 0f, 1f);

            t = t * t*(3 - 2 * t); // làm mượt chuyển động

            transform.position = Vector3.Lerp(startPos, destination, t);

            //doi het frame
            yield return null;
        }
        m_isMoving = false;
    }

    public void ScorePoints()
    {
        if(ScoreManager.Instance != null)
        {
            ScoreManager.Instance.AddScore(scoreValue);
        } 
    }

}
