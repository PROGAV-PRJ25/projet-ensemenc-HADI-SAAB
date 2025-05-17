// Magasin.cs
using System;
using System.Collections.Generic;
using System.Linq;

public class Magasin
{
    public List<Plante> PlantesDisponibles { get; private set; }
    public double PrixTerrain { get; private set; } = 200.0;
    private Random rng = new Random();

    public Magasin()
    {
        InitialiserCatalogue();
    }

    private void InitialiserCatalogue()
    {
        var mildiou = new Maladie("Mildiou", 7, 3, 0.4);
        var rouille = new Maladie("Rouille", 5, 2, 0.5);
        var cochenille = new Maladie("Cochenille", 5, 4, 0.6);

        PlantesDisponibles = new List<Plante>
        {
            new Comestible("🍅 Tomate", "Légume", new List<string>{"Été"}, "Terre", (15, 30), 1, 1.2, new List<Maladie>{mildiou}, 3, 0.6, 0.8, 5),
            new Comestible("🥕 Carotte", "Légume", new List<string>{"Printemps"}, "Terre", (10, 25), 0.8, 1.0, new List<Maladie>{rouille}, 2, 0.5, 0.7, 4),
            new Comestible("🍉 Pastèque", "Fruit", new List<string>{"Été", "Fin du printemps"}, "Sable", (20, 35), 1.5, 0.3, new List<Maladie>{mildiou}, 4, 5.0, 8.0, 3.0),
            new Comestible("🍓 Fraise", "Fruit", new List<string>{"Printemps", "Été"}, "Sol riche, frais et bien drainé", (15, 25), 0.3, 0.4, new List<Maladie>{cochenille}, 20, 2.0, 6.0, 0.25),
            
            // 🌸 Fleurs
            new Fleur("🌷 Tulipe", "Fleur", new List<string>{"Printemps"}, "Terre", (10, 20), 0.5, 0.8, new List<Maladie>{rouille}, 1, 0.4, 0.6, 0.4),
            new Fleur("🌻 Tournesol", "Fleur", new List<string>{"Été"}, "Terre", (20, 35), 0.6, 1.0, new List<Maladie>{mildiou}, 2, 1.0, 1.0, 1.5),
            
            // 🌿 Médicinales
            new Medicinale("🌿 Menthe", "Aromatique", new List<string>{"Printemps", "Été"}, "Terre", (10, 25), 0.4, 0.6, new List<Maladie>{cochenille}, 3, 0.8, 0.7, 0.3),
            new Medicinale("🪻 Lavande", "Aromatique", new List<string>{"Été"}, "Sable", (15, 30), 0.3, 0.5, new List<Maladie>{rouille}, 2, 0.6, 0.9, 0.6),
        };

    }

    public double GetPrix(Plante plante) => plante.Espace * 10 + plante.Productivite * 2;

    public Plante ChoisirPlanteAvecFleches(Joueur joueur)
    {
        int indexSelection = 0;
        ConsoleKeyInfo touche;

        do
        {
            Console.Clear();
            Console.WriteLine("▼ Utilisez les flèches pour choisir ▼\n");
            Console.WriteLine($"Argent: {joueur.Argent:F2}€\n");

            for (int i = 0; i < PlantesDisponibles.Count; i++)
            {
                var p = PlantesDisponibles[i];

                if (i == indexSelection)
                {
                    Console.BackgroundColor = ConsoleColor.White;
                    Console.ForegroundColor = ConsoleColor.Black;
                }

                Console.WriteLine($"{p.Nom} - {GetPrix(p):F2}€");
                Console.ResetColor();
                Console.WriteLine($"  Saisons: {string.Join(", ", p.Saisons)}");
                Console.WriteLine($"  Espace: {p.Espace}m² | Productivité: {p.Productivite}\n");
            }

            Console.WriteLine("\n[Entrée] Acheter • [Échap] Annuler");
            touche = Console.ReadKey(true);

            if (touche.Key == ConsoleKey.DownArrow)
                indexSelection = Math.Min(indexSelection + 1, PlantesDisponibles.Count - 1);
            if (touche.Key == ConsoleKey.UpArrow)
                indexSelection = Math.Max(indexSelection - 1, 0);

        } while (touche.Key != ConsoleKey.Enter && touche.Key != ConsoleKey.Escape);

        return touche.Key == ConsoleKey.Enter ? PlantesDisponibles[indexSelection] : null;
    }

    public void GererMagasin(Simulateur simulateur)
    {
        bool enMagasin = true;
        while (enMagasin)
        {
            Console.Clear();
            Console.WriteLine("=== 🏪 MAGASIN ===");
            Console.WriteLine($"Argent disponible: {simulateur.Joueur.Argent:F2}€");
            Console.WriteLine("\nQue souhaitez-vous faire ?");
            Console.WriteLine("1. 🌱 Acheter des plantes");
            Console.WriteLine("2. 🛒 Acheter des fournitures");
            Console.WriteLine("3. 💰 Vendre les récoltes");
            Console.WriteLine("4. 🔙 Retour");
            Console.Write("\nVotre choix : ");

            var choix = Console.ReadKey().KeyChar;
            Console.WriteLine();

            switch (choix)
            {
                case '1': // Acheter plantes
                    var plante = ChoisirPlanteAvecFleches(simulateur.Joueur);
                    if (plante != null)
                    {
                        double prix = GetPrix(plante);
                        if (simulateur.Joueur.Argent >= prix)
                        {
                            bool enNavigation = true;
                            while (enNavigation)
                            {
                                simulateur.navigation.AfficherAvecCurseur($"Plante à semer: {plante.Nom}");
                                enNavigation = simulateur.navigation.Naviguer();

                                if (!enNavigation)
                                {
                                    if (simulateur.Terrain.AjouterPlante(plante.Clone()))
                                    {
                                        simulateur.Joueur.DepenserArgent(prix);
                                        Console.WriteLine("Plante achetée et semée avec succès!");
                                    }
                                    else
                                    {
                                        Console.WriteLine("Emplacement indisponible!");
                                        enNavigation = true; // Recommencer le choix
                                    }
                                }
                            }
                        }
                        else
                        {
                            Console.WriteLine("Fonds insuffisants!");
                        }
                        Console.WriteLine("\nAppuyez sur une touche pour continuer...");
                        Console.ReadKey();
                    }
                    break;

                case '2': // Acheter fournitures
                    AfficherMenuAchat(simulateur.Joueur);
                    break;

                case '3': // Vendre récoltes
                    simulateur.Inventaire.Afficher(simulateur.Joueur.Argent);
                    if (simulateur.Inventaire.Recoltes.Any())
                    {
                        Console.Write("\nNuméro de la récolte à vendre (0 pour annuler) : ");
                        if (int.TryParse(Console.ReadLine(), out int index) && index > 0 && index <= simulateur.Inventaire.Recoltes.Count)
                        {
                            simulateur.Inventaire.VendreRecolte(index - 1, simulateur.Joueur);
                            Console.WriteLine("\nAppuyez sur une touche pour continuer...");
                            Console.ReadKey();
                        }
                    }
                    else
                    {
                        Console.WriteLine("\nAucune récolte à vendre.");
                        Console.WriteLine("Appuyez sur une touche pour continuer...");
                        Console.ReadKey();
                    }
                    break;

                case '4': // Retour
                    enMagasin = false;
                    break;
            }
        }
    }

    private void AfficherMenuAchat(Joueur joueur)
    {
        Console.Clear();
        Console.WriteLine("=== FOURNITURES ===");
        Console.WriteLine($"Argent disponible: {joueur.Argent:F2}€");
        Console.WriteLine("\nQue souhaitez-vous acheter ?");
        Console.WriteLine($"1. Arrosoir (5€) - Stock: {joueur.Arrosoirs}");
        Console.WriteLine($"2. Traitement (8€) - Stock: {joueur.Traitements}");
        Console.WriteLine($"3. Pack de départ (15€) - 3 arrosoirs + 2 traitements");
        Console.WriteLine("4. Retour");
        Console.Write("\nVotre choix : ");

        var choix = Console.ReadKey().KeyChar;
        Console.WriteLine();

        switch (choix)
        {
            case '1':
                if (joueur.DepenserArgent(5))
                {
                    joueur.AcheterArrosoir();
                    Console.WriteLine($"\nAchat réussi ! Arrosoirs disponibles: {joueur.Arrosoirs}");
                }
                else
                {
                    Console.WriteLine("Fonds insuffisants !");
                }
                break;

            case '2':
                if (joueur.DepenserArgent(8))
                {
                    joueur.AcheterTraitement();
                    Console.WriteLine($"\nAchat réussi ! Traitements disponibles: {joueur.Traitements}");
                }
                else
                {
                    Console.WriteLine("Fonds insuffisants !");
                }
                break;

            case '3':
                if (joueur.DepenserArgent(15))
                {
                    joueur.AcheterArrosoir(3);
                    joueur.AcheterTraitement(2);
                    Console.WriteLine("\nPack acheté ! Vous obtenez :");
                    Console.WriteLine($"- 3 arrosoirs (Total: {joueur.Arrosoirs})");
                    Console.WriteLine($"- 2 traitements (Total: {joueur.Traitements})");
                }
                else
                {
                    Console.WriteLine("Fonds insuffisants pour le pack !");
                }
                break;
        }

        if (choix != '4')
        {
            Console.WriteLine("\nAppuyez sur une touche pour continuer...");
            Console.ReadKey();
        }
    }
}