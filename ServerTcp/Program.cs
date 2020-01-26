using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;


namespace ServerTcp
{
    class Program
    {
        static void ByteToImage(byte[] b)
        {
            System.IO.MemoryStream memoryStream1 = new System.IO.MemoryStream();
            foreach (byte b1 in b) memoryStream1.WriteByte(b1);
            Image image1 = Image.FromStream(memoryStream1);
            image1.Save("C:\\pict.bmp", System.Drawing.Imaging.ImageFormat.Bmp);
        }
        //https://www.youtube.com/watch?v=ZRGgBtUgJKE;
        static void Main(string[] args)
        {
            const string ip = "127.0.0.1";
            const int port = 8080;

            //Задаем параметры ip и порт
            var tcpEndPoint = new IPEndPoint(IPAddress.Parse(ip), port);

            //0 - использование ipv4, 1 -,2 протокол tcp
            var tcpSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            //Что именно он должен слушать
            tcpSocket.Bind(tcpEndPoint);
            //Сколько клиентов может быть подключено
            tcpSocket.Listen(5);

            while (true)
            {
                //подсокет для обрабатывания клиента обрабатывает данные и уничтожается и по новой для следующего клиента.
                var listener = tcpSocket.Accept();
                //хранилище данных
                byte[] buffer = new byte[14634140];
                //колличество реально полученных байт
                var size = 0;
                //собирает данные
                byte[] data = new byte[1000];

                do
                {
                    //Сколько байт было переданно
                    size = listener.Receive(buffer);
                    //обрабатывает по колличеству в заданном буфере и добавляет все в строку дата
                    //data[1]= Encoding.UTF8.GetBytes(buffer, 0, size);

                }
                while (listener.Available < 0);

                Console.WriteLine(buffer[12]);
                ByteToImage(buffer);

                listener.Send(Encoding.UTF8.GetBytes("Успех"));

                listener.Shutdown(SocketShutdown.Both);

                Console.ReadLine();



            }

        }

       
    }
}
