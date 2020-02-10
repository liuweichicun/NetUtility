using System;
using System.Net.Sockets;

namespace NetUtility
{
    public class NetUtil
    {
        bool isConnected = false;
        public NetUtil()
        {
            if(SocketInfo.socketInfo == null)
            {
                try
                {
                    SocketInfo.socketInfo = SocketInfo.GetSocket();
                    Console.WriteLine("Create socket successful");
                }catch(FormatException e)
                {
                    Console.WriteLine("配置文件异常，请查看配置文件");
                    throw e;                   
                }catch(SocketException e)
                {
                    Console.WriteLine("Socket 问题");
                    throw e;
                }catch(Exception e)
                {
                    throw e;
                }
                
                
            }
        }

        public void Connect()
        {
            switch (SocketInfo.socketInfo.protocolType)
            {
                case System.Net.Sockets.ProtocolType.Tcp:
                    try
                    {
                        SocketInfo.socketInfo.socket.Connect(SocketInfo.socketInfo.ipEndpoint);
                        isConnected = true;
                    }catch (SocketException e)
                    {
                        throw e;
                    }catch(Exception e)
                    {
                        throw e;
                    }
                    break;
                case System.Net.Sockets.ProtocolType.Udp:
                    // ...
                    break;
            }
        }
        public void Disconnect()
        {
            if (SocketInfo.socketInfo.protocolType == System.Net.Sockets.ProtocolType.Tcp)
                if(isConnected)
                {
                    SocketInfo.socketInfo.socket.Disconnect(true);
                    isConnected = false;
                }               
        }
        public void SendMessage(byte[] message)
        {
            try
            {
                switch (SocketInfo.socketInfo.protocolType)
                {
                    case System.Net.Sockets.ProtocolType.Tcp:
                        SocketInfo.socketInfo.socket.Send(message);
                        break;
                    case System.Net.Sockets.ProtocolType.Udp:
                        SocketInfo.socketInfo.socket.SendTo(message, SocketInfo.socketInfo.byteLength, System.Net.Sockets.SocketFlags.None, SocketInfo.socketInfo.ipEndpoint);
                        break;
                }
            }catch(SocketException e)
            {
                throw e;
            }catch(Exception e)
            {
                throw e;
            }
            
        }
    }
}
