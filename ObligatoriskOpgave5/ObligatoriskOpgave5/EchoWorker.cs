using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Opgave_1;

namespace ObligatoriskOpgave5
{
    public class EchoWorker
    {
        private static List<FootballPlayer> footballPlayers;
        public EchoWorker()
        {
            footballPlayers = new List<FootballPlayer>()
            {
                new FootballPlayer(1, "Johan", 1000, 10),
                new FootballPlayer(2, "Jakob", 10000, 12),
                new FootballPlayer(3, "Johannes", 10, 90)
            };
        }

        public void Start()
        {
            TcpListener listener = new TcpListener(2121);
            listener.Start();

            while (true)
            {
                TcpClient socket = listener.AcceptTcpClient();
                Task.Run(() =>
                {
                    DoClient(socket);
                });
            }
        }

        public void DoClient(TcpClient socket)
        {
            using (StreamReader streamReader = new StreamReader(socket.GetStream()))
            using (StreamWriter streamWriter = new StreamWriter(socket.GetStream()))
            {
                String str1 = streamReader.ReadLine();
                String str2 = streamReader.ReadLine();

                if (str1 == "HentAlle")
                {
                    var objectsJson = JsonSerializer.Serialize(footballPlayers.ToList());

                    streamWriter.WriteLine(objectsJson);
                }

                if (str1 == "Hent" && str2 != null)
                {
                    var id = Convert.ToInt16(str2);
                    var player = footballPlayers.Find(f => f.Id == id);

                    var objectJson = JsonSerializer.Serialize(player);

                    streamWriter.WriteLine(objectJson);
                }

                if (str1 == "Gem" && str2 == null)
                {
                    JsonSerializer.Serialize(footballPlayers);
                }

                streamWriter.Flush();

            }
        }
    }
}
