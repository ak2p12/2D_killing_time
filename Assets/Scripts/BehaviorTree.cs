using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//행동트리 (BehaviorTree)
//기본적으로 모든 태스크는 결과값을 가지고 있다.
public abstract class Node
{
    protected bool result = false;
    public abstract bool Result(Enemy _enemy);
}

//메인
public class BehaviorTree : Node
{
    Node root;
    bool endRoot;

    public void Init(Node _node)
    {
        root = _node;
        endRoot = false;
    }

    public override bool Result(Enemy _enemy)
    {
        if (!endRoot)
            endRoot = root.Result(_enemy);
        return endRoot;
    }
}

//Composite Task
//실질적인 루프 
public abstract class Composite : Node
{
    protected List<Node> list_ChildNode = new List<Node>();     //자식 태스크를 담을 리스트

    public void AddNode(Node _node)
    {
        list_ChildNode.Add(_node);
    }

    public List<Node> GetListNode()
    {
        return list_ChildNode;
    }

    public abstract override bool Result(Enemy _enemy);
}

//자식 노드중 하나라도 true 반환하면 즉시 true 반환
//자식 노드중 전부 false 반환하면 false 반환
public class Selecter : Composite
{
    public override bool Result(Enemy _enemy)
    {
        for (int i = 0; i < list_ChildNode.Count; ++i)
        {
            if (true == list_ChildNode[i].Result(_enemy))
            {
                return true;
            }
        }

        return false;
    }
}

//자식 노드중 하나라도 false 반환하면 즉시 false 반환
//자식 노드중 전부 true 반환하면 true 반환
public class Sequence : Composite
{
    public override bool Result(Enemy _enemy)
    {
        for (int i = 0; i < list_ChildNode.Count; ++i)
        {
            if (false == list_ChildNode[i].Result(_enemy))
            {
                return false;
            }
        }

        return true;
    }
}

//자식 노드 순차적 실행
//넣은 순서로 실행
public class Parallel : Composite
{
    public override bool Result(Enemy _enemy)
    {
        for (int i = 0; i < list_ChildNode.Count; ++i)
        {
            list_ChildNode[i].Result(_enemy);
        }

        return true;
    }
}

//조건 성립하면 자식노드 실행
//자식노드가 true를 반환하면 true 반환
//조건에는 성립하지만 자식노드가 false를 반환하면 false 반환

public abstract class Decorator : Node
{
    protected Node childNode;
    public abstract void SetNode(Node _node);
    public abstract bool ChackCondition(Enemy _enemy);
    public abstract override bool Result(Enemy _enemy);
}

public abstract class Condition : Decorator
{
    public abstract override void SetNode(Node _node);
    public abstract override bool ChackCondition(Enemy _enemy);
    public abstract override bool Result(Enemy _enemy);
}

public abstract class TimeOut : Decorator
{
    protected float timeDelay;
    protected float currentTime;
    protected float originTime;

    public abstract override void SetNode(Node _node);
    public abstract void SetTime(float _time);
    public abstract override bool ChackCondition(Enemy _enemy);
    public abstract override bool Result(Enemy _enemy);
}

public abstract class ActionNode : Node
{
    protected bool isStart = false;
    public abstract void OnStart(Enemy _enemy);
    public abstract bool OnUpdate(Enemy _enemy);
    public abstract bool OnEnd(Enemy _enemy);
    public abstract override bool Result(Enemy _enemy);
}


