// Enumération représentant les différentes saisons
public enum Saison { Printemps, Ete, Automne, Hiver }

public class CycleSaisonnier
{
    // Saison actuelle, accessible en lecture seule à l'extérieur
    public Saison SaisonActuelle { get; private set; } = Saison.Printemps;
    
    // Semaine en cours dans la saison (de 1 à 12)
    public int SemaineDansSaison { get; private set; } = 1;
    
    // Générateur de nombres aléatoires
    private Random rng = new Random();
    
    // Méthode pour avancer d'une semaine dans le cycle saisonnier
    public void AvancerSemaine()
    {
        SemaineDansSaison++; // Incrémenter la semaine
        
        // Si on dépasse la 12e semaine (fin de la saison)
        if (SemaineDansSaison > 12) // 12 semaines = 3 mois par saison
        {
            SemaineDansSaison = 1; // Réinitialiser à la semaine 1
            
            // Passer à la saison suivante (en bouclant après Hiver)
            SaisonActuelle = (Saison)(((int)SaisonActuelle + 1) % 4);
            
            // Afficher un message indiquant la nouvelle saison
            Console.WriteLine($"\n=== Nouvelle saison : {SaisonActuelle} ===\n");
        }
    }
    
    // Méthode pour obtenir la température moyenne actuelle selon la saison
    public double GetTemperatureMoyenne()
    {
        // Déterminer la température de base en fonction de la saison avec une variation aléatoire
        double baseTemp = SaisonActuelle switch {
            Saison.Printemps => 12 + rng.Next(-3, 4),  // entre 9 et 15
            Saison.Ete => 25 + rng.Next(-5, 6),        // entre 20 et 30
            Saison.Automne => 10 + rng.Next(-5, 3),    // entre 5 et 13
            Saison.Hiver => 3 + rng.Next(-5, 3),       // entre -2 et 6
            _ => 15 // valeur par défaut (au cas où)
        };
        return Math.Round(baseTemp, 1); // Arrondi à une décimale
    }
    
    // Méthode pour obtenir la probabilité de pluie selon la saison
    public double GetPluieProba()
    {
        // Retourne une probabilité entre deux valeurs selon la saison avec une petite variation aléatoire
        return SaisonActuelle switch {
            Saison.Printemps => 0.6 + rng.NextDouble() * 0.2, // entre 0.6 et 0.8
            Saison.Ete => 0.3 + rng.NextDouble() * 0.2,       // entre 0.3 et 0.5
            Saison.Automne => 0.5 + rng.NextDouble() * 0.2,   // entre 0.5 et 0.7
            Saison.Hiver => 0.4 + rng.NextDouble() * 0.2,     // entre 0.4 et 0.6
            _ => 0.5 // valeur par défaut
        };
    }
    
    // Surcharge de ToString() pour afficher la saison et la semaine courante
    public override string ToString()
    {
        return $"{SaisonActuelle} (Semaine {SemaineDansSaison}/12)";
    }
}
