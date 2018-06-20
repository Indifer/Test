using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Index : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }

    protected void txtKey_TextChanged(object sender, EventArgs e)
    {
        DataTable dt = new DataTable();
        dt.Columns.Add(new DataColumn("col1"));
        DataRow dataRow = dt.NewRow();
        dataRow[0] = "aaaaaaaaa";
        dt.Rows.Add(dataRow);

        dataRow = dt.NewRow();
        dataRow[0] = "aaaba";
        dt.Rows.Add(dataRow);

        dataRow = dt.NewRow();
        dataRow[0] = "acccccca";
        dt.Rows.Add(dataRow);

        string text = this.txtKey.Text;
        DataRow[] newRows = dt.Select("col1='%" + text + "%'");
        dt.Rows.Clear();
        dt.Rows.Add(newRows);
        this.txtKey.Focus();
        if (text != string.Empty)
        {
            this.listBox.DataSource = dt;
            this.listBox.DataTextField = dt.Columns[0].ColumnName;
            this.listBox.DataBind();
        }
    }
}