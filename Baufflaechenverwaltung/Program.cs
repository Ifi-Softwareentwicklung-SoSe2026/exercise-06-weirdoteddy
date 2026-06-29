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
    }

    public class Antragsteller
    {
        public string Name { get; set; } = string.Empty;
        public string Kontaktdaten { get; set; } = string.Empty;
        public string Firma { get; set; } = string.Empty;
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
            var grundstueck = new Grundstueck { FlurstueckNummer = "0015 00012" };
            var flaeche = new Bauflaeche
            {
                FlurstueckNummer = "0015 00012 001/002",
                Groesse = 500.0,
                Lage = "Leipzig-Nord",
                AktuelleNutzung = Nutzungstyp.Brachflaeche,
                Bebaubarkeit = Bebaubarkeit.Ja,
                Status = FlaechenStatus.Frei,
                Metadaten = new Metadaten
                {
                    BPlanNummer = "BP-2022-089 – Wohngebiet Leipzig-Nord",
                    Bodenrichtwert = 500.0,
                    Eigentuemer = "Max Mustermann"
                }
            };
            grundstueck.Bauflaechen.Add(flaeche);

            var vorhaben = new Bauvorhaben
            {
                Id = 1,
                Antragsteller = new Antragsteller { Name = "Erika Musterfrau", Firma = "Bau GmbH" },
                GeplanteNutzung = Nutzungstyp.Wohnnutzung,
                Beginn = DateTime.Now,
                Fertigstellung = DateTime.Now.AddYears(1),
                Status = VorhabenStatus.AntragEingereicht
            };

            vorhaben.VerknuepfeFlaeche(flaeche);
            flaeche.MarkiereAls(FlaechenStatus.Reserviert);
            vorhaben.AktualisiereStatus(VorhabenStatus.Genehmigt);

            Console.WriteLine($"Bauvorhaben {vorhaben.Id} Status: {vorhaben.Status}");
            Console.WriteLine($"Zugeordnete Fläche {flaeche.FlurstueckNummer} Status: {flaeche.Status}");
        }
    }
}