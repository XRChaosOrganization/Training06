using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimerComponent : MonoBehaviour
{
    public Transform transformRoot; 
    public LineRenderer line; 
    private List<Transform> path = new List<Transform>(); 
    public float totalDistanceOfTimer = 0.0f; 

    void Start()
    {
        RebuildTimer();
    }

    public void RebuildTimer ()
    {
        line.positionCount = transformRoot.childCount;
        path.Clear(); 

        for (int i = 0; i < transformRoot.childCount; i++)
        {
            Transform trans = transformRoot.GetChild(i);
            path.Add(trans);
            line.SetPosition(i, trans.position);
        }

        for (int i = 0; i < path.Count; i++)
        {
            if(i >= path.Count - 1)
                break;
            totalDistanceOfTimer += Vector3.Distance(path[i].position, path[i + 1].position);
        }
    }

    public void UpdateTimer (float _timerValue, float _remainingTime)
    {
        //remaining time / max time 
        float progress = _remainingTime / _timerValue;
        progress /= totalDistanceOfTimer; 

        line.SetPosition(0, Vector3.Lerp(line.GetPosition(0), path[0 + 1].position, Time.deltaTime));

        if(Vector3.Distance(line.GetPosition(0), line.GetPosition(0 + 1)) <= 0.1f)
        {
            path.RemoveAt(0);
            line.positionCount = path.Count;
            for (int i = 0; i < path.Count; i++)
            {
                line.SetPosition(i, path[i].position);
            }
        }
    }
}
