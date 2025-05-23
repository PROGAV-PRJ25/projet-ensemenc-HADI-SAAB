// Joueur.cs
public class Joueur
{
    // Nombre de semis gratuits restants (initialement 3)
    public int SemisGratuitsRestants { get; private set; } = 3;
    
    // Argent disponible du joueur (initialement 100.0)
    public double Argent { get; private set; } = 100.0;
    
    // Nombre d'arrosoirs possédés par le joueur (initialement 3)
    public int Arrosoirs { get; private set; } = 3;
    
    // Nombre de traitements possédés par le joueur (initialement 3)
    public int Traitements { get; private set; } = 3;

    // Indique si le joueur peut encore semer gratuitement (au moins un semis gratuit restant)
    public bool PeutSemerGratuit() => SemisGratuitsRestants > 0;

    // Utilise un semis gratuit s'il en reste (décrémente le compteur)
    public void UtiliserSemisGratuit()
    {
        if (SemisGratuitsRestants > 0)
            SemisGratuitsRestants--;
    }

    // Ajoute une certaine somme d'argent au joueur
    public void AjouterArgent(double montant) => Argent += montant;

    // Tente de dépenser une somme d'argent ; retourne vrai si réussi, faux sinon
    public bool DepenserArgent(double montant)
    {
        if (Argent >= montant)
        {
            Argent -= montant;
            return true;
        }
        return false;
    }

    // Utilise un arrosoir si le joueur en possède au moins un ; retourne vrai si utilisé
    public bool UtiliserArrosoir()
    {
        if (Arrosoirs > 0)
        {
            Arrosoirs--;
            return true;
        }
        return false;
    }

    // Utilise un traitement si le joueur en possède au moins un ; retourne vrai si utilisé
    public bool UtiliserTraitement()
    {
        if (Traitements > 0)
        {
            Traitements--;
            return true;
        }
        return false;
    }

    // Achète un ou plusieurs arrosoirs (coût 5.0 € chacun) si le joueur a assez d'argent
    public void AcheterArrosoir(int quantite = 1)
    {
        double cout = quantite * 5.0;
        if (DepenserArgent(cout))
            Arrosoirs += quantite;
    }

    // Achète un ou plusieurs traitements (coût 8.0 € chacun) si le joueur a assez d'argent
    public void AcheterTraitement(int quantite = 1)
    {
        double cout = quantite * 8.0;
        if (DepenserArgent(cout))
            Traitements += quantite;
    }
}
