using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CharacterEditor.Models
{

    public class FakeFrameEnumerator : IEnumerator<FakeFrame>
    {
        private SimpleAnimation parent;
        private int curIndex = -1;
        private FakeFrame curFrame;


        public FakeFrameEnumerator(SimpleAnimation argParent)
        {
            parent = argParent;
            curIndex = -1;
            curFrame = default(FakeFrame);
        }

        public FakeFrame Current
        {
            get
            {
                return curFrame;
            }
        }

        public void Dispose() { }

        object System.Collections.IEnumerator.Current
        {
            get { return Current; }
        }

        public bool MoveNext()
        {
            if (++curIndex > Math.Abs(parent.EndIndex - parent.StartIndex))
            {
                return false;
            }
            else
            {
                curFrame = new FakeFrame(parent, curIndex);
            }
            return true;
        }

        public void Reset()
        {
            curIndex = -1;
        }
    }

    public class FakeFrameCollection : IList<FakeFrame>
    {
        private SimpleAnimation parent;

        public FakeFrameCollection(SimpleAnimation argParent)
        {
            parent = argParent;
        }

        public int IndexOf(FakeFrame item)
        {
            return item.Index;
        }

        public void Insert(int index, FakeFrame item)
        {
            return;
        }

        public void RemoveAt(int index)
        {
            return;
        }

        public FakeFrame this[int index]
        {
            get
            {
                return new FakeFrame(parent, index);
            }
            set
            {
                return;
            }
        }

        public void Add(FakeFrame item)
        {
            return;
        }

        public void Clear()
        {
            return;
        }

        public bool Contains(FakeFrame item)
        {
            return (item.Index >= parent.StartIndex && item.Index <= parent.EndIndex) ||
                (item.Index <= parent.StartIndex && item.Index >= parent.EndIndex);
        }

        public void CopyTo(FakeFrame[] array, int arrayIndex)
        {
            return;
        }

        public int Count
        {

            get { return Math.Abs(parent.EndIndex - parent.StartIndex); }
        }

        public bool IsReadOnly
        {
            get { return true; }
        }

        public bool Remove(FakeFrame item)
        {
            return false;
        }

        public IEnumerator<FakeFrame> GetEnumerator()
        {
            return new FakeFrameEnumerator(parent);
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }
    }

}
