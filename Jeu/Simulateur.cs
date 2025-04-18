public class Simulateur
{

    void Semer(Plante p, Terrain t)
    {
        if (t.Surface >= p.Espace)
        {
            t.Plantes.Add(p);
            t.Surface -= p.Espace;
            Console.WriteLine($"{p.Nom} à été a été semée dans le terrain {t.Type} !");
        }
        else 
        {
            Console.WriteLine($"Espace insuffisant dans le terrain pour semer la plante {p.Nom}.");
        }
        
        
    }

    public void Recolter(Plante p , Terrain t)
    {
        if (p.PeutRecolter())
        {
            t.Plantes.Remove(p);
        }
        else 
        {
            Console.WriteLine($"La plante {p.Nom} n’est pas encore prête à être récoltée.");
        }
    }
}