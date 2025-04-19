public abstract class Terrain
{
    public string Type { get; protected set; }
    public double Surface { get; set; }
    public List<Plante>? Plantes { get; protected set; }
    public bool ADesMauvaiseHerbes { get; protected set; }
    public Random Rng { get; set; }

    public Terrain(string type, double surface, List<Plante> plantes)
    {
        Type = type;
        Surface = surface;
        Plantes = plantes;
        Rng = new Random();

        // 20% de chance pour que le terrain possède des mauvaises herbes
        ADesMauvaiseHerbes = Rng.NextDouble() < 0.2;
    }

    public void AjouterPlante(Plante plante)
    {
        Plantes.Add(plante);
    }

    public void SupprimerPlante(Plante p)
    {
        // Dans la boucle du jeu ajouter les conditions : EstMort ? ou PeutRecolter
        Plantes.Remove(p);
        Surface += p.Espace;
    }

    public void Desherber()
    {
        if (ADesMauvaiseHerbes)
        {
            ADesMauvaiseHerbes = false;
            Console.WriteLine("Le terrain a été désherbé");
        }
        else
        {
            Console.WriteLine("Ce terrain est déjà propre");
        }
    }
}
