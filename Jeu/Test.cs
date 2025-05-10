

/*
using System.Threading.Channels;

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
    new Comestible("🍅 Tomate", "Légume", new List<string>{"Été"}, "Terre", (15, 30), 1, 1.2, new List<Maladie>{mildiou}, 3, 0.6, 0.8, 5),
    new Comestible("🥕 Carotte", "Légume", new List<string>{"Printemps"}, "Terre", (10, 25), 0.8, 1.0, new List<Maladie>{rouille}, 2, 0.5, 0.7, 4),
    new Comestible("🌵 Cactus", "Cactaceae", new List<string> {"Été"}, "Sable", (10, 40), 0.3, 0.2,  new List<Maladie> {cochenille}, 2, 0.2, 0.9, 1.5),
    new Comestible("🍉 Pastèque","Fruit",new List<string> { "Été", "Fin du printemps" },"Sable",(20, 35),1.5,0.3,new List<Maladie>{fongique},4,5.0,8.0,3.0),
    new Comestible("🍓 Fraise","Fruit",new List<string> { "Printemps", "Été" }, "Sol riche, frais et bien drainé",(15, 25), 0.3,0.4, new List<Maladie>{botrytis},20,2.0,6.0,0.25),
};




// Créer le simulateur
Simulateur simulateur = new Simulateur(terrain);

int nbActions = 3;
Menu menu = new Menu();
int tour = 0;

terrain.AfficherJardin();

Console.WriteLine("Voici la liste des plantes : ");
for (int i = 0; i < catalogue.Count(); i++)
{
    Console.WriteLine($"{i + 1} : {catalogue[i].Nom}");
}
Console.Write("Choisissez une plante à semer : ");
int index = int.Parse(Console.ReadLine());
Console.Write("Choisissez un ligne : ");
int ligne = int.Parse(Console.ReadLine());
Console.Write("Choisissez un ligne : ");
int colonne = int.Parse(Console.ReadLine());
terrain.AjouterPlante(catalogue[index - 1], ligne - 1, colonne - 1);

Meteo meteo;

void PasserTour()
{
    if (!terrain.Plantes.Any())
    {
        Console.WriteLine("Aucune plante semée ! Tour pas possible.");
        Console.WriteLine("Appuyez sur une touche pour continuer...");
        Console.ReadKey();
        return;
    }

    tour++;

    // Générer la météo du tour
    meteo = Meteo.Generer();

    // Génération d’un animal aléatoire avec une probabilité d’apparition
    Animaux nouvelAnimal = Animaux.GenererAnimalAleatoire();
    if (nouvelAnimal != null)
    {
        terrain.AnimauxDansLeTerrain.Add(nouvelAnimal);
    }

    // Placement et action des animaux
    foreach (var animal in terrain.AnimauxDansLeTerrain.ToList())
    {
        int x = terrain.Rng.Next(0, 5);
        int y = terrain.Rng.Next(0, 5);
        animal.Position = (x, y);

        if (terrain.GrillePlantes.TryGetValue((x, y), out Plante? plante) && plante != null)
        {
            animal.AttaquerPlante(plante);
            Console.WriteLine($"Un {animal.GetType().Name} attaque une plante à ({x}, {y}) !");

            // Menu d'urgence
            Console.WriteLine("Menu d'urgence :");
            Console.WriteLine("1. Retirer l'animal");
            Console.WriteLine("2. Ignorer");
            string? choix = Console.ReadLine();

            if (choix == "1")
            {
                terrain.AnimauxDansLeTerrain.Remove(animal);
                Console.WriteLine("L'animal a été retiré du jardin.");
                break;
            }
        }
    }

    // Affichage du jardin
    terrain.AfficherJardin();

    // Affichage de la météo
    Console.WriteLine(meteo);

    // Évolution des plantes
    foreach (var plante in terrain.Plantes.ToList())
    {
        plante.Pousser(meteo, terrain);

        if (plante.EstMort)
        {
            terrain.SupprimerPlante(plante);
            
        }
        else if (plante.PeutRecolter())
        {
            Console.WriteLine($"✅ Vous pouvez récolter {plante.Recolter()} {plante.Nom}");
        }
    }

    Console.WriteLine("✔️ Tour terminé");

    nbActions = 3;
    Console.WriteLine("Appuyez sur une touche pour continuer...");
    Console.ReadKey();
}


while (true)
{
    int choix;
    meteo = Meteo.Generer();
    if (!meteo.Urgence)
    {
        choix = menu.AfficherMenu();
        if (choix == 1)
        {
            if (nbActions > 0)
            {
                Console.WriteLine("Voici la liste des plantes : ");
                for (int i = 0; i < catalogue.Count(); i++)
                {
                    Console.WriteLine($"{i + 1} : {catalogue[i].Nom}");
                }
                Console.Write("Choisissez une plante à semer : ");
                int indice = int.Parse(Console.ReadLine());
                Console.Write("Choisissez un ligne : ");
                int l = int.Parse(Console.ReadLine());
                Console.Write("Choisissez un ligne : ");
                int c = int.Parse(Console.ReadLine());
                terrain.AjouterPlante(catalogue[indice - 1], l - 1, c - 1);

                nbActions--;
            }

            else
            {
                Console.WriteLine($"Le nombre des actions possibles par tour est atteint ! un tour va etre passer");
                PasserTour();
            }

        }
        else if (choix == 2)
        {
            if (nbActions > 0)
            {
                Console.WriteLine("De quelle quantité voulez vous arroser la plante ? (entre 0.0-1.0)");
                double quantite = Convert.ToDouble(Console.ReadLine());
                Console.WriteLine("Quelle Plante voulez vs arroser ?");
                for (int i = 0; i < terrain.Plantes.Count(); i++)
                {
                    Console.WriteLine($"{i + 1} : {terrain.Plantes[i].Nom}");
                }
                int indice = int.Parse(Console.ReadLine());
                terrain.Plantes[indice - 1].Arrosser(quantite);
                nbActions--;
                Console.WriteLine("Appuyez sur une touche pour continuer...");
                Console.ReadKey();
            }
            else
            {
                Console.WriteLine("Le nombre des actions possibles par tours est atteint ! un tour va etre passer");
                PasserTour();
            }

        }
        else if (choix == 3)
        {
            if (nbActions > 0)
            {
                Console.WriteLine("De quelle quantité voulez vous arroser les plantes ? (entre 0.0-1.0)");
                double quant = Convert.ToDouble(Console.ReadLine());
                foreach (var p in terrain.Plantes)
                {
                    p.Arrosser(quant);
                }
                nbActions--;
                Console.WriteLine("Appuyez sur une touche pour continuer...");
                Console.ReadKey();
            }
            else
            {
                Console.WriteLine("Le nombre des actions possibles par tours est atteint ! un tour va etre passer");
                PasserTour();
            }
        }

        else if (choix == 4)
        {
            if (nbActions > 0)
            {
                Console.WriteLine("Quelle plante voulez vous traiter  ?");
                for (int i = 0; i < terrain.Plantes.Count(); i++)
                {
                    Console.WriteLine($"{i + 1}: {terrain.Plantes[i].Nom}");
                }
                int n = int.Parse(Console.ReadLine());
                terrain.Plantes[n - 1].AppliquerTraitement();
                nbActions--;
                Console.WriteLine("Appuyez sur une touche pour continuer...");
                Console.ReadKey();
            }
            else
            {
                Console.WriteLine("Le nombre des actions possibles par tours est atteint ! un tour va etre passer");
                PasserTour();
            }
        }
        else if (choix == 5)
        {
            if (nbActions > 0)
            {
                Console.WriteLine($"Semaine 🗓️ {tour} :");
                foreach (var p in terrain.Plantes)
                {
                    p.AfficherEtat();
                }
                nbActions--;
                Console.WriteLine("Appuyez sur une touche pour continuer...");
                Console.ReadKey();
            }
            else
            {
                Console.WriteLine("Le nombre des actions possibles par tours est atteint ! un tour va etre passer ");
                PasserTour();
            }
        }
        else if (choix == 6)
        {
            PasserTour();
        }
        else if (choix == 7)
        {

            //simulateur.Sauvegarder();
            nbActions--;
            Console.ReadKey();
        }
        
    


    else if (choix == 8)
    {
        if (nbActions > 0)
        {
            //simulateur.Charger();
            nbActions--;
            Console.ReadKey();
        }
        else
        {
            Console.WriteLine("Le nombre des actions possibles par tours est atteint ! un tour va etre passer ");
            PasserTour();
        }
    }
    else if (choix == 9)
    {
        Console.WriteLine("Au revoir !");
        return;
    }
    }
    

}

*/

// afficher les semaines

