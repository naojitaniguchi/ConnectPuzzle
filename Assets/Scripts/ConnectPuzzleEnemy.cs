using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConnectPuzzleEnemy : MonoBehaviour
{
    
    public enum ENEMY_TYPE { ENEMY_VETICAL, ENEMY_HOLIZOTAL };
    [SerializeField] ENEMY_TYPE enemyType;
    public enum ENEMY_MOVE_DIR_TYPE { ENEMY_MOVE_1_TO_2, ENEMY_MOVE_2_TO_1 };
    [SerializeField] ENEMY_MOVE_DIR_TYPE moveDirType;
    [SerializeField] int point1X;
    [SerializeField] int point1Y;
    [SerializeField] int point2X;
    [SerializeField] int point2Y;
    [SerializeField] int startX;
    [SerializeField] int startY;

    [SerializeField] float speed;
    [SerializeField] float hitRadious = 0.4f;
    Vector3 point1;
    Vector3 point2;
    Vector3 startPoint;

    ConnectPuzzleManager connectPuzzleManager = null;

    // Start is called before the first frame update
    void Start()
    {
        GameObject managerObject = GameObject.FindGameObjectWithTag("ConnectPuzzleManager");
        if (managerObject != null)
        {
            connectPuzzleManager = managerObject.GetComponent<ConnectPuzzleManager>();
        }
        if (connectPuzzleManager == null)
        {
            return;
        }

        point1 = connectPuzzleManager.calcCenterPosFromIndex(point1X, point1Y);
        point2 = connectPuzzleManager.calcCenterPosFromIndex(point2X, point2Y);
        startPoint = connectPuzzleManager.calcCenterPosFromIndex(startX, startY);

        gameObject.transform.position = startPoint;

    }

    // Update is called once per frame
    void Update()
    {
        if (connectPuzzleManager != null)
        {
            if ( !connectPuzzleManager.isStarted() )
            {
                return;
            }
            if ( moveDirType == ENEMY_MOVE_DIR_TYPE.ENEMY_MOVE_1_TO_2 )
            {
                if ( enemyType == ENEMY_TYPE.ENEMY_VETICAL)
                {
                    float yDir = point2.y - point1.y;
                    yDir /= Mathf.Abs(yDir);
                    gameObject.transform.Translate( Vector3.up * yDir * speed * Time.deltaTime);
                    if ( yDir > 0.0f)
                    {
                        if ( gameObject.transform.position.y > point2.y )
                        {
                            moveDirType = ENEMY_MOVE_DIR_TYPE.ENEMY_MOVE_2_TO_1;
                            gameObject.transform.position = point2;
                        }
                    }
                    else if ( yDir < 0.0f )
                    {
                        if (gameObject.transform.position.y < point2.y)
                        {
                            moveDirType = ENEMY_MOVE_DIR_TYPE.ENEMY_MOVE_2_TO_1;
                            gameObject.transform.position = point2;
                        }
                    }
                }
                if (enemyType == ENEMY_TYPE.ENEMY_HOLIZOTAL)
                {
                    float xDir = point2.x - point1.x;
                    xDir /= Mathf.Abs(xDir);
                    gameObject.transform.Translate(Vector3.right * xDir * speed * Time.deltaTime);
                    if (xDir > 0.0f)
                    {
                        if (gameObject.transform.position.x > point2.x)
                        {
                            moveDirType = ENEMY_MOVE_DIR_TYPE.ENEMY_MOVE_2_TO_1;
                            gameObject.transform.position = point2;
                        }
                    }else if ( xDir < 0.0f)
                    {
                        if (gameObject.transform.position.x < point2.x)
                        {
                            moveDirType = ENEMY_MOVE_DIR_TYPE.ENEMY_MOVE_2_TO_1;
                            gameObject.transform.position = point2;
                        }
                    }
                }
            }else if (moveDirType == ENEMY_MOVE_DIR_TYPE.ENEMY_MOVE_2_TO_1)
            {
                if (enemyType == ENEMY_TYPE.ENEMY_VETICAL)
                {
                    float yDir = point1.y - point2.y;
                    yDir /= Mathf.Abs(yDir);
                    gameObject.transform.Translate(Vector3.up * yDir * speed * Time.deltaTime);
                    if (yDir > 0.0f)
                    {
                        if (gameObject.transform.position.y > point1.y)
                        {
                            moveDirType = ENEMY_MOVE_DIR_TYPE.ENEMY_MOVE_1_TO_2;
                            gameObject.transform.position = point1;
                        }
                    }
                    else if (yDir < 0.0f)
                    {
                        if (gameObject.transform.position.y < point1.y)
                        {
                            moveDirType = ENEMY_MOVE_DIR_TYPE.ENEMY_MOVE_1_TO_2;
                            gameObject.transform.position = point1;
                        }
                    }
                }
                if (enemyType == ENEMY_TYPE.ENEMY_HOLIZOTAL)
                {
                    float xDir = point1.x - point2.x;
                    xDir /= Mathf.Abs(xDir);
                    gameObject.transform.Translate(Vector3.right * xDir * speed * Time.deltaTime);
                    if (xDir > 0.0f)
                    {
                        if (gameObject.transform.position.x > point1.x)
                        {
                            moveDirType = ENEMY_MOVE_DIR_TYPE.ENEMY_MOVE_1_TO_2;
                            gameObject.transform.position = point1;
                        }
                    }
                    else if (xDir < 0.0f)
                    {
                        if (gameObject.transform.position.x < point1.x)
                        {
                            moveDirType = ENEMY_MOVE_DIR_TYPE.ENEMY_MOVE_1_TO_2;
                            gameObject.transform.position = point1;
                        }
                    }
                }
            }

            // マウスの位置をチェック

        }
    }

    public bool mouseHitCheck( Vector3 mouseWorldPos)
    {
        Vector3 mp = mouseWorldPos;
        mp.z = 0;

        Vector3 ep = gameObject.transform.position;
        ep.z = 0;

        // Debug.Log( "mp:" + mp.ToString() + " ep:" + ep.ToString() + " dist:" + Vector3.Distance(mp, ep).ToString());
        if ( Vector3.Distance(mp, ep) < hitRadious)
        {
            // Debug.Log("Hit!!");
            return true;
            
        }
        else
        {
            return false;
        }
    }
}
