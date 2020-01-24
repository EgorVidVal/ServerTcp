using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace ServerTcp
{
    class Program
    {
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

            while(true)
            {
                //подсокет для обрабатывания клиента обрабатывает данные и уничтожается и по новой для следующего клиента.
                var listener = tcpSocket.Accept();
                //хранилище данных
                var buffer = new byte[256];
                //колличество реально полученных байт
                var size = 0;
                //собирает данные
                var data = new StringBuilder();

                do
                {
                    //Сколько байт было переданно
                    size = listener.Receive(buffer);
                    //обрабатывает по колличеству в заданном буфере и добавляет все в строку дата
                    data.Append(Encoding.UTF8.GetString(buffer, 0, size));

                }
                while (listener.Available < 0);

                Console.WriteLine(data);

                listener.Send(Encoding.UTF8.GetBytes("Успех"));

                listener.Shutdown(SocketShutdown.Both);

                Console.ReadLine();



            }

        }
    }
}
