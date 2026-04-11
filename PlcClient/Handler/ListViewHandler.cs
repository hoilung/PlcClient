using MiniExcelLibs;
using NewLife;
using NewLife.Reflection;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;
namespace PlcClient.Handler
{
    internal class ListViewHandler
    {
        public readonly ListView listView;
        private ListViewColumnSorter lvwColumnSorter = new ListViewColumnSorter();

        public ListViewHandler(ListView listView)
        {
            this.listView = listView;
        }

        /// <summary>
        /// 保存scv文件
        /// </summary>
        /// <param name="fileprefix"></param>
        /// <returns>成功返回路径，不成功或取消返回empty</returns>
        public string ExportSCV(string fileprefix = "")
        {
            return ExportExcel(fileprefix);
        }

        public string ExportExcel(string fileprefix = "")
        {
            var dt = this.ToDataTable();
            var filename = fileprefix + System.DateTime.Now.ToString("_yyyy-MM-dd_ffff");
            return Export(dt, filename);
        }

        public string Export(DataTable dt, string filename)
        {
            SaveFileDialog fileDialog = new SaveFileDialog();
            fileDialog.Filter = "Excel 工作薄(*.xlsx)|*.xlsx|Excel 97-2003 工作薄(*.xls)|*.xls|CSV UTF-8(逗号分隔)(*.csv)|*.csv";
            fileDialog.Title = "保存文件";
            fileDialog.RestoreDirectory = true;
            fileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            fileDialog.DefaultExt = "xlsx";
            fileDialog.FileName = filename;
            if (fileDialog.ShowDialog() != DialogResult.OK)
            {
                return string.Empty;
            }
            //var dt = this.ToDataTable();
            MiniExcel.SaveAs(fileDialog.FileName, dt);
            return fileDialog.FileName;
        }

        public virtual DataTable ToDataTable()
        {
            DataTable dt = new DataTable();
            for (int i = 0; this.listView.Columns.Count > i; i++)
            {
                dt.Columns.Add(this.listView.Columns[i].Text);
            }
            foreach (ListViewItem item in this.listView.Items)
            {
                var row = dt.NewRow();
                for (int j = 0; j < dt.Columns.Count; j++)
                {
                    row[dt.Columns[j].ColumnName] = item.SubItems[j].Text;
                }
                dt.Rows.Add(row);
            }
            return dt;
        }
        public virtual void ColuminSort()
        {
            this.listView.ListViewItemSorter = lvwColumnSorter;
            this.listView.ColumnClick += ListView_ColumnClick;
        }

        private void ListView_ColumnClick(object sender, ColumnClickEventArgs e)
        {
            if (e.Column == lvwColumnSorter.SortColumn)
            {
                // Reverse the current sort direction for this column.
                if (lvwColumnSorter.Order == SortOrder.Ascending)
                {
                    lvwColumnSorter.Order = SortOrder.Descending;
                }
                else
                {
                    lvwColumnSorter.Order = SortOrder.Ascending;
                }
            }
            else
            {
                // Set the column number that is to be sorted; default to ascending.
                lvwColumnSorter.SortColumn = e.Column;
                lvwColumnSorter.Order = SortOrder.Ascending;
            }

            // Perform the sort with these new sort options.
            this.listView.Sort();
        }

        public long IPv4ToLong(string ip)
        {
            long result = 0;
            if (string.IsNullOrEmpty(ip))
                return result;
            string[] parts = ip.Split('.');
            if (parts.Length != 4)
                return ip.GetHashCode();
            foreach (string part in parts)
            {
                ushort val;
                if (!ushort.TryParse(part, out val) || val < 0 || val > 255)
                {
                    //throw new ArgumentException("Invalid IP address part");
                    continue;
                }
                result = (result << 8) | val;
            }
            return result;
        }
    }

    internal class ListViewHandler<T> : ListViewHandler
    {

        private readonly List<T> _dataCache = new List<T>();
        private readonly Dictionary<int, ListViewItem> _itemCache = new Dictionary<int, ListViewItem>();

        public int DataCount => this._dataCache.Count;
        public int ItemCount => this.listView.Items.Count;

        public T this[int index]
        {
            get
            {
                return this._dataCache[index];
            }
        }

        public IEnumerable<T> Data => this._dataCache;

        public IEnumerable<ListViewItem> Items => this._itemCache.Values;

        public ListViewHandler(ListView listView) : base(listView) { }

        private PropertyInfo[] _properties;
        public virtual void SetupVirtualMode()
        {
            this.listView.VirtualMode = true;
            this.listView.RetrieveVirtualItem += ListView_RetrieveVirtualItem;
            this.listView.CacheVirtualItems += ListView_CacheVirtualItems;
            this.listView.VirtualListSize = this._dataCache.Count;

            _properties = typeof(T).GetProperties();
            foreach (var item in _properties)
            {
                var displayName = item.GetDisplayName() ?? item.Name;
                this.listView.Columns.Add(new ColumnHeader { Name = item.Name, Text = displayName, Width = 60 });
            }
            //this.listView.AutoResizeColumns(ColumnHeaderAutoResizeStyle.HeaderSize);
        }

        public override void ColuminSort()
        {
            this.listView.ColumnClick += ListView_ColumnClick;
        }

        private void ListView_ColumnClick(object sender, ColumnClickEventArgs e)
        {
            if (this._properties == null || e.Column >= _properties.Length)
                return;
            var property = this._properties[e.Column];
            if (property.PropertyType == typeof(int))
            {
                this._dataCache.Sort((item1, item2) => Comparer<int>.Default.Compare((int)property.GetValue(item1), (int)property.GetValue(item2)));
            }
            else if (property.PropertyType == typeof(float))
            {
                this._dataCache.Sort((item1, item2) => Comparer<float>.Default.Compare((float)property.GetValue(item1), (float)property.GetValue(item2)));
            }
            else if (property.PropertyType == typeof(double))
            {
                this._dataCache.Sort((item1, item2) => Comparer<double>.Default.Compare((double)property.GetValue(item1), (double)property.GetValue(item2)));
            }
            else if (property.PropertyType == typeof(string))
            {
                this._dataCache.Sort((item1, item2) => Comparer<string>.Default.Compare((string)property.GetValue(item1), (string)property.GetValue(item2)));
            }
            else
            {
                this._dataCache.Sort((item1, item2) => Comparer.Default.Compare(property.GetValue(item1), property.GetValue(item2)));
            }

            this.listView.VirtualListSize = this._dataCache.Count;
            this.listView.Invalidate();
        }

        private void ListView_CacheVirtualItems(object sender, CacheVirtualItemsEventArgs e)
        {
            for (int index = e.StartIndex; index <= e.EndIndex; index++)
            {
                if (!_itemCache.ContainsKey(index))
                {
                    var item = _dataCache[index];
                    var listViewItem = CreateListViewItem(index);
                    _itemCache[index] = listViewItem;
                }
            }
        }

        private void ListView_RetrieveVirtualItem(object sender, RetrieveVirtualItemEventArgs e)
        {

            if (this._dataCache.Any() && e.ItemIndex < this._dataCache.Count)
            {
                if (!this._itemCache.TryGetValue(e.ItemIndex, out ListViewItem item))
                {
                    item = CreateListViewItem(e.ItemIndex);
                    _itemCache[e.ItemIndex] = item;
                }
                else
                {
                    UpdateListViewItem(item, e.ItemIndex);
                }
                e.Item = item;

            }
        }

        private ListViewItem CreateListViewItem(int index)
        {

            var data = this._dataCache[index];

            var subitems = _properties.Select(m => new ListViewItem.ListViewSubItem()
            {
                Name = m.Name,
                Text = m.GetValue(data)?.ToString()
            });
            var item = new ListViewItem(subitems.ToArray(), -1);
            item.Tag = data;
            return item;
        }

        private void UpdateListViewItem(ListViewItem item, int index)
        {
            if (index % 2 == 0)
            {
                item.BackColor = System.Drawing.Color.AliceBlue;
            }
            var data = this._dataCache[index];
            item.Tag = data;
            foreach (var property in _properties)
            {
                item.SubItems[property.Name].Text = property.GetValue(data)?.ToString();
            }

        }

        public void LoadAdd(IEnumerable<T> list)
        {
            this._dataCache.Clear();
            foreach (var item in list)
            {
                this._dataCache.Add(item);
            }
            this.listView.VirtualListSize = this._dataCache.Count;
            this.listView.Invalidate();
        }
        public void Add(T item)
        {
            this._dataCache.Add(item);
            this.listView.VirtualListSize = this._dataCache.Count;
        }
        public void Clear()
        {
            this._dataCache.Clear();
            this.listView.VirtualListSize = this._dataCache.Count;
        }

        /// <summary>
        /// 刷新所有项
        /// </summary>
        public void NotifyAllChanged()
        {
            this.listView.Invalidate();
        }
        /// <summary>
        /// 刷新指定项
        /// </summary>
        /// <param name="index"></param>
        public void NotifyItemChanged(int index)
        {
            this.listView.Invalidate(this.listView.GetItemRect(index));
        }

        public override DataTable ToDataTable()
        {
            if (!this.listView.VirtualMode)
            {
                return base.ToDataTable();
            }
            DataTable dt = new DataTable();
            for (int i = 0; this.listView.Columns.Count > i; i++)
            {
                dt.Columns.Add(this.listView.Columns[i].Text);
            }
            foreach (ListViewItem item in this._itemCache.Values)
            {
                var row = dt.NewRow();
                for (int j = 0; j < dt.Columns.Count; j++)
                {
                    row[dt.Columns[j].ColumnName] = item.SubItems[j].Text;
                }
                dt.Rows.Add(row);
            }
            return dt;
        }
    }
}
