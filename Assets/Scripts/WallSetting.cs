using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;


public class WallSetting : MonoBehaviour
{
    public enum WALL_TYPE{ WALL_VETICAL, WALL_HOLIZOTAL};
    public WALL_TYPE type;
    public int point1X;
    public int point1Y;
    public int point2X;
    public int point2Y;

    public bool CheckCrossWall( int p1X, int p1Y, int p2X, int p2Y)
    {
        if ( ( point1X == p1X && point1Y == p1Y ) && ( point2X == p2X && point2Y == p2Y ))
        {
            return true;
        }
        if ((point1X == p2X && point1Y == p2Y) && (point2X == p1X && point2Y == p1Y))
        {
            return true;
        }
        return false;
    }
}
