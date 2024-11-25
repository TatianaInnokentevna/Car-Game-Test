using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class RoutePoint
{
    public Vector3 position;
    public Quaternion rotation;

    public RoutePoint(Vector3 pos, Quaternion rot)
    {
        position = pos;
        rotation = rot;
    }
}

[System.Serializable]
public class RouteData
{
    public List<RoutePoint> points = new List<RoutePoint>();
}
