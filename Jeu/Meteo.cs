public class Meteo
{
    public string Saison{get; set;}
    public double Temperature{get; set;}
    public List<string> ConditionsMeteo {get; set; }
    public List<string> Urgence {get; set; } //c'est quoi l'urgence
    public Meteo(string saison, double temperature, List<string> conditionMeteo, List<string> urgence)
    {
        Saison = saison;
        Temperature = temperature;
        ConditionsMeteo = conditionMeteo; 
        Urgence = urgence;
    }

    
}