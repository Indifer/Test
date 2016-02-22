using Microsoft.AspNet.SignalR.Client;
using Microsoft.AspNet.SignalR.Client.Transports;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WinFormClient
{
    public partial class Form1 : Form
    {
        static HubConnection hubConnection = null;
        static IHubProxy stockTickerHubProxy = null;
        static IHubProxy strongHubProxy = null;

        public Form1()
        {
            InitializeComponent();
            this.btnInvok.Enabled = false;
            this.btnCon.Click += async (o, e) => await btnCon_Click(o, e);
            this.btnInvok.Click += async (o, e) => await btnInvok_Click(o, e);
        }


        private async Task btnCon_Click(object sender, EventArgs e)
        {
            this.btnInvok.Enabled = false;

            hubConnection = new HubConnection(txtUrl.Text);
            stockTickerHubProxy = hubConnection.CreateHubProxy("TickerHub");
            strongHubProxy = hubConnection.CreateHubProxy("StrongHub");

            this.btnCon.Enabled = false;
            try
            {
                await hubConnection.Start(new WebSocketTransport());

                this.btnInvok.Enabled = true;
            }
            catch (Exception ex)
            {
                Exception temp = ex;
                string msg = temp.Message;
                while (temp.InnerException != null)
                {
                    temp = temp.InnerException;
                    msg += "\r\n" + temp.Message;
                }
                MessageBox.Show(msg);
            }
            this.btnCon.Enabled = true;

        }


        private async Task btnInvok_Click(object sender, EventArgs e)
        {
            try
            {
                var p1 = txtparams1.Text;
                var p2 = txtparams2.Text;
                var res = await stockTickerHubProxy.Invoke<string>("NewContosoChatMessage", p1,  p2);
                txtRes.Text = res;
            }
            catch { }
        }




        //public static bool operator ==(A e1, B e2)
        //{
        //    return (int)e1 == (int)e2;
        //}

        //public static bool operator !=(A e1, B e2)
        //{
        //    return (int)e1 != (int)e2;
        //}
    }

    //public enum A : int
    //{

    //}

    //public enum B : int
    //{ }


}
