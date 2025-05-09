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

    
    
}