using System;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Diagnostics;
//服务器程序
namespace SocketServer
{
    class Program
    {
        private static byte[] result = new Byte[1024];
        private static int myprot = 8887;
        static Socket serverSocket;
        static int a = 0;
        static void Main(string[] args)
        {
            //服务器IP地址
            //IPAddress ip = IPAddress.Parse("127.0.0.1");
            IPAddress ip = IPAddress.Parse("194.168.1.124");
            //InterNetwork:IPv4。  Stream：流传输、数据不重发，一般不保留边界。
            serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream,ProtocolType.Tcp);
            //IPEndPoint：IP和端口绑定后给Bind类，Bind类再给serverSocket。
            serverSocket.Bind(new IPEndPoint(ip, myprot));
            //启用侦听，最多能连接10客户端。
            serverSocket.Listen(10);
            //侦听成功后，输出本地的IP和端口号：LocalEndPoint是serverSocket的方法。
            Console.WriteLine("启动监听{0}成功", serverSocket.LocalEndPoint.ToString());
            Console.WriteLine("（位置1）主线程ID:{0}", Thread.CurrentThread.ManagedThreadId);
            //通过clientsocket发送数据。创建一个线程，用来连接客户端。
            Thread myThread = new Thread(ListenClientConnect);
            Console.WriteLine("（位置2）主线程ID:{0}", Thread.CurrentThread.ManagedThreadId);
            myThread.Start();
            Console.WriteLine("（位置3）主线程ID:{0}", Thread.CurrentThread.ManagedThreadId);
            Console.ReadLine();
        }
        /// <summary>
        /// 接收连接
        /// </summary>
        private static void ListenClientConnect()
        {
            while (true)
            {
                Console.WriteLine("（位置1）ListenClientConnect线程ID:{0}", Thread.CurrentThread.ManagedThreadId);
                //Accept()表示服务器阻塞，等待客户端连接成功。若是连接成功，则返回客户端的IP和端口号、以及返回服务器的IP和端口号。
                //可以循环调用Accept()，连接不同的用户，从而响应不同的用户数。
                //int a = 0;
                //while ( (a<2) )
                //{
                Socket clientsocket = serverSocket.Accept();
                //服务器确认连接后，给客户端发送确认连接信息，表示双方已经握手成功。
                clientsocket.Send(Encoding.ASCII.GetBytes("Server Say Hello"));
                Console.WriteLine("（位置2）ListenClientConnect线程ID:{0}", Thread.CurrentThread.ManagedThreadId);
                //创建第三个线程，用来接收客户端信息。
                Thread receiveThread = new Thread(ReceiveMessage);
                Console.WriteLine("（位置3）ListenClientConnect线程ID:{0}", Thread.CurrentThread.ManagedThreadId);
                receiveThread.Start(clientsocket);
                Console.WriteLine("（位置4）ListenClientConnect线程ID:{0}", Thread.CurrentThread.ManagedThreadId);
                    //a++;
                //}
              
            }
        }
        /// <summary>
        /// 接收信息
        /// </summary>
        /// <param name="clientSocket">包含客户机信息的套接字</param>
        private static void ReceiveMessage(Object clientSocket)
        {
            Console.WriteLine("（位置1）ReceiveMessage线程ID:{0}", Thread.CurrentThread.ManagedThreadId);
            //创建一个包含客户端信息的类
            Socket myClientSocket = (Socket)clientSocket;
            Console.WriteLine("（位置2）ReceiveMessage线程ID:{0}", Thread.CurrentThread.ManagedThreadId);
            while (true)
            {
                try
                {
                    //通过clientsocket接收数据
                    Console.WriteLine("（位置3）ReceiveMessage线程ID:{0}", Thread.CurrentThread.ManagedThreadId);
                    //创建一个包含客户端信息的类，并用Receive接收客户端传输过来的信息，该信息放在result数组中。
                    int receiveNumber = myClientSocket.Receive(result);
                    //将int类型转换为字符串输出
                    Console.WriteLine("接收号客户端{0}消息{1}",myClientSocket.RemoteEndPoint.ToString(), Encoding.ASCII.GetString(result, 0, receiveNumber));
                    //Console.WriteLine("接收{0}号客户端{1}消息{2}",a,myClientSocket.RemoteEndPoint.ToString(), Encoding.ASCII.GetString(result, 0, receiveNumber));
                    Console.WriteLine("（位置4）ReceiveMessage线程ID:{0}", Thread.CurrentThread.ManagedThreadId);
                }
                catch (Exception ex)
                {
                    Console.WriteLine("（位置5）ReceiveMessage线程ID:{0}", Thread.CurrentThread.ManagedThreadId);
                    Console.WriteLine(ex.Message);
                    Console.WriteLine("（位置6）ReceiveMessage线程ID:{0}", Thread.CurrentThread.ManagedThreadId);
                    myClientSocket.Shutdown(SocketShutdown.Both);
                    Console.WriteLine("（位置7）ReceiveMessage线程ID:{0}", Thread.CurrentThread.ManagedThreadId);
                    myClientSocket.Close();
                    break;
                }
            }
        }
    }
}
