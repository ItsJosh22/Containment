using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
public class FastIKFabric : MonoBehaviour
{
    //ChainLength
    public int ChainLength = 2;

    //Target chain should be bent to
    public Transform Target;
    public Transform Pole;


    public float distance;
    // Solver interations per update
    [Header("Solver Parameters")]
    public int Interations = 10;

    //Distance when the solver stops
    public float Delta = 0.001f;

    //Strenght of going back to start pos
    [Range(0, 1)]
    public float SnapBackStrength = 1f;


    protected float[] BonesLenght;
    protected float CompleteLength;
    protected Transform[] Bones;
    protected Vector3[] Positions;

    protected Vector3[] StartDirectionSucc;
    protected Quaternion[] StartRotationBone;
    protected Quaternion StartRotationTarget;
    protected Quaternion StartRotationRoot;


    void Initialize()
    {
        Bones = new Transform[ChainLength + 1];
        Positions = new Vector3[ChainLength + 1];
        BonesLenght = new float[ChainLength];
        StartDirectionSucc = new Vector3[ChainLength + 1];
        StartRotationBone = new Quaternion[ChainLength + 1];


        if (Target == null)
        {
            Target = new GameObject(gameObject.name + " Target").transform;
            Target.position = transform.position;
        }
        StartRotationTarget = Target.rotation;


        CompleteLength = 0;

        var current = transform;
        for (var i = Bones.Length - 1; i >= 0 ; i--)
        {
            Bones[i] = current;
            StartRotationBone[i] = current.rotation;
            if (i == Bones.Length - 1)
            {
                StartDirectionSucc[i] = Target.position - current.position;
            }
            else
            {
                StartDirectionSucc[i] = Bones[i + 1].position - current.position;
                BonesLenght[i] = StartDirectionSucc[i].magnitude;
                CompleteLength += BonesLenght[i];
            }

            current = current.parent;
        }
    }


    private void Awake()
    {
        Initialize();
    }


    private void LateUpdate()
    {
        //distance = Vector3.Distance(Target.position, Bones[ChainLength - 1].position);
        //if (distance > 7)
        //{
        //    Target.position += new Vector3(10, 0, 0);
        //}
        ResolveIK();


        


    }


    private void ResolveIK()
    {
        if (Target == null)
        {
            return;
        }

        if (BonesLenght.Length != ChainLength)
        {
            Initialize();
        }


        //Get pos
        for (int i = 0; i < Bones.Length; i++)
        {
            Positions[i] = Bones[i].position;
        }

        var RootRot = (Bones[0].parent != null) ? Bones[0].parent.rotation : Quaternion.identity;
        var RootRotDiff = RootRot * Quaternion.Inverse(StartRotationRoot);


        // math
        if ((Target.position - Bones[0].position).sqrMagnitude >= CompleteLength * CompleteLength)
        {
            var direction = (Target.position - Positions[0]).normalized;

            for (int i = 1; i < Positions.Length; i++)
            {
                Positions[i] = Positions[i - 1] + direction * BonesLenght[i - 1];
            }
        }
        else
        {
            for (int i = 0; i < Positions.Length - 1; i++)
            {
                Positions[i + 1] = Vector3.Lerp(Positions[i + 1], Positions[i] + RootRotDiff * StartDirectionSucc[i], SnapBackStrength);
            }

            for (int iteration = 0; iteration < Interations; iteration++)
            {

                for (int i = Positions.Length - 1; i > 0; i--)
                {
                    if (i == Positions.Length - 1)
                    {
                        Positions[i] = Target.position;
                    }
                    else
                    {
                        Positions[i] = Positions[i + 1] + (Positions[i] - Positions[i + 1]).normalized * BonesLenght[i];
                    }
                }


                for (int i = 1; i < Positions.Length; i++)
                {
                    Positions[i] = Positions[i - 1] + (Positions[i] - Positions[i - 1]).normalized * BonesLenght[i - 1];
                }


                // stop if close to target
                if ((Positions[Positions.Length - 1] - Target.position).sqrMagnitude < Delta * Delta)
                {
                    break;
                }
            }
        }

        if (Pole != null)
        {
            for (int i = 1; i < Positions.Length - 1; i++)
            {
                var plane = new Plane(Positions[i + 1] - Positions[i - 1], Positions[i - 1]);
                var projectilePole = plane.ClosestPointOnPlane(Pole.position);
                var projectileBone = plane.ClosestPointOnPlane(Positions[i]);
                var angle = Vector3.SignedAngle(projectileBone - Positions[i - 1], projectilePole - Positions[i - 1],plane.normal);
                Positions[i] = Quaternion.AngleAxis(angle, plane.normal) * (Positions[i] - Positions[i - 1]) + Positions[i - 1];
            }
        }




        //Set pos
        for (int i = 0; i < Positions.Length; i++)
        {
            if (i == Positions.Length - 1)
            {
                Bones[i].rotation = Target.rotation * Quaternion.Inverse(StartRotationTarget) * StartRotationBone[i];
            }
            else
            {
                Bones[i].rotation = Quaternion.FromToRotation(StartDirectionSucc[i], Positions[i + 1] - Positions[i]) * StartRotationBone[i];
            }

           Bones[i].position = Positions[i];
        }

    }








    private void OnDrawGizmos()
    {
       // var current = this.transform;
        //for (int i = 0; i < ChainLength && current != null && current.parent != null; i++)
        //{
        //    var scale = Vector3.Distance(current.position, current.parent.position) * 0.1f;
        //    Handles.matrix = Matrix4x4.TRS(current.position, Quaternion.FromToRotation(Vector3.up, current.parent.position - current.position), new Vector3(scale,Vector3.Distance(current.parent.position,current.position),scale));
        //    Handles.color = Color.green;
        //    Handles.DrawWireCube(Vector3.up * 0.5f, Vector3.one);

        //    current = current.parent;

        //}
    }
}
