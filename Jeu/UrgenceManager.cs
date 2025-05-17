public class UrgenceManager
{
    private Terrain terrain;
    private Random rng = new Random();
    
    public bool EnCours { get; set; }
    public List<Animaux> AnimauxDangereux { get; } = new List<Animaux>();
    public List<Intemperie> Intemperies { get; } = new List<Intemperie>();
    public string MessageUrgence { get; private set; }
    public string CauseUrgence { get; private set; }
    private List<string> options = new List<string>();

    public UrgenceManager(Terrain terrain)
    {
        this.terrain = terrain;
    }

    public void DeclencherUrgence(Meteo meteo)
    {
        EnCours = true;
        AnimauxDangereux.Clear();
        Intemperies.Clear();

        // 60% chance d'animaux nuisibles, 40% d'intempéries
        if (rng.NextDouble() < 0.6)
        {
            GenererAnimauxDangereux(meteo);
            CauseUrgence = "Animaux nuisibles";
        }
        else
        {
            GenererIntemperies(meteo);
            CauseUrgence = "Intempéries";
        }

        MessageUrgence = GenererMessageUrgence();
    }

    private void GenererAnimauxDangereux(Meteo meteo)
    {
        int nbAnimaux = meteo.Condition.Contains("Pluie") ? rng.Next(2, 5) : rng.Next(1, 3);
        
        for (int i = 0; i < nbAnimaux; i++)
        {
            var animal = Animaux.GenererAnimalAleatoire();
            animal.Position = (rng.Next(0, terrain.Largeur), rng.Next(0, terrain.Hauteur));
            animal.Degats *= 2; // Double dégâts en urgence
            AnimauxDangereux.Add(animal);
        }
    }

    private void GenererIntemperies(Meteo meteo)
    {
        string type = meteo.Saison switch
        {
            Saison.Ete => rng.NextDouble() > 0.5 ? "Canicule" : "Sécheresse",
            Saison.Hiver => rng.NextDouble() > 0.5 ? "Tempête de neige" : "Gel intense",
            _ => rng.NextDouble() > 0.5 ? "Tempête" : "Grêle"
        };

        Intemperies.Add(new Intemperie(
            type: type,
            duree: rng.Next(2, 5),
            intensite: rng.Next(3, 6)
        ));
    }

    private string GenererMessageUrgence()
    {
        if (Intemperies.Any())
        {
            var intemp = Intemperies.First();
            return intemp.Type switch
            {
                "Canicule" => "Vague de chaleur extrême !",
                "Gel intense" => "Gel menaçant les cultures !",
                "Tempête" => "Tempête violente en approche !",
                _ => "Conditions météo dangereuses !"
            };
        }
        else
        {
            return AnimauxDangereux.Count switch
            {
                1 => "Animal nuisible détecté !",
                >1 => "Invasion d'animaux nuisibles !",
                _ => "Situation d'urgence !"
            };
        }
    }

    public void AfficherMenuUrgence()
    {
        Console.Clear();
        
        // En-tête visuelle améliorée
        Console.ForegroundColor = ConsoleColor.DarkRed;
        Console.WriteLine("╔════════════════════════════════════════════════════════════╗");
        Console.WriteLine("║                  ██  URGENCE  ██                          ║");
        Console.WriteLine("╠════════════════════════════════════════════════════════════╣");
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine($"║  {MessageUrgence,-50}  ║");
        Console.ForegroundColor = ConsoleColor.DarkRed;
        Console.WriteLine("╠════════════════════════════════════════════════════════════╣");
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine($"║  Cause: {CauseUrgence,-45}  ║");
        Console.ForegroundColor = ConsoleColor.DarkRed;
        Console.WriteLine("╚════════════════════════════════════════════════════════════╝");
        Console.ResetColor();

        // Détails des menaces avec plus de visibilité
        Console.ForegroundColor = ConsoleColor.DarkYellow;
        Console.WriteLine("\n  🚨 MENACES ACTIVES :");
        Console.ResetColor();

        foreach (var animal in AnimauxDangereux)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.Write($"  🐾 {animal.Nom} ");
            Console.ResetColor();
            Console.WriteLine($"à la position {animal.Position} (Dégâts: {animal.Degats}/tour)");
        }

        foreach (var intemp in Intemperies)
        {
            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.Write($"  ⚡ {intemp.Type} ");
            Console.ResetColor();
            Console.WriteLine($"(Intensité: {intemp.Intensite}/5) - {intemp.Duree} tours restants");
        }

        // Options d'action avec icônes
        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.WriteLine("\n  🛠️ ACTIONS DISPONIBLES :");
        Console.ResetColor();

        options = new List<string>
        {
            "🔊 Effrayer les animaux (50% de succès)",
            "🛡️ Protéger les plantes (coût: 20€)",
            "🌿 Répulsif naturel (réduit dégâts de 50%)",
            "⏳ Ignorer (risque important)"
        };

        // Ajout d'options spécifiques aux intempéries
        if (Intemperies.Any(i => i.Type.Contains("Gel") || i.Type.Contains("neige")))
        {
            options.Add("🔥 Chauffage d'urgence (coût: 30€)");
        }

        for (int i = 0; i < options.Count; i++)
        {
            Console.WriteLine($"  {i+1}. {options[i]}");
        }
    }

    public bool TraiterUrgence(int choix, Joueur joueur, out bool ignorer)
    {
        bool succes = false;
        ignorer = false;

        switch (choix)
        {
            case 1: // Effrayer
                succes = EffrayerAnimaux();
                break;

            case 2: // Protection
                if (joueur.DepenserArgent(20))
                {
                    terrain.Plantes.ForEach(p => p.ModifierSante(15));
                    succes = true;
                }
                break;

            case 3: // Répulsif
                AnimauxDangereux.ForEach(a => a.Degats = (int)(a.Degats * 0.5));
                succes = true;
                break;

            case 4: // Ignorer
                ignorer = true;
                AppliquerDegats(); // Applique les dégâts immédiatement
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("\n⚠ Vous ignorez l'urgence ! Les dégâts continueront...");
                Console.ResetColor();
                return false;

            case 5 when options.Count > 4: // Chauffage
                if (joueur.DepenserArgent(30))
                {
                    Intemperies.RemoveAll(i => i.Type.Contains("Gel") || i.Type.Contains("neige"));
                    succes = true;
                }
                break;
        }

        // Vérifier si l'urgence est terminée
        if (!AnimauxDangereux.Any() && !Intemperies.Any())
        {
            EnCours = false;
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("\n✅ Urgence résolue avec succès !");
            Console.ResetColor();
        }

        return succes;
    }

    private bool EffrayerAnimaux()
    {
        int nbEffrayes = AnimauxDangereux.RemoveAll(a => rng.NextDouble() > 0.5);

        Console.ForegroundColor = nbEffrayes > 0 ? ConsoleColor.Green : ConsoleColor.Red;
        Console.WriteLine($"\n{(nbEffrayes > 0 ? "✅ " : "❌ ")}{nbEffrayes} animaux ont été effrayés !");
        Console.ResetColor();

        return nbEffrayes > 0;
    }

    public void AppliquerDegats()
    {
        foreach (var animal in AnimauxDangereux)
        {
            if (animal.Position.HasValue)
            {
                var pos = animal.Position.Value;
                if (terrain.GrillePlantes.TryGetValue(pos, out var plante))
                {
                    plante.ModifierSante(-animal.Degats);
                }
            }
        }

        foreach (var intemp in Intemperies)
        {
            terrain.Plantes.ForEach(p => p.ModifierSante(-intemp.Intensite));
            intemp.Duree--;
        }

        Intemperies.RemoveAll(i => i.Duree <= 0);
    }

    public void AfficherBilanTour()
    {
        if (!EnCours) return;

        Console.ForegroundColor = ConsoleColor.DarkRed;
        Console.WriteLine("\n═════════ BILAN URGENCE ═════════");
        Console.ResetColor();

        if (AnimauxDangereux.Any())
        {
            Console.WriteLine($"Animaux restants: {AnimauxDangereux.Count}");
        }

        foreach (var intemp in Intemperies)
        {
            Console.WriteLine($"{intemp.Type} - {intemp.Duree} tours restants");
        }
    }
}