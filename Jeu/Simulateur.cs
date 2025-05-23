// Simulateur.cs
using System;
using System.Collections.Generic;
using System.Linq;

public class Simulateur
{
    // Propriétés du simulateur
    public Joueur Joueur { get; private set; }  // Le joueur
    public Terrain Terrain { get; private set; }  // Le terrain de jeu
    public Magasin Magasin { get; private set; }  // Le magasin pour acheter des plantes
    public Inventaire Inventaire { get; private set; }  // L'inventaire des récoltes
    public CycleSaisonnier CycleSaisonnier { get; private set; }  // Gestion des saisons
    public UrgenceManager UrgenceManager { get; private set; }  // Gestion des urgences
    public bool EnCours { get; set; } = true;  // Etat du jeu (en cours ou terminé)
    public Menu Menu { get; set; }  // Menu principal
    private Random rng = new Random();  // Générateur de nombres aléatoires
    private int tour = 0;  // Numéro du tour actuel
    private int nbActions = 3;  // Nombre d'actions restantes
    public NavigationManager navigation;  // Gestion de la navigation dans le terrain

    // Catalogue de plantes disponibles dans le jeu
    private List<Plante> catalogue;

    // Constructeur du simulateur
    public Simulateur()
    {
        InitialiserJeu();
    }

    // Méthode d'initialisation du jeu
    public void InitialiserJeu()
    {
        // Initialisation des maladies possibles
        var mildiou = new Maladie("Mildiou", 7, 3, 0.4);
        var rouille = new Maladie("Rouille", 5, 2, 0.5);
        var cochenille = new Maladie("Cochenille", 5, 4, 0.6);

        // Création du catalogue de plantes avec leurs caractéristiques
        catalogue = new List<Plante>
        {
            new Comestible("🍅 Tomate", "Légume", new List<string>{"Été"}, "Terre", (10, 30), 1, 1.2, new List<Maladie>{mildiou}, 3, 0.6, 0.8, 5),
            new Comestible("🥕 Carotte", "Légume", new List<string>{"Printemps"}, "Terre", (5, 25), 0.8, 1.0, new List<Maladie>{rouille}, 2, 0.5, 0.7, 4),
            new Comestible("🌵 Cactus", "Cactaceae", new List<string> {"Été"}, "Sable", (10, 40), 0.3, 0.2, new List<Maladie>{cochenille}, 2, 0.2, 0.9, 1.5),
            new Comestible("🍉 Pastèque","Fruit",new List<string> { "Été", "Fin du printemps" },"Sable",(20, 35),1.5,0.3,new List<Maladie>{mildiou},4,5.0,8.0,3.0),
            new Comestible("🍓 Fraise","Fruit",new List<string> { "Printemps", "Été" }, "Terre",(8, 25), 0.3,0.4, new List<Maladie>{cochenille},20,2.0,6.0,0.25)
        };

        // Initialisation des différents composants du jeu
        Joueur = new Joueur();
        Terrain = ChoisirTerrain();
        Magasin = new Magasin();
        Inventaire = new Inventaire();
        CycleSaisonnier = new CycleSaisonnier();
        UrgenceManager = new UrgenceManager(Terrain);
        navigation = new NavigationManager(Terrain);
        Menu = new Menu();
    }

    // Méthode pour choisir le type de terrain au début du jeu
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

    // Méthode principale pour exécuter une action du joueur
    public void ExecuterAction()
    {
        // Vérification s'il y a une urgence en cours
        if (UrgenceManager.EnCours)
        {
            GererUrgence();
            return;
        }
        
        // Affichage des informations du tour
        Console.WriteLine($"=== Tour {tour} | Saison: {CycleSaisonnier.SaisonActuelle} ===");
        Console.WriteLine($"Actions restantes: {nbActions}/3 | Argent: {Joueur.Argent:F2}€");
        Console.WriteLine("Que souhaitez-vous faire ?\n");
        
        // Affichage du menu et récupération du choix
        int choix = Menu.AfficherMenu();
        Console.Clear();
        
        // Vérification des conditions de fin de jeu
        if (Joueur.Argent == 0 && Joueur.SemisGratuitsRestants == 0 && Inventaire.Recoltes.Count() == 0)
        {
            Console.WriteLine($"Plus possible de faire quelque chose : argent null, semer grauit atteint et pas de recoltes à vendre");
            EnCours = false;
        }
        else
        {
            // Traitement du choix du joueur
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
                case 4: // Desherber
                    if (nbActions > 0)
                    {
                        Terrain.Desherber();
                        nbActions--;
                        Console.WriteLine("Appuyez sur une touche pour continuer...");
                        Console.ReadKey();
                    }
                    else ActionsEpuipees();
                    break;

                case 5: // Magasin
                    Magasin.GererMagasin(this);
                    break;

                case 6: // Passer tour
                    PasserTour();
                    nbActions = 3;
                    break;

                case 7: // Afficher recommandations
                    AfficherRecommandations();
                    break;

                case 8: // Afficher le jardin
                    Terrain.AfficherJardin();
                    Console.WriteLine("Appuyez sur une touche pour continuer...");
                    Console.ReadKey();
                    break;

                case 9: // Afficher l'état des plantes
                    AfficherEtatPlantes();
                    break;

                case 10: // Quitter le jeu
                    EnCours = false;
                    Console.WriteLine("Au revoir !");
                    break;
            }
        }
    }

    // Méthode pour gérer le semis des plantes
    private void GererSemis()
    {
        Console.WriteLine("\nMode d'obtention :");
        Console.WriteLine("1. Utiliser un semis gratuit");
        Console.WriteLine("2. Acheter au magasin");
        var choix = Console.ReadKey().KeyChar;

        Plante plante = null;
        bool aAchete = false;

        // Choix entre semis gratuit ou achat
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

        // Si une plante a été sélectionnée
        if (plante != null)
        {
            bool enNavigation = true;
            while (enNavigation)
            {
                navigation.AfficherAvecCurseur($"Plante à semer: {plante.Nom}");
                var key = navigation.Naviguer();

                if (key == ConsoleKey.Enter)
                {
                    // Validation : tenter d’ajouter la plante
                    if (aAchete)
                    {
                        double prix = Magasin.GetPrix(plante);
                        if (Joueur.Argent >= prix)
                        {
                            if (Terrain.AjouterPlante(plante.Clone()))
                            {
                                Joueur.DepenserArgent(prix);
                                Console.WriteLine("Plante achetée et semée avec succès!");
                                enNavigation = false;
                            }
                            else
                            {
                                Console.WriteLine("Emplacement indisponible!");
                                // Reste en navigation
                            }
                        }
                        else
                        {
                            Console.WriteLine("Fonds insuffisants!");
                            // Reste en navigation ou sortir ?
                            enNavigation = false; // Ou true si tu veux retenter
                        }
                    }
                    else
                    {
                        if (Terrain.AjouterPlante(plante.Clone()))
                        {
                            Console.WriteLine("Plante semée avec succès!");
                            enNavigation = false;
                        }
                        else
                        {
                            Console.WriteLine("Emplacement indisponible!");
                            // Reste en navigation
                        }
                    }
                }
                else if (key == ConsoleKey.Escape)
                {
                    // Annulation du semis
                    Console.WriteLine("Semis annulé.");
                    enNavigation = false;
                }
                // Sinon continuer navigation (flèches, etc.)
            }
        }

        Console.WriteLine("Appuyez sur une touche pour continuer...");
        Console.ReadKey();
    }
    // Méthode pour arroser une plante
    private void ArroserPlante()
    {
        // Vérification de la disponibilité des arrosoirs
        if (!Joueur.UtiliserArrosoir())
        {
            Console.WriteLine("Plus d'arrosoirs disponibles !");
            Console.WriteLine("Voulez-vous en acheter ? (O/N)");
            if (Console.ReadKey(true).Key == ConsoleKey.O)
            {
                Joueur.AcheterArrosoir();
            }
            return;
        }

        bool enNavigation = true;
        while (enNavigation)
        {
            navigation.AfficherAvecCurseur("Sélectionnez la plante à arroser");
            var key = navigation.Naviguer();

            if (key == ConsoleKey.Enter)
            {
                // Validation : plante sélectionnée, demander quantité
                Console.WriteLine("\nSaisir une quantité :");
                if (double.TryParse(Console.ReadLine(), out double quantite) && quantite > 0)
                {
                    // Ici, appliquer l'arrosage à la plante sélectionnée
                    // Exemple : Terrain.ArroserPlante(positionSelection, quantite);
                    Console.WriteLine($"Plante arrosée avec {quantite} unités.");
                    enNavigation = false;
                }
                else
                {
                    Console.WriteLine("Quantité invalide, réessayez.");
                    // Rester en navigation pour retenter
                }
            }
            else if (key == ConsoleKey.Escape)
            {
                Console.WriteLine("\nArrosage annulé.");
                enNavigation = false;
            }
            // Sinon : flèches → continuer navigation
        }
    }
    // Méthode pour traiter une plante contre les maladies
    private void TraiterPlante()
    {
        // Vérification de la disponibilité des traitements
        if (!Joueur.UtiliserTraitement())
        {
            Console.WriteLine("Plus de traitements disponibles !");
            Console.WriteLine("Voulez-vous en acheter ? (O/N)");
            if (Console.ReadKey(true).Key == ConsoleKey.O)
            {
                Joueur.AcheterTraitement();
            }
            return;
        }

        bool enNavigation = true;
        while (enNavigation)
        {
            navigation.AfficherAvecCurseur("Sélectionnez la plante à traiter");
            var key = navigation.Naviguer();

            if (key == ConsoleKey.Enter)
            {
                // Valider le traitement sur la plante sélectionnée dans le terrain
                if (Terrain.TraiterPlante()) // À adapter selon signature : devrait traiter la plante sous le curseur
                {
                    Console.WriteLine("\nPlante traitée avec succès !");
                }
                else
                {
                    Console.WriteLine("\nAucune plante à cet emplacement !");
                }
                enNavigation = false;
            }
            else if (key == ConsoleKey.Escape)
            {
                Console.WriteLine("\nTraitement annulé.");
                enNavigation = false;
            }
            // Sinon, continuer la navigation avec les flèches
        }
    }
 // Méthode pour gérer les urgences
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

    // Méthode pour passer au tour suivant
    private void PasserTour()
    {
        tour++;
        CycleSaisonnier.AvancerSemaine();

        // Génération de la météo pour ce tour
        var meteo = Meteo.Generer(CycleSaisonnier.SaisonActuelle);
        meteo.Afficher();

        // Mise à jour de toutes les plantes
        foreach (var p in Terrain.Plantes.ToList())
        {
            p.Pousser(meteo, Terrain, CycleSaisonnier.SaisonActuelle);

            // Vérification si la plante peut être récoltée ou est morte
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

        // Affichage du bilan du tour
        UrgenceManager.AfficherBilanTour();

        Console.WriteLine($"\n=== Tour {tour} terminé ===");
        Console.WriteLine($"Saison actuelle: {CycleSaisonnier.SaisonActuelle}");
        Console.ReadKey();
    }

    // Méthode appelée quand le joueur n'a plus d'actions
    private void ActionsEpuipees()
    {
        Console.WriteLine("Actions épuisées pour ce tour!");
        PasserTour();
        nbActions = 3;
        Console.WriteLine("Appuyez sur une touche pour continuer...");
        Console.ReadKey();
    }

    // Méthode pour afficher les recommandations selon la saison
    private void AfficherRecommandations()
    {
        Console.WriteLine("=== Recommandations ===");
        Console.WriteLine($"Saison actuelle: {CycleSaisonnier.SaisonActuelle}");

        // Filtrage des plantes recommandées pour la saison actuelle
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
    
    // Méthode pour afficher l'état de toutes les plantes
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