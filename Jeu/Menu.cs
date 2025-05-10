public class Menu
{
    private int indexSelectionne = 0;

    private string[] options = new string[]
    {
        "Semer une plante 🍀 ",
        "Arroser une plante 🪣 ",
        //"Arroser toutes les plantes 🪣 ",
        "Traiter une plante",
        "Afficher état des plantes 👀 ",
        "Passer un tour",
        "Quitter ❌ "
    };

    private string[] optionsUrgence = new string[]
    {
        "Couvrir les plantes (contre la pluie)",
        "Allumer un chauffage d’appoint (contre le gel)",
        "Installer un voile d’ombrage (contre la canicule)",
        "Fixer des tuteurs ou des filets (contre le vent fort)"
    };

    public int AfficherMenu()
    {
        return Afficher(options, "Menu principal");
    }

    public int AfficherMenuUrgence()
    {
        return Afficher(optionsUrgence, "Menu d'urgence");
    }

    private int Afficher(string[] menuOptions, string titre)
    {
        ConsoleKeyInfo key;
        indexSelectionne = 0;

        do
        {
            Console.Clear();
            Console.WriteLine("===== Menu =====\n");

            for (int i = 0; i < menuOptions.Length; i++)
            {
                if (i == indexSelectionne)
                {
                    Console.BackgroundColor = ConsoleColor.Gray;
                    Console.ForegroundColor = ConsoleColor.Black;
                }

                Console.WriteLine(menuOptions[i]);
                Console.ResetColor();
            }

            key = Console.ReadKey(true);

            if (key.Key == ConsoleKey.UpArrow)
            {
                indexSelectionne = (indexSelectionne == 0) ? menuOptions.Length - 1 : indexSelectionne - 1;
            }
            else if (key.Key == ConsoleKey.DownArrow)
            {
                indexSelectionne = (indexSelectionne == menuOptions.Length - 1) ? 0 : indexSelectionne + 1;
            }

        } while (key.Key != ConsoleKey.Enter);

        return indexSelectionne + 1;
    }
}
