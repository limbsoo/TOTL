using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goal : MonoBehaviour, Spawn
{
    Vector3[] _gridCenters;
    Transform _mapTransform;

    public void Init(Vector3[] gridCenters, Transform mapTransform)
    {
        //gameObject.SetActive(false);
        _gridCenters = gridCenters;
        _mapTransform = mapTransform;
    }



}
