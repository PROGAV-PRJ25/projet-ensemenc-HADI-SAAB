public class Intemperie
{
    public string Type { get; } // "Grêle", "Tempête", "Sécheresse", etc.
    public int Duree { get; set; }
    public int Intensite { get; }
    public (int X, int Y)? Position { get; set; }
    
    public Intemperie(string type, int duree, int intensite)
    {
        Type = type;
        Duree = duree;
        Intensite = intensite;
    }
    
    public void AffecterPlante(Plante plante)
    {
        int degats = Type switch {
            "Grêle" => Intensite * 3,
            "Tempête" => Intensite * 2,
            "Sécheresse" => Intensite,
            _ => Intensite
        };
        
        plante.ModifierSante(-degats);
        Console.WriteLine($"La {Type} affecte {plante.Nom} (-{degats}% santé)");
    }
    
    public void ReduireDuree()
    {
        Duree--;
    }
}

