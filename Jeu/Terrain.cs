public abstract class Terrain
{
    public string Type { get; protected set; }
    public double Surface { get; set; }
    public double SurfaceOccupee { get; protected set; }
    public List<Plante>? Plantes { get; protected set; }
    public bool ADesMauvaiseHerbes { get; protected set; }
    public Random Rng { get; set; }
    public List<Animaux> AnimauxDansLeTerrain { get; private set; } = new List<Animaux>();
    public Plante?[,] GrillePlantes { get; protected set; } = new Plante[10, 10];

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

    public void AjouterAnimaux(List<Animaux> animaux)
    {
        AnimauxDansLeTerrain = animaux;
    }


   public bool AjouterPlante(Plante plante)
   {
        if (SurfaceOccupee + plante.Espace > Surface)
            return false;

        int essais = 0;
        int x, y;
        do
        {
            x = Rng.Next(0, 10);
            y = Rng.Next(0, 10);
            essais++;
            if (essais > 100)
                return false;
        } while (GrillePlantes[x, y] != null);

        GrillePlantes[x, y] = plante;
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

    public string[,] GetGrille()
    {
        int lignes = 10; // Hauteur du terrain (Y)
        int colonnes = 10; // Largeur du terrain (X)

        string[,] grille = new string[lignes, colonnes];

        // Remplir la grille avec des points ou espaces (terrain vide)
        for (int y = 0; y < lignes; y++)
        {
            for (int x = 0; x < colonnes; x++)
            {
                grille[y, x] = "[ ]"; // Tu peux choisir un autre symbole de case vide si tu veux
            }
        }

        return grille;
    }

   public void AfficherJardin()
   {
        string[,] grille = new string[10, 10];

        for (int y = 0; y < 10; y++)
        {
            for (int x = 0; x < 10; x++)
            {
                if (GrillePlantes[x, y] != null)
                {
                    grille[y, x] = GrillePlantes[x, y]!.Nom.Substring(0, 2);
                }
                else
                {
                    grille[y, x] = "[ ]";
                }
            }
        }

        foreach (var animal in AnimauxDansLeTerrain)
        {
            if (animal.Position != null)
            {
                var (x, y) = animal.Position.Value;
                grille[y, x] = "🐾";
            }
        }

        Console.WriteLine("Jardin actuel :");
        for (int y = 0; y < 10; y++)
        {
            for (int x = 0; x < 10; x++)
            {
                Console.Write(grille[y, x] + " ");
            }
            Console.WriteLine();
        }
    }

}
