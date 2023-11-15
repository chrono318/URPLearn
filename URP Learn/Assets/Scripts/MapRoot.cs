using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapRoot : MonoBehaviour
{
    public int width = 20;
    public GameObject[] cubes;
    public Material[] materials;
    public List<Vector2Int> obstancles;

    public Vector2Int startPoint;
    public Vector2Int endPoint;

    const string BASECOLOR = "_BaseColor";
    [ContextMenu("生成map")]
    void GenerateCubes()
    {
        for(int i = 0; i < cubes.Length; i++)
        {
            DestroyImmediate(cubes[i]);
            DestroyImmediate(materials[i]);
        }
        cubes = new GameObject[width * width];
        materials = new Material[width * width];
        for(int i = 0; i < width; i++)
        {
            for(int j = 0; j < width; j++)
            {
                GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
                cubes[i * width + j] = cube;
                cube.transform.parent = transform;
                cube.transform.position = new Vector3(j + (j - 1) * 0.5f, i + (i - 1) * 0.5f, 0);
                Material mat = new Material(Shader.Find("Custom RP/Unlit"));
                cube.GetComponent<MeshRenderer>().sharedMaterial = mat;
                mat.color = Color.white;
                materials[i * width + j] = mat;
            }
        }
    }
    [ContextMenu("更新障碍物")]
    void UpdateObstancles()
    {
        for(int i = 0; i < cubes.Length; i++)
        {
            materials[i].SetColor(BASECOLOR, Color.white);
        }
        for(int i = 0; i < obstancles.Count; i++)
        {
            SetColor(obstancles[i], Color.black);
        }
        SetColor(startPoint, Color.blue);
        SetColor(endPoint, Color.red);
    }
    [ContextMenu("生成路径")]
    void FindPath()
    {
        List<Vector2Int> list = BFS();
        if(list != null)
        {
            for(int i = 0; i < list.Count; i++)
            {
                SetColor(list[i], Color.blue);
            }
        }
        else
        {
            Debug.Log("没找到终点");
        }
        SetColor(startPoint, Color.blue);
    }

    #region BFS
    List<Vector2Int> _CheckedPoints;
    List<Vector2Int> _InQueuePoints;
    List<int> _LastPoints = new List<int>();

    List<Vector2Int> BFS()
    {
        List<Vector2Int> list = new List<Vector2Int>();
        Queue<Vector2Int> queue = new Queue<Vector2Int>();
        _CheckedPoints = new List<Vector2Int>();
        _InQueuePoints = new List<Vector2Int>();
        _LastPoints = new List<int>();

        if (startPoint == endPoint) return list;
        _LastPoints.Add(-1);
        _InQueuePoints.Add(startPoint);
        PushChildrenQueue(ref queue, startPoint);
        while (queue.Count > 0)
        {
            Vector2Int p = queue.Dequeue();
            if(p == endPoint)
            {
                int index = _CheckedPoints.Count;
                int lastID = _LastPoints[index];
                while(lastID > 0)
                {
                    list.Add(_CheckedPoints[lastID]);
                    lastID = _LastPoints[lastID];
                }

                return list;
            }
            PushChildrenQueue(ref queue, p);
        }

        return null;
    }

    int PushChildrenQueue(ref Queue<Vector2Int> queue, Vector2Int point)
    {
        _CheckedPoints.Add(point);
        _InQueuePoints.Remove(point);
        SetColor(point, Color.yellow);
        int lastID = _CheckedPoints.Count - 1;
        int count = 0;
        if (PointValid(point + Vector2Int.left))
        {
            queue.Enqueue(point + Vector2Int.left);
            _InQueuePoints.Add(point + Vector2Int.left);
            count++;
            _LastPoints.Add(lastID);
        }
        if (PointValid(point + Vector2Int.right))
        {
            queue.Enqueue(point + Vector2Int.right);
            _InQueuePoints.Add(point + Vector2Int.right);
            count++;
            _LastPoints.Add(lastID);
        }
        if (PointValid(point + Vector2Int.up))
        {
            queue.Enqueue(point + Vector2Int.up);
            _InQueuePoints.Add(point + Vector2Int.up);
            count++;
            _LastPoints.Add(lastID);
        }
        if (PointValid(point + Vector2Int.down))
        {
            queue.Enqueue(point + Vector2Int.down);
            _InQueuePoints.Add(point + Vector2Int.down);
            count++;
            _LastPoints.Add(lastID);
        }
        return count;
    }

    #endregion

    void SetColor(Vector2Int point, Color color)
    {
        materials[point.x * width + point.y].SetColor(BASECOLOR, color);
    }

    bool PointValid(Vector2Int point)
    {
        if(point.x >= 0 && point.y >= 0 && point.x < width && point.y < width 
            && !_CheckedPoints.Contains(point)
            && !obstancles.Contains(point)
            && !_InQueuePoints.Contains(point))
        {
            return true;
        }
        return false;
    }
}
