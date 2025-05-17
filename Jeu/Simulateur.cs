// Simulateur.cs
using System;
using System.Collections.Generic;
using System.Linq;

public class Simulateur
{
    public Joueur Joueur { get; private set; }
    public Terrain Terrain { get; private set; }
    public Magasin Magasin { get; private set; }
    public Inventaire Inventaire { get; private set; }
    public CycleSaisonnier CycleSaisonnier { get; private set; }
    public UrgenceManager UrgenceManager { get; private set; }
    public bool EnCours { get; set; } = true;
    public Menu Menu { get; set; }
    private Random rng = new Random();
    private int tour = 0;
    private int nbActions = 3;
    public NavigationManager navigation;


    // Catalogue de plantes
    private List<Plante> catalogue;

    public Simulateur()
    {
        InitialiserJeu();
    }

    public void InitialiserJeu()
    {
        // Initialisation des maladies
        var mildiou = new Maladie("Mildiou", 7, 3, 0.4);
        var rouille = new Maladie("Rouille", 5, 2, 0.5);
        var cochenille = new Maladie("Cochenille", 5, 4, 0.6);

        catalogue = new List<Plante>
        {
            new Comestible("🍅 Tomate", "Légume", new List<string>{"Été"}, "Terre", (10, 30), 1, 1.2, new List<Maladie>{mildiou}, 3, 0.6, 0.8, 5),
            new Comestible("🥕 Carotte", "Légume", new List<string>{"Printemps"}, "Terre", (5, 25), 0.8, 1.0, new List<Maladie>{rouille}, 2, 0.5, 0.7, 4),
            new Comestible("🌵 Cactus", "Cactaceae", new List<string> {"Été"}, "Sable", (10, 40), 0.3, 0.2, new List<Maladie>{cochenille}, 2, 0.2, 0.9, 1.5),
            new Comestible("🍉 Pastèque","Fruit",new List<string> { "Été", "Fin du printemps" },"Sable",(20, 35),1.5,0.3,new List<Maladie>{mildiou},4,5.0,8.0,3.0),
            new Comestible("🍓 Fraise","Fruit",new List<string> { "Printemps", "Été" }, "Terre",(8, 25), 0.3,0.4, new List<Maladie>{cochenille},20,2.0,6.0,0.25)
        };

        Joueur = new Joueur();
        Terrain = ChoisirTerrain();
        Magasin = new Magasin();
        Inventaire = new Inventaire();
        CycleSaisonnier = new CycleSaisonnier();
        UrgenceManager = new UrgenceManager(Terrain);
        navigation = new NavigationManager(Terrain);
        Menu = new Menu();
    }

    private Terrain ChoisirTerrain()
    {
        Console.WriteLine("Choisissez le type de terrain :");
        Console.WriteLine("1. Terre");
        Console.WriteLine("2. Sable");
        Console.WriteLine("3. Argile");

        int choix;
        while (!int.TryParse(Console.ReadLine(), out choix) || choix < 1 || choix > 3)
        {
            Console.WriteLine("Choix invalide. Entrez 1, 2 ou 3 :");
        }

        Console.Write("Entrez la surface du terrain (en m²) : ");
        double surface;
        while (!double.TryParse(Console.ReadLine(), out surface) || surface <= 0)
        {
            Console.WriteLine("Surface invalide. Veuillez entrer un nombre positif.");
        }

        return choix switch
        {
            1 => new Terre(surface),
            2 => new Sable(surface),
            3 => new Argile(surface),
            _ => new Terre(surface)
        };
    }



   
    public void ExecuterAction()
    {
        if (UrgenceManager.EnCours)
        {
            GererUrgence();
            return;
        }

        
        
        Console.WriteLine($"=== Tour {tour} | Saison: {CycleSaisonnier.SaisonActuelle} ===");
        Console.WriteLine($"Actions restantes: {nbActions}/3 | Argent: {Joueur.Argent:F2}€");
        Console.WriteLine("Que souhaitez-vous faire ?\n");
        int choix = Menu.AfficherMenu();
        Console.Clear();

        switch (choix)
        {
            case 1: // Semer
                if (nbActions > 0)
                {
                    GererSemis();
                    nbActions--;
                }
                else ActionsEpuipees();
                break;

            case 2: // Arroser
                if (nbActions > 0)
                {
                    ArroserPlante();
                    nbActions--;
                    Console.WriteLine("Appuyez sur une touche pour continuer...");
                    Console.ReadKey();
                }
                else ActionsEpuipees();
                break;

            case 3: // Traiter
                if (nbActions > 0)
                {
                    TraiterPlante();
                    nbActions--;
                    Console.WriteLine("Appuyez sur une touche pour continuer...");
                    Console.ReadKey();
                }
                else ActionsEpuipees();
                break;

            case 4: // Magasin
                Magasin.GererMagasin(this);
                break;

            case 5: // Passer tour
                PasserTour();
                nbActions = 3;
                break;

            case 6: // Afficher recommandations
                AfficherRecommandations();
                break;

            case 7:
                Terrain.AfficherJardin();
                Console.WriteLine("Appuyez sur une touche pour continuer...");
                Console.ReadKey();
                break;

            case 8:
                AfficherEtatPlantes();
                break;

            case 9: // Quitter
                EnCours = false;
                Console.WriteLine("Au revoir !");
                break;
        }
    }

    private void GererSemis()
    {
        Console.WriteLine("\nMode d'obtention :");
        Console.WriteLine("1. Utiliser un semis gratuit");
        Console.WriteLine("2. Acheter au magasin");
        var choix = Console.ReadKey().KeyChar;

        Plante plante = null;
        bool aAchete = false;

        if (choix == '1' && Joueur.PeutSemerGratuit())
        {
            plante = new SelectionPlante(catalogue).AfficherEtChoisir();
            if (plante != null) Joueur.UtiliserSemisGratuit();
        }
        else if (choix == '2')
        {
            plante = Magasin.ChoisirPlanteAvecFleches(Joueur);
            if (plante != null) aAchete = true;
        }

        if (plante != null)
        {
            bool enNavigation = true;
            while (enNavigation)
            {
                navigation.AfficherAvecCurseur($"Plante à semer: {plante.Nom}");
                enNavigation = navigation.Naviguer();

                if (!enNavigation)
                {
                    if (Terrain.AjouterPlante(plante.Clone()))
                    {
                        Console.WriteLine($"Plante {(aAchete ? "achetée et " : "")}semée avec succès!");
                        if (aAchete) Joueur.DepenserArgent(Magasin.GetPrix(plante));
                    }
                    else
                    {
                        Console.WriteLine("Emplacement indisponible!");
                        if (aAchete) Joueur.AjouterArgent(Magasin.GetPrix(plante));
                        enNavigation = true;
                    }
                }
            }
        }
        Console.WriteLine("Appuyez sur une touche pour continuer...");
        Console.ReadKey();
    }

    private void ArroserPlante()
    {
        if (!Joueur.UtiliserArrosoir())
        {
            Console.WriteLine("Plus d'arrosoirs disponibles !");
            Console.WriteLine("Voulez-vous en acheter ? (O/N)");
            if (Console.ReadKey().Key == ConsoleKey.O)
            {
                Joueur.AcheterArrosoir();
                return;
            }
            return;
        }

        bool enNavigation = true;
        while (enNavigation)
        {
            navigation.AfficherAvecCurseur("Sélectionnez la plante à arroser");
            enNavigation = navigation.Naviguer();
            Console.WriteLine("Saisir une quantité :");
            double quantite = Convert.ToDouble(Console.ReadLine());
            if (!enNavigation)
            {
                Terrain.ArroserPlante(quantite);
                Console.WriteLine("Plante arrosée avec succès !");
            }
        }
    }

    private void TraiterPlante()
    {
        if (!Joueur.UtiliserTraitement())
        {
            Console.WriteLine("Plus de traitements disponibles !");
            Console.WriteLine("Voulez-vous en acheter ? (O/N)");
            if (Console.ReadKey().Key == ConsoleKey.O)
            {
                Joueur.AcheterTraitement();
                return;
            }
            return;
        }

        bool enNavigation = true;
        while (enNavigation)
        {
            navigation.AfficherAvecCurseur("Sélectionnez la plante à traiter");
            enNavigation = navigation.Naviguer();

            if (!enNavigation)
            {
                Terrain.TraiterPlante();
                Console.WriteLine("Plante traitée avec succès !");
            }
        }
    }

    private void GererUrgence()
    {
        while (UrgenceManager.EnCours)
        {
            UrgenceManager.AfficherMenuUrgence();

            if (int.TryParse(Console.ReadKey().KeyChar.ToString(), out int choix))
            {
                bool ignorer;
                UrgenceManager.TraiterUrgence(choix, Joueur, out ignorer);

                if (ignorer)
                {
                    UrgenceManager.EnCours = false;
                    Console.WriteLine("\nAppuyez sur une touche pour retourner au menu principal...");
                    Console.ReadKey();
                    return;
                }

                if (!UrgenceManager.EnCours)
                {
                    Console.WriteLine("\nAppuyez sur une touche pour continuer...");
                    Console.ReadKey();
                }
            }
        }
    }

    private void PasserTour()
    {
        tour++;
        CycleSaisonnier.AvancerSemaine();

        var meteo = Meteo.Generer(CycleSaisonnier.SaisonActuelle);
        meteo.Afficher();

        foreach (var p in Terrain.Plantes.ToList())
        {
            p.Pousser(meteo, Terrain, CycleSaisonnier.SaisonActuelle);

            if (p.EstMort || p.PeutRecolter())
            {
                Console.WriteLine($"[DEBUG] {p.Nom} - EstMort: {p.EstMort}, PeutRecolter: {p.PeutRecolter()}");
                if (p.PeutRecolter())
                {
                    int quantite = p.Recolter();
                    double gain = quantite * Magasin.GetPrix(p) * 0.3;
                    Joueur.AjouterArgent(gain);
                    Inventaire.AjouterRecolte(p, CycleSaisonnier.SaisonActuelle, Terrain);
                }
                Terrain.SupprimerPlante(p);
            }
        }

        UrgenceManager.AfficherBilanTour();

        Console.WriteLine($"\n=== Tour {tour} terminé ===");
        Console.WriteLine($"Saison actuelle: {CycleSaisonnier.SaisonActuelle}");
        Console.ReadKey();
    }

    private void ActionsEpuipees()
    {
        Console.WriteLine("Actions épuisées pour ce tour!");
        PasserTour();
        nbActions = 3;
        Console.WriteLine("Appuyez sur une touche pour continuer...");
        Console.ReadKey();
    }

    private void AfficherRecommandations()
    {
        Console.WriteLine("=== Recommandations ===");
        Console.WriteLine($"Saison actuelle: {CycleSaisonnier.SaisonActuelle}");

        var plantesRecommandees = catalogue
            .Where(p => p.Saisons.Contains(CycleSaisonnier.SaisonActuelle.ToString()))
            .ToList();

        if (plantesRecommandees.Any())
        {
            Console.WriteLine("\nPlantes recommandées pour cette saison:");
            foreach (var p in plantesRecommandees)
            {
                Console.WriteLine($"- {p.Nom} (Prix: {Magasin.GetPrix(p):F2}€)");
            }
        }
        else
        {
            Console.WriteLine("\nAucune plante spécifiquement recommandée cette saison.");
        }

        Console.WriteLine("\nAppuyez sur une touche pour continuer...");
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
}