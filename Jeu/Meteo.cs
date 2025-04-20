public class Meteo
{
    public double Pluie { get; set; }      // entre 0.0 et 1.0
    public double Soleil { get; set; }     // entre 0.0 et 1.0
    public double Temperature { get; set; }
    public string Condition { get; protected set; }
    public bool Urgence => Condition == "Grele" || Condition == "Tempete"; // en °C

    public static Meteo Generer()
    {
        Random rng = new Random();
        return new Meteo
        {
            Pluie = Math.Round(rng.NextDouble(), 2),
            Soleil = Math.Round(rng.NextDouble(), 2),
            Temperature = rng.Next(-5, 36)
        };
    }

    public override string ToString()
    {
        return $"Soleil : {Soleil}, Pluie : {Pluie}, Température : {Temperature}°C";
    }
}
