using Loto.Modules;

namespace Loto.FileManager
{
    //Klasa namjenjena za manipulaciju fajlovima sa podacima o izvlačenju (upis, ispis, provjere...)
    public class DrawFileManager
    { 
        // Putanje do korištenih fajlova 
        public static string LastDrawDataPath = "C:\\Users\\edink\\source\\repos\\Loto\\Loto\\Data\\LastDrawData.txt";
        public static string DrawsPath = "C:\\Users\\edink\\source\\repos\\Loto\\Loto\\Data\\Draws.txt";

        // Funkcija namijenjena za uvećavanje broja kola izvlačenja
        public static async Task<int> IncrementDrawRound()
        {
            int LastRoundValue = ReadDrawRound(); // Učitati broj kola
            int CurrentRoundValue = LastRoundValue + 1; // Uvećati ga za jedan

            // Fragment koda za kreiranje i smiještanje broja novog kola u pravilnom formatu
            using (StreamWriter writer = File.CreateText(LastDrawDataPath))
            {
                await writer.WriteLineAsync($"RoundNumber={CurrentRoundValue}");
                await writer.FlushAsync();
                writer.Close();
            }
            return CurrentRoundValue;
        }

        // Metoda za čitanje i vraćanje aktuelnog broja kola izvlačenja
        public static int ReadDrawRound()
        {
            int RoundNumber;
            using (StreamReader reader = new StreamReader(LastDrawDataPath))
            {
                // Vrijednost broja runde se nalazi nakon znaka '=' u fajlu
                RoundNumber = Convert.ToInt32(reader.ReadLine().Split("=")[1]); // Iz stringa u broj
            }
            return RoundNumber;
        }

        // Metoda za spašavanje podataka o izvlačenju
        public static async void SaveDraw(string DrawText)
        {
            // Spašavaju se podaci samo o zadnjem izvlačenju
            using (StreamWriter writer = File.CreateText(DrawsPath))
            {
                await writer.WriteLineAsync(DrawText);
                await writer.FlushAsync();
                writer.Close();
            }
        }
        
        // Metoda za čitanje posljednje izvlačenja iz fajla
        public static Draw LoadDrawFromFile()
        {
            Draw draw = new Draw(); // Prazan objekat, spreman za popunjavanje čitajući linije iz fajla

            using (StreamReader reader = new StreamReader(DrawsPath))
            {
                string line; // Spremnik za liniju
                while ((line = reader.ReadLine()) != null)
                {
                    ParseLine(draw, line); // Poziv metode za ispitivanje i obradu pojedinačne linije
                }
            }

            return draw;
        }

        // Metoda za ispitivanje i obradu svake linije u fajlu
        private static void ParseLine(Draw draw, string line)
        {
            // Ispituju se 'ključevi' te se potom poziva odgovarajuća metoda za kupljenje vrijednosti
            if (line.Contains("Round"))
            {
                draw.Round = ExtractValue(line); // Poziv metodate za vađenje vrijednosti iz fajla
            }
            else if (line.Contains("WinningSupplementaryNumber"))
            {
                draw.WinningSupplementaryNumber = ExtractValue(line);
            }
            else if (line.Contains("LottoWinningCombination") || line.Contains("JokerWinningCombination"))
            {
                int[] winningCombination = ExtractIntArray(line); // Poziv metode za veđenje vrijednosti iz niza(kombinacija)

                // Ispitati da li se radi o Lotto ili Joker kombinaciji kako bismo znali odredište u objektu
                if (line.Contains("LottoWinningCombination"))
                {
                    draw.WinningLottoCombination = winningCombination;
                }
                else
                {
                    draw.WinningJokerCombination = winningCombination;
                }
            }
        }

        //Metoda za čitanje vrijednosti pored odogovarajućeg ključa
        private static int ExtractValue(string line)
        {
            // Razdvoji ključ i vrijednost dvotačkom u niz i vrati drugi element niza
            return Convert.ToInt32(line.Split(":")[1]); 
        }

        // Posebna metoda za manipulaciju nizova vrijednosti (kombinacije brojeva) 
        private static int[] ExtractIntArray(string line)
        {
            // Razdvoji ključ i vrijednost po ':' i uzmi drugi element (vrijednost)
            // Potom razbij članove u niz razdvajajući ih space-om
            string[] combinationString = line.Split(":")[1].Split(" ");
            int[] winningCombination = new int[combinationString.Length];
            // Svaki element prvog niza pretvori u broj i smijesti u novi niz brojeva
            for (int i = 0; i < combinationString.Length; i++)
            {
                winningCombination[i] = Convert.ToInt32(combinationString[i]);
            }
            return winningCombination;
        }

    }
}
