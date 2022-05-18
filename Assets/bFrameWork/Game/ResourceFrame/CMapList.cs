
//双向链表结构节点
using System;
using System.Collections.Generic;
using bFrameWork.Game.ResourceFrame;

namespace bFrame.Game.ResourceFrame
{
    public class DoubleLinkedListNode<T> where T : class, new()
    {
        //前一个节点
        public DoubleLinkedListNode<T> Prev = null;

        //后一个节点
        public DoubleLinkedListNode<T> Next = null;

        public T t = null;
    }

    /// <summary>
    ///  双向链表 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class DoubleLinkedList<T> where T : class, new()
    {

        //表头
        public DoubleLinkedListNode<T> Head = null;

        //表尾
        public DoubleLinkedListNode<T> Tail = null;

        //双向链表结构类对象池
        private readonly ClassObjectPool<DoubleLinkedListNode<T>> _mDoubleLinkNodePool =
            PoolManager.Instance.GetOrCreateClassPool<DoubleLinkedListNode<T>>(500);

        //个数
        private int Count { get; set; } = 0;

        /// <summary>
        /// 添加一个节点到头部
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        public DoubleLinkedListNode<T> AddToHeader(T t)
        {
            DoubleLinkedListNode<T> pNode = _mDoubleLinkNodePool.Spawn(true);
            pNode.Next = null;
            pNode.Prev = null;
            pNode.t = t;
            return AddToHeader(pNode);
        }

        /// <summary>
        /// 添加一个节点到头部
        /// </summary>
        /// <param name="pNode"></param>
        /// <returns></returns>
        public DoubleLinkedListNode<T> AddToHeader(DoubleLinkedListNode<T> pNode)
        {
            if (pNode == null)
            {
                return null;
            }

            pNode.Prev = null;
            if (Head == null)
            {
                Head = Tail = pNode;
            }
            else
            {
                pNode.Next = Head;
                Head.Prev = pNode;
                Head = pNode;
            }

            Count++;
            return Head;
        }

        /// <summary>
        /// 添加节点到尾部
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        public DoubleLinkedListNode<T> AddToTail(T t)
        {
            DoubleLinkedListNode<T> pList = _mDoubleLinkNodePool.Spawn(true);
            pList.Next = null;
            pList.Prev = null;
            pList.t = t;
            return AddToTail(pList);
        }

        /// <summary>
        /// 添加节点到尾部
        /// </summary>
        /// <param name="pNode"></param>
        /// <returns></returns>
        private DoubleLinkedListNode<T> AddToTail(DoubleLinkedListNode<T> pNode)
        {
            if (pNode == null)
            {
                return null;
            }

            pNode.Next = null;
            if (Tail == null)
            {
                Head = Tail = pNode;
            }
            else
            {
                pNode.Prev = Tail;
                Tail.Next = pNode;
                Tail = pNode;
            }

            Count++;
            return Tail;
        }

        /// <summary>
        /// 移除某个节点
        /// </summary>
        /// <param name="pNode"></param>
        public void RemoveNode(DoubleLinkedListNode<T> pNode)
        {
            if (pNode == null)
            {
                return;
            }

            if (pNode == Head)
            {
                Head = pNode.Next;
            }

            if (pNode == Tail)
            {
                Tail = pNode.Next;
            }

            if (pNode.Prev != null)
            {
                pNode.Prev.Next = pNode.Next;
            }

            if (pNode.Next != null)
            {
                pNode.Next.Prev = pNode.Prev;
            }

            pNode.Next = pNode.Prev = null;
            pNode.t = null;
            _mDoubleLinkNodePool.Recycle(pNode);
            Count--;
        }

        /// <summary>
        /// 把某个节点移动到头部
        /// </summary>
        /// <param name="pNode"></param>
        public void MoveToHead(DoubleLinkedListNode<T> pNode)
        {
            if (pNode == null || pNode == Head)
            {
                return;
            }

            if (pNode.Prev == null && pNode.Next == null)
            {
                return;
            }

            if (pNode == Tail)
            {
                Tail = pNode.Prev;
            }

            if (pNode.Prev != null)
            {
                pNode.Prev.Next = pNode.Next;
            }

            if (pNode.Next != null)
            {
                pNode.Next.Prev = pNode.Prev;
            }

            pNode.Prev = null;
            pNode.Next = Head;
            Head.Prev = pNode;
            Head = pNode;
            if (Tail == null)
            {
                Tail = Head;
            }
        }
    }

    public class CMapList<T> where T : class, new()
    {
        private readonly DoubleLinkedList<T> _mDLink = new DoubleLinkedList<T>();
        private readonly Dictionary<T, DoubleLinkedListNode<T>> _mFindMap = new Dictionary<T, DoubleLinkedListNode<T>>();

        //垃圾回收时自动调用
        ~CMapList()
        {
            Clear();
        }

        /// <summary>
        /// 清空列表
        /// </summary>
        private void Clear()
        {
            while (_mDLink.Tail != null)
            {
                Remove(_mDLink.Tail.t);
            }
        }

        /// <summary>
        /// 插入一个基点到表头
        /// </summary>
        /// <param name="t"></param>
        public void InsertToHead(T t)
        {
            if (_mFindMap.TryGetValue(t, out var node) && node != null)
            {
                _mDLink.AddToHeader(node);
                return;
            }

            _mDLink.AddToHeader(t);
            _mFindMap.Add(t, _mDLink.Head);
        }

        /// <summary>
        /// 从表尾弹出一个节点
        /// </summary>
        public void Pop()
        {
            if (_mDLink.Tail != null)
            {
                Remove(_mDLink.Tail.t);
            }
        }

        /// <summary>
        /// 删除某个节点
        /// </summary>
        /// <param name="t"></param>
        public void Remove(T t)
        {
            if (!_mFindMap.TryGetValue(t, out var node) || node == null)
            {
                return;
            }

            _mDLink.RemoveNode(node);
            _mFindMap.Remove(t);
        }

        /// <summary>
        /// 获取到尾部节点
        /// </summary>
        /// <returns></returns>
        public T Back()
        {
            //return _mDLink.Tail == null ? null : _mDLink.Tail.t; 老写法
            return _mDLink.Tail?.t;
        }

        /// <summary>
        /// 返回节点个数
        /// </summary>
        /// <returns></returns>
        public int Size()
        {
            return _mFindMap.Count;
        }

        /// <summary>
        /// 查找是否存在该节点
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        public bool Find(T t)
        {
            return _mFindMap.TryGetValue(t, out var node) && node != null;
        }

        /// <summary>
        /// 刷新某个节点 把节点移动到头部
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        public bool Refresh(T t)
        {
            if (!_mFindMap.TryGetValue(t, out var node) || node == null)
            {
                return false;
            }

            _mDLink.MoveToHead(node);
            return true;
        }
        
       
    }
}