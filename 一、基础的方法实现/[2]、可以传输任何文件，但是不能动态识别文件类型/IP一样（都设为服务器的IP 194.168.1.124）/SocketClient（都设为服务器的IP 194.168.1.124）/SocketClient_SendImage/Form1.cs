using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SocketClient_SendImage
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        static Socket sendsocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        //OpenFileDialog表示打开文件对话框。
        OpenFileDialog openFileDialog1 = new OpenFileDialog();
        Byte[] imgByte = new byte[1024];

        /// <summary>
        /// 打开本地文件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btm_scane_Click(object sender, EventArgs e)
        {
            btm_scane.Text = "已经打开";
            //可以打开文件的类型
            this.openFileDialog1.Filter = "Image Files(*.BMP;*.JPG;*.GIF;*.PNG)|*.BMP;*.JPG;*.GIF;*.PNG" + "|All Files (*.*)|*.*";
            //如果文件对话框已经打开
            if (this.openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    string path = this.openFileDialog1.FileName;
                    lab_path.Text = path;
                    //path表示文件所在的路径。
                    //FileMode.Open表示打开文件。 FileMode.Create表示创建一个文件并覆盖原有的文件。当然还有其他方式。
                    //表示可以读文件（只读）。
                    FileStream fs = new FileStream(path, FileMode.Open, FileAccess.Read);
                    imgByte = new Byte[fs.Length]; 
                    //将文件读到imgByte中。
                    fs.Read(imgByte, 0, imgByte.Length);
                    fs.Close();
                }
                catch (Exception)
                {
                }
            }
        }
        /// <summary>
        /// 向服务器发送数据
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_send_Click(object sender, EventArgs e)
        {
            //实例化socket        
            IPEndPoint ipendpiont = new IPEndPoint(IPAddress.Parse("194.168.1.124"), 8888);
            sendsocket.Connect(ipendpiont);
            MessageBox.Show("服务器IP:" + sendsocket.RemoteEndPoint);
            sendsocket.Send(imgByte);
            sendsocket.Shutdown(System.Net.Sockets.SocketShutdown.Send);
            sendsocket.Close();
            sendsocket.Dispose();
        }

    }
}
