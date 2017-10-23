using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Description : Hermite Curve creation using particles
public class ProceduralMeshgen : MonoBehaviour
{
    [Header("Generating Trail")]
    [Tooltip("How dense the point should be generated between distance the head traveling")]
    public float IntervalDist = 1f;

    static float accumulatedDist = 0f;
    static float currentDist = 0f;

    public GameObject Source;

    public float UVScale = 0.5f;

    int NodeCount = 0;

    int MaxNodeNumber = 5000;

    private Vector3[] nodes;
    private Vector3 prevPos;

    [HideInInspector]
    public GameObject MeshObj;

    [Space(10)]
    [Header("Mesh Shape")]
    [Tooltip("How many actual vertices needed to create this curve")]
    public float Width = 4f;

    public Material Mat;

    private MeshFilter Filter;
    private List<Vector3> Vertices = new List<Vector3>();
    private List<Vector2> UVs = new List<Vector2>();
    private List<int> Triangles = new List<int>();
    private Vector3[] CrossVectors = new Vector3[2];

    void Start()
    {
        ResetNodes();
    }

    public void ResetNodes()
    {
        NodeCount = 0;
        prevPos = Source.transform.position;

        nodes = new Vector3[MaxNodeNumber];
    }

    public void DetachMesh() {
        if (MeshObj) {
            ResetNodes();

            MeshObj.transform.parent = null;
            CreateNewMesh();
        }
        
    }

    void CreateNewMesh() {
        MeshObj = new GameObject();
        MeshObj.AddComponent<MeshFilter>();
        MeshObj.AddComponent<MeshRenderer>();
        MeshObj.transform.parent = transform;
        MeshObj.transform.position = new Vector3(0f, 0f, 0f);
        MeshObj.transform.localScale = new Vector3(1f, 1f, 1f);
        MeshObj.name = "ProceduralMesh";
        Filter = MeshObj.GetComponent<MeshFilter>();
        Filter.sharedMesh = new Mesh();
    }

    // Update is called once per frame
    void Update()
    {
        float dist = Vector3.Distance(Source.transform.position, prevPos);
        if (dist > IntervalDist)
        {
            AddNode(prevPos);
            prevPos = Source.transform.position;
        }

        if (!MeshObj)
        {
            CreateNewMesh();
        }
        else if (MeshObj && !Filter)
        {
            Filter = MeshObj.GetComponent<MeshFilter>();
            Filter.sharedMesh = new Mesh();
        }
        CreateProceduralMesh();
    }

    void AddNode(Vector3 position)
    {
        if (NodeCount >= MaxNodeNumber)
        {
            Debug.Log("at max number!");
        }

        position.y = Time.timeSinceLevelLoad * 0.001f;
        nodes[NodeCount] = position;
        NodeCount++; // last node is always not ready
    }

    private void CalculateSideVectors(int i, float WidthR, float WidthL)
    {
        Vector3 CrossL = Vector3.zero;
        Vector3 CrossR = Vector3.zero;

        Vector3 upvector = nodes[i];
        upvector.y = 100f;
        upvector = upvector - nodes[i];
        upvector.Normalize();

        currentDist = Vector3.Distance(nodes[i + 1], nodes[i]);

        Vector3 tangent = (nodes[i + 1] - nodes[i]).normalized;
        CrossL = Vector3.Cross(upvector, tangent).normalized;
        CrossR = CrossL * -1f;

        Vector3 r = nodes[i] + CrossR * WidthR;
        Vector3 l = nodes[i] + CrossL * WidthL;

        CrossVectors[0] = r;
        CrossVectors[1] = l;

        accumulatedDist += currentDist * UVScale;
        if (accumulatedDist > 1f)
        {
            accumulatedDist = 0;
            accumulatedDist += currentDist * UVScale;
        }
    }

    private void AddCurvePoint(Vector3 R, Vector3 L, int id, int count)
    {
        
        int start;

        Vertices.Add(R);
        Vertices.Add(L);

        UVs.Add(new Vector2(accumulatedDist, 0f));
        UVs.Add(new Vector2(accumulatedDist, 1f));

        if (Vertices.Count >= 4)
        {
            start = Vertices.Count - 4;
            Triangles.Add(start + 0);
            Triangles.Add(start + 1);
            Triangles.Add(start + 2);
            Triangles.Add(start + 1);
            Triangles.Add(start + 3);
            Triangles.Add(start + 2);
        }
    }

    private void CreateProceduralMesh()
    {
        Vertices.Clear();
        Triangles.Clear();
        UVs.Clear();

        if (nodes != null)
        {
            for (int i = 0; i < NodeCount - 1; i++)
            {
                CalculateSideVectors(i, Width, Width);
                AddCurvePoint(CrossVectors[1], CrossVectors[0], i, nodes.Length);
            }
        }

        Mesh mesh = Filter.sharedMesh;
        mesh.Clear();

        mesh.vertices = Vertices.ToArray();

        mesh.uv = UVs.ToArray();

        mesh.triangles = Triangles.ToArray();

        mesh.RecalculateBounds();

        if (MeshObj != null)
        {
            MeshObj.transform.position = Vector3.zero;
            MeshObj.transform.localScale = Vector3.one;
            MeshObj.transform.rotation = Quaternion.identity;
            MeshObj.GetComponent<MeshRenderer>().material = Mat;
        }
    }
}
