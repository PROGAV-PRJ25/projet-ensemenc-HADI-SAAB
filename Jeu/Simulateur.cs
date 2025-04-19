public class Simulateur
{
    public Terrain T { get; private set; }

    public Simulateur(Terrain t)
    {
        T = t;
    }

    public void Semer(Plante p)
    {
        if (T.Surface >= p.Espace)
        {
            T.Plantes.Add(p);
            T.Surface -= p.Espace;
            Console.WriteLine($"{p.Nom} à été a été semée dans le terrain {T.Type} !");
        }
        else 
        {
            Console.WriteLine($"Espace insuffisant dans le terrain pour semer la plante {p.Nom}.");
        }    
    }

    public void SupprimerPlante(Plante p) //Recolter ou mort
    {
        //Dans la boucle du jeu ajouter les conditions : Estmort ? ou PeutRecolter
        T.Plantes.Remove(p);
        T.Surface += p.Espace;
        
    }
    

    //Dans la boucle du jeu ajouter 
    /*
    public void TourSuivant() 
    {
        if (DureeMaladieRestante > 0) 
        { 
            DureeMaladieRestante--; 
            if (DureeMaladieRestante == 0) MaladieActuelle = null; 
        } 
    } 
    */
}