using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class WaypointManager : MonoBehaviour
{
    [Inject]
    MapController mapController;

    public List<Transform> waypoints = new List<Transform>(); 
    private void Start()
    {
        foreach (var item in mapController.PathElements)
        {
            waypoints.Add(item.transform);  
        } 
    }

    public Transform GetFirstPoint()
    {
        return waypoints[0];
    }

    public Transform GetNextPoint(Transform currentPoint)
    {
        int currentIndex = waypoints.IndexOf(currentPoint);
        if (currentIndex < waypoints.Count - 1)
        {
            return waypoints[currentIndex + 1];
        }
        else
        {
            return null;
        }
    }

}
