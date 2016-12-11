/*
HaxWin - lightweight xulrunner based browser for play haxball
Copyright (C) 2016  Oskari Pöntinen <mail.morko@gmail.com>

This program is free software: you can redistribute it and/or modify
it under the terms of the GNU General Public License as published by
the Free Software Foundation, either version 3 of the License, or
(at your option) any later version.

This program is distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
GNU General Public License for more details.

You should have received a copy of the GNU General Public License
along with this program.  If not, see <http://www.gnu.org/licenses/>.
*/
using System;
using System.Diagnostics;
using System.IO;
using System.IO.Pipes;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;

namespace HaxWin
{
    public class StdInReciever
    {
        public const string EXIT = "X";
        public const string NAVIGATE = "NAV";

        public volatile bool started = false;
        public volatile bool stopRequest = false;

        public delegate void MessageRecievedEventHandler(object sender,
                                            MessageRecievedEventArgs e);

        private Thread recieverThread;
        Stream inputStream;

        // buffers for parsing the message
        private byte[] messageBuffer = new byte[8];
        private string messageChunk = string.Empty;

        public StdInReciever()
        {
            inputStream = Console.OpenStandardInput();
        }

        public event MessageRecievedEventHandler MessageRecieved;


        public void start()
        {
            if (!started)
            {
                started = true;
                stopRequest = false;
                this.recieverThread = new Thread(new ThreadStart(this.recieve));
                // this does the magic trick of closing the thread even if there is read blocking
                this.recieverThread.IsBackground = true;
                this.recieverThread.Start();
            }
        }
        public void stop()
        {
            this.stopRequest = true;
        }

        private void recieve()
        {
            Message msg;
            while (!stopRequest)
            {
                try
                {
                    msg = CheckForReceivedMessage(inputStream);
                }
                catch (Exception e)
                {
                    Debug.WriteLine("Exception when reading inputStream: " + e.ToString());
                    break;
                }
                if (msg != null && !stopRequest)
                {
                    Debug.WriteLine("Firing MessageRecieved with: " + msg.code);
                    MessageRecieved?.Invoke(this, new MessageRecievedEventArgs(msg));
                }
            }
            Debug.WriteLine("Reciever stopping.");
            inputStream.Dispose();
        }


        private Message CheckForReceivedMessage(Stream inputStream)
        {
            Debug.WriteLine("Checking for recieved message.");
            messageBuffer.Initialize();
            messageChunk = string.Empty;
            StringBuilder messageBuilder = new StringBuilder();
            int byteCounter = 0;
            int byteInt;
            try
            {
                // read bytes untill 0 byte or end of file (-1)
                while ((byteInt = inputStream.ReadByte()) > 0)
                {
                    if (byteCounter == messageBuffer.Length)
                    {
                        byteCounter = 0;
                        messageChunk = Encoding.UTF8.GetString(messageBuffer);
                        messageBuilder.Append(messageChunk);
                        messageBuffer.Initialize();
                    }

                    messageBuffer[byteCounter] = (byte)byteInt;
                    byteCounter++;
                }
            }
            catch
            {
                throw;
            }
            // if byteCounter is not 0 there is something left in messageBuffer
            if (byteCounter != 0)
            {
                byte[] restOfTheBytes = new byte[byteCounter];
                Array.Copy(messageBuffer, restOfTheBytes, byteCounter);
                messageChunk = Encoding.UTF8.GetString(restOfTheBytes);
                messageBuilder.Append(messageChunk);
            }
            if (messageBuilder.Length == 0)
                return null;
            return new Message(messageBuilder);
        }
    }

    public class Message
    {
        public string code;
        public string data;

        public Message(StringBuilder msg)
        {
            string[] msgTmp = msg.ToString().Split(new char[] { ' ' }, 2);
            this.code = msgTmp[0];
            if (msgTmp.Length > 1)
            {
                this.data = msgTmp[1];
            }
        }

        public Message(string code, string data)
        {
            this.code = code;
            this.data = data;
        }
        public Message(string code)
        {
            this.code = code;
        }
    }

    public class MessageRecievedEventArgs : EventArgs
    {
        public Message msg { get; }
        public MessageRecievedEventArgs(Message msg)
        {
            this.msg = msg;
        }
    }
}