using System;
using System.Collections.Generic;


Meteo meteo = new Meteo(
    temperature: 18,
    condition: "Pluie",
    soleil: 0.4,
    pluie: 0.8
);


List<Maladie> maladies = new List<Maladie>(){new Maladie("Mildiou", 3, 7, 0.1)};


Comestible tomate = new Comestible(
    nom: "Tomate",
    nature: "Comestible",
    saisons: new List<string> { "Printemps", "Été" },
    terrainPrefere: "Terre",
    zonesTempPreferee: new List<double> { 15, 30 },
    espace: 0.3,
    vitesse: 1.0,
    maladies: maladies,
    productivite: 5,
    besoinEau: 0.6,
    besoinLumineux: 0.5
);

Comestible carotte = new Comestible(
    nom: "Carotte",
    nature: "Comestible",
    saisons: new List<string> { "Printemps", "Automne" },
    terrainPrefere: "Terre",
    zonesTempPreferee: new List<double> { 10, 25 },
    espace: 0.2,
    vitesse: 1.2,
    maladies: maladies,
    productivite: 4,
    besoinEau: 0.5,
    besoinLumineux: 0.4
);


Terrain terrain = new Terre(
    surface: 10,
    plantes: new List<Plante> { tomate, carotte }
);



Console.WriteLine("Semaine 1 - Météo actuelle :");
meteo.Afficher();

Console.WriteLine("État des plantes :\n");

foreach (var plante in terrain.Plantes)
{
    plante.ConditionsPalnte(meteo, terrain);
}
