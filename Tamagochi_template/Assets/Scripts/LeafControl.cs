using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeafControl : MonoBehaviour
{
    private WSD_AI ai;
    private void Start()
    {
        ai = WSD_AI.instance;
    }
    private void Update()
    {
        if ((ai.detector.transform.position - transform.position).magnitude < ai.detect_range)
        {
            ai.stress-=10;
            Destroy(gameObject);
        }
    }
}
