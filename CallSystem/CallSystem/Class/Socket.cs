using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Speech.Synthesis;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace CallSystem.Class
{
  public class Socket
    {
      public string Lsocket(NetworkStream stream, int encoderNo, RichTextBox tb)
      {
          byte[] buffer = new byte[15000];
          string str = string.Empty;
          int length;
          do
          {
              length = stream.Read(buffer, 0, buffer.GetLength(0));
          }
          while (stream.DataAvailable);
          if (length > 0)
          {
              byte[] bytes = new byte[length];
              Array.Copy((Array)buffer, (Array)bytes, length);
              str = Encoding.GetEncoding(encoderNo).GetString(bytes);
              //str = Encoding.GetEncoding("UTF-8").GetString(bytes);
              Console.WriteLine(string.Format("接收: {0}", (object)str));
              // tb.AppendText(string.Format("接收: {0}", (object)str) + "\r\n");
              output(string.Format(PublicClass.PortLeft+"接收："+(object)str) + "\r" ,tb);
          }
          return str;
      }
      public string Rsocket(NetworkStream stream,int encoderNo, RichTextBox tb)
        {
            byte[] buffer = new byte[15000];
            string str = string.Empty;
            int length;
            do
            {
                length = stream.Read(buffer, 0, buffer.GetLength(0));
            }
            while (stream.DataAvailable);
            if (length > 0)
            {
                byte[] bytes = new byte[length];
                Array.Copy((Array)buffer, (Array)bytes, length);
                str = Encoding.GetEncoding(encoderNo).GetString(bytes);
                //str = Encoding.GetEncoding("UTF-8").GetString(bytes);
                Console.WriteLine(string.Format("接收: {0}", (object)str));
                //tb.AppendText(string.Format("接收: {0}", (object)str) + "\r\n");
                output(string.Format(PublicClass.PortRight + "接收：" + (object)str) + "\r", tb);
            }
            return str;
        }
      public void Responsesocket(NetworkStream stream, int encoderNo)
      {
          byte[] bytes = Encoding.GetEncoding(encoderNo).GetBytes("DataGet");
          stream.Write(bytes, 0, bytes.Length);
          Thread.Sleep(100);
      }

        private delegate void outputDelegate(string msg, RichTextBox tb);
        private void output(string msg, RichTextBox tb)
        {
            tb.Dispatcher.Invoke(new outputDelegate(outputAction), msg,tb);
        }

        private void outputAction(string msg, RichTextBox tb)
        {
            tb.AppendText(msg);
        }

        #region 冗余代码
        /*
               public string ReturnSendSocket2(NetworkStream objStm,TextBox tb)
        {
            byte[] buffer = new byte[1024];
            int length = objStm.Read(buffer, 0, buffer.Length);
            byte[] bytes = new byte[length];
            Array.Copy((Array)buffer, (Array)bytes, length);
            string @string = Encoding.GetEncoding("SHIFT-JIS").GetString(bytes);
            if (@string != "デ\x30FCタ取得")
                throw new Exception();
            Console.WriteLine(string.Format("接收: {0}", (object)@string));
            tb.AppendText(string.Format("接收: {0}", (object)@string) + "\r\n");
            return @string;
        }

        public string socket(NetworkStream stream, ref bool isSuccess, int encoderNo,TextBox tb)
        {
            byte[] buffer = new byte[15000];
            string str = string.Empty;
            int length;
            do
            {
                length = stream.Read(buffer, 0, buffer.GetLength(0));
            }
            while (stream.DataAvailable);
            if (length > 0)
            {
                byte[] bytes = new byte[length];
                Array.Copy((Array)buffer, (Array)bytes, length);
                str = Encoding.GetEncoding(encoderNo).GetString(bytes);
                Console.WriteLine(string.Format("接收: {0}", (object)str));
                tb.AppendText(string.Format("接收: {0}", (object)str) + "\r");
            }
            isSuccess = true;
            return str;
        }


        public void SendSocket(NetworkStream objStm, string sendtext, int encoderNo)
        {
            byte[] bytes = Encoding.GetEncoding(encoderNo).GetBytes(sendtext);
            objStm.Write(bytes, 0, bytes.Length);
        }

        public void ReturnSendSocket(NetworkStream objStm, int encoderNo,TextBox tb)
        {
            byte[] buffer = new byte[1024];
            int length = objStm.Read(buffer, 0, buffer.Length);
            byte[] bytes = new byte[length];
            Array.Copy((Array)buffer, (Array)bytes, length);
            string @string = Encoding.GetEncoding(encoderNo).GetString(bytes);
            if (@string != "DataGet")
                throw new Exception();
            Console.WriteLine(string.Format("接收: {0}", (object)@string));
            tb.AppendText(string.Format("接收: {0}", (object)@string) + "\r");
        }
         *   public string socket(NetworkStream stream, ref bool isSuccess, RichTextBox tb)
        {
            byte[] buffer = new byte[15000];
            string str = string.Empty;
            int length;
            do
            {
                length = stream.Read(buffer, 0, buffer.GetLength(0));
            }
            while (stream.DataAvailable);
            if (length > 0)
            {
                byte[] bytes = new byte[length];
                Array.Copy((Array)buffer, (Array)bytes, length);
                str = Encoding.GetEncoding("SHIFT-JIS").GetString(bytes);
                Console.WriteLine(string.Format("接收: {0}", (object)str));
               // tb.AppendText(string.Format("接收: {0}", (object)str) + "\r\n");
                output(string.Format("接收: {0}", (object)str) + "\r\n", tb);
            }
            isSuccess = true;
            return str;
        }

        public void Responsesocket(NetworkStream stream)
        {
            byte[] bytes = Encoding.GetEncoding("SHIFT-JIS").GetBytes("デ\x30FCタ取得");
            stream.Write(bytes, 0, bytes.Length);
            Thread.Sleep(100);
        }

        public void SendSocket(NetworkStream objStm, string sendtext)
        {
            byte[] bytes = Encoding.GetEncoding("SHIFT-JIS").GetBytes(sendtext);
            objStm.Write(bytes, 0, bytes.Length);
        }

        public void ReturnSendSocket(NetworkStream objStm, RichTextBox tb)
        {
            byte[] buffer = new byte[1024];
            int length = objStm.Read(buffer, 0, buffer.Length);
            byte[] bytes = new byte[length];
            Array.Copy((Array)buffer, (Array)bytes, length);
            string @string = Encoding.GetEncoding("SHIFT-JIS").GetString(bytes);
            if (@string != "デ\x30FCタ取得")
                throw new Exception();
            Console.WriteLine(string.Format("接收: {0}", (object)@string));
           // tb.AppendText(string.Format("接收: {0}", (object)@string) + "\r\n");
            output(string.Format("接收: {0}", (object)@string) + "\r\n", tb);
        }

       */
        #endregion
    }
}
