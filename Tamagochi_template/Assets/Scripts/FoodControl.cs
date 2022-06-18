using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodControl : MonoBehaviour
{
    private WSD_AI ai;
    private void Start()
    {
        ai = WSD_AI.instance;
    }
    private void Update()
    {
        if ((ai.detector.transform.position - transform.position).magnitude < ai.detect_range && ai.food<ai.food_max) {

            ai.food += 300;
            Destroy(gameObject);
        }
    }
}
