using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Sockets;

namespace Gate.Networking
{
        public sealed class Connection
        {
                private readonly Action<Connection, byte[]> callback_;
                private readonly Dictionary<int, int[]> protocol_;
                private readonly byte[] receiveBuffer_ = new byte[8192];
                private readonly Socket socket_;

                public Connection(Socket socket, Action<Connection, byte[]> callback, Dictionary<int, int[]> protocol)
                {
                        socket_ = socket;
                        callback_ = callback;
                        protocol_ = protocol;
                }

                public void BeginReceiving()
                {
                        socket_.BeginReceive(receiveBuffer_, 0, receiveBuffer_.Length, SocketFlags.None, ReceiveCallback, null);
                }

                public void Disconnect()
                {
                        socket_.Close();
                }

                private void ReceiveCallback(IAsyncResult asyncResult)
                {
                        try
                        {
                                int bytesRead = socket_.EndReceive(asyncResult);

                                callback_(this, SubArray(receiveBuffer_, 0, bytesRead));

                                socket_.BeginReceive(receiveBuffer_, 0, receiveBuffer_.Length, SocketFlags.None, ReceiveCallback,
                                                     null);
                        }
                        catch (Exception e)
                        {
                                Disconnect();
                        }
                }

                private static byte[] SubArray(byte[] array, int start, int count)
                {
                        var result = new byte[count];
                        Array.Copy(array, start, result, 0, count);
                        return result;
                }

                private void Send(byte[] data)
                {
                        try
                        {
                                socket_.Send(data);
                        }
                        catch (Exception)
                        {
                                Disconnect();
                        }
                }

                public void Send(int messageId, params object[] parameters)
                {
                        using (var stream = new MemoryStream())
                        using (var writer = new BinaryWriter(stream))
                        {
                                writer.Write((ushort) messageId);

                                for (int i = 0; i < parameters.Length; i++)
                                {
                                        dynamic value = parameters[i];

                                        if (value is string) value = value.ToCharArray();

                                        if (value is Array)
                                        {
                                                switch (PrefixSize(messageId, i + 1))
                                                {
                                                        case 1:
                                                                writer.Write((byte) value.Length);
                                                                break;
                                                        case 2:
                                                                writer.Write((ushort) value.Length);
                                                                break;
                                                }

                                                foreach (dynamic element in value)
                                                {
                                                        if (element is char)
                                                        {
                                                                writer.Write((ushort) element);
                                                        }
                                                        else
                                                        {
                                                                writer.Write(element);
                                                        }
                                                }
                                        }
                                        else
                                        {
                                                writer.Write(value);
                                        }
                                }

                                Send(stream.ToArray());
                        }
                }

                private uint PrefixSize(int messageId, int field)
                {
                        if (messageId > protocol_.Count) return 0;

                        int current = 0;
                        int actual = 0;

                        while (actual++ < field)
                        {
                                switch (protocol_[messageId][current++] & 0xF)
                                {
                                        case 6:
                                        case 10:
                                                actual--;
                                                break;
                                        case 7:
                                        case 11:
                                                if (field == actual) return 2;
                                                break;
                                        case 12:
                                                if (field == actual) return 1;
                                                actual--;
                                                break;
                                }
                        }

                        return 0;
                }
        }
}