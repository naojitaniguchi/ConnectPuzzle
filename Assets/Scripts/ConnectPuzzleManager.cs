using System.Collections;
using System.Collections.Generic;
using System.IO;
using Unity.VisualScripting;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;


public class ConnectPuzzleManager : MonoBehaviour
{
    [SerializeField] string MapCSVFile;
    [SerializeField] int mapInfoX = 8;
    [SerializeField] int mapInfoY = 7;
    [SerializeField] GameObject mapBaseTop;
    [SerializeField] float mapXStep = 1.0f;
    [SerializeField] float mapYStep = 1.0f;
    [SerializeField] Vector2 mapLeftTop;
    [SerializeField] GameObject mapTip;
    [SerializeField] GameObject mapTopObj;
    [SerializeField] GameObject CheckPoint;
    [SerializeField] GameObject CPTopObj;
    [SerializeField] GameObject WallVerticalObj;
    [SerializeField] GameObject WallHorizotalObj;
    [SerializeField] WallSetting[] WallSettings;
    [SerializeField] GameObject WallTopObj;
    [SerializeField] GameObject startObj;
    [SerializeField] GameObject goalObj;
    [SerializeField] int startX, startY;
    [SerializeField] int goalX, goalY;
    [SerializeField] GameObject startAndGoalTopObj;
    [SerializeField] float hitRadious = 0.3f;
    [SerializeField] GameObject hitCircleObj;
    [SerializeField] GameObject hitCircleLineTop;
    [SerializeField] GameObject lineBaseObj;
    [SerializeField] GameObject deadUIObj;
    [SerializeField] GameObject clearUIObj;
    [SerializeField] GameObject startUIObj;
    [SerializeField] GameObject checkPointMissUIObj;
    [SerializeField] TextMeshProUGUI countDownText;
    [SerializeField] TextMeshProUGUI currentScoreText;
    [SerializeField] TextMeshProUGUI timeScoreText;
    [SerializeField] TextMeshProUGUI totalScoreText;
    [SerializeField] int timeLimit;
    [SerializeField] int timeBonus = 1;
    [SerializeField] ConnectPuzzleEnemy[] enemys;
    [SerializeField] AudioSource bgm;
    [SerializeField] AudioSource se;
    [SerializeField] AudioClip pingClip;
    [SerializeField] AudioClip fanfareClip;
    [SerializeField] AudioClip dieClip;
    [SerializeField] AudioClip missClip;
    [SerializeField] AudioClip checkPointClip;
    Vector3 mouseWorldPosition;
    bool dragging = false;
    int currentIndexX = -1;
    int currentIndexY = -1;
    int lastHitX = -1;
    int lastHitY = -1;
    Vector3 lastHitPosition;
    bool dead = false;
    bool started = false;
    int currentScore = 0;
    int timeRemain = 0;
    int currentTotalScore = 0;


    // Start is called before the first frame update

    int[,] mapInfo = null;
    int[,] mapHit = null;

    void loadMapCSVFile()
    {
        mapInfo = new int[mapInfoY, mapInfoX];

        TextAsset csvFile;
        csvFile = Resources.Load(MapCSVFile) as TextAsset;
        StringReader reader = new StringReader(csvFile.text);

        int i_line = 0;
        while (reader.Peek() != -1) // reader.Peaekが-1になるまで
        {
            if (i_line < mapInfoY)
            {
                string line = reader.ReadLine(); // 一行ずつ読み込み
                string[] sprited = line.Split(','); // , 区切りでリストに追加

                for (int j = 0; j < sprited.Length; j++)
                {
                    if (j < mapInfoX)
                    {
                        mapInfo[i_line,j] = int.Parse(sprited[j]);
                    }
                }
                i_line ++;
            }
        }

        reader.Close();

        for ( int i = 0; i < mapInfoY; i++)
        {
            string line = "";
            for ( int j = 0;j < mapInfoX; j++)
            {
                // Debug.Log(mapInfo[i,j]);
                line += mapInfo[i, j].ToString();
                line += ",";
            }
            // Debug.Log(line);
        }

        mapHit = new int[mapInfoY, mapInfoX];
        for ( int i = 0;i < mapInfoY; i++)
        {
            for ( int j = 0; j < mapInfoX ; j++)
            {
                mapHit[i, j] = 0;
            }
        }
    }

    void setMap()
    {
        for ( int i = 0;i < mapInfoY;i++)
        {
            for( int j = 0; j < mapInfoX; j++)
            {
                if (mapInfo[i, j] > 0)
                {
                    Vector2 pos = mapLeftTop;
                    pos.x += (float)j * mapXStep;
                    pos.y += (float)i * mapYStep;

                    GameObject tip = Instantiate(mapTip);
                    tip.transform.position = pos;
                    tip.transform.parent = mapTopObj.transform;
                    tip.name = j.ToString() + "_" + i.ToString();
                }
            }
        }
    }

    void setCheckPoints()
    {
        for (int i = 0; i < mapInfoY; i++)
        {
            for (int j = 0; j < mapInfoX; j++)
            {
                if (mapInfo[i, j] == 2 )
                {
                    Vector2 pos = mapLeftTop;
                    pos.x += (float)j * mapXStep;
                    pos.y += (float)i * mapYStep;

                    GameObject cp = Instantiate(CheckPoint);
                    cp.transform.position = pos;
                    cp.transform.parent = CPTopObj.transform;
                    cp.name = "cp_" + j.ToString() + "_" + i.ToString();
                }
            }
        }
    }

    void setWalls()
    {
        for (int i = 0;i < WallSettings.Length; i++)
        {
            GameObject wallObj = null;
            if (WallSettings[i].type == WallSetting.WALL_TYPE.WALL_VETICAL)
            {
                wallObj = Instantiate(WallVerticalObj);
            }else if (WallSettings[i].type == WallSetting.WALL_TYPE.WALL_HOLIZOTAL)
            {
                wallObj = Instantiate(WallHorizotalObj);
            }
            if (wallObj != null)
            {
                Vector2 pos = mapLeftTop;
                pos.x += (float)(WallSettings[i].point1X + WallSettings[i].point2X) * 0.5f * mapXStep;
                pos.y += (float)(WallSettings[i].point1Y + WallSettings[i].point2Y) * 0.5f * mapYStep;
                wallObj.transform.position = pos;
                wallObj.transform.parent = WallTopObj.transform;
            }
            
        }
    }

    void setStartAndGoal()
    {
        GameObject obj = Instantiate(startObj);
        Vector2 pos = mapLeftTop;
        pos.x += (float)startX * mapXStep;
        pos.y += (float)startY * mapYStep;
        obj.transform.position = pos;

        obj.transform.parent = startAndGoalTopObj.transform;

        obj = Instantiate(goalObj);
        pos = mapLeftTop;
        pos.x += (float)goalX * mapXStep;
        pos.y += (float)goalY * mapYStep;
        obj.transform.position = pos;

        obj.transform.parent = startAndGoalTopObj.transform;

    }

    void Start()
    {
        loadMapCSVFile();
        setMap();
        setCheckPoints();
        setWalls();
        setStartAndGoal();
        countDownText.text = timeLimit.ToString();
    }

    void calcCurrentTip()
    {
        currentIndexX = -1; currentIndexY = -1;


        currentIndexX = (int)((mouseWorldPosition.x - mapLeftTop.x + mapXStep * 0.5f) / mapXStep);
        currentIndexY = (int)((mapLeftTop.y - mouseWorldPosition.y + mapYStep * -0.5f) / ( mapYStep  * -1.0f));

        // Debug.Log(mapLeftTop.y - mouseWorldPosition.y);

        if (currentIndexX < 0 || currentIndexX >= mapInfoX || currentIndexY < 0 || currentIndexY >= mapInfoY)
        {

            return;
        }

        //Debug.Log("IndexX:" + currentIndexX.ToString() + " IndexY:" + currentIndexY.ToString());
        // Debug.Log(mapInfo[currentIndexY, currentIndexX]);

        if (mapInfo[currentIndexY,currentIndexX] == 0)
        {
            currentIndexX = -1;
            currentIndexY = -1;
        }

       Debug.Log(currentIndexX);
       Debug.Log(currentIndexY);

    }

    bool checkCheckPoints()
    {
        for (int i = 0; i < mapInfoY; i++)
        {
            for (int j = 0; j < mapInfoX; j++)
            {
                if (mapInfo[i, j] == 2)
                {
                    if (mapHit[i, j] != 1)
                    {
                        return false;
                    }
                }
            }
        }

        return true;
    }

    void goalEndGame()
    {
        started = false;
        if ( checkCheckPoints() )
        {
            bgm.Stop();
            countCurrentScore();
            clearUIObj.SetActive(true);
            se.PlayOneShot(fanfareClip);
        }
        else
        {
            bgm.Stop();
            checkPointMissUIObj.SetActive(true);
            se.PlayOneShot(missClip);
        }
        
    }

    void checkHit()
    {
        if ( currentIndexX == -1 || currentIndexY == -1)
        {
            return;
        }

        Vector2 center2 = mapLeftTop;
        center2.x += (float)currentIndexX * mapXStep;
        center2.y += (float)currentIndexY * mapYStep;
        Vector3 center3;
        center3.x = center2.x;
        center3.y = center2.y;
        center3.z = 0.0f;

        if ( Vector3.Distance( center3, mouseWorldPosition) < hitRadious )
        {
            if (mapHit[currentIndexY,currentIndexX] == 0)
            {
                if ( ( currentIndexX == startX && currentIndexY == startY ) || ( currentIndexX == lastHitX ) || (currentIndexY == lastHitY ))
                {
                    // 壁の判定を入れる
                    bool hitWall = false;
                    for (int i = 0; i < WallSettings.Length; i++)
                    {
                        if (WallSettings[i].CheckCrossWall(currentIndexX, currentIndexY, lastHitX, lastHitY))
                        {
                            hitWall = true;
                        }
                    }
                    if ( !hitWall )
                    {
                        GameObject hit = Instantiate(hitCircleObj);
                        hit.transform.position = center3;
                        hit.transform.parent = hitCircleLineTop.transform;
                        mapHit[currentIndexY, currentIndexX] = 1;

                        if (!(currentIndexX == startX && currentIndexY == startY))
                        {
                            GameObject line = Instantiate(lineBaseObj);
                            line.transform.position = center3;
                            LineRenderer lineRenderer = line.GetComponent<LineRenderer>();
                            lineRenderer.startWidth = 0.2f;
                            lineRenderer.endWidth = 0.2f;
                            lineRenderer.positionCount = 2;
                            lineRenderer.SetPosition(0, lastHitPosition);
                            lineRenderer.SetPosition(1, center3);
                            line.transform.parent = hitCircleLineTop.transform;

                            if (mapInfo[currentIndexY, currentIndexX] == 1)
                            {
                                se.PlayOneShot(pingClip);
                            }else if (mapInfo[currentIndexY, currentIndexX] == 2)
                            {
                                se.PlayOneShot(checkPointClip);
                            }

                            // ゴール判定
                            if ( currentIndexX == goalX && currentIndexY == goalY)
                            {
                                goalEndGame();
                            }
                        }

                        lastHitX = currentIndexX;
                        lastHitY = currentIndexY;
                        lastHitPosition = center3;
                    }
                    
                }
                
            }
        }
        // Debug.Log(center);


    }

    bool checkEnemyHit()
    {
        for (int i = 0; i < enemys.Length; i ++)
        {
            if (enemys[i] != null)
            {
                if (enemys[i].mouseHitCheck(mouseWorldPosition) ) 
                {
                    started = false;
                    dead = true;
                    bgm.Stop();
                    deadUIObj.SetActive(true);
                    se.PlayOneShot(dieClip);
                    return true;
                }
            }
        }
        return false;
    }

    // Update is called once per frame
    void Update()
    {
        if ( !started )
        {
            return;
        }
        if ( Input.GetMouseButtonDown(0))
        {
            Vector3 thisPosition = Input.mousePosition;
            //スクリーン座標→ワールド座標
            mouseWorldPosition = Camera.main.ScreenToWorldPoint(thisPosition);
            mouseWorldPosition.z = 0f;

            // Debug.Log(mouseWorldPosition);
            if ( !checkEnemyHit())
            {
                calcCurrentTip();
                checkHit();
            }
            dragging = true;
        }
        if ( Input.GetMouseButtonUp(0))
        {
            dragging = false;
        }
        if ( dragging )
        {
            Vector3 thisPosition = Input.mousePosition;
            //スクリーン座標→ワールド座標
            mouseWorldPosition = Camera.main.ScreenToWorldPoint(thisPosition);
            mouseWorldPosition.z = 0f;
            if (!checkEnemyHit())
            {
                calcCurrentTip();
                checkHit();
            }
            // Debug.Log(mouseWorldPosition);
        }
    }

    public Vector3 calcCenterPosFromIndex( int indexX, int indexY)
    {
        Vector2 pos = mapLeftTop;
        pos.x += (float)indexX * mapXStep;
        pos.y += (float)indexY * mapYStep;

        Vector3 vector3 = pos;
        vector3.z = 0f;

        return vector3;
    }

    IEnumerator CountDownTimer()
    {
        timeRemain = timeLimit;
        for ( int i = timeLimit; i > 0; i-- )
        {
           if ( dead)
            {
                yield return null;
            }
           if( !started )
            {
                yield return null;
            }
            yield return new WaitForSeconds(1.0f);
            countDownText.text = i.ToString();
            timeRemain = i;
        }
        dead = true;

        deadUIObj.SetActive(true);
    }

    public void puzzleGameStart()
    {
        dead = false;
        started = true;
        deadUIObj.SetActive(false);
        StartCoroutine(CountDownTimer());
        startUIObj.SetActive(false);

        bgm.Play();
    }

    public bool isStarted()
    {
        return started;
    }

    public void retry()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void countCurrentScore()
    {
        currentScore = 0;
        for (int i = 0; i < mapInfoY; i++)
        {
            for (int j = 0; j < mapInfoX; j++)
            {
                if (mapHit[i, j] == 1)
                {
                    currentScore++;
                }
            }
        }
        currentScoreText.text = currentScore.ToString();
        int timeBonusScore = timeRemain * timeBonus;
        timeScoreText.text = timeBonusScore.ToString();

        currentTotalScore = currentScore + timeBonusScore;
        totalScoreText.text = currentTotalScore.ToString();
    }

    public void countScoreAndSetScoreToManager()
    {

        ScoreManager.Instance.Score += currentTotalScore;
        ScoreManager.Instance.CleardStageCount++;
    }

}
