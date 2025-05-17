public abstract class Plante 
{
    public string Nom { get; protected set; }
    public string Nature { get; protected set; }
    public List<string> Saisons { get; protected set; } 
    public string TerrainPrefere { get; protected set; } 
    public (double Min, double Max) ZonesTempPreferee { get; protected set; }
    public double Espace { get; protected set; }
    public double VitesseCroissance { get; protected set; }
    public List<Maladie> MaladiesPossibles { get; protected set; }
    public int Productivite { get; protected set; }
    public double BesoinEau { get; protected set; }  // entre 0.0 et 1.0
    public double BesoinLumineux { get; protected set; } // entre 0.0 et 1.0
    public int Age { get; protected set; }
    public int Sante { get; protected set; }
    public bool EstMort { get; set; }
    public double Taille { get; protected set; }
    public double TailleMax { get; protected set; }
    public Maladie? MaladieActuelle { get; protected set; }
    public int DureeMaladieRestante { get; protected set; }
    public int RecoltesRestantes { get; set; }
    public Random rng { get; set; }
    public bool EstProtegee { get; set; } = false;
    public bool ProtectionPhysique { get; set; } = false;




    public Plante(string nom, string nature, List<string> saisons, string terrainPrefere, (double Min, double Max) zonesTemp, double espace, double vitesse, List<Maladie> maladies, int productivite, double besoinEau, double besoinLumineux, double tailleMax)
    {
        Nom = nom;
        Nature = nature;
        Saisons = saisons;
        TerrainPrefere = terrainPrefere;
        ZonesTempPreferee = zonesTemp;
        Espace = espace;
        VitesseCroissance = vitesse;
        MaladiesPossibles = maladies;
        Productivite = productivite;
        BesoinEau = besoinEau;
        BesoinLumineux = besoinLumineux;
        Age = 0;
        Sante = 100;
        EstMort = false;
        Taille = 0;
        TailleMax = tailleMax;
        RecoltesRestantes = productivite;
        rng = new Random();

    }

    public abstract Plante Clone();

    public void ModifierSante(int data)
    {
        if (data > 0)
        {
            Sante = Math.Min(Sante + data, 100);
        }
        else
        {
            Sante = Math.Max(Sante + data, 0);
        }
        if (Sante == 0)
        {
            EstMort = true;
        }
    }
    public bool VerifierConditions(Meteo meteo, Terrain terrain)
    {
        if (EstMort) return false;

        int conditionScore = 0;
        
        // Température
        if (meteo.Temperature >= ZonesTempPreferee.Min && meteo.Temperature <= ZonesTempPreferee.Max)
            conditionScore += 25;
        else
            ModifierSante(-15);

        // Terrain
        if (terrain.Type == TerrainPrefere)
            conditionScore += 25;
        else
            ModifierSante(-10);

        // Eau
        if (meteo.Pluie >= BesoinEau) 
            conditionScore += 25;
        else
            ModifierSante(-(int)((BesoinEau - meteo.Pluie) * 20));

        // Lumière
        if (meteo.Soleil >= BesoinLumineux)
            conditionScore += 25;
        else
            ModifierSante(-(int)(BesoinLumineux - meteo.Soleil) * 20);

        if (conditionScore < 50)
            {
                EstMort = true;
                Sante = 0;
                return false;
            }

        // Bonus de croissance si conditions optimales
        if (conditionScore > 90)
            VitesseCroissance *= 1.2;

        return true;
    }


    public virtual void Arroser(double quantite)
    {
        Taille += 1.5;
        if (quantite < BesoinEau)
        {
            ModifierSante(-10);
            Console.WriteLine($"{Nom} n'a pas assez d'eau. La santé a diminué.");
        }
        else
        {
            ModifierSante(15);

        }
    }
 

   
    public virtual void Pousser(Meteo meteo, Terrain terrain, Saison saison)
    {
        if (EstMort)
        {
            Console.WriteLine($"{Nom} est morte et ne peut plus pousser.");
            return;
        }

        if (!VerifierConditions(meteo, terrain))
            return;

        Age++;

        if (!Saisons.Contains(saison.ToString()))
        {
            ModifierSante(-20);
            VitesseCroissance *= 0.6;
            Console.WriteLine($"{Nom} souffre en {saison}...");
        }
        else
        {
            VitesseCroissance *= 1.1; // Bonus en bonne saison
        }
    
        

        if (MaladieActuelle != null)
            {
                DureeMaladieRestante--;
                if (DureeMaladieRestante <= 0)
                {
                    MaladieActuelle = null;
                    Console.WriteLine($"{Nom} a guéri de sa maladie.");
                }
                else
                {
                    Taille -= 0.5;
                    ModifierSante(-15);
                    Console.WriteLine($"{Nom} souffre de {MaladieActuelle.Nom}.");
                    return;
                }
            }

        // Croissance normale
        double croissanceBase = VitesseCroissance;
        
        // Modificateurs de croissance
        if (terrain.Type == TerrainPrefere)
            croissanceBase *= 1.1;
    
        if (terrain.ADesMauvaiseHerbes)
        {
            croissanceBase *= 0.8;
            ModifierSante(-5);
            Console.WriteLine($"{Nom} est génée par des mauvaises herbes ‼️‼️!");
        }
        Taille += croissanceBase;
        ModifierSante(2);

        // Chance d'attraper une maladie
        AppliquerMaladie();
    }

    public virtual bool PeutRecolter()
    {
       return Taille >= TailleMax * 0.95 && Sante >= 50;
    }
    public virtual int Recolter()
    {
        if (!PeutRecolter())
            return 0;

        int recolte = (int)(Productivite * (Sante / 100.0));
        RecoltesRestantes--;
        
        if (RecoltesRestantes <= 0)
            EstMort = true;

        return recolte;
    }

    
     
    public virtual void AppliquerMaladie() 
    { 
        if (MaladieActuelle != null)
        {
            return;
        }
        foreach (var maladie in MaladiesPossibles)
        {
            if (rng.NextDouble() < maladie.ProbabiliteApparition) 
            { 
                MaladieActuelle = maladie; 
                DureeMaladieRestante = maladie.Duree;
                Console.WriteLine($"{Nom} est tombée malade : {maladie.Nom}");
                break; // sorti , car la plante peux attraper une seule maladie
            } 
        }
        
    }

    public virtual void AppliquerTraitement() // Soigner la plante si elle est malade
    {
        if (MaladieActuelle != null)
        {
            MaladieActuelle = null;
            ModifierSante(10);
            Console.WriteLine($"{Nom} est traitée");
        }
        else 
        {
            Console.WriteLine($"{Nom} est déjà en bonne santé 🥳 ");
        }
    } 

    public virtual void AfficherEtat()
    {
        Console.WriteLine("----- État de la plante -----");
        Console.WriteLine($"Nom              : {Nom}");
        Console.WriteLine($"Nature           : {Nature}");
        Console.WriteLine($"Âge              : {Age} semaine(s)");
        Console.WriteLine($"Taille           : {Taille:F2}");
        Console.WriteLine($"Taille Max       : {TailleMax:F2}");
        Console.WriteLine($"Santé            : {Sante}%");
        Console.WriteLine($"Croissance       : {VitesseCroissance:F2}");
        Console.WriteLine($"Recoltes rest.   : {RecoltesRestantes}");

        if (MaladieActuelle != null)
        {
            Console.WriteLine($"Maladie     🦠     : {MaladieActuelle.Nom} ({DureeMaladieRestante} sem. restantes)");
        }
        else
        {
            Console.WriteLine("Maladie          : Aucune");
        }

        Console.WriteLine($"État             : {(EstMort ? "Morte" : "Vivante")}");
        Console.WriteLine("------------------------------\n");
    }
     

}