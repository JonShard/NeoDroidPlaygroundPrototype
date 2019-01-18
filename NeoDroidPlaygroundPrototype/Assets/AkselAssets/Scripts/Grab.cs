using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grab : MonoBehaviour
{
    public bool doGrab;
    public bool doRelease;

    private bool isPivot;
    private bool isHolding;

    [Tooltip("reach of hand in radius")]
    [SerializeField]
    private float handReach = 0.5f;
    [SerializeField]
    public Transform hand;
    [SerializeField]
    public Transform otherHand;

    Collider[] colliderOverlap;


    // Start is called before the first frame update
    void Start()
    {
        if(hand == null)
        {
            hand = GetComponent<Transform>();
        }
        if(otherHand == null)
        {
            Debug.LogError("otherHand is null in grab");
        }

        colliderOverlap = new Collider[10];
    }

    // Update is called once per frame
    void Update()
    {
        if(doGrab == true)
        {
            int count = Physics.OverlapSphereNonAlloc(hand.position, handReach, colliderOverlap);
            int nearest = NearestObject(count);
        }
    }

    int NearestObject(int count)
    {
        if (count <= 0) return -1;

        Collider nearest = colliderOverlap[0];
        int closest = 0;
        for (int i = 1; i < count; i++)
        {
            if ((nearest.ClosestPoint(hand.position) - hand.position).sqrMagnitude > (colliderOverlap[i].ClosestPoint(hand.position) - hand.position).sqrMagnitude)
            {
                nearest = colliderOverlap[i];
                closest = 0;
            }
        }

        return closest;
    }
}
