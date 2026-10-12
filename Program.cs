namespace ExceptionsDemo
{
    internal class Program
    {
        static void Main(string[] args)
        {
            {
                Console.WriteLine("=== Start av programmet ===");
                bool successful = true;
                try
                {
                    Console.WriteLine("Försöker läsa fil och räkna...");
                    var path = Path.Combine(AppContext.BaseDirectory, "numbers.txt");
                    double result = ProcessFile(path);

                    Console.WriteLine($"\nResultat: {result}");
                }

                catch (ArgumentException ex)
                {
                    // Specifikt fel om argumentet är ogiltigt
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine($"Okänt fel: {ex.Message}");
                    successful = false;
                }
                catch (FileNotFoundException ex)
                {
                    // File not found: logiskt fel vi vill signalera
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine($"Filen hittades inte: {ex.Message}");
                    successful = false;
                }
                catch (DirectoryNotFoundException ex)
                {
                    // Directory not found: logiskt fel vi vill signalera
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine($"Fil katalogen hittades inte: {ex.Message}");
                    successful = false;
                }
                catch (FormatException ex)
                {
                    // Specifikt fel om texten inte kan tolkas som tal
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine($"Formatfel: {ex.Message}");
                    successful = false;
                }
                catch (DivideByZeroException ex)
                {
                    // Specifikt fel om nolldivision
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine($"Kan inte dividera med noll: {ex.Message}");
                    successful = false;
                }
                catch (IOException ex)
                {
                    // IO-fel: logiskt fel vi vill signalera
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine($"IO-fel: {ex.Message}");
                    successful = false;
                }
                catch (InvalidOperationException ex)
                {
                    // Filen är tom: logiskt fel vi vill signalera
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine($"Filen är tom: {ex.Message}");
                    successful = false;
                }
                catch (Exception ex)
                {
                    // Fallback för alla övriga obekanta fel
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine($"Exception: {ex.Message}");
                    successful = false;
                }
                finally
                {
                    // Körs ALLTID, även om det blev undantag
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine("Cleanup: Logging avslutat anrop.");
                }

                if (successful)
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine("Programmet avslutas normalt.");
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Programmet avslutas med Exceptuion.");
                }

                Console.ResetColor();
            }

            //---------------------------------------------------------//
            // Exempel på metod som själv kastar ett undantag (throw)  //
            //---------------------------------------------------------//
            static double ProcessFile(string fileName)
            {
                StreamReader? reader = null;
                try
                {

                    if (string.IsNullOrWhiteSpace(fileName))
                    {
                        throw new ArgumentException("Filnamn får inte vara tomt eller null.",
                            nameof(fileName));
                    }

                    // Läs text från en fil, kan ge följande exceptions: ArgumentException, ArgumentNullException,   
                    //   FileNotFoundException, DirectoryNotFoundException, IOException
                    reader = new StreamReader(fileName);
                    string? line = reader.ReadLine();
                    if (line == null)
                        throw new InvalidOperationException("Filen är tom.");

                    // Försöker omvandla text till tal, kan ge följande exceptions: ArgumentNullException,
                    //    FormatException, OverflowException
                    int number = int.Parse(line);

                    // Division: kan ge DivideByZeroException
                    if (number != 0)
                    {
                        return 100.0 / number;
                    }
                    else
                    {
                        // Specifikt fel vid nolldivision
                        throw new DivideByZeroException("Division by zero.");
                    }
                }
                catch (ArgumentNullException ex)
                {
                    // Om filnamnet är tomt: logiskt fel vi vill signalera
                    throw; // Låter anroparen hantera felet
                }
                catch (ArgumentException ex)
                {
                    // Om filnamnet är tomt: logiskt fel vi vill signalera
                    throw; // Låter anroparen hantera felet
                }
                catch (FileNotFoundException)
                {
                    // File not found: logiskt fel vi vill signalera
                    throw; // Låter anroparen hantera felet
                }
                catch (DirectoryNotFoundException ex)
                {
                    // Directory not found: logiskt fel vi vill signalera
                    throw; // Låter anroparen hantera felet
                }
                catch (IOException ex)
                {
                    // IO-fel: logiskt fel vi vill signalera
                    throw; // Låter anroparen hantera felet
                }
                catch (InvalidOperationException ex)
                {
                    // Filen är tom: logiskt fel vi vill signalera
                    throw; // Låter anroparen hantera felet
                }
                catch (FormatException ex)
                {
                    // Formatfel i ProcessFile, logiskt fel vi vill signalera
                    throw; // Låter anroparen hantera felet
                }
                catch (DivideByZeroException ex)
                {
                    // Specifikt fel om nolldivision
                    throw; // Låter anroparen hantera felet
                }
                catch (Exception ex)
                {
                    // Om vi vill ge en mer meningsfull feltyp till anroparen
                    throw new InvalidOperationException(
                                "Det gick inte att processa filen.",
                                ex); // InnerException = ursprunglig fel
                }
                finally
                {
                    // Garanterad stängning av resurs
                    reader?.Close();
                    Console.WriteLine("finally i ProcessFile: StreamReader stängd.");
                }
            }
        }
    }
}
