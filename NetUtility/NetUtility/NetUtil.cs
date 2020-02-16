using System;
using System.Net.Sockets;
using System.Text;

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
                    SocketInfo.socketInfo.WriteLog("成功创建套接字管理类");
                }catch(FormatException e)
                {
                    SocketInfo.socketInfo.WriteLog("套接字创建失败，请检查您的配置文件", "error");
                    throw e;                   
                }catch(SocketException e)
                {
                    SocketInfo.socketInfo.WriteLog("套接字创建失败，请检查端口是否被占用", "error");
                    throw e;
                }catch(Exception e)
                {
                    SocketInfo.socketInfo.WriteLog("发生未知错位，请重新启动", "error");
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
                        SocketInfo.socketInfo.WriteLog("成功连接到服务器");
                        isConnected = true;
                    }catch (SocketException e)
                    {
                        SocketInfo.socketInfo.WriteLog("连接服务器失败，请检查目标ip和端口是否正确","error");
                        throw e;
                    }catch(Exception e)
                    {
                        SocketInfo.socketInfo.WriteLog("在Connect时发生未知错误", "error");
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
                    SocketInfo.socketInfo.WriteLog("成功断开连接");
                    isConnected = false;
                }               
        }

        StringBuilder sb = new StringBuilder();
        public string HexMsg(byte[] msg)
        {
            sb.Clear();
            foreach (byte item in msg)
            {

                sb.Append(item.ToString("X"));
            }
            return sb.ToString();
        }
        public void SendMessage(byte[] message)
        {
            try
            {
                switch (SocketInfo.socketInfo.protocolType)
                {
                    case System.Net.Sockets.ProtocolType.Tcp:
                        SocketInfo.socketInfo.socket.Send(message);
                        SocketInfo.socketInfo.WriteLog("使用Tcp发送消息 :-> \"" + HexMsg(message) + "\"");
                        break;
                    case System.Net.Sockets.ProtocolType.Udp:
                        SocketInfo.socketInfo.socket.SendTo(message, SocketInfo.socketInfo.byteLength, System.Net.Sockets.SocketFlags.None, SocketInfo.socketInfo.ipEndpoint);
                        SocketInfo.socketInfo.WriteLog("使用Udp发送消息 :-> \"" + HexMsg(message) + "\"");
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
