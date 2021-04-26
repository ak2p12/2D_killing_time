using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
[System.Serializable]
public struct PostionNode
{
    public Vector2 Position; //노드 위치
    public int[] ConnectedNodeIndex; //연결된 노드 인덱스
}

public struct DijkstraNode
{
    public float Deistance;
    public bool Check;
}


public class Dijkstra : MonoBehaviour
{
    public PostionNode[] Nodes;
    DijkstraNode[,] Distances; 

    public int[] final;
    List<int> a;
    DijkstraNode[] finalDistances; 

    // Start is called before the first frame update
    void Start()
    {
        Distances = new DijkstraNode[Nodes.Length, Nodes.Length];
        final = new int[Nodes.Length];
        finalDistances = new DijkstraNode[Nodes.Length];
        a = new List<int>();
        for (int i = 0; i < finalDistances.Length; ++i)
        {
            finalDistances[i].Deistance = 9999.9f;
            final[i] = 9999;
            finalDistances[i].Check = false;
        }

        for (int i = 0; i < Nodes.Length; ++i)
        {
            for(int j = 0; j < Nodes.Length; ++j)
            {
                Distances[i, j].Deistance = 9999.9f;
            }
        }

        for (int i = 0; i < Nodes.Length; ++ i)
        {
            Distances[i,i].Deistance = 0.0f;
            for (int j = 0; j < Nodes[i].ConnectedNodeIndex.Length; ++ j)
            {
                float dist = Vector2.Distance(Nodes[Nodes[i].ConnectedNodeIndex[j]].Position , Nodes[i].Position);
                Distances[i, Nodes[i].ConnectedNodeIndex[j]].Deistance = dist;
            }
        }

        StartDijkstra(0);
    }
    int Min(DijkstraNode[] _finalDistances)
    {
        float dist = 99999.0f;
        int min = 99999;

        for (int i = 0; i < _finalDistances.Length; ++i)
        {
            if (dist > _finalDistances[i].Deistance && _finalDistances[i].Check == false)
            {
                dist = _finalDistances[i].Deistance;
                min = i;
            }
        }
        if (min > Nodes.Length)
            return min;

        for (int i = 0; i < final.Length; ++i)
        {
            if (final[i] > min)
            {
                final[i] = min;
                break;
            }
                
        }
        _finalDistances[min].Check = true;
        return min;
    }

    public void SetUpDijkstra(int _startIndex)
    {
       int min =  Min(finalDistances);
        a.Add(min);
        if (min > Nodes.Length)
            return;

        for (int i = 0; i < Nodes[min].ConnectedNodeIndex.Length; ++i )
        {
            float dist = finalDistances[min].Deistance + 
                         Distances[min, Nodes[min].ConnectedNodeIndex[i]].Deistance;

            if (dist < finalDistances[Nodes[min].ConnectedNodeIndex[i]].Deistance)
                finalDistances[Nodes[min].ConnectedNodeIndex[i]].Deistance = dist;
        }

        SetUpDijkstra(_startIndex);
    }

    public void StartDijkstra(int _startIndex)
    {
        for (int i = 0; i < Nodes.Length; ++i)
        {
            finalDistances[i] = Distances[_startIndex, i];
        }

        SetUpDijkstra(_startIndex);
    }
    private void OnDrawGizmos()
    {
        if (Nodes != null)
        {
            foreach(PostionNode _node in Nodes)
            {
                Gizmos.DrawWireSphere(_node.Position, 0.3f);
                for (int i = 0; i < _node.ConnectedNodeIndex.Length; ++i )
                {
                    Gizmos.DrawLine(_node.Position , Nodes[_node.ConnectedNodeIndex[i]].Position);
                }
            }
        }

        //Gizmos.DrawWireSphere(leftEndPoint, 1.0f);
        //Gizmos.DrawWireSphere(rightEndPoint, 1.0f);
    }
    private void OnGUI()
    {
        if (Nodes != null)
        {
            for (int i = 0; i < Nodes.Length; ++i)
            {
                Vector3 temp = Camera.main.WorldToScreenPoint(Nodes[i].Position);
                //Debug.Log(temp.ToString());
                GUI.Label(Rect.MinMaxRect(
                    temp.x - 10,
                    (1080 - temp.y),
                    temp.x + 10,
                    (1080 - temp.y) + 100), i.ToString());
            }
        }
        
    }
}
