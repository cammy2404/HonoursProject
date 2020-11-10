using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class ArmIK : MonoBehaviour
{

    public int chainLength = 2;

    public Transform target;
    [Range(0, 1)]
    public int hand;
    public Transform pole;

    [Header("Solver Parameters")]
    public int iterations = 10;
    public float delta = 0.001f;

    [Range(0, 1)]
    public float snapBackStrength = 1f;

    protected float[] bonesLength;
    protected float completeLength;
    protected Transform[] bones;
    protected Vector3[] positions;

    protected Vector3[] startDirectionSucc;
    protected Quaternion[] startRotationBone;
    protected Quaternion startRotationTarget;
    protected Quaternion startRotationRoot;

    private Transform controller = null;
    private readonly string _righthand_ = "RightHand";
    private readonly string _lefthand_ = "LeftHand";

    void Awake()
    {
        Init();
    }

    void Init()
    {
        bones = new Transform[chainLength + 1];
        positions = new Vector3[chainLength + 1];
        bonesLength = new float[chainLength];
        startDirectionSucc = new Vector3[chainLength + 1];
        startRotationBone = new Quaternion[chainLength + 1];

        if (target == null)
        {
            target = new GameObject(gameObject.name + "Target").transform;
            target.position = transform.position;
        }

        startRotationTarget = target.rotation;
        completeLength = 0;

        Transform current = transform;
        for (int i = bones.Length - 1; i >= 0; i--)
        {
            bones[i] = current;
            startRotationBone[i] = current.rotation;

            if (i == bones.Length - 1)
            {
                startDirectionSucc[i] = target.position - current.position;
            }
            else
            {
                bonesLength[i] = (bones[i + 1].position - current.position).magnitude;
                completeLength += bonesLength[i];
            }

            current = current.parent;
        }

        }

    void Update()
    {
        if (controller == null)
        {
            string targetTag = (hand == 0) ? _lefthand_ : _righthand_;
            try
            {
                controller = GameObject.FindGameObjectWithTag(targetTag).transform;
            }
            catch
            {
                //Debug.LogWarning("Hands not found");
            }
        } else
        {
            target.position = controller.position;
            target.rotation = controller.rotation;
        }
    }

    void LateUpdate()
    {
        ResolveIK();
    }


    // Fabric IK Method
    private void ResolveIK()
    {
        if (target == null) 
            return;

        if (bonesLength.Length != chainLength) 
            Init();

        // GET positions
        for (int i = 0; i < bones.Length; i++)
            positions[i] = bones[i].position;

        Quaternion rootRot = (bones[0].parent != null) ? bones[0].parent.rotation : Quaternion.identity;
        Quaternion rootRotDiff = rootRot * Quaternion.Inverse(startRotationRoot);

        // CALCULATE new positions
        if ((target.position - bones[0].position).sqrMagnitude >= (completeLength * completeLength)) // Squaring both sides of the equation makes the process faster as there is no need to calculate a square root
        {
            Vector3 direction = (target.position - positions[0]).normalized;

            for (int i = 1; i < positions.Length; i++)
                positions[i] = positions[i - 1] + direction * bonesLength[i - 1];

        } else
        {
            for (int i = 0; i < iterations; i++)
            {
                // BACK
                for (int j = positions.Length - 1; j > 0; j--)
                {
                    if (j == positions.Length - 1)
                        positions[j] = target.position;
                    else
                        positions[j] = positions[j + 1] + (positions[j] - positions[j + 1]).normalized * bonesLength[j];
                }

                // FORWARD
                for (int j = 1; j < positions.Length; j++)
                    positions[j] = positions[j - 1] + (positions[j] - positions[j - 1]).normalized * bonesLength[j - 1];


                // Within close enough range
                if ((positions[positions.Length - 1] - target.position).sqrMagnitude < delta * delta)
                    break;
            }
        }


        // CALCULATE rotation towards pole
        if (pole != null)
        {
            for (int i = 1; i < positions.Length - 1; i++)
            {
                Plane plane = new Plane(positions[i + 1] - positions[i - 1], positions[i - 1]);
                Vector3 projectedPole = plane.ClosestPointOnPlane(pole.position);
                Vector3 projectedBone = plane.ClosestPointOnPlane(positions[i]);
                float angle = Vector3.SignedAngle(projectedBone - positions[i - 1], projectedPole - positions[i - 1], plane.normal);
                positions[i] = Quaternion.AngleAxis(angle, plane.normal) * (positions[i] - positions[i - 1]) + positions[i - 1];
            }
        }


        // SET positions and rotations
        for (int i = 0; i < positions.Length; i++)
        {
            if (i == positions.Length - 1)
                bones[i].rotation = target.rotation * Quaternion.Inverse(startRotationTarget) * startRotationBone[i];
            else
                bones[i].rotation = Quaternion.FromToRotation(startDirectionSucc[i], positions[i + 1] - positions[i]) * startRotationBone[i];

            bones[i].position = positions[i];
        }
    }

    void OnDrawGizmos()
    {
        Transform current = transform;
        for (int i = 0; i < chainLength && current != null && current.parent != null; i++)
        {
            float scale = Vector3.Distance(current.position, current.parent.position) * 0.1f;

            Handles.matrix = Matrix4x4.TRS(current.position, Quaternion.FromToRotation(Vector3.up, current.parent.position - current.position), new Vector3(scale, Vector3.Distance(current.parent.position, current.position), scale));
            Handles.color = Color.green;
            Handles.DrawWireCube(Vector3.up * 0.5f, Vector3.one);

            current = current.parent;
        }
    }
}
