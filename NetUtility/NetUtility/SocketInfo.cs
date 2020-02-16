using System;
using System.IO;
using System.Net.Sockets;
using System.Net;
using System.Xml;
using System.Threading;
namespace NetUtility
{
    class SocketInfo
    {
        public static SocketInfo socketInfo;
        public Socket socket;
        public IPEndPoint ipEndpoint;
        public int port = 0;
        public ProtocolType protocolType = ProtocolType.Udp;

        public int byteLength;

        private SocketType socketType;

        private FileStream logFile;
        public StreamWriter logWriter;
        private SocketInfo()
        {
            DirectoryInfo dirInfo = new DirectoryInfo(Directory.GetCurrentDirectory());
            byte[] ipBytes = new byte[4];
            XmlDocument xmlDoc = new XmlDocument();
            foreach (FileInfo fileInfo in dirInfo.GetFiles())
            {
                if (!fileInfo.Name.Equals("net.xml")) continue;

                if(fileInfo.Name.Equals("net.xml"))
                {   
                    xmlDoc.Load(fileInfo.Name);
                    foreach (XmlNode node in xmlDoc.FirstChild.ChildNodes)
                    {
                        if (node.Name.ToLower().Trim().Equals("ip"))
                        {
                            string[] ipstr = node.InnerText.Trim().Split('.');
                            for (int i = 0; i < ipBytes.Length; i++)
                            {
                                try
                                {
                                    ipBytes[i] = byte.Parse(ipstr[i]);

                                }
                                catch (FormatException e)
                                {
                                    throw e;
                                }
                            }

                        }

                        if(node.Name.ToLower().Trim().Equals("port"))
                        {
                            try
                            {
                                port = int.Parse(node.InnerText.Trim());
                            }catch(FormatException e)
                            {
                                throw e;
                            }
                        }

                        if (node.Name.ToLower().Trim().Equals("type"))
                        {
                            if (node.InnerText.ToLower().Trim().Equals("udp"))
                            {
                                protocolType = ProtocolType.Udp;
                            }
                            else if (node.InnerText.ToLower().Trim().Equals("tcp"))
                            {
                                protocolType = ProtocolType.Tcp;
                            }
                        }

                        if(node.Name.ToLower().Trim().Equals("sockettype"))
                        {
                            if(node.InnerText.ToLower().Trim().Equals("") || node.InnerText.ToLower().Trim().StartsWith("un"))
                            {
                                socketType = SocketType.Unknown;
                            }
                            if(node.InnerText.ToLower().Trim().StartsWith("st"))
                            {
                                socketType = SocketType.Stream;
                            }
                            if(node.InnerText.ToLower().Trim().StartsWith("dg"))
                            {
                                socketType = SocketType.Dgram;
                            }
                            if(node.InnerText.ToLower().Trim().StartsWith("ra"))
                            {
                                socketType = SocketType.Raw;
                            }
                            if(node.InnerText.ToLower().Trim().StartsWith("rd"))
                            {
                                socketType = SocketType.Rdm;
                            }
                            if(node.InnerText.ToLower().Trim().StartsWith("se"))
                            {
                                socketType = SocketType.Seqpacket;
                            }
                        }

                        if(node.Name.ToLower().Trim().Equals("bytelen"))
                        {
                            try
                            {
                                byteLength = int.Parse(node.InnerText.Trim());
                            }
                            catch (FormatException e)
                            {
                                throw e;
                            }
                        }
                    }
                }
                if (port != 0 && ipBytes[0] != 0)
                {
                    string path = Directory.GetCurrentDirectory() + "\\log.txt";
                    logFile = File.Open(path, FileMode.Create, FileAccess.Write, FileShare.Read);
                    logWriter = new StreamWriter(logFile);
                    logWriter.AutoFlush = false;
                    WriteLog("读取配置文件完成~");
                    WriteLog("当前配置ip为: " + ipBytes[0] + "." + ipBytes[1] + "." + ipBytes[2] + "." + ipBytes[3]);
                    WriteLog("当前配置port为: " + port);
                    WriteLog("当前配置的套接字类型为: " + socketType);
                    WriteLog("当前配置的协议为: " + protocolType);

                    ipEndpoint = new IPEndPoint(new IPAddress(ipBytes), port);

                    socket = new Socket(AddressFamily.InterNetwork, socketType, protocolType);
                    WriteLog("成功创建套接字对象");
                }
                break;
            }
        }

        public void WriteLog(string msg,string type = "msg")
        {
            if(logWriter != null)
            {       
                logWriter.WriteLine("[ {0}h : {1}min : {2}s : {3}ms ] ::=>  [{4}] {5} \n\n",DateTime.Now.Hour,DateTime.Now.Minute,DateTime.Now.Second,DateTime.Now.Millisecond,type,msg);
                logWriter.Flush();
            }
        }

         ~SocketInfo()
        {
            if(logWriter != null)
            {
                logWriter.Close();                
            }
        }

        public static SocketInfo GetSocket()
        {
            if(socketInfo == null)
            {
                try
                {
                    socketInfo = new SocketInfo();
                }catch(FormatException)
                {
                    throw new FormatException();
                }catch (Exception)
                {
                    throw new SocketException();
                }

                return socketInfo;
            }else
            {
                return socketInfo;
            }
        }
    }
}
