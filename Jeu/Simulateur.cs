public class Simulateur
{
    private Terrain Terrain;
    private List<Plante> Catalogue;

    public Simulateur(Terrain terrain, List<Plante> catalogue)
    {
        Terrain = terrain;
        Catalogue = catalogue;
    }

    public void AfficherCatalogueEtSemer()
    {
        Console.WriteLine("Catalogue de plantes disponibles :");
        for (int i = 0; i < Catalogue.Count; i++)
        {
            Console.WriteLine($"{i + 1}. {Catalogue[i].Nom}");
        }

        Console.WriteLine("Combien de plantes voulez-vous semer ?");
        if (int.TryParse(Console.ReadLine(), out int nombrePlantes))
        {
            for (int i = 0; i < nombrePlantes; i++)
            {
                Console.WriteLine("Choisissez une plante à semer (numéro) :");
                if (int.TryParse(Console.ReadLine(), out int index) && index > 0 && index <= Catalogue.Count)
                {
                    Console.WriteLine("Coordonnée X : ");
                    int x = int.Parse(Console.ReadLine());
                    Console.WriteLine("Coordonnée Y : ");
                    int y = int.Parse(Console.ReadLine());
                    Terrain.SemerPlante(Catalogue[index - 1].Clone(), x, y);
                }
                else
                {
                    Console.WriteLine("Numéro invalide.");
                }
            }
        }
        Console.WriteLine("Appuyez sur une touche pour continuer...");
        Console.ReadKey();
    }

    public void PasserTour(int t)
    {
        if (!Terrain.Plantes.Any())
        {
            Console.WriteLine("Aucune plante semée !");
            Console.ReadKey();
            return;
        }

        Console.WriteLine($"\n--------  Semaine N° {t} --------");

        var meteo = Meteo.Generer();
        Console.WriteLine(meteo);

        var animal = Animaux.GenererAnimalAleatoire();
        if (animal != null)
        {
            Terrain.AnimauxDansLeTerrain.Add(animal);
            int x = Terrain.Rng.Next(0, 5);
            int y = Terrain.Rng.Next(0, 5);
            animal.Position = (x, y);

            if (Terrain.GrillePlantes.TryGetValue((x, y), out var plante) && plante != null)
            {
                animal.AttaquerPlante(plante);
                Console.WriteLine($"\n⚠️ {animal.GetType().Name} attaque une plante à ({x},{y}) !");
                Console.WriteLine("Menu d'urgence : 1. Retirer  2. Ignorer");
                string choix = Console.ReadLine();
                if (choix == "1")
                {
                    Terrain.AnimauxDansLeTerrain.Remove(animal);
                }
            }
        }

        Terrain.AfficherJardin();

        foreach (var plante in Terrain.Plantes.ToList())
        {
            plante.Pousser(meteo, Terrain);
            if (plante.EstMort)
            {
                Terrain.SupprimerPlante(plante);
            }
            else if (plante.PeutRecolter())
            {
                Console.WriteLine($"✅ Vous pouvez récolter {plante.Recolter()} {plante.Nom}");
            }
        }
        Console.WriteLine("✔️ Tour terminé");
        Console.WriteLine("Appuyez sur une touche pour continuer...");
        Console.ReadKey();
    }

    public void ArroserUnePlante()
    {
        Console.WriteLine("Quelle Plante voulez vs arroser ?");
        for (int i = 0; i < Terrain.Plantes.Count(); i++)
        {
            Console.WriteLine($"{i + 1} : {Terrain.Plantes[i].Nom}");
        }
        int indice = int.Parse(Console.ReadLine());
        Console.WriteLine("De quelle quantité voulez vous arroser la plante ? (entre 0.0-1.0)");
        double quantite = Convert.ToDouble(Console.ReadLine());
        Terrain.Plantes[indice - 1].Arrosser(quantite);
        Console.WriteLine("Appuyez sur une touche pour continuer...");
        Console.ReadKey();
    }

    public void TraiterUnePlante()
    {
        Console.WriteLine("Coordonnées de la plante à traiter (x y) :");
        string[] coords = Console.ReadLine()?.Split();
        if (coords?.Length == 2 &&
            int.TryParse(coords[0], out int x) &&
            int.TryParse(coords[1], out int y))
        {
            Terrain.TraiterPlante(x, y);
        }
        Console.WriteLine("Appuyez sur une touche pour continuer...");
        Console.ReadKey();
    }

    public void AfficherEtatPlantes()
    {
        foreach (var plante in Terrain.Plantes)
        {
            plante.AfficherEtat();
        }
        Console.WriteLine("Appuyez sur une touche pour continuer...");
        Console.ReadKey();
    }


    public void ProtegerContrePluie()
    {
        foreach (var plante in Terrain.Plantes)
        {
            plante.ProtectionPhysique = true;
        }
        Console.WriteLine("Toutes les plantes sont couvertes contre la pluie.");
        Console.WriteLine("Appuyez sur une touche pour continuer...");
        Console.ReadKey();
    }

    public void ProtegerContreGel()
    {
        foreach (var plante in Terrain.Plantes)
        {
            plante.EstProtegee = true;
        }
        Console.WriteLine("Un chauffage a été activé. Les plantes sont protégées du gel.");
        Console.WriteLine("Appuyez sur une touche pour continuer...");
        Console.ReadKey();
    }

    public void ProtegerContreCanicule()
    {
        foreach (var plante in Terrain.Plantes)
        {
            plante.EstProtegee = true;
        }
        Console.WriteLine("Un voile d’ombrage protège les plantes de la canicule.");
        Console.WriteLine("Appuyez sur une touche pour continuer...");
        Console.ReadKey();
    }

    public void ProtegerContreVent()
    {
        foreach (var plante in Terrain.Plantes)
        {
            plante.ProtectionPhysique = true;
        }
        Console.WriteLine("Des tuteurs et filets protègent les plantes du vent fort.");
        Console.WriteLine("Appuyez sur une touche pour continuer...");
        Console.ReadKey();
    }

}
