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
    public List<Intemperie> IntemperiesActuelles { get; } = new List<Intemperie>();
    public (int X, int Y) PositionSelection { get; private set; } = (0, 0);
    public bool UrgenceEnCours { get; private set; }
    public string MessageUrgence { get; private set; }
    public int Largeur { get; private set; }
    public int Hauteur { get; private set; }


    public Terrain(string type, double surface)
    {
        Type = type;
        Surface = surface;
        SurfaceOccupee = 0;
        Plantes = new List<Plante>();
        Rng = new Random();
        ADesMauvaiseHerbes = Rng.NextDouble() < 0.2;

        // Déduire la taille (arrondie au sol pour que ça tienne)
        int taille = (int)Math.Floor(Math.Sqrt(surface));
        Largeur = taille;
        Hauteur = taille;
    }


    public void DeplacerSelection(int deltaX, int deltaY)
    {
        PositionSelection = (
            Math.Clamp(PositionSelection.X + deltaX, 0, Largeur - 1),
            Math.Clamp(PositionSelection.Y + deltaY, 0, Hauteur - 1)
        );
    }


    public void DeclencherUrgence(Meteo meteo)
    {
        UrgenceEnCours = true;
        MessageUrgence = GenererMessageUrgence(meteo);
    }
    private string GenererMessageUrgence(Meteo meteo)
    {
        if (meteo.Condition == "Grêle")
            return "ALERTE GRÊLE ! Protégez vos plantes !";
        else if (AnimauxDansLeTerrain.Any())
            return $"ATTAQUE DE {AnimauxDansLeTerrain.First().Nom.ToUpper()} !";

        return "SITUATION D'URGENCE !";
    }

    public void VerifierFinUrgence()
    {
        if (AnimauxDansLeTerrain.Count == 0 && IntemperiesActuelles.Count == 0)
        {
            UrgenceEnCours = false;
            MessageUrgence = "";
        }
    }




    public void AjouterAnimaux(List<Animaux> animaux)
    {
        AnimauxDansLeTerrain = animaux;
    }


    // Supprimez les anciennes méthodes et gardez celles-ci :
    public bool AjouterPlante(Plante plante)
    {
        if (GrillePlantes.ContainsKey(PositionSelection))
            return false;

        GrillePlantes[PositionSelection] = plante;
        Plantes.Add(plante);
        SurfaceOccupee += plante.Espace;
        return true;
    }


    public void ArroserPlante(double quantiteEau)
    {
        if (GrillePlantes.TryGetValue(PositionSelection, out Plante plante))
        {
            plante.Arroser(quantiteEau);
        }
    }

    public bool TraiterPlante()
    {
        if (GrillePlantes.TryGetValue(PositionSelection, out Plante plante))
        {
            plante.AppliquerTraitement();
            return true;
        }
        return false;
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
        int lignes = Hauteur;
        int colonnes = Largeur;

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

    public void GenererIntemperieAleatoire()
    {
        string[] types = { "Grêle", "Tempête", "Sécheresse", "Vent violent" };
        string type = types[Rng.Next(types.Length)];
        var intemperie = new Intemperie(type, Rng.Next(3, 7), Rng.Next(1, 4));

        // Position aléatoire pour les intempéries localisées
        if (type == "Grêle" || type == "Tempête")
        {
            intemperie.Position = (Rng.Next(0, 5), Rng.Next(0, 5));
        }

        IntemperiesActuelles.Add(intemperie);
        Console.WriteLine($"ALERTE : Une {type} approche !");
    }



    public void AfficherJardin(bool modeUrgence = false)
    {
        Console.OutputEncoding = System.Text.Encoding.UTF8;

        if (modeUrgence)
        {
            Console.ForegroundColor = ConsoleColor.DarkRed;
            Console.WriteLine("═══════════ ! URGENCE ! ═══════════");
            Console.ResetColor();
        }

        for (int y = 0; y < Hauteur; y++)
        {
            for (int x = 0; x < Largeur; x++)
            {
                // Vérifier d'abord les animaux (priorité en mode urgence)
                var animal = AnimauxDansLeTerrain.FirstOrDefault(a => a.Position == (x, y));
                if (animal != null && modeUrgence)
                {
                    Console.ForegroundColor = ConsoleColor.Magenta;
                    Console.Write("🐛 "); // Symbole animal
                    Console.ResetColor();
                    continue;
                }

                // Afficher la plante sinon
                GrillePlantes.TryGetValue((x, y), out var plante);
                if (plante != null)
                {
                    Console.ForegroundColor = plante.EstMort ? ConsoleColor.DarkGray :
                        (plante.Sante < 30 ? ConsoleColor.Red :
                        (plante.Sante < 70 ? ConsoleColor.Yellow : ConsoleColor.Green));

                    Console.Write(" " + plante.Nom.Substring(0, 2) + "  ");
                }
                else if (animal != null)
                {
                    Console.ForegroundColor = ConsoleColor.Magenta;
                    Console.Write(animal.Nom.StartsWith("Lapin") ? "🐇" :
                                    animal.Nom.StartsWith("Pigeon") ? "🕊" :
                                    animal.Nom.StartsWith("Taupe") ? "🦡" : "🐛");
                    Console.ResetColor();
                }
                else
                {
                    Console.Write(Type == "Terre" ? " 🟫 " : " 🟨 ");
                }
                Console.ResetColor();
            }
            Console.WriteLine();
        }

        if (modeUrgence)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("═══════════════════════════════════════");
            Console.ResetColor();
        }
    }
    

   
}
