public class Intemperie
{
    // Type d'intempérie (ex : "Grêle", "Tempête", "Sécheresse", etc.)
    public string Type { get; }
    
    // Durée restante de l'intempérie (en unités de temps)
    public int Duree { get; set; }
    
    // Intensité de l'intempérie (niveau d'impact)
    public int Intensite { get; }
    
    // Position de l'intempérie sur le terrain (nullable)
    public (int X, int Y)? Position { get; set; }
    
    // Constructeur pour initialiser une intempérie avec son type, sa durée et son intensité
    public Intemperie(string type, int duree, int intensite)
    {
        Type = type;
        Duree = duree;
        Intensite = intensite;
    }
    
    // Méthode pour affecter une plante selon le type d'intempérie et son intensité
    public void AffecterPlante(Plante plante)
    {
        // Calcul des dégâts selon le type d'intempérie
        int degats = Type switch {
            "Grêle" => Intensite * 3,       // Grêle inflige 3 fois l'intensité en dégâts
            "Tempête" => Intensite * 2,     // Tempête inflige 2 fois l'intensité en dégâts
            "Sécheresse" => Intensite,      // Sécheresse inflige dégâts égaux à l'intensité
            _ => Intensite                  // Par défaut, dégâts égaux à l'intensité
        };
        
        // Appliquer les dégâts à la plante (réduction de sa santé)
        plante.ModifierSante(-degats);
        
        // Afficher un message décrivant l'effet de l'intempérie sur la plante
        Console.WriteLine($"La {Type} affecte {plante.Nom} (-{degats}% santé)");
    }
    
    // Méthode pour diminuer la durée restante de l'intempérie
    public void ReduireDuree()
    {
        Duree--;
    }
}
