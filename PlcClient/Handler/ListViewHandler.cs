using MiniExcelLibs;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Text;
using System.Windows.Forms;


using NewLife.Reflection;
using NewLife;
using System.Linq;
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
            SaveFileDialog fileDialog = new SaveFileDialog();
            fileDialog.Filter = "Save File(*.csv)|*.csv";
            fileDialog.Title = "保存文件";
            fileDialog.RestoreDirectory = true;
            fileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            fileDialog.DefaultExt = "csv";
            fileDialog.FileName = fileprefix + System.DateTime.Now.ToString("_yyyy-MM-dd_ffff");
            if (fileDialog.ShowDialog() == DialogResult.OK)
            {
                StringBuilder stringBuilder = new StringBuilder();
                //stringBuilder.AppendLine("序号,内存区,地址,数据类型,数值");
                for (int i = 0; i < listView.Columns.Count; i++)
                {
                    if (i == listView.Columns.Count - 1)
                    {
                        stringBuilder.AppendFormat("{0}\r\n", listView.Columns[i].Text);
                        break;
                    }
                    stringBuilder.AppendFormat("{0},", listView.Columns[i].Text);
                }

                for (int i = 0; i < listView.Items.Count; i++)
                {
                    var item = listView.Items[i];

                    for (int j = 0; j < item.SubItems.Count; j++)
                    {
                        if (j == item.SubItems.Count - 1)
                        {
                            stringBuilder.AppendFormat("{0}\r\n", item.SubItems[j].Text);
                            break;
                        }
                        stringBuilder.AppendFormat("{0},", item.SubItems[j].Text);
                    }
                }

                File.WriteAllText(fileDialog.FileName, stringBuilder.ToString(), Encoding.Default);
                return fileDialog.FileName;
                //this.OnMsg($"保存文件：{fileDialog.FileName}");
                //MessageBox.Show("保存文件成功", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            return string.Empty;
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
        public void ColuminSort()
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

        public virtual void SetupVirtualMode()
        {
            this.listView.VirtualMode = true;
            this.listView.RetrieveVirtualItem += ListView_RetrieveVirtualItem;
            this.listView.VirtualListSize = this._dataCache.Count;

            var kv = typeof(T).GetProperties();
            foreach (var item in kv)
            {
                var displayName = item.GetDisplayName() ?? item.Name;
                this.listView.Columns.Add(new ColumnHeader { Name = item.Name, Text = displayName, Width = 60 });
            }
            //this.listView.AutoResizeColumns(ColumnHeaderAutoResizeStyle.HeaderSize);
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
                UpdateListViewItem(item, e.ItemIndex);
                e.Item = item;
            }
        }

        private ListViewItem CreateListViewItem(int index)
        {
            ListViewItem item = new ListViewItem();
            var data = this._dataCache[index];
            item.Tag = data;
            var kv = data.GetType().GetProperties().ToDictionary(m => m.Name, m => m.GetValue(data));
            for (int i = 0; i < this.listView.Columns.Count; i++)
            {
                if (i == 0)
                    item.Text = kv[this.listView.Columns[i].Name].ToString();
                else
                {
                    item.SubItems.Add(new ListViewItem.ListViewSubItem()
                    {
                        Tag = kv[this.listView.Columns[i].Name],
                        Text = kv[this.listView.Columns[i].Name].ToString()
                    });
                }
            }

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
            var kv = data.GetType().GetProperties().ToDictionary(m => m.Name, m => m.GetValue(data));
            for (int i = 0; i < this.listView.Columns.Count; i++)
            {
                if (i == 0)
                    item.Text = kv[this.listView.Columns[i].Name].ToString();
                else
                {
                    item.SubItems[i].Tag = kv[this.listView.Columns[i].Name];
                    item.SubItems[i].Text = kv[this.listView.Columns[i].Name].ToString();
                }
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
