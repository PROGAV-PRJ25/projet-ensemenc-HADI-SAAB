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
    public bool EstMort { get; protected set; }
    public double Taille { get; protected set; }
    public double TailleMax { get; protected set; }
    public Maladie? MaladieActuelle { get; protected set; }
    public int DureeMaladieRestante { get; protected set; }
    public int RecoltesRestantes { get; protected set; }
    public Random rng { get; set; }



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

     public bool VerifierConditions(Meteo meteo, Terrain terrain)
    {
        if (EstMort) return false;

        int conditionScore = 0;
        
        // Température
        if (meteo.Temperature >= ZonesTempPreferee.Min && meteo.Temperature <= ZonesTempPreferee.Max)
            conditionScore += 25;
        else
            Sante = Math.Max(Sante - 15, 0);

        // Terrain
        if (terrain.Type == TerrainPrefere)
            conditionScore += 25;
        else
            Sante = Math.Max(Sante - 10, 0);

        // Eau
        if (meteo.Pluie >= BesoinEau) // ou mieux meteo.Pluie >= BesoinEau*0.7 : Tolérance de 30%
            conditionScore += 25;
        else
            Sante = Math.Max(Sante - (int)((BesoinEau - meteo.Pluie) * 20), 0);

        // Lumière
        if (meteo.Soleil >= BesoinLumineux) //meme question pour l'eau 
            conditionScore += 25;
        else
            Sante = Math.Max(Sante - (int)((BesoinLumineux - meteo.Soleil) * 20), 0);

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


    public virtual void Arrosser(double quantite)
    {
        if (quantite < BesoinEau)
        {
            Sante = Math.Max(Sante - 10, 0);
            Console.WriteLine($"{Nom} n'a pas assez d'eau. La santé a diminué.");
        }
        else 
        {
            Sante = Math.Min(Sante + 10, 100);
            Console.WriteLine($"{Nom} a été correctement arrosée.");
        }
    }

    public virtual void AppliquerLumiere(double quantite)
    {
        if (quantite < BesoinLumineux)
        {
            Sante = Math.Max(Sante - 10, 0);
            Console.WriteLine($"{Nom} n'a pas assez de lumière. Sa santé a diminué.");
        }
        else
        {
            Sante = Math.Min(Sante + 10, 100);
            Console.WriteLine($"{Nom} a eu correctement la lumière.");
        }
    }

    public virtual void Pousser(Meteo meteo, Terrain terrain)
    {
        if (EstMort)
        {
            Console.WriteLine($"{Nom} est morte et ne peut plus pousser.");
            return;
        }

        if (!VerifierConditions(meteo, terrain))
            return;

        Age++;
        
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
                Sante = Math.Max(Sante - 15, 0);
                Console.WriteLine($"{Nom} souffre de {MaladieActuelle.Nom}.");
                return;
            }
        }

        // Croissance normale
        double croissanceBase = VitesseCroissance;
        
        // Modificateurs de croissance
        if (terrain.Type == TerrainPrefere)
            croissanceBase *= 1.1;
        
        if (meteo.Pluie > BesoinEau * 1.2) // Trop d'eau
            croissanceBase *= 0.8;
        if (terrain.ADesMauvaiseHerbes)
        {
            croissanceBase *= 0.8;
            Sante = Math.Max(Sante - 5, 0);
            Console.WriteLine($"{Nom} est génée par des mauvaises herbes !");
        }
        Taille += croissanceBase;
        Sante = Math.Min(Sante + 2, 100);

        // Chance d'attraper une maladie
        AppliquerMaladie();
    }

    public virtual bool PeutRecolter()
    {
        if (Taille >= TailleMax && Sante >= 75)
        {
            return true;
        }
        return false;
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
            Sante = Math.Min(Sante + 10, 100);
            Console.WriteLine($"{Nom} est traitée");
        }
        else 
        {
            Console.WriteLine($"{Nom} est déjà en bonne santé");
        }
    } 

    public virtual void AfficherEtat()
    {
        Console.WriteLine("----- État de la plante -----");
        Console.WriteLine($"Nom              : {Nom}");
        Console.WriteLine($"Nature           : {Nature}");
        Console.WriteLine($"Âge              : {Age} semaine(s)");
        Console.WriteLine($"Taille           : {Taille:F2}");
        Console.WriteLine($"Santé            : {Sante}%");
        Console.WriteLine($"Croissance       : {VitesseCroissance:F2}");
        Console.WriteLine($"Recoltes rest.   : {RecoltesRestantes}");

        if (MaladieActuelle != null)
        {
            Console.WriteLine($"Maladie          : {MaladieActuelle.Nom} ({DureeMaladieRestante} sem. restantes)");
        }
        else
        {
            Console.WriteLine("Maladie          : Aucune");
        }

        Console.WriteLine($"État             : {(EstMort ? "Morte" : "Vivante")}");
        Console.WriteLine("------------------------------\n");
    }
     

}