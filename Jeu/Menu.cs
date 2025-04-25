public class Menu
{
    private int indexSelectionne = 0;
    private string[] options = new string[]
    {
        "Semer une plante 🍀 ",
        "Arroser une plante 🪣 ",
        "Arroser toutes les plantes 🪣 ",
        "Traiter une plante",
        "Afficher état des plantes 👀 ",
        "Passer un tour",
        "Sauvegarder ✅ ",
        "Charger ↩️ ",
        "Quitter ❌ "
    };

    public int AfficherMenu()
    {
        ConsoleKeyInfo key;

        do
        {
            Console.Clear();
            Console.WriteLine("===== Menu =====\n");

            for (int i = 0; i < options.Length; i++)
            {
                if (i == indexSelectionne)
                {
                    Console.BackgroundColor = ConsoleColor.Gray;
                    Console.ForegroundColor = ConsoleColor.Black;
                }

                Console.WriteLine(options[i]);

                Console.ResetColor();
            }

            key = Console.ReadKey(true);

            if (key.Key == ConsoleKey.UpArrow)
            {
                indexSelectionne = (indexSelectionne == 0) ? options.Length - 1 : indexSelectionne - 1;
            }
            else if (key.Key == ConsoleKey.DownArrow)
            {
                indexSelectionne = (indexSelectionne == options.Length - 1) ? 0 : indexSelectionne + 1;
            }

        } 
        while (key.Key != ConsoleKey.Enter);

        return indexSelectionne + 1;
    }
}
