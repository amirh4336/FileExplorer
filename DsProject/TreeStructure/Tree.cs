using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DsProject.TreeStructure
{
    public interface IPosition<E>
    {
        E Element { get; }
    }

    public interface ITree<E> : IEnumerable<E>
    {
        // accessor methods

        IPosition<E> Root { get; }
        IPosition<E> Parent(IPosition<E> p);
        IEnumerable<IPosition<E>> Children(IPosition<E> p);
        int numChildren(IPosition<E> p);


        // query methods

        bool IsInternal(IPosition<E> p);
        bool IsExternal(IPosition<E> p);
        bool IsRoot(IPosition<E> p);


        int Size { get; }
        bool IsEmpty();
        IEnumerable<IPosition<E>> Positions();
    }

    public abstract class AbstractTree<E> : ITree<E>
    {


        public virtual int Size
        {
            get
            {
                int count = 0;
                foreach (IPosition<E> p in Positions())
                {
                    count++;
                }
                return count;
            }
        }

        public bool IsEmpty()
        {
            return Size == 0;
        }

        public bool IsExternal(IPosition<E> p)
        {
            return numChildren(p) == 0;
        }

        public bool IsInternal(IPosition<E> p)
        {
            return numChildren(p) > 0;
        }

        public bool IsRoot(IPosition<E> p)
        {
            return p == Root;
        }

        public int numChildren(IPosition<E> p)
        {
            int count = 0;
            foreach (var child in Children(p))
                count++;
            return count;
        }

        public abstract IPosition<E> Root { get; }
        public abstract IPosition<E> Parent(IPosition<E> p);
        public abstract IEnumerable<IPosition<E>> Positions();
        public abstract IEnumerable<IPosition<E>> Children(IPosition<E> p);

        public IEnumerable GetEnumerator() =>
            throw new Exception();

        IEnumerator IEnumerable.GetEnumerator() =>
            throw new Exception();
        public IEnumerable<IPosition<E>> Elements() =>
            throw new Exception();
        IEnumerator<E> IEnumerable<E>.GetEnumerator() => throw new Exception();

        public int Depth(IPosition<E> p)
        {
            if (IsRoot(p)) return 0;
            return 1 + Depth(Parent(p));
        }

        public int Height(IPosition<E> p)
        {
            int h = 0;
            foreach (IPosition<E> c in Children(p))
                h = Math.Max(h, 1 + Height(c));
            return h;
        }

    }





    public class GeneralTree<E> : AbstractTree<E>
    {

        private Node root = null;
        private int size = 0;


        protected class Node : IPosition<E>
        {
            public E Element { get; internal set; }
            public Node Parent { get; internal set; }

            public List<Node> Children { get; internal set; } = new List<Node>();

            public Node(E e, Node above, List<Node> children)
            {
                Element = e;
                Parent = above;
                if (children != null) Children = children;
            }
        }

        protected Node CreateNode(E e, Node parent, List<Node> children)
        {
            return new Node(e, parent, children);
        }


        public override IPosition<E> Root { get { return root; } }
        public override int Size { get { return size; } }

        public GeneralTree(E rootElm)
        {
            root = CreateNode(rootElm, null , null);
            size = 0;
        }

        protected Node Validate(IPosition<E> p)
        {
            Node node = p as Node;
            if (p == null) throw new Exception("Not valid position type.");
            if (node.Parent == node) throw new Exception("Position p is no longer in the tree.");
            return node;
        }

        public override IPosition<E> Parent(IPosition<E> p)
        {
            Node node = Validate(p);
            return node.Parent;
        }

        public override IEnumerable<IPosition<E>> Children(IPosition<E> p)
        {
            Node node = p as Node;
            return node.Children;
        }

        public override IEnumerable<IPosition<E>> Positions()
        {
            throw new NotImplementedException();
        }

        public IPosition<E> AddChild(IPosition<E> p, E e)
        {
            Node parent = p as Node;
            Node child = CreateNode(e, parent, null);
            parent.Children.Add(child);
            size++;
            return child;
        }
    }

}
