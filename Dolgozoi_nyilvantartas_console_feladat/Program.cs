using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace Dolgozoi_nyilvantartas
{
    internal class Program
    {
        static List<Dolgozo> dolgozok = new List<Dolgozo>();
        static async Task Main(string[] args)
        {
            await DolgozoiNyilvantartas();
            Console.ReadKey();
        }

        private static async Task DolgozoiNyilvantartas()
        {
            string url = "https://retoolapi.dev/Kc6xuH/data";
            var client = new HttpClient();
            var request = new HttpRequestMessage(HttpMethod.Get, url);
            var response = await client.SendAsync(request);
            if (!response.IsSuccessStatusCode)
            {
                throw new Exception("Hiba a lekérdezés során");
            }

            string jsonString = await response.Content.ReadAsStringAsync(); // JSON string

            // JSON deserializálás
            var dolgozok = Dolgozo.FromJson(jsonString);
            // Elemek száma
            Console.WriteLine($"Az elemek száma: {dolgozok.Count}");

            // Legmagasabb fizetéssel rendelkező dolgozó neve
            var legmagasabbFizetesuDolgozo = dolgozok.OrderByDescending(d => d.DolgozoSalary).FirstOrDefault();
            Dolgozo legjobbanKereso = dolgozok.Find(a => a.DolgozoSalary == dolgozok.Max(d => d.DolgozoSalary));
            if (legmagasabbFizetesuDolgozo != null)
            {
                Console.WriteLine($"A legmagasabb fizetéssel rendelkező dolgozó neve: {legjobbanKereso.DolgozoName}");
            }
            else
            {
                Console.WriteLine("Nincs dolgozó az adatok között.");
            }

            // Munkakörök csoportosítása és számolása
            var munkakorok = dolgozok.GroupBy(d => d.DolgozoPosition).Select(group => new { DolgozoPosition = group.Key, DolgozoSzama = group.Count() });

            // Kiírás
            Console.WriteLine("\nMunkakörök és dolgozóik száma:");
            foreach (var munkakor in munkakorok)
            {
                Console.WriteLine($"{munkakor.DolgozoPosition}: {munkakor.DolgozoSzama}");
            }
        }
    }
}
