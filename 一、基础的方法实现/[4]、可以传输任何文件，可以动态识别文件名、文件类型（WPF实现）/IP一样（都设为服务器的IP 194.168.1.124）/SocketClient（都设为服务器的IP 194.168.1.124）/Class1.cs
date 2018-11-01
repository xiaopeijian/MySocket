//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using System.Windows;
//using System.Windows.Controls;
//using System.Windows.Data;
//using System.Windows.Documents;
//using System.Windows.Input;
//using System.Windows.Media;
//using System.Windows.Media.Imaging;
//using System.Windows.Navigation;
//using System.Windows.Shapes;
//using System.ComponentModel;
//using System.Data;
//using System.Drawing;
//using System.IO;
//using System.Net;
//using System.Net.Sockets;
//using Microsoft.Win32;

//namespace SocketClient
//{
//    class Class1
//    {

//        private void btnSend_Click(object sender, EventArgs e)
//        {
//            //组合出远程终结点  
//            IPAddress ipAddress = IPAddress.Parse(this.txtIP3.Text);
//            IPEndPoint hostEP = new IPEndPoint(ipAddress, Convert.ToInt32(this.txtPort3.Text));
//            Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
//            try
//            {
//                socket.Connect(hostEP);

//                //1.发送用户协议
//                string path1 = Environment.CurrentDirectory; //获取应用程序的当前工作目录。
//                string doc = "YourSendFile.pdf";
//                string path = Path.Combine(path1, doc);
//                FileStream fs = File.Open(path, FileMode.Open);

//                //文件内容
//                byte[] bdata = new byte[fs.Length];
//                fs.Read(bdata, 0, bdata.Length);
//                fs.Close();

//                //文件扩展名，固定3字节
//                byte[] fileExtArray = Encoding.UTF8.GetBytes(string.Format("{0:D3}", currentDocExt));

//                //文件长度, 固定为20字节，前面会自动补零
//                byte[] fileLengthArray = Encoding.UTF8.GetBytes(bdata.Length.ToString("D20"));

//                //合并byte数组
//                byte[] fileArray = CombomBinaryArray(fileExtArray, fileLengthArray);

//                //合并byte数组
//                byte[] bdata1 = CombomBinaryArray(fileArray, bdata);

//                //发文件长度+文件内容
//                socket.Send(bdata1, bdata1.Length, 0);

//                //2.接收
//                //声明接收返回内容的字符串  
//                string recvStr = "";

//                //声明字节数组，一次接收数据的长度为 1024 字节  
//                byte[] recvBytes = new byte[1024];

//                //返回实际接收内容的字节数  
//                int bytes = 0;

//                //循环读取，直到接收完所有数据  
//                while (true)
//                {
//                    bytes = socket.Receive(recvBytes, recvBytes.Length, 0);
//                    //读取完成后退出循环  
//                    if (bytes <= 0) break;

//                    //将读取的字节数转换为字符串  
//                    recvStr += Encoding.UTF8.GetString(recvBytes, 0, bytes);
//                }


//                //禁用 Socket  
//                socket.Shutdown(SocketShutdown.Both);

//                //关闭 Socket  
//                socket.Close();

//                //... do some busness logic ...

//            }
//            catch (Exception e1)
//            {
//                throw e1;
//            }
//        }
//    }
//}
