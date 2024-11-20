using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;

namespace Wetterdatenauswertung
{
    class Program
    {
        static void Main(string[] args)
        {
            bool running = true;
            string jsonPfad = @"Pfad/zu/deiner/JSON";

            while (running)
            {
                string jsonString = File.ReadAllText(jsonPfad);
                Wetterdaten? wetterdaten = JsonSerializer.Deserialize<Wetterdaten>(jsonString); // ? lässt null Werte zu

                if (wetterdaten == null)
                {
                    Console.WriteLine("Fehler beim Laden der Wetterdaten.");
                    return;
                }

                double durchschnittlicheTemperatur = wetterdaten.Staedte.Average(stadt => stadt.Temperatur); //Wetterdaten (Objekt), greift auf stadt (Liste) zu
                double durchschnittlicheLuftfeuchtigkeit = wetterdaten.Staedte.Average(stadt => stadt.Luftfeuchtigkeit);
                double maxGeschwindigkeit = wetterdaten.Staedte.Max(stadt => stadt.Windgeschwindigkeit);

                Console.WriteLine($"Durchschnittliche Temperatur: {durchschnittlicheTemperatur}C");
                Console.WriteLine($"Durchschnittliche Luftfeuchtigkeit: {durchschnittlicheLuftfeuchtigkeit}%");
                Console.WriteLine($"Maximale Windgeschwindigkeit: {maxGeschwindigkeit} km/h");
                Console.WriteLine();

                foreach (var stadt in wetterdaten.Staedte)
                {
                    Console.WriteLine($"Stadt: {stadt.Name}, Temperatur: {stadt.Temperatur}C, Luftfeuchtigkeit: {stadt.Luftfeuchtigkeit}%, Windgeschwindigkeit: {stadt.Windgeschwindigkeit} km/h");
                }

                Console.WriteLine();
                // Luftfeuchtigkeit mindestens 80%
                Console.WriteLine("Luftfeuchtigkeit mindestens 80%:");
                var nasseStaedte = wetterdaten.Staedte.Where(stadt => stadt.Luftfeuchtigkeit >= 80);
                foreach (var stadt in nasseStaedte)
                {
                    Console.WriteLine($"Stadt: {stadt.Name}, Luftfeuchtigkeit: {stadt.Luftfeuchtigkeit}%");
                }

                Console.WriteLine();
                // Temperatur mindestens 20C
                Console.WriteLine("Temperatur mindestens 20C:");
                var warmeStaedte = wetterdaten.Staedte.Where(stadt => stadt.Temperatur >= 20);
                foreach (var stadt in warmeStaedte)
                {
                    Console.WriteLine($"Stadt: {stadt.Name}, Temperatur: {stadt.Temperatur}C");
                }


                // Neue Stadt hinzufügen
                Console.WriteLine();
                Console.WriteLine("Neue Stadt hinzufügen:");
                Console.Write("Name der Stadt: ");
                string neuerStadtName = Console.ReadLine();

                Console.Write("Temperatur in C: ");
                int neueTemperatur = int.Parse(Console.ReadLine());

                Console.Write("Luftfeuchtigkeit in %: ");
                int neueLuftfeuchtigkeit = int.Parse(Console.ReadLine());

                Console.Write("Windgeschwindigkeit in km/h: ");
                int neueWindgeschwindigkeit = int.Parse(Console.ReadLine());

                // Zur Liste hinzufügen
                wetterdaten.Staedte.Add(new Stadt
                {
                    Name = neuerStadtName,
                    Temperatur = neueTemperatur,
                    Luftfeuchtigkeit = neueLuftfeuchtigkeit,
                    Windgeschwindigkeit = neueWindgeschwindigkeit
                });

                // Zeichenkette in JSON umwandeln
                string neueJsonDaten = JsonSerializer.Serialize(wetterdaten, new JsonSerializerOptions { WriteIndented = true });
                File.WriteAllText(jsonPfad, neueJsonDaten);

                Console.WriteLine($"Die Stadt '{neuerStadtName}' wurde gespeichert.");

                Console.WriteLine();
                Console.WriteLine("Programm neu starten? (Y/N):");

                
                ConsoleKeyInfo keyInfo = Console.ReadKey();
                Console.WriteLine();
                if (keyInfo.Key == ConsoleKey.Y)
                {
                    Console.Clear();
                    continue;
                }
                else
                {
                    running = false;
                }
            }
        }
    }
}