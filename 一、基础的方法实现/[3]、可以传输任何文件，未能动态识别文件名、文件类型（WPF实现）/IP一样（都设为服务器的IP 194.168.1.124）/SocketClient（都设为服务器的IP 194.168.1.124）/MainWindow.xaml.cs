using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Net;
using System.Net.Sockets;
using Microsoft.Win32;

namespace SocketClient
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
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
        private void btn_OpenFile_Click(object sender, EventArgs e)
        {
            this.btn_OpenFile.Content = "已经打开";
            //可以打开文件的类型
            this.openFileDialog1.Filter = "Image Files(*.BMP;*.JPG;*.GIF;*.PNG)|*.BMP;*.JPG;*.GIF;*.PNG" + "|All Files (*.*)|*.*";
            //如果文件对话框已经打开
            if (this.openFileDialog1.ShowDialog() == true)
            {
                try
                {
                    string path = this.openFileDialog1.FileName;
                    TxtBox_SendFilePath.Text = path;
                    
                    //path表示文件所在的路径。
                    //FileMode.Open表示打开文件。 FileMode.Create表示创建一个文件并覆盖原有的文件。当然还有其他方式。
                    //表示可以读文件（只读）。
                    FileStream fs = new FileStream(path, FileMode.Open, FileAccess.Read);

                    //将文件名字、类型读到fileNameByte中。
                    byte[] fileNameByte = new byte[40];
                    string TxtBox_SendFileName_str = System.IO.Path.GetFileName(path);    //获取文件名称，带有后缀。
                    TxtBox_SendFileName.Text = TxtBox_SendFileName_str;
                    
                


                       //将文件内容读到imgByte中。
                    imgByte = new Byte[fs.Length];
                    //fs.Read(imgByte, 0, imgByte.Length);
                    fs.Read(imgByte, 20, imgByte.Length);//读取流中的数据，并写到自己imgByte中的20号自己的位置中去。
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
        private void btn_SendFile_Click(object sender, EventArgs e)
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
