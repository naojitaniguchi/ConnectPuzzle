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
}
