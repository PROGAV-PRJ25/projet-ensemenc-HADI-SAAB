Console.OutputEncoding = System.Text.Encoding.UTF8;

// Créer un terrain
Terrain terrain = new Terre( 5); // 5m²

// Créer quelques maladies
Maladie mildiou = new Maladie("Mildiou", 7, 3, 0.4);
Maladie rouille = new Maladie("Rouille", 5, 2, 0.5);
Maladie cochenille = new Maladie("Cochenille", 5, 4, 0.6);
Maladie fongique = new Maladie("fongique", 9, 4, 3);
Maladie botrytis = new Maladie("Botrytis", 2, 3, 4);
// Créer un catalogue de plantes
List<Plante> catalogue = new List<Plante>
{
    new Comestible("Tomate 🍅 ", "Légume", new List<string>{"Été"}, "Terre", (15, 30), 1, 1.2, new List<Maladie>{mildiou}, 3, 0.6, 0.8, 5),
    new Comestible("Carotte 🥕 ", "Légume", new List<string>{"Printemps"}, "Terre", (10, 25), 0.8, 1.0, new List<Maladie>{rouille}, 2, 0.5, 0.7, 4),
    new Comestible("Cactus 🌵", "Cactaceae", new List<string> {"Été"}, "Sable", (10, 40), 0.3, 0.2,  new List<Maladie> {cochenille}, 2, 0.2, 0.9, 1.5),
    new Comestible("Pastèque 🍉","Fruit",new List<string> { "Été", "Fin du printemps" },"Sable",(20, 35),1.5,0.3,new List<Maladie>{fongique},4,5.0,8.0,3.0),
    new Comestible("Fraise 🍓","Fruit",new List<string> { "Printemps", "Été" }, "Sol riche, frais et bien drainé",(15, 25), 0.3,0.4, new List<Maladie>{botrytis},20,2.0,6.0,0.25),

};

// Créer le simulateur
Simulateur simulateur = new Simulateur(terrain);

Menu menu = new Menu();
int tour = 0;
while (true)
{
    int choix = menu.AfficherMenu();


    switch (choix)
    {
        case 1:
            Console.WriteLine("Voici la liste des plantes : ");
            for (int i = 0; i < catalogue.Count(); i++)
            {
                Console.WriteLine($"{i + 1} : {catalogue[i].Nom}");
            }
            Console.Write("Choisissez une plante à semer : ");
            int index = int.Parse(Console.ReadLine());
            simulateur.Semer(catalogue[index - 1]);
            break;

        case 2:
            Console.WriteLine("De quelle quantité voulez vous arroser la plante ? (entre 0.0-1.0)");
            double quantite = Convert.ToDouble(Console.ReadLine());
            Console.WriteLine("Quelle Plante voulez vs arroser ?");
            for (int i = 0; i < terrain.Plantes.Count(); i++)
            {
                Console.WriteLine($"{i + 1} : {terrain.Plantes[i].Nom}");
            }
            int indice = int.Parse(Console.ReadLine());
            terrain.Plantes[indice - 1].Arrosser(quantite);
            Console.WriteLine("Appuyez sur une touche pour continuer...");
            Console.ReadKey();
            break;

        case 3:
            Console.WriteLine("De quelle quantité voulez vous arroser les plantes ? (entre 0.0-1.0)");
            double quant = Convert.ToDouble(Console.ReadLine());
            foreach (var p in terrain.Plantes)
            {
                p.Arrosser(quant);
            }
            Console.WriteLine("Appuyez sur une touche pour continuer...");
            Console.ReadKey();
            break;

        case 4:
            Console.WriteLine("Quelle plante voulez vous traiter  ?");
            for (int i = 0; i < terrain.Plantes.Count(); i++)
            {
                Console.WriteLine($"{i + 1}: {terrain.Plantes[i].Nom}");
            }
            int n = int.Parse(Console.ReadLine());
            terrain.Plantes[n - 1].AppliquerTraitement();
            Console.WriteLine("Appuyez sur une touche pour continuer...");
            Console.ReadKey();
            break;

        case 5:
            Console.WriteLine($"Semaine 🗓️ {tour} :");
            foreach (var p in terrain.Plantes)
            {
                p.AfficherEtat();
            }
            Console.WriteLine("Appuyez sur une touche pour continuer...");
            Console.ReadKey();
            break;

        case 6:
            tour++;
            Meteo meteo = Meteo.Generer();
            Animaux animaux = Animaux.GenererAnimaux();
            Console.WriteLine(meteo);
            foreach (var p in terrain.Plantes.ToList())
            {
                p.Pousser(meteo, terrain, animaux);
                if (p.EstMort)
                {
                    terrain.SupprimerPlante(p);
                }
            }
            Console.WriteLine("Tour terminé");
            Console.WriteLine("Appuyez sur une touche pour continuer...");
            Console.ReadKey();
            break;

        case 7:
            //simulateur.Sauvegarder();
            Console.ReadKey();
            break;

        case 8:
            //simulateur.Charger();
            Console.ReadKey();
            break;

        case 9:
            Console.WriteLine("Au revoir !");
            return;
    }
     
}

// à modifier le prmier tours on est oubligé de choisir Semer une Plante
// afficher les semaines
// controler le nombre d'action par tour 
