public abstract class Terrain
{
    public string Type { get; protected set; }
    public double Surface { get; set; }
    public double SurfaceOccupee { get; protected set; }
    public List<Plante>? Plantes { get; protected set; }
    public bool ADesMauvaiseHerbes { get; protected set; }
    public Random Rng { get; set; }
    public List<Animaux> AnimauxDansLeTerrain { get; private set; } = new List<Animaux>();
    public Dictionary<(int x, int y), Plante> GrillePlantes { get; set; } = new Dictionary<(int x, int y), Plante>();


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


   public bool AjouterPlante(Plante plante, int x, int y)
   {
        if (SurfaceOccupee + plante.Espace > Surface)
            return false;

        if (GrillePlantes.ContainsKey((x, y)))
        return false; // déjà occupé

        Plantes.Add(plante);
        GrillePlantes[(x, y)] = plante;
        SurfaceOccupee += plante.Espace;
        return true;
    }

    public void ArroserPlante(int x, int y)
    {
        if (GrillePlantes.TryGetValue((x, y), out Plante plante) && plante != null)
        {
            Console.WriteLine($"Vous avez choisi la plante : {plante.Nom} à ({x},{y})");
            Console.WriteLine("De quelle quantité voulez-vous arroser la plante ? (entre 0.0 - 1.0)");
            if (double.TryParse(Console.ReadLine(), out double quantite) && quantite >= 0.0 && quantite <= 1.0)
            {
                plante.Arrosser(quantite);
                
            }
            else
            {
                Console.WriteLine("Quantité invalide.");
            }
        }
        else
        {
            Console.WriteLine("Aucune plante trouvée à ces coordonnées.");
        }
    }

    public void TraiterPlante(int x, int y)
    {
        if (GrillePlantes.TryGetValue((x, y), out Plante plante) && plante != null)
        {
            plante.AppliquerTraitement();
            
        }
        else
        {
            Console.WriteLine("Aucune plante trouvée à ces coordonnées.");
        }
    }


    public void SemerPlante(Plante p, int x, int y)
    {
        if (GrillePlantes.ContainsKey((x, y)) && GrillePlantes[(x, y)] != null)
        {
            Console.WriteLine("Il y a déjà une plante à cet endroit.");
            return;
        }

        else if (Surface >= p.Espace)
        {
            GrillePlantes[(x, y)] = p;
            Console.WriteLine($"{p.Nom} semée en ({x}, {y}).");
            Plantes.Add(p);
            Surface -= p.Espace;
            
        }
        else 
        {
            Console.WriteLine($"Espace insuffisant dans le terrain pour semer la plante {p.Nom}.");
        }    
    }


    public void SupprimerPlante(Plante p)
    {
        // Dans la boucle du jeu ajouter les conditions : EstMort ? ou PeutRecolter
        Plantes.Remove(p);
        Surface += p.Espace;
        var key = GrillePlantes.FirstOrDefault(entry => entry.Value == p).Key;
        if (GrillePlantes.ContainsKey(key))
            GrillePlantes.Remove(key);
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
                grille[y, x] = "[  ]"; // Tu peux choisir un autre symbole de case vide si tu veux
            }
        }

        return grille;
    }

    public void AfficherJardin()
    {
        string[,] grille = new string[5, 5];

        // Initialiser la grille à vide
        for (int y = 0; y < 5; y++)
        {
            for (int x = 0; x < 5; x++)
            {
                grille[y, x] = "[  ]";
            }
        }

        // Afficher les plantes en fonction de leur position
        foreach (var kvp in GrillePlantes)
        {
            var (x, y) = kvp.Key;
            var plante = kvp.Value;
            grille[y, x] = "[" + plante.Nom.Substring(0, 2) + "]";
        }

        // Afficher les animaux (écrase l'affichage de la plante s'il y en a une dessous)
        foreach (var animal in AnimauxDansLeTerrain)
        {
            if (animal.Position != null)
            {
                var (x, y) = animal.Position.Value;
                grille[y, x] = "[" + animal.Nom.Substring(0, 2) + "]";
            }
        }

        // Affichage en console
        Console.WriteLine("Jardin actuel :");
        for (int y = 0; y < 5; y++)
        {
            for (int x = 0; x < 5; x++)
            {
                Console.Write(grille[y, x] + " ");
            }
            Console.WriteLine();
        }
    }

}
