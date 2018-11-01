using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Threading;

//客户端程序
namespace SocketClient
{
    class Program
    {
        private static byte[] result = new Byte[1024];
        static void Main(string[] args)
        {
            //服务器IP地址
            //IPAddress ip = IPAddress.Parse("127.0.0.1");
            IPAddress ip = IPAddress.Parse("194.168.1.124");
            Socket clientSocket = new Socket(AddressFamily.InterNetwork,SocketType.Stream, ProtocolType.Tcp);
            try
            {
                //Connect表示客户端向服务器发起连接请求。而在服务端，用Accept()响应该请求。
                clientSocket.Connect(new IPEndPoint(ip, 8887));
                Console.WriteLine("连接服务器成功");
                
            }
            catch
            {
                Console.WriteLine("连接服务器失败,请按回车键退出");
                Console.Read();
                return;
            }
            //通过clientSocket接收数据。Receive表示接受服务器的数据。
            int receiveLength = clientSocket.Receive(result);
            Console.WriteLine("接收服务器消息：{0}",Encoding.ASCII.GetString(result, 0, receiveLength));
            // 通过clientSocket发送数据
            for (int i = 0; i < 15; i++)
            {
                try
                {
                    Thread.Sleep(1000);
                    string sendMessage = "（客户端1）client send Message Hello" + DateTime.Now;
                    //向武器器发送消息。
                    clientSocket.Send(Encoding.ASCII.GetBytes(sendMessage));
                    Console.WriteLine("（客户端1）向服务器发送消息:{0}", sendMessage);
                }
                catch
                {
                    clientSocket.Shutdown(SocketShutdown.Both);
                    clientSocket.Close();
                    break;
                }
            }
            Console.WriteLine("发送完毕，按回车键退出");
            Console.ReadKey();
        }
    }
}
