public class Meteo
{
    public double Pluie { get; set; }      // entre 0.0 et 1.0
    public double Soleil { get; set; }     // entre 0.0 et 1.0
    public double Temperature { get; set; }
    public string Condition { get; protected set; }
    public static List<string> ListConditions = new List<string>{
        "Grele", "Tempete", "Chaud", "Frois"
    };
    public bool Urgence; // en °C

    public Meteo(double pluie, double soleil, double temp, string condition)
    {
        Pluie = pluie;
        Soleil = soleil;
        Temperature = temp;
        Condition = condition;
    }

    public static Meteo Generer()
    {
        Random rng = new Random();
        string condition = ListConditions[rng.Next(ListConditions.Count())];
        double pluie = Math.Round(rng.NextDouble(), 2);
        double soleil = Math.Round(rng.NextDouble(), 2);
        double temperature = rng.Next(-5, 36);
        return new Meteo(pluie, soleil, temperature, condition);
    }

    public override string ToString()
    {
        return $"Soleil 🌞 : {Soleil}, Pluie 🌧️ : {Pluie}, Température 🌡️ : {Temperature}°C";
    }
}

