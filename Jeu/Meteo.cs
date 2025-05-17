public class Meteo
{
    // Propriétés
    public double Pluie { get; } // entre 0.0 et 1.0
    public double Soleil { get; } // entre 0.0 et 1.0
    public double Temperature { get; } // en °C
    public string Condition { get; }
    public bool EstExtreme { get; }
    public Saison Saison { get; }

    // Conditions météo possibles
    public static List<string> ConditionsNormales = new List<string> 
    { 
        "Pluie légère", "Ensoleillé", "Nuageux", "Venteux", "Brouillard" 
    };
    
    public static List<string> ConditionsExtremes = new List<string>
    {
        "Tempête", "Canicule", "Gel intense", "Grêle", "Vent violent"
    };

    public Meteo(double pluie, double soleil, double temperature, string condition, Saison saison, bool estExtreme = false)
    {
        Pluie = Math.Round(Math.Clamp(pluie, 0, 1), 2);
        Soleil = Math.Round(Math.Clamp(soleil, 0, 1), 2);
        Temperature = Math.Round(temperature, 1);
        Condition = condition;
        Saison = saison;
        EstExtreme = estExtreme;
    }

    // Génération selon la saison
    public static Meteo Generer(Saison saison, bool forcerUrgence = false)
    {
        Random rng = new Random();
        bool estUrgence = forcerUrgence || rng.NextDouble() < 0.15;

        if (estUrgence)
        {
            return GenererMeteoExtreme(saison, rng);
        }

        // Paramètres de base par saison avec variations plus marquées
        var (pluieBase, soleilBase, tempMin, tempMax) = saison switch
        {
            Saison.Printemps => (0.5, 0.6, 5, 18),  // Printemps humide et doux
            Saison.Ete => (0.2, 0.9, 18, 35),       // Été sec et chaud
            Saison.Automne => (0.6, 0.4, 0, 15),    // Automne pluvieux et frais
            Saison.Hiver => (0.3, 0.3, -5, 8),      // Hiver froid et nuageux
            _ => (0.5, 0.5, 10, 25)                // Par défaut
        };

        // Variation aléatoire avec plus d'impact de la saison
        double pluie = Math.Clamp(pluieBase * (0.7 + rng.NextDouble() * 0.6), 0, 1);
        double soleil = Math.Clamp(soleilBase * (0.7 + rng.NextDouble() * 0.6), 0, 1);
        
        // Température plus variable selon la saison
        double temp = tempMin + (tempMax - tempMin) * (0.3 + rng.NextDouble() * 0.7);
        
        // Conditions météo adaptées à la saison
        string condition = saison switch
        {
            Saison.Printemps when pluie > 0.7 => "Pluie printanière",
            Saison.Printemps when soleil > 0.7 => "Beau temps printanier",
            Saison.Ete when soleil > 0.8 => "Grand soleil",
            Saison.Ete when pluie > 0.5 => "Orage estival",
            Saison.Automne when pluie > 0.6 => "Pluie automnale",
            Saison.Hiver when temp < 0 => "Gelée matinale",
            _ => ConditionsNormales[rng.Next(ConditionsNormales.Count)]
        };
        
        return new Meteo(pluie, soleil, temp, condition, saison);
    }

    private static Meteo GenererMeteoExtreme(Saison saison, Random rng)
    {
        return saison switch
        {
            Saison.Printemps => new Meteo(
                pluie: 0.9,
                soleil: 0.1,
                temperature: rng.Next(2, 8),
                condition: "Tempête printanière",
                saison: saison,
                estExtreme: true),

            Saison.Ete => new Meteo(
                pluie: 0.1,
                soleil: 1.0,
                temperature: rng.Next(35, 42),
                condition: "Canicule",
                saison: saison,
                estExtreme: true),

            Saison.Automne => new Meteo(
                pluie: 0.8,
                soleil: 0.2,
                temperature: rng.Next(-2, 5),
                condition: "Tempête automnale",
                saison: saison,
                estExtreme: true),

            Saison.Hiver => new Meteo(
                pluie: 0.7,
                soleil: 0.3,
                temperature: rng.Next(-10, -2),
                condition: "Tempête de neige",
                saison: saison,
                estExtreme: true),

            _ => new Meteo(1.0, 0.0, 5.0, "Conditions extrêmes", saison, true)
        };
    }

    // Affichage coloré amélioré
    public void Afficher()
    {
        ConsoleColor couleur = EstExtreme ? ConsoleColor.Red : 
                             Saison == Saison.Printemps ? ConsoleColor.Green :
                             Saison == Saison.Ete ? ConsoleColor.Yellow :
                             Saison == Saison.Automne ? ConsoleColor.DarkYellow : ConsoleColor.Cyan;
        
        Console.ForegroundColor = couleur;
        Console.WriteLine("┌────────────────────────────────────┐");
        Console.WriteLine($"│ METEO {(EstExtreme ? "EXTRÊME" : Saison.ToString().ToUpper()),-26} │");
        Console.WriteLine("├────────────────────────────────────┤");
        
        Console.Write("│ ");
        Console.ForegroundColor = ConsoleColor.Blue;
        Console.Write($"🌧 Pluie: {Pluie * 100}%".PadRight(18));
        
        Console.ForegroundColor = couleur;
        Console.Write(" │ ");
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.Write($"☀ Soleil: {Soleil * 100}%".PadRight(17));
        
        Console.ForegroundColor = couleur;
        Console.WriteLine(" │");
        
        Console.Write("│ ");
        Console.ForegroundColor = Temperature > 30 ? ConsoleColor.Red : 
                                 Temperature < 0 ? ConsoleColor.Cyan : ConsoleColor.White;
        Console.Write($"🌡 Temp: {Temperature}°C".PadRight(18));
        
        Console.ForegroundColor = couleur;
        Console.Write(" │ ");
        Console.ForegroundColor = EstExtreme ? ConsoleColor.Red : ConsoleColor.White;
        Console.Write($"📛 {Condition}".PadRight(17));
        
        Console.ForegroundColor = couleur;
        Console.WriteLine(" │");
        Console.WriteLine("└────────────────────────────────────┘");
        Console.ResetColor();
    }

    public override string ToString()
    {
        return $"{Condition} | Pluie: {Pluie*100}% | Soleil: {Soleil*100}% | Temp: {Temperature}°C";
    }
}