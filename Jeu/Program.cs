// Programme.cs
using System;

class Program
{
    static void Main(string[] args)
    {
        Simulateur simulateur = new Simulateur();
        
        while (simulateur.EnCours)
        {
            simulateur.ExecuterAction();
        }
    }
}