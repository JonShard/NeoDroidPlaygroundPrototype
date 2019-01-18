using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grab : MonoBehaviour
{
    public bool doGrab;
    public bool doRelease;

   // public bool IsPivot { get; private set; }
    public Transform grabbedObject;

    [Tooltip("reach of hand in radius")]
    [SerializeField]
    private float handReach = 0.5f;
    [SerializeField]
    public Transform hand;
    [SerializeField]
    private Grab otherHand;
    private Transform otherHandTransform;

    Collider[] colliderOverlap;

    Vector3 relationUnitVector;


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
        else
        {
            otherHandTransform = otherHand.GetComponent<Transform>();
        }

        colliderOverlap = new Collider[10];
    }

    // Update is called once per frame
    void Update()
    {
        if(doGrab == true)
        {
            int count = Physics.OverlapSphereNonAlloc(hand.position, handReach, colliderOverlap);
            int nearestIndex = NearestObject(count);
            if(nearestIndex >= 0)
            {
                grabbedObject = colliderOverlap[nearestIndex].GetComponent<Transform>();
            }

            if(otherHand?.grabbedObject == grabbedObject)
            {
                relationUnitVector = otherHandTransform.position - hand.position;
            }

            doGrab = false;
        }

        if (doRelease)
        {
            relationUnitVector = Vector3.zero;
            grabbedObject = null;
            doRelease = false;
        }

        DoTransformGrabbed();
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

    void DoTransformGrabbed()
    {
        if (grabbedObject == null || relationUnitVector == Vector3.zero) return;

        Vector3 currentRelationVector = otherHandTransform.position - hand.position;
        float magnitude = currentRelationVector.sqrMagnitude / relationUnitVector.sqrMagnitude;
        grabbedObject.localScale = new Vector3(relationUnitVector.magnitude, relationUnitVector.magnitude, relationUnitVector.magnitude) * magnitude;

       // Vector3 up = Vector3.Cross(relationUnitVector, currentRelationVector);

        grabbedObject.rotation = Quaternion.FromToRotation(relationUnitVector, currentRelationVector);

        grabbedObject.position = (hand.position + otherHandTransform.position) * 0.5f;
    }
}
