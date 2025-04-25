public abstract class Terrain
{
    public string Type { get; protected set; }
    public double Surface { get; set; }
    public double SurfaceOccupee { get; protected set; }
    public List<Plante>? Plantes { get; protected set; }
    public bool ADesMauvaiseHerbes { get; protected set; }
    public Random Rng { get; set; }

    public Terrain(string type, double surface)
    {
        Type = type;
        Surface = surface;
        SurfaceOccupee = 0;
        Plantes = new List<Plante>();
        Rng = new Random();

        // 20% de chance pour que le terrain possède des mauvaises herbes
        ADesMauvaiseHerbes = Rng.NextDouble() < 0.2;
    }

    public bool AjouterPlante(Plante plante)
    {
        if (SurfaceOccupee + plante.Espace > Surface)
            return false;

        Plantes.Add(plante);
        SurfaceOccupee += plante.Espace;
        return true;

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
