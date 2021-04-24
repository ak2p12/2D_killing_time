using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
[System.Serializable]
public struct PostionNode
{
    public Vector2 Position; //노드 위치
    public int NodeIndex; //자기 자신의 인덱스
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
    DijkstraNode[] finalDistances; 

    // Start is called before the first frame update
    void Start()
    {
        Distances = new DijkstraNode[Nodes.Length, Nodes.Length];
        final = new int[Nodes.Length];
        finalDistances = new DijkstraNode[Nodes.Length];

        for (int i = 0; i < finalDistances.Length; ++i)
        {
            finalDistances[i].Deistance = 9999.0f;
            finalDistances[i].Check = false;
        }

        for (int i = 0; i < Nodes.Length; ++i)
        {
            for(int j = 0; j < Nodes.Length; ++j)
            {
                Distances[i, j].Deistance = 9999.0f;
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

        StartDijkstra(0, 6);
    }
    int Min(DijkstraNode[] _finalDistances)
    {
        float dist = 99999.0f;
        int min = 9999;

        for (int i = 0; i < _finalDistances.Length; ++i)
        {
            if (dist > _finalDistances[i].Deistance)
            {
                dist = _finalDistances[i].Deistance;
                min = i;
            }
        }
 
        return min;
    }

    public void SetUpDijkstra(int _startIndex , int _endIndex , int _nextIndex = 0)
    {
       int min =  Min(finalDistances);
        //if (finalDistances[_nextIndex].Check == true)
        //    return;

        //finalDistances[_nextIndex].Check = true;

        //for (int i = 0; i < Nodes[_nextIndex].ConnectedNodeIndex.Length; ++i )
        //{
        //    float dist = Distances[_startIndex, _nextIndex].Deistance +
        //                 Distances[_startIndex, Nodes[_nextIndex].ConnectedNodeIndex[i]].Deistance;

        //    if (dist < Distances[_startIndex, Nodes[_nextIndex].ConnectedNodeIndex[i]].Deistance)
        //    {
        //        Distances[_startIndex, Nodes[_nextIndex].ConnectedNodeIndex[i]].Deistance = dist;
        //    }
        //}
    }

    public void StartDijkstra(int _startIndex , int _endIndex)
    {
        for (int i = 0; i < Nodes.Length; ++i)
        {
            finalDistances[i] = Distances[_startIndex, i];
        }

        SetUpDijkstra(_startIndex , _endIndex);
    }
    private void OnDrawGizmos()
    {
        if (Nodes != null)
        {
            foreach(PostionNode _node in Nodes)
            {
                Gizmos.DrawWireSphere(_node.Position, 0.5f);
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
            foreach (PostionNode _node in Nodes)
            {
                Vector3 temp = Camera.main.WorldToScreenPoint(_node.Position);
                //Debug.Log(temp.ToString());
                GUI.Label(Rect.MinMaxRect(
                    temp.x - 10,
                    (1080 - temp.y),
                    temp.x + 10,
                    (1080 - temp.y) + 100) , _node.NodeIndex.ToString());

            }
        }
        
    }
}
