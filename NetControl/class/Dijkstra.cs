using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Collections;
using System.Collections.ObjectModel;

namespace NetControl
{
    public class Dijkstra
    {
        public static int NodeCnt = 0;
        public Nodes nodes = new Nodes();

        public Dijkstra()
        {
            NodeCnt = 0;
            nodes = new Nodes();
        }
        public class Arc
        {
            public int NodeNo;  //from 0 to x
            public double distance;

            //Create a new Arc 
            public Arc(int No, double length)
            {
                NodeNo = No;
                distance = length;
            }
        }

        public class NodeArcs : Collection<Arc>
        {
        };

        public class Node
        {
            /*业务数据*/
            public int DeviceID;    //device id
            public List<int> lines = new List<int>(); //device connect

            public int NodeNo;  //from 0 to x            
            public Point point;
            public NodeArcs arcs;
            public string mtype;

            //Create new Node
            public Node(int ID, double x, double y)
            {
                NodeNo = NodeCnt++;
                DeviceID = ID;
                point = new Point(x, y);
                arcs = new NodeArcs();
            }
            public Node(int ID, double x, double y, List<int> ls)
            {
                NodeNo = NodeCnt++;
                DeviceID = ID;
                point = new Point(x, y);
                arcs = new NodeArcs();
                lines = ls;
            }
            public Node(int ID, Point p)
            {
                NodeNo = NodeCnt++;
                DeviceID = ID;
                point = new Point(p.X, p.Y);
                arcs = new NodeArcs();
            }

            public Node(int ID, double x, double y, List<int> ls,string type)
            {
                NodeNo = NodeCnt++;
                DeviceID = ID;
                point = new Point(x, y);
                arcs = new NodeArcs();
                lines = ls;
                mtype = type;
            }
            //Add new Arc
            public void AddArc(int otherNo, double length)
            {
                this.arcs.Add(new Arc(otherNo, length));
            }
        }

        public class Nodes : Collection<Node>
        {
            //Add a Arc to GraphNodes

            //Insert By DeviceId
            public void InsertArcById(int id1, int id2)
            {
                int p1_No = -1, p2_No = -1;
                foreach (Node node in this)
                {
                    if (p1_No < 0 && node.DeviceID == id1) p1_No = node.NodeNo;
                    if (p2_No < 0 && node.DeviceID == id2) p2_No = node.NodeNo;
                    if (p1_No >= 0 && p2_No >= 0 && p1_No != p2_No)
                        InsertArcByNo(p1_No, p2_No, 100);   //长度暂定，需加入经纬度算距离
                }
            }
            public void InsertArcByNo(int p1_No, int p2_No, double length)
            {
                this[p1_No].AddArc(p2_No, length);
                //无向图
                this[p2_No].AddArc(p1_No, length);
            }

            public bool addArc_By_Line(int lineID)
            {
                bool ret = false;
                int No1 = -1;
                int No2 = -1;
                foreach(Node node in this)
                {
                    if (No1 < 0 && node.lines.Contains(lineID)) No1 = node.NodeNo;
                    else if (No2 < 0 && node.lines.Contains(lineID)) No2 = node.NodeNo;

                    if (No1 >= 0 && No2 > 0)
                    {
                        InsertArcByNo(No1, No2, 100);   //长度暂定，需加入经纬度算距离
                        ret = true;
                        break;
                    }
                }

                return ret;
            }
        }

        //------------------------------------------------
        public class FindPath
        {
            struct HeapValue
            {
                public double value;
                public int index;
            }

            public ArrayList pathPoint;
            public double shortDistance;

            public FindPath()
            {
                pathPoint = new ArrayList();
            }

            enum NodeState
            {
                INIT,
                ACCESSED,
                FINISHED
            }

            static int[] heapIndex;
            HeapValue[] heapValue;
            int heapSize;
            NodeState[] state;
            int[] father;

            void heapUpdate(int fa, int index, double value)
            {
                switch (state[index])
                {
                    case NodeState.INIT:
                        state[index] = NodeState.ACCESSED;
                        heapIndex[index] = heapSize;
                        heapValue[heapSize].value = value;
                        heapValue[heapSize].index = index;
                        ++heapSize;
                        heapUpdate(heapSize - 1);
                        father[index] = fa;
                        break;
                    case NodeState.ACCESSED:
                        if (value < heapValue[heapIndex[index]].value)
                        {
                            father[index] = fa;
                            heapValue[heapIndex[index]].value = value;
                            heapUpdate(heapIndex[index]);
                        }
                        break;
                    case NodeState.FINISHED:
                    default:
                        //Impossible;
                        break;
                }
            }

            void heapAssign(int index, HeapValue x)
            {
                heapValue[index] = x;
                heapIndex[heapValue[index].index] = index;
            }

            void heapUpdate(int index)
            {
                HeapValue x = heapValue[index];
                while (index > 0 && x.value < heapValue[index / 2].value)
                {
                    heapAssign(index, heapValue[index / 2]);
                    index /= 2;
                }

                int leaf = index + index + 1;
                while (leaf < heapSize)
                {
                    if (leaf + 1 < heapSize && heapValue[leaf].value > heapValue[leaf + 1].value)
                    {
                        ++leaf;
                    }
                    if (x.value > heapValue[leaf].value)
                    {
                        heapAssign(index, heapValue[leaf]);
                        index = leaf;
                        leaf = index + index + 1;
                    }
                    else
                    {
                        break;
                    }
                }

                heapAssign(index, x);
            }

            private void heapAssign(int index, int p)
            {
                throw new NotImplementedException();
            }

            public bool DijkstraSearch(Nodes g, int src, int des)
            {
                bool ret = false;
                DijkstraProcess(g, src, des);
                if (pathPoint.Count > 0) ret = true;
                return ret;
            }

            public void DijkstraProcess(Nodes g, int src, int des)
            {
                pathPoint.Clear();
                shortDistance = double.MaxValue;


                if (0 == g.Count)
                {
                    return;
                }

                if (src == des)
                {
                    des = (des + 1) % g.Count;
                }
                heapSize = 0;
                heapValue = new HeapValue[g.Count];
                heapIndex = new int[g.Count];
                state = new NodeState[g.Count];
                father = new int[g.Count];

                heapUpdate(-1, src, 0.0);

                while (heapSize > 0)
                {
                    int v = heapValue[0].index;
                    state[v] = NodeState.FINISHED;
                    if (state[des] == NodeState.FINISHED)
                    {
                        break;
                    }
                    double dis = heapValue[0].value;
                    foreach (Arc a in g[v].arcs)
                    {
                        heapUpdate(v, a.NodeNo, dis + a.distance);
                    }
                    heapAssign(0, heapValue[heapSize - 1]);
                    --heapSize;
                    heapUpdate(0);
                }

                if (state[des] != NodeState.FINISHED)
                {
                    return;
                }
                shortDistance = heapValue[heapIndex[des]].value;
                int u = des;
                while (u != -1)
                {
                    pathPoint.Add(new Point(g[u].point.X, g[u].point.Y));   //输出路径
                    u = father[u];
                }
            }
        }
    }
}
