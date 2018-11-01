using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SocketServer_SendImage
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        //Form1_Load表示默认的加载，相当于初始化。
        private void Form1_Load(object sender, EventArgs e)
        {
            lab_pro.Text = "接收:0/100";
            //button1.Text = "BBBBB";
        }
        /// <summary>
        /// 开启服务
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button1_Click(object sender, EventArgs e)
        {
            button1.Text = "监听中...";
            button1.Enabled = false;
            Socket receiveSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            IPEndPoint hostIpEndPoint = new IPEndPoint(IPAddress.Parse("194.168.1.124"), 8888);

            //设置接收数据缓冲区的大小
            byte[] b = new byte[4096];
            receiveSocket.Bind(hostIpEndPoint);
            //监听
            receiveSocket.Listen(2);
            //接受客户端连接
            Socket hostSocket = receiveSocket.Accept();
            //内存流fs的初始容量大小为0，随着数据增长而扩展。
            MemoryStream fs = new MemoryStream();
            //string Path = "C:\\Users\\lanmage2\\Desktop\\AA";
            //FileStream fs = new FileStream(Path, FileMode.Open);
            int length = 0;
            //每接受一次，只能读取小于等于缓冲区的大小4096个字节
            while ((length = hostSocket.Receive(b)) > 0)
            {
                //将接受到的数据b，按长度length放到内存流中。
                fs.Write(b, 0, length);

                if (progressBar1.Value < 100)
                {
                    //进度条的默认值为0
                    progressBar1.Value++;
                    lab_pro.Text = "接收:" + progressBar1.Value + "/100";
                }
            }
            progressBar1.Value = 100;
            lab_pro.Text = "接收:" + progressBar1.Value + "/100";
            fs.Flush();
 
            fs.Seek(0, SeekOrigin.Begin);
            byte[] byteArray = new byte[fs.Length];
            int count = 0;
            while (count < fs.Length)
            {
                byteArray[count] = Convert.ToByte(fs.ReadByte());
                count++;
            }
            string Path = "C:\\Users\\lanmage2\\Desktop\\AA\\文件3.dcm";
            //FileStream filestream = new FileStream(Path + "\\文件1.txt", FileMode.OpenOrCreate);

            //FileStream filestream  = File.Create(Path);
            //filestream.Write(byteArray, 0, byteArray.Length);//不能用
            System.IO.File.WriteAllBytes(Path, byteArray);//能用
         
            /*Bitmap类，可以将*/
            //Bitmap Img = new Bitmap(fs);
            //Img.Save(@"reveive.jpg", ImageFormat.Png);
            //关闭写文件流
            fs.Close();
            //关闭接收数据的Socket
            hostSocket.Shutdown(SocketShutdown.Receive);
            hostSocket.Close();
            //关闭发送连接
            receiveSocket.Close();
        }


    }
}
