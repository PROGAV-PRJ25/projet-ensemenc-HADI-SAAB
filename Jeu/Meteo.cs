public class Meteo
{
    public double Temperature { get; protected set; }
    public string Condition { get; protected set; }
    public double Pluie { get; protected set; }        // entre 0.0 et 1.0
    public double Soleil { get; protected set; }       // entre 0.0 et 1.0
    public bool Urgence => Condition == "Grele" || Condition == "Tempete"; 

    public Meteo(double temperature, string condition, double soleil, double pluie = 0)
    {
        Temperature = temperature;
        Condition = condition;
        Soleil = soleil;
        Pluie = pluie;
    }

    public void Afficher()
    {
        Console.WriteLine($"Condition : {Condition}, Température : {Temperature}°C, Eau : {Pluie}   Soleil : {Soleil}\n");
        
    }

}