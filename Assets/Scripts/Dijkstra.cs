using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
[System.Serializable]
public struct PostionNode
{
    public Vector2 Position; //노드 위치
    public int[] ConnectedNodeIndex; //연결된 노드 인덱스
    public bool IsJump;
    public bool IsDrop;
}

public struct DijkstraNode
{
    public int Index;
    public float Deistance;
    public bool Check;
}


public class Dijkstra : MonoBehaviour
{
    public PostionNode[] Nodes;
    DijkstraNode[,] Distances; 

    public Stack<int> FinalIndex_Stack;
    public DijkstraNode[] FinalDistances; 

    // Start is called before the first frame update
    void Start()
    {
        Distances = new DijkstraNode[Nodes.Length, Nodes.Length];
        FinalIndex_Stack = new Stack<int>();
        FinalDistances = new DijkstraNode[Nodes.Length];

        for (int i = 0; i < FinalDistances.Length; ++i)
        {
           FinalDistances[i].Deistance = 9999.9f;
           FinalDistances[i].Index = 9999;
           FinalDistances[i].Check = false;
        }

        for (int i = 0; i < Nodes.Length; ++i)
        {
            for(int j = 0; j < Nodes.Length; ++j)
            {
                Distances[i, j].Deistance = 9999.9f;
                Distances[i, j].Check = false;
                Distances[i, j].Index = 9999;
            }
        }

        for (int i = 0; i < Nodes.Length; ++ i)
        {
            Distances[i,i].Deistance = 0.0f;
            for (int j = 0; j < Nodes[i].ConnectedNodeIndex.Length; ++ j)
            {
                float dist = Vector2.Distance(Nodes[Nodes[i].ConnectedNodeIndex[j]].Position , Nodes[i].Position);
                Distances[i, Nodes[i].ConnectedNodeIndex[j]].Deistance = dist;
                Distances[i, Nodes[i].ConnectedNodeIndex[j]].Index = i;
            }
        }

        //StartDijkstra(9 , 4);
        
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

        _finalDistances[min].Check = true;
        return min;
    }

    public void SetUpDijkstra(int _startIndex , int _endIndex)
    {
       int min =  Min(FinalDistances);
        if (min > Nodes.Length)
            return;

        for (int i = 0; i < Nodes[min].ConnectedNodeIndex.Length; ++i )
        {
            float dist = FinalDistances[min].Deistance + 
                         Distances[min, Nodes[min].ConnectedNodeIndex[i]].Deistance;

            if (dist < FinalDistances[Nodes[min].ConnectedNodeIndex[i]].Deistance)
            {
                FinalDistances[Nodes[min].ConnectedNodeIndex[i]].Deistance = dist;
                FinalDistances[Nodes[min].ConnectedNodeIndex[i]].Index = min;
            }
                
        }

        if (min == _endIndex)
        {
            int index = _endIndex;
            FinalIndex_Stack.Push(index);
            while (true)
            {
                if (FinalDistances[index].Index > Nodes.Length)
                    break;

                FinalIndex_Stack.Push(FinalDistances[index].Index);
                index = FinalDistances[index].Index;
            }

            return;
            
        }
             

        SetUpDijkstra(_startIndex , _endIndex);
    }

    public void StartDijkstra(int _startIndex, int _endIndex)
    {
        for (int i = 0; i < Nodes.Length; ++i)
        {
            FinalDistances[i] = Distances[_startIndex, i];
        }
        FinalIndex_Stack.Clear();
        SetUpDijkstra(_startIndex, _endIndex);
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
