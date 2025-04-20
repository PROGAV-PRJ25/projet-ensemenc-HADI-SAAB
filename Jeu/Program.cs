Console.OutputEncoding = System.Text.Encoding.UTF8;

// Créer un terrain
Terrain terrain = new Terre( 5); // 5m²

// Créer quelques maladies
Maladie mildiou = new Maladie("Mildiou", 7, 3, 0.4);
Maladie rouille = new Maladie("Rouille", 5, 2, 0.5);

// Créer un catalogue de plantes
List<Plante> catalogue = new List<Plante>
{
    new Comestible("Tomate", "Légume", new List<string>{"Été"}, "Terre", (15, 30), 1, 1.2, new List<Maladie>{mildiou}, 3, 0.6, 0.8, 5),
    new Comestible("Carotte", "Légume", new List<string>{"Printemps"}, "Terre", (10, 25), 0.8, 1.0, new List<Maladie>{rouille}, 2, 0.5, 0.7, 4),
};

// Créer le simulateur
Simulateur simulateur = new Simulateur(terrain);

Menu menu = new Menu();

// Boucle principale
while (true)
{
    Console.Clear();

    int choix = menu.AfficherMenu();

    switch (choix)
    {
        case 1:
            Console.WriteLine("\nCatalogue des plantes :");
            for (int i = 0; i < catalogue.Count; i++)
                Console.WriteLine($"{i + 1}. {catalogue[i].Nom}");

            Console.Write("Choisissez une plante à semer : ");
            if (int.TryParse(Console.ReadLine(), out int index) && index >= 1 && index <= catalogue.Count)
            {
                Plante planteChoisie = catalogue[index - 1];
                simulateur.Semer(planteChoisie); // Ajoute au terrain
            }
            else
                Console.WriteLine("Choix invalide.");
            break;

        case 2:
            foreach (var p in terrain.Plantes)
                p.AfficherEtat();
            Console.WriteLine("Appuyez sur une touche pour continuer...");
            Console.ReadKey();
            break;

        case 3:
            Meteo meteo = Meteo.Generer(); // génère météo aléatoire
            Console.WriteLine($"\nSoleil : {meteo.Soleil}, Pluie : {meteo.Pluie}, Température : {meteo.Temperature}°C");
            foreach (var p in terrain.Plantes.ToList()) // ToList() pour éviter modification collection
            {
                p.Pousser(meteo, terrain);
                if (p.EstMort)
                    terrain.SupprimerPlante(p);
            }
            Console.WriteLine("Tour terminé !");
            Console.ReadKey();
            break;

        case 4:
            //simulateur.Sauvegarder();
            Console.ReadKey();
            break;

        case 5:
            //simulateur.Charger();
            Console.ReadKey();
            break;

        case 6:
            Console.WriteLine("Au revoir !");
            return;
    }
}
