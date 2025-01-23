using MiniExcelLibs;
using System;
using System.Data;
using System.IO;
using System.Text;
using System.Windows.Forms;

namespace PlcClient.Handler
{
    internal class ListViewHandler
    {
        private readonly ListView listView;
        private ListViewColumnSorter lvwColumnSorter;

        public ListViewHandler(ListView listView)
        {
            this.listView = listView;
            lvwColumnSorter = new ListViewColumnSorter();
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

            SaveFileDialog fileDialog = new SaveFileDialog();
            fileDialog.Filter = "Excel 工作薄(*.xlsx)|*.xlsx|Excel 97-2003 工作薄(*.xls)|*.xls|CSV UTF-8(逗号分隔)(*.csv)|*.csv";
            fileDialog.Title = "保存文件";
            fileDialog.RestoreDirectory = true;
            fileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            fileDialog.DefaultExt = "xlsx";
            fileDialog.FileName = fileprefix + System.DateTime.Now.ToString("_yyyy-MM-dd_ffff");
            if (fileDialog.ShowDialog() != DialogResult.OK)
            {
                return string.Empty;
            }
            var dt = this.ToDataTable();
            MiniExcel.SaveAs(fileDialog.FileName, dt);
            return fileDialog.FileName;
        }

        public DataTable ToDataTable()
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
}
