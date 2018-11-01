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
        OpenFileDialog openFileDialog1 = new OpenFileDialog();//OpenFileDialog表示打开文件对话框。
        IPEndPoint ipendpiont = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 8888);
        Byte[] fileContentByte = new byte[0];//定义全局变量
        Byte[] fileNameByte = new byte[0];//定义全局变量
        Byte[] combomArray = new byte[0];//定义全局变量


        /// <summary>
        /// 打开本地文件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// 

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
                    TxtBox_SendFilePath.Text = path;//文件路径，显示
                    string SendFileName_str = System.IO.Path.GetFileName(path);    //获取文件名称，带有后缀。
                    string SendFileNameProperty_str = System.IO.Path.GetExtension(path);    //获取后缀。
                    TxtBox_SendFileName.Text = SendFileName_str; //文件名称，显示。
                    //必须判断，是图片执行，并显示。
                    if ((SendFileNameProperty_str == ".jpg") 
                        || (SendFileNameProperty_str == ".png") 
                        || (SendFileNameProperty_str == ".bmp")
                        || (SendFileNameProperty_str == ".jpeg")
                        || (SendFileNameProperty_str == ".JPG")
                        || (SendFileNameProperty_str == ".PNG")
                        || (SendFileNameProperty_str == ".BMP")
                        || (SendFileNameProperty_str == ".JPEG"))
                    {
                        Image_PlayImage.Source = new BitmapImage(new Uri(path, UriKind.Absolute));//绝对路径，显示图片
                    }
                    fileNameByte = GetFileNameByte(SendFileName_str);
                    fileContentByte = GetfileContentByte(path);
                    combomArray = GetCombomFileByte(fileNameByte, fileContentByte);
                }
                catch (Exception)
                {

                }
            }
        }

        private void btn_OpenConnect_Click(object sender, RoutedEventArgs e)
        {
            sendsocket.Connect(ipendpiont);
        }
        /// <summary>
        /// 向服务器发送数据
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_SendFile_Click(object sender, EventArgs e)
        {
            //实例化socket        
            //IPEndPoint ipendpiont = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 8888);
            //sendsocket.Connect(ipendpiont);
            MessageBox.Show("服务器IP:" + sendsocket.RemoteEndPoint);
            sendsocket.Send(combomArray);

            //sendsocket.Shutdown(System.Net.Sockets.SocketShutdown.Send);
            //sendsocket.Close();
            //sendsocket.Dispose();

        }

        /// <summary>
        /// 获取文件名的字节数
        /// </summary>
        /// <param name="SendFileName_str"></param>
        /// <returns></returns>
        private Byte[] GetFileNameByte(string SendFileName_str)
        {
            byte[] fileNameByteVar = System.Text.Encoding.UTF8.GetBytes(SendFileName_str);
            return fileNameByteVar;
        }
        /// <summary>
        /// 获取文件内容所占的字节数
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        private Byte[] GetfileContentByte(string path)
        {

            FileStream fs = new FileStream(path, FileMode.Open, FileAccess.Read);//读取文件
            Byte[] fileContentByteVar = new Byte[fs.Length];
            try
            {
                fs.Read(fileContentByteVar, 0, fileContentByteVar.Length);
                fs.Close();
                //return fileContentByteVar;
            }
            catch 
            {
                MessageBox.Show("获取文件内容字节失败");
                //return fileContentByteVar;
            }
            return fileContentByteVar;
    
        }
        /// <summary>
        /// 将文件名字节数与文件内容字节数，以及带有文件名长度的一个字节合并（三部分合并，以便解析）
        /// </summary>
        /// <param name="fileNameByte"></param>
        /// <param name="fileContentByte"></param>
        /// <returns></returns>
        private Byte[] GetCombomFileByte(Byte[] fileNameByte, Byte[] fileContentByte)
        {
            int fileNameLong = fileNameByte.Length;//将文件的长度保存为一个整数。
            byte[] fileNameLongByte = new byte[4];//将整数放在4个字节中传输。
            fileNameLongByte = BitConverter.GetBytes(fileNameLong);//一个int等于四个字节。

            int combomFileLong = fileNameByte.Length + fileNameLongByte.Length + fileContentByte.Length;//总字节长度
            Byte[] combomFileByteTemple = new byte[combomFileLong];
            for (int i = 0; i < combomFileLong; i++ )
            {
                if (i < fileNameLongByte.Length)
                {
                    combomFileByteTemple[i] = fileNameLongByte[i];
                }
                else if (i < (fileNameByte.Length + fileNameLongByte.Length))
                {
                    combomFileByteTemple[i] = fileNameByte[ i - fileNameLongByte.Length ];//从第0号位置读取fileNameByte
                }
                else
                {
                    combomFileByteTemple[i] = fileContentByte[(i - fileNameByte.Length - fileNameLongByte.Length)];//从第0号位置读取fileContentByte
                }
            }
            return combomFileByteTemple;
        }

        private void btn_CloseConnect_Click(object sender, RoutedEventArgs e)
        {
            sendsocket.Shutdown(System.Net.Sockets.SocketShutdown.Send);
            sendsocket.Close();
            sendsocket.Dispose();
        }
    }
}
