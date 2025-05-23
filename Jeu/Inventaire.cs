// Inventaire.cs
using System;
using System.Collections.Generic;
using System.Linq;

public class Inventaire
{
    // Liste des récoltes présentes dans l'inventaire (lecture seule de l'extérieur)
    public List<Recolte> Recoltes { get; private set; } = new List<Recolte>();

    // Méthode pour ajouter une récolte issue d'une plante, selon la saison et le terrain
    public void AjouterRecolte(Plante plante, Saison saison, Terrain terrain)
    {
        // Vérifie si la plante peut être récoltée, sinon on sort de la méthode
        if (!plante.PeutRecolter()) return;

        // Calcul initial de la qualité basée sur la santé de la plante
        double qualite = 0.7 + (plante.Sante / 200.0);
        
        // Bonus de qualité si le terrain correspond au terrain préféré de la plante
        if (plante.TerrainPrefere == terrain.Type)
            qualite *= 1.2;
        
        // Bonus de qualité si la saison actuelle fait partie des saisons favorables à la plante
        if (plante.Saisons.Contains(saison.ToString()))
            qualite *= 1.3;

        // Limiter la qualité entre 0.5 et 1.5
        qualite = Math.Clamp(qualite, 0.5, 1.5);

        // Détermination du type de produit : nature comestible ou "Plante" par défaut
        string typeProduit = plante is Comestible c ? c.Nature : "Plante";

        // Calcul de la quantité récoltée en fonction de la productivité et de la qualité
        int quantite = (int)(plante.Productivite * (1 + qualite));
        
        // Ajout de la récolte à la liste avec ses caractéristiques
        Recoltes.Add(new Recolte(
            plante.Nom,
            typeProduit,
            quantite,
            qualite,
            saison
        ));

        // Décrémentation du nombre de récoltes restantes pour la plante
        plante.RecoltesRestantes--;
        
        // Si plus de récoltes restantes, la plante est considérée comme morte
        if (plante.RecoltesRestantes <= 0)
            plante.EstMort = true;
    }

    // Méthode pour vendre une récolte à un joueur en fonction de son index dans la liste
    public void VendreRecolte(int index, Joueur joueur)
    {
        // Vérification que l'index est valide dans la liste des récoltes
        if (index < 0 || index >= Recoltes.Count) return;

        // Récupération de la récolte sélectionnée
        var recolte = Recoltes[index];
        
        // Calcul du prix total de la vente : prix unitaire x quantité
        double prix = recolte.GetVenteBase() * recolte.Quantite;
        
        // Ajout de l'argent gagné au joueur
        joueur.AjouterArgent(prix);
        
        // Suppression de la récolte vendue de l'inventaire
        Recoltes.RemoveAt(index);

        // Affichage d'un message récapitulatif de la vente
        Console.WriteLine($"Vendu: {recolte.Quantite}x {recolte.NomPlante} " +
                         $"(Qualité: {recolte.Qualite * 100:F0}%) " +
                         $"pour {prix:F2}€");
    }

    // Méthode pour afficher le contenu de l'inventaire et l'argent disponible
    public void Afficher(double argent)
    {
        Console.WriteLine("\n=== INVENTAIRE ===");
        Console.WriteLine($"Argent: {argent:F2}€");
        Console.WriteLine("Récoltes:");
        
        // Si aucune récolte dans l'inventaire, afficher un message
        if (!Recoltes.Any())
        {
            Console.WriteLine("Aucune récolte disponible");
            return;
        }

        // Affichage détaillé des récoltes présentes dans l'inventaire
        for (int i = 0; i < Recoltes.Count; i++)
        {
            var r = Recoltes[i];
            Console.WriteLine($"{i+1}. {r.Quantite}x {r.NomPlante} " +
                            $"(Saison: {r.SaisonRecolte}) " +
                            $"Valeur: {r.GetVenteBase()*r.Quantite*r.Qualite:F2}€");
        }
    }
}
