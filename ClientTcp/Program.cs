using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace ClientTcp
{
    class Program
    {
        static void Main(string[] args)
        {
            const string ip = "127.0.0.1";
            const int port = 8082;

            //Задаем параметры ip и порт
            var udpEndPoint = new IPEndPoint(IPAddress.Parse(ip), port);

            //0 - использование ipv4, 1 -,2 протокол tcp
            var udpSocket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
            udpSocket.Bind(udpEndPoint);

            while(true)
            {
                Console.WriteLine("Введите сообщение");
                var message = Console.ReadLine();

                EndPoint ServerEndPoint = new IPEndPoint(IPAddress.Parse("127.0.0.1"),8080);
                udpSocket.SendTo(Encoding.UTF8.GetBytes(message), ServerEndPoint);


                var buffer = new byte[256];
                //колличество реально полученных байт
                var size = 0;
                //собирает данные
                var data = new StringBuilder();
                EndPoint senderEndPoint = new IPEndPoint(IPAddress.Any, 0);
                do
                {
                    //будет сохранен адрес клиента

                    size = udpSocket.ReceiveFrom(buffer, ref senderEndPoint);
                    data.Append(Encoding.UTF8.GetString(buffer));
                }
                while (udpSocket.Available > 0);

                Console.WriteLine(data);
                Console.ReadLine();
                
            }
        }
        public void TCPclient()
        {
            const string ip = "127.0.0.1";
            const int port = 8080;

            //Задаем параметры ip и порт
            var tcpEndPoint = new IPEndPoint(IPAddress.Parse(ip), port);

            //0 - использование ipv4, 1 -,2 протокол tcp
            var tcpSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            Console.WriteLine("Введите сообщение");
            var message = Console.ReadLine();

            //кодируем в байты сообщение message
            var data = Encoding.UTF8.GetBytes(message);
            //подключаемся к серверу по ip;
            tcpSocket.Connect(tcpEndPoint);
            //отправляем сообщение
            tcpSocket.Send(data);

            var buffer = new byte[256];
            //колличество реально полученных байт
            var size = 0;
            //собирает данные
            var answer = new StringBuilder();

            do
            {
                //Сколько байт было переданно
                size = tcpSocket.Receive(buffer);
                //обрабатывает по колличеству в заданном буфере и добавляет все в строку дата
                answer.Append(Encoding.UTF8.GetString(buffer, 0, size));

            }
            while (tcpSocket.Available < 0);

            Console.WriteLine(answer.ToString());

            tcpSocket.Shutdown(SocketShutdown.Both);
            tcpSocket.Close();

            Console.ReadLine();

        }
    }

   

}
