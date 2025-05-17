// Joueur.cs
public class Joueur
{
    public int SemisGratuitsRestants { get; private set; } = 3;
    public double Argent { get; private set; } = 100.0;
    public int Arrosoirs { get; private set; } = 3;
    public int Traitements { get; private set; } = 3;

    public bool PeutSemerGratuit() => SemisGratuitsRestants > 0;

    public void UtiliserSemisGratuit()
    {
        if (SemisGratuitsRestants > 0)
            SemisGratuitsRestants--;
    }

    public void AjouterArgent(double montant) => Argent += montant;

    public bool DepenserArgent(double montant)
    {
        if (Argent >= montant)
        {
            Argent -= montant;
            return true;
        }
        return false;
    }

    public bool UtiliserArrosoir()
    {
        if (Arrosoirs > 0)
        {
            Arrosoirs--;
            return true;
        }
        return false;
    }

    public bool UtiliserTraitement()
    {
        if (Traitements > 0)
        {
            Traitements--;
            return true;
        }
        return false;
    }

    public void AcheterArrosoir(int quantite = 1)
    {
        double cout = quantite * 5.0;
        if (DepenserArgent(cout))
            Arrosoirs += quantite;
    }

    public void AcheterTraitement(int quantite = 1)
    {
        double cout = quantite * 8.0;
        if (DepenserArgent(cout))
            Traitements += quantite;
    }
}