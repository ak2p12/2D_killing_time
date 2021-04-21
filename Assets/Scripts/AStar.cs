using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
[System.Serializable]
public struct AStarNode
{
    public Vector2 Position; //노드 위치
    public float G; //시작노드부터 현재 노드까지의 거리
    public float H; //현재 노드부터 도착 노드까지 거리
    public float F; //G + F
    public bool Check; //상태 open close
    public int NodeIndex;
    public int[] ConnectedNodeIndex;
   
}


public class AStar : MonoBehaviour
{
    public AStarNode[] Nodes;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnDrawGizmos()
    {
        if (Nodes != null)
        {
            foreach(AStarNode _node in Nodes)
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
            foreach (AStarNode _node in Nodes)
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
