using System;
using System.Net.Sockets;
using System.Windows.Controls;

namespace CallSystem.Class
{
   public class ConConnection
    {
       private TcpClient client;
       public NetworkStream ConConne(TcpListener server,RichTextBox tb)
       {
           Console.WriteLine("等待连接中");
           //tb.AppendText("等待连接中" + "\r\n");
           output("等待连接中" + "\r", tb);
           this.client = server.AcceptTcpClient();
           Console.WriteLine("已连接");
           //tb.AppendText("已连接" + "\r\n");
           output("已连接" + "\r",tb);
           return this.client.GetStream();
       }

       public void conconne()
       {
           try
           {
               if (this.client == null)
                   return;
               this.client.Close();

           }
           catch (Exception ex) {
               System.Windows.MessageBox.Show(ex.ToString());
           }
       }
       private delegate void outputDelegate(string msg, RichTextBox tb);
       private void output(string msg, RichTextBox tb)
       {
           tb.Dispatcher.Invoke(new outputDelegate(outputAction), msg,tb);
       }

       private void outputAction(string msg, RichTextBox tb)
       {
           tb.AppendText(msg);
          // tb.AppendText("\n");
       }
    }
}
