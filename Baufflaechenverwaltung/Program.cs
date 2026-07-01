using System;
using System.Collections.Generic;

namespace Baufflaechenverwaltung
{
    public enum Nutzungstyp
    {
        Gewerbe, Landwirtschaft, Forst, Wohnnutzung, Brachflaeche, Infrastruktur
    }

    public enum Bebaubarkeit
    {
        Ja, Nein, Auflagen
    }

    public enum FlaechenStatus
    {
        Frei, Reserviert, Bebaut
    }

    public enum VorhabenStatus
    {
        AntragEingereicht, Genehmigt, Abgelehnt, InBearbeitung, Abgeschlossen
    }

    public class Metadaten
    {
        public string BPlanNummer { get; set; } = string.Empty;
        public double Bodenrichtwert { get; set; }
        public string Eigentuemer { get; set; } = string.Empty;
    }

    public class Bauflaeche
    {
        public string FlurstueckNummer { get; set; } = string.Empty;
        public double Groesse { get; set; }
        public string Lage { get; set; } = string.Empty;
        public Nutzungstyp AktuelleNutzung { get; set; }
        public Bebaubarkeit Bebaubarkeit { get; set; }
        public FlaechenStatus Status { get; set; }
        public Metadaten Metadaten { get; set; } = new Metadaten();

        public void MarkiereAls(FlaechenStatus neuerStatus)
        {
            Status = neuerStatus;
        }
    }

    public class Grundstueck
    {
        public string FlurstueckNummer { get; set; } = string.Empty;
        public List<Bauflaeche> Bauflaechen { get; set; } = new List<Bauflaeche>();

        public Grundstueck()
        { }

        public Grundstueck(string flurstueckNummer)
        {
            FlurstueckNummer = flurstueckNummer;
        }
    }

    public class Antragsteller
    {
        public string Name { get; set; } = string.Empty;
        public string Kontaktdaten { get; set; } = string.Empty;
        public string Firma { get; set; } = string.Empty;

        public Antragsteller()
        { }

        public Antragsteller(string name, string kontaktdaten, string firma)
        {
            Name = name;
            Kontaktdaten = kontaktdaten;
            Firma = firma;
        }
    }

    public class Bauvorhaben
    {
        public int Id { get; set; }
        public Antragsteller Antragsteller { get; set; } = new Antragsteller();
        public Nutzungstyp GeplanteNutzung { get; set; }
        public DateTime Beginn { get; set; }
        public DateTime Fertigstellung { get; set; }
        public VorhabenStatus Status { get; set; }
        public List<Bauflaeche> ZugewieseneFlaechen { get; set; } = new List<Bauflaeche>();

        public Bauvorhaben(int id, Antragsteller antragssteller, Nutzungstyp nutzung, DateTime beginn, DateTime fertigstellung, VorhabenStatus status)
        {
            Id = id;
            Antragsteller = antragssteller;
            GeplanteNutzung = nutzung;
            Beginn = beginn;
            Fertigstellung = fertigstellung;
            Status = status;
        }

        public void AktualisiereStatus(VorhabenStatus neuerStatus)
        {
            Status = neuerStatus;
        }

        public void VerknuepfeFlaeche(Bauflaeche flaeche)
        {
            if (!ZugewieseneFlaechen.Contains(flaeche))
            {
                ZugewieseneFlaechen.Add(flaeche);
            }
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            // Demonstration der Funktionalität
            var grundstueck = new Grundstueck("0015 00012");
            var meta = new Metadaten("BP-2022-089 – Wohngebiet Leipzig-Nord", 500.0, "Max Mustermann");
            var flaeche = new Bauflaeche("0015 00012 001/002", 500.0, "Leipzig-Nord", Nutzungstyp.Brachflaeche, Bebaubarkeit.Ja, FlaechenStatus.Frei, meta);
            grundstueck.Bauflaechen.Add(flaeche);

            var antragssteller = new Antragsteller("Erika Musterfrau", "erika@example.com", "Bau GmbH");
            var vorhaben = new Bauvorhaben(1, antragssteller, Nutzungstyp.Wohnnutzung, DateTime.Now, DateTime.Now.AddYears(1), VorhabenStatus.AntragEingereicht);

            vorhaben.VerknuepfeFlaeche(flaeche);
            flaeche.MarkiereAls(FlaechenStatus.Reserviert);
            vorhaben.AktualisiereStatus(VorhabenStatus.Genehmigt);

            Console.WriteLine($"Bauvorhaben {vorhaben.Id} Status: {vorhaben.Status}");
            Console.WriteLine($"Zugeordnete Fläche {flaeche.FlurstueckNummer} Status: {flaeche.Status}");
        }
    }
}