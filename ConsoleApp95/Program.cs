using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
namespace ConsoleApp95
{
    internal class Program
    {
        static async Task Main()
        {
            TcpListener server = null;

            try
            {
                IPAddress ipAddress = IPAddress.Parse("127.0.0.1");
                int port = 8888;

                server = new TcpListener(ipAddress, port);

                // Старт TcpListener та очікування клієнта
                server.Start();

                Console.WriteLine($"Сервер слухає на {ipAddress}:{port}");

                while (true)
                {
                    // Очікування асинхронного підключення клієнта
                    TcpClient client = await server.AcceptTcpClientAsync();

                    // Обробка клієнта в асинхронному режимі
                    _ = HandleClientAsync(client);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Помилка: {ex.Message}");
            }
            finally
            {
                server?.Stop();
            }
        }

        static async Task HandleClientAsync(TcpClient client)
        {
            try
            {
                NetworkStream stream = client.GetStream();

                byte[] data = new byte[256];
                int bytesRead = await stream.ReadAsync(data, 0, data.Length);
                string clientMessage = Encoding.ASCII.GetString(data, 0, bytesRead);

                Console.WriteLine($"Отримано від {((IPEndPoint)client.Client.RemoteEndPoint).Address}: {clientMessage}");

                string responseMessage = $"Привіт, клієнте!";
                byte[] responseData = Encoding.ASCII.GetBytes(responseMessage);
                await stream.WriteAsync(responseData, 0, responseData.Length);
            }
            finally
            {
                client.Close();
            }

        }
    }
}