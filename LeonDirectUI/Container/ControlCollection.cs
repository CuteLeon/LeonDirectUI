using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LeonDirectUI.Container
{

    //TODO: 将 Controls 类型修改为 Control.ControlCollection 类型，并 [提醒] 在 Add Remove 方法订阅和取消订阅控件绘制请求事件等处理工作；

    public class ControlCollection
    {
        /*
        public class ControlCollection : IList, ICollection, IEnumerable, ICloneable
        {
            // Token: 0x06005505 RID: 21765 RVA: 0x0016672B File Offset: 0x0016492B
            public ControlCollection(Control owner)
            {
                this.owner = owner;
            }

            // Token: 0x06005506 RID: 21766 RVA: 0x00166741 File Offset: 0x00164941
            public virtual bool ContainsKey(string key)
            {
                return this.IsValidIndex(this.IndexOfKey(key));
            }

            // Token: 0x06005507 RID: 21767 RVA: 0x00166750 File Offset: 0x00164950
            public virtual void Add(Control value)
            {
                if (value == null)
                {
                    return;
                }
                if (value.GetTopLevel())
                {
                    throw new ArgumentException(SR.GetString("TopLevelControlAdd"));
                }
                if (this.owner.CreateThreadId != value.CreateThreadId)
                {
                    throw new ArgumentException(SR.GetString("AddDifferentThreads"));
                }
                Control.CheckParentingCycle(this.owner, value);
                if (value.parent == this.owner)
                {
                    value.SendToBack();
                    return;
                }
                if (value.parent != null)
                {
                    value.parent.Controls.Remove(value);
                }
                base.InnerList.Add(value);
                if (value.tabIndex == -1)
                {
                    int num = 0;
                    for (int i = 0; i < this.Count - 1; i++)
                    {
                        int tabIndex = this[i].TabIndex;
                        if (num <= tabIndex)
                        {
                            num = tabIndex + 1;
                        }
                    }
                    value.tabIndex = num;
                }
                this.owner.SuspendLayout();
                try
                {
                    Control parent = value.parent;
                    try
                    {
                        value.AssignParent(this.owner);
                    }
                    finally
                    {
                        if (parent != value.parent && (this.owner.state & 1) != 0)
                        {
                            value.SetParentHandle(this.owner.InternalHandle);
                            if (value.Visible)
                            {
                                value.CreateControl();
                            }
                        }
                    }
                    value.InitLayout();
                }
                finally
                {
                    this.owner.ResumeLayout(false);
                }
                LayoutTransaction.DoLayout(this.owner, value, PropertyNames.Parent);
                this.owner.OnControlAdded(new ControlEventArgs(value));
            }

            // Token: 0x06005508 RID: 21768 RVA: 0x001668C8 File Offset: 0x00164AC8
            int IList.Add(object control)
            {
                if (control is Control)
                {
                    this.Add((Control)control);
                    return this.IndexOf((Control)control);
                }
                throw new ArgumentException(SR.GetString("ControlBadControl"), "control");
            }

            // Token: 0x06005509 RID: 21769 RVA: 0x00166900 File Offset: 0x00164B00
            [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
            public virtual void AddRange(Control[] controls)
            {
                if (controls == null)
                {
                    throw new ArgumentNullException("controls");
                }
                if (controls.Length != 0)
                {
                    this.owner.SuspendLayout();
                    try
                    {
                        for (int i = 0; i < controls.Length; i++)
                        {
                            this.Add(controls[i]);
                        }
                    }
                    finally
                    {
                        this.owner.ResumeLayout(true);
                    }
                }
            }

            // Token: 0x0600550A RID: 21770 RVA: 0x00166960 File Offset: 0x00164B60
            object ICloneable.Clone()
            {
                Control.ControlCollection controlCollection = this.owner.CreateControlsInstance();
                controlCollection.InnerList.AddRange(this);
                return controlCollection;
            }

            // Token: 0x0600550B RID: 21771 RVA: 0x001124C0 File Offset: 0x001106C0
            public bool Contains(Control control)
            {
                return base.InnerList.Contains(control);
            }

            // Token: 0x0600550C RID: 21772 RVA: 0x00166988 File Offset: 0x00164B88
            public Control[] Find(string key, bool searchAllChildren)
            {
                if (string.IsNullOrEmpty(key))
                {
                    throw new ArgumentNullException("key", SR.GetString("FindKeyMayNotBeEmptyOrNull"));
                }
                ArrayList arrayList = this.FindInternal(key, searchAllChildren, this, new ArrayList());
                Control[] array = new Control[arrayList.Count];
                arrayList.CopyTo(array, 0);
                return array;
            }

            // Token: 0x0600550D RID: 21773 RVA: 0x001669D8 File Offset: 0x00164BD8
            private ArrayList FindInternal(string key, bool searchAllChildren, Control.ControlCollection controlsToLookIn, ArrayList foundControls)
            {
                if (controlsToLookIn == null || foundControls == null)
                {
                    return null;
                }
                try
                {
                    for (int i = 0; i < controlsToLookIn.Count; i++)
                    {
                        if (controlsToLookIn[i] != null && WindowsFormsUtils.SafeCompareStrings(controlsToLookIn[i].Name, key, true))
                        {
                            foundControls.Add(controlsToLookIn[i]);
                        }
                    }
                    if (searchAllChildren)
                    {
                        for (int j = 0; j < controlsToLookIn.Count; j++)
                        {
                            if (controlsToLookIn[j] != null && controlsToLookIn[j].Controls != null && controlsToLookIn[j].Controls.Count > 0)
                            {
                                foundControls = this.FindInternal(key, searchAllChildren, controlsToLookIn[j].Controls, foundControls);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    if (ClientUtils.IsSecurityOrCriticalException(ex))
                    {
                        throw;
                    }
                }
                return foundControls;
            }

            // Token: 0x0600550E RID: 21774 RVA: 0x00166AA8 File Offset: 0x00164CA8
            public override IEnumerator GetEnumerator()
            {
                return new Control.ControlCollection.ControlCollectionEnumerator(this);
            }

            // Token: 0x0600550F RID: 21775 RVA: 0x00112818 File Offset: 0x00110A18
            public int IndexOf(Control control)
            {
                return base.InnerList.IndexOf(control);
            }

            // Token: 0x06005510 RID: 21776 RVA: 0x00166AB0 File Offset: 0x00164CB0
            public virtual int IndexOfKey(string key)
            {
                if (string.IsNullOrEmpty(key))
                {
                    return -1;
                }
                if (this.IsValidIndex(this.lastAccessedIndex) && WindowsFormsUtils.SafeCompareStrings(this[this.lastAccessedIndex].Name, key, true))
                {
                    return this.lastAccessedIndex;
                }
                for (int i = 0; i < this.Count; i++)
                {
                    if (WindowsFormsUtils.SafeCompareStrings(this[i].Name, key, true))
                    {
                        this.lastAccessedIndex = i;
                        return i;
                    }
                }
                this.lastAccessedIndex = -1;
                return -1;
            }

            // Token: 0x06005511 RID: 21777 RVA: 0x001128A8 File Offset: 0x00110AA8
            private bool IsValidIndex(int index)
            {
                return index >= 0 && index < this.Count;
            }

            // Token: 0x17001426 RID: 5158
            // (get) Token: 0x06005512 RID: 21778 RVA: 0x00166B2D File Offset: 0x00164D2D
            public Control Owner
            {
                get
                {
                    return this.owner;
                }
            }

            // Token: 0x06005513 RID: 21779 RVA: 0x00166B38 File Offset: 0x00164D38
            public virtual void Remove(Control value)
            {
                if (value == null)
                {
                    return;
                }
                if (value.ParentInternal == this.owner)
                {
                    value.SetParentHandle(IntPtr.Zero);
                    base.InnerList.Remove(value);
                    value.AssignParent(null);
                    LayoutTransaction.DoLayout(this.owner, value, PropertyNames.Parent);
                    this.owner.OnControlRemoved(new ControlEventArgs(value));
                    ContainerControl containerControl = this.owner.GetContainerControlInternal() as ContainerControl;
                    if (containerControl != null)
                    {
                        containerControl.AfterControlRemoved(value, this.owner);
                    }
                }
            }

            // Token: 0x06005514 RID: 21780 RVA: 0x00166BB8 File Offset: 0x00164DB8
            void IList.Remove(object control)
            {
                if (control is Control)
                {
                    this.Remove((Control)control);
                }
            }

            // Token: 0x06005515 RID: 21781 RVA: 0x00166BCE File Offset: 0x00164DCE
            public void RemoveAt(int index)
            {
                this.Remove(this[index]);
            }

            // Token: 0x06005516 RID: 21782 RVA: 0x00166BE0 File Offset: 0x00164DE0
            public virtual void RemoveByKey(string key)
            {
                int index = this.IndexOfKey(key);
                if (this.IsValidIndex(index))
                {
                    this.RemoveAt(index);
                }
            }

            // Token: 0x17001427 RID: 5159
            public new virtual Control this[int index]
            {
                get
                {
                    if (index < 0 || index >= this.Count)
                    {
                        throw new ArgumentOutOfRangeException("index", SR.GetString("IndexOutOfRange", new object[]
                        {
                            index.ToString(CultureInfo.CurrentCulture)
                        }));
                    }
                    return (Control)base.InnerList[index];
                }
            }

            // Token: 0x17001428 RID: 5160
            public virtual Control this[string key]
            {
                get
                {
                    if (string.IsNullOrEmpty(key))
                    {
                        return null;
                    }
                    int index = this.IndexOfKey(key);
                    if (this.IsValidIndex(index))
                    {
                        return this[index];
                    }
                    return null;
                }
            }

            // Token: 0x06005519 RID: 21785 RVA: 0x00166C94 File Offset: 0x00164E94
            public virtual void Clear()
            {
                this.owner.SuspendLayout();
                CommonProperties.xClearAllPreferredSizeCaches(this.owner);
                try
                {
                    while (this.Count != 0)
                    {
                        this.RemoveAt(this.Count - 1);
                    }
                }
                finally
                {
                    this.owner.ResumeLayout();
                }
            }

            // Token: 0x0600551A RID: 21786 RVA: 0x00166CF0 File Offset: 0x00164EF0
            public int GetChildIndex(Control child)
            {
                return this.GetChildIndex(child, true);
            }

            // Token: 0x0600551B RID: 21787 RVA: 0x00166CFC File Offset: 0x00164EFC
            public virtual int GetChildIndex(Control child, bool throwException)
            {
                int num = this.IndexOf(child);
                if (num == -1 && throwException)
                {
                    throw new ArgumentException(SR.GetString("ControlNotChild"));
                }
                return num;
            }

            // Token: 0x0600551C RID: 21788 RVA: 0x00166D2C File Offset: 0x00164F2C
            internal virtual void SetChildIndexInternal(Control child, int newIndex)
            {
                if (child == null)
                {
                    throw new ArgumentNullException("child");
                }
                int childIndex = this.GetChildIndex(child);
                if (childIndex == newIndex)
                {
                    return;
                }
                if (newIndex >= this.Count || newIndex == -1)
                {
                    newIndex = this.Count - 1;
                }
                base.MoveElement(child, childIndex, newIndex);
                child.UpdateZOrder();
                LayoutTransaction.DoLayout(this.owner, child, PropertyNames.ChildIndex);
            }

            // Token: 0x0600551D RID: 21789 RVA: 0x00166D8B File Offset: 0x00164F8B
            public virtual void SetChildIndex(Control child, int newIndex)
            {
                this.SetChildIndexInternal(child, newIndex);
            }

            // Token: 0x0400375E RID: 14174
            private Control owner;

            // Token: 0x0400375F RID: 14175
            private int lastAccessedIndex = -1;

            // Token: 0x02000848 RID: 2120
            private class ControlCollectionEnumerator : IEnumerator
            {
                // Token: 0x06006D62 RID: 28002 RVA: 0x00191CFC File Offset: 0x0018FEFC
                public ControlCollectionEnumerator(Control.ControlCollection controls)
                {
                    this.controls = controls;
                    this.originalCount = controls.Count;
                    this.current = -1;
                }

                // Token: 0x06006D63 RID: 28003 RVA: 0x00191D1E File Offset: 0x0018FF1E
                public bool MoveNext()
                {
                    if (this.current < this.controls.Count - 1 && this.current < this.originalCount - 1)
                    {
                        this.current++;
                        return true;
                    }
                    return false;
                }

                // Token: 0x06006D64 RID: 28004 RVA: 0x00191D56 File Offset: 0x0018FF56
                public void Reset()
                {
                    this.current = -1;
                }

                // Token: 0x17001780 RID: 6016
                // (get) Token: 0x06006D65 RID: 28005 RVA: 0x00191D5F File Offset: 0x0018FF5F
                public object Current
                {
                    get
                    {
                        if (this.current == -1)
                        {
                            return null;
                        }
                        return this.controls[this.current];
                    }
                }

                // Token: 0x040042DD RID: 17117
                private Control.ControlCollection controls;

                // Token: 0x040042DE RID: 17118
                private int current;

                // Token: 0x040042DF RID: 17119
                private int originalCount;
            }
        }
    */
    }
}
