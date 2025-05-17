public enum Saison { Printemps, Ete, Automne, Hiver }

public class CycleSaisonnier
{
    public Saison SaisonActuelle { get; private set; } = Saison.Printemps;
    public int SemaineDansSaison { get; private set; } = 1;
    private Random rng = new Random();
    
    public void AvancerSemaine()
    {
        SemaineDansSaison++;
        if (SemaineDansSaison > 12) // 3 mois par saison
        {
            SemaineDansSaison = 1;
            SaisonActuelle = (Saison)(((int)SaisonActuelle + 1) % 4);
            Console.WriteLine($"\n=== Nouvelle saison : {SaisonActuelle} ===\n");
        }
    }
    
    public double GetTemperatureMoyenne()
    {
        double baseTemp = SaisonActuelle switch {
            Saison.Printemps => 12 + rng.Next(-3, 4),
            Saison.Ete => 25 + rng.Next(-5, 6),
            Saison.Automne => 10 + rng.Next(-5, 3),
            Saison.Hiver => 3 + rng.Next(-5, 3),
            _ => 15
        };
        return Math.Round(baseTemp, 1);
    }
    
    public double GetPluieProba()
    {
        return SaisonActuelle switch {
            Saison.Printemps => 0.6 + rng.NextDouble() * 0.2,
            Saison.Ete => 0.3 + rng.NextDouble() * 0.2,
            Saison.Automne => 0.5 + rng.NextDouble() * 0.2,
            Saison.Hiver => 0.4 + rng.NextDouble() * 0.2,
            _ => 0.5
        };
    }
    
    public override string ToString()
    {
        return $"{SaisonActuelle} (Semaine {SemaineDansSaison}/12)";
    }
}