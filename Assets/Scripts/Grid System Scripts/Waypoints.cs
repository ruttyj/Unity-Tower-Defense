using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Waypoints : MonoBehaviour
{
    [SerializeField]
    GameObject m_waypoint;

    private Transform startTransform;
    private List<Transform> m_waypointList;

    /*
     * Awake is called before the first frame update
     * and after initialization of all objects
     */
    private void Awake()
    {
        m_waypointList = new List<Transform>();
    }

    public void CreateWaypoints()
    {
        Debug.Log("creating waypoints");

        // get necessary variables from Parent:Map
        MapV2 map = gameObject.GetComponentInParent<MapV2>();
        List<PathSegment> pathSegments = map.GetPathSegments();

        // initialize starting point (spawn point for mobs)
        NodeV2 node = map.GetNode(pathSegments[0].start.y, pathSegments[0].start.x);
        GameObject obj = Instantiate(m_waypoint, node.transform.position + node.GetEnvOffset(), Quaternion.identity) as GameObject;
        obj.name = "StartingPoint";
        obj.transform.parent = gameObject.transform;
        startTransform = obj.transform;

        // for each end node of the segment, create a waypoint for mobs to go towards
        for (int i = 0; i < pathSegments.Count; i++)
        {
            node = map.GetNode(pathSegments[i].end.y, pathSegments[i].end.x);
            obj = Instantiate(m_waypoint, node.transform.position + node.GetEnvOffset(), Quaternion.identity) as GameObject;
            obj.name = "Waypoint" + (i + 1).ToString();
            obj.transform.parent = gameObject.transform;
            m_waypointList.Add(obj.transform);
        }
    }

    public Transform GetStartingPoint()
    {
        return startTransform;
    }

    public Transform GetNextWaypoint(int nextWaypointIndex)
    {
        if (nextWaypointIndex == m_waypointList.Count)
            return null;
        return m_waypointList[nextWaypointIndex];
    }

    public void ResetWaypoints()
    {
        for (int i = 0; i < m_waypointList.Count; i++)
        {
            Destroy(m_waypointList[i].gameObject);
        }
        m_waypointList.RemoveAll(x => x == null);
        Awake();
    }
}
