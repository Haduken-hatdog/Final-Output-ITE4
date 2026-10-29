using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FinalsSystem
{
    // ===================== CLASSES (used in Intermediate menu) =====================
    class Student
    {
        public string Name { get; set; }
        public int Grade { get; set; }

        public Student(string name, int grade)
        {
            Name = name;
            Grade = grade;
        }

        public string Remarks()
        {
            return Grade >= 75 ? "PASSED" : "FAILED";
        }
    }

    class Question
    {
        public string Text;
        public string[] Choices;
        public int Answer; // 1-based

        public Question(string text, string[] choices, int answer)
        {
            Text = text;
            Choices = choices;
            Answer = answer;
        }
    }

    // ===================== MAIN PROGRAM =====================
    class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = Encoding.UTF8;
            Console.Title = "C# FINALS SYSTEM";

            if (!Login())
                return;

            MainMenu();
        }

        // ---------- UI HELPERS ----------
        static void Print(string text, ConsoleColor color)
        {
            Console.ForegroundColor = color;
            Console.Write(text);
            Console.ResetColor();
        }

        static void PrintLine(string text, ConsoleColor color)
        {
            Print(text + "\n", color);
        }

        static void Center(string text, ConsoleColor color)
        {
            int width = 60;
            int pad = Math.Max(0, (width - text.Length) / 2);
            PrintLine(new string(' ', pad) + text, color);
        }

        static void Header(string title)
        {
            Console.Clear();
            Console.BackgroundColor = ConsoleColor.Black;
            PrintLine("╔" + new string('═', 58) + "╗", ConsoleColor.Cyan);
            string t = title.Length > 56 ? title.Substring(0, 56) : title;
            int left = (58 - t.Length) / 2;
            int right = 58 - t.Length - left;
            Print("║", ConsoleColor.Cyan);
            Print(new string(' ', left) + t + new string(' ', right), ConsoleColor.Yellow);
            PrintLine("║", ConsoleColor.Cyan);
            PrintLine("╚" + new string('═', 58) + "╝", ConsoleColor.Cyan);
            Console.WriteLine();
        }

        static void Beep(int freq, int dur)
        {
            try { Console.Beep(freq, dur); } catch { /* sound not supported on this OS */ }
        }

        static void Pause()
        {
            PrintLine("\nPress any key to continue...", ConsoleColor.DarkGray);
            Console.ReadKey(true);
        }

        static int ReadInt(string prompt)
        {
            while (true)
            {
                Print(prompt, ConsoleColor.White);
                if (int.TryParse(Console.ReadLine(), out int n))
                    return n;
                PrintLine("  Invalid number. Try again.", ConsoleColor.Red);
            }
        }

        static double ReadDouble(string prompt)
        {
            while (true)
            {
                Print(prompt, ConsoleColor.White);
                if (double.TryParse(Console.ReadLine(), out double n))
                    return n;
                PrintLine("  Invalid number. Try again.", ConsoleColor.Red);
            }
        }

        // Try Again or Quit loop used by EVERY sub-menu application
        static void RunApp(Action app)
        {
            bool again;
            do
            {
                app();
                again = AskTryAgain();
            } while (again);
        }

        static bool AskTryAgain()
        {
            while (true)
            {
                Console.WriteLine();
                PrintLine("  [T] Try Again      [Q] Quit (back to menu)", ConsoleColor.Magenta);
                Print("  Your choice: ", ConsoleColor.White);
                string c = (Console.ReadLine() ?? "").Trim().ToUpper();
                if (c == "T" || c == "TRY AGAIN") return true;
                if (c == "Q" || c == "QUIT") return false;
                PrintLine("  Invalid Selection", ConsoleColor.Red);
            }
        }

        // ---------- LOGIN ----------
        static bool Login()
        {
            const int maxAttempts = 3;

            for (int attempt = 1; attempt <= maxAttempts; attempt++)
            {
                Console.Clear();
                PrintLine("  ┌──────────────────────────────────────────┐", ConsoleColor.Green);
                PrintLine("  │   _      ___   ____   ___  _   _          │", ConsoleColor.Green);
                PrintLine("  │  | |    / _ \\ / ___| |_ _|| \\ | |         │", ConsoleColor.Green);
                PrintLine("  │  | |   | | | | |  _   | | |  \\| |         │", ConsoleColor.Green);
                PrintLine("  │  | |___| |_| | |_| |  | | | |\\  |         │", ConsoleColor.Green);
                PrintLine("  │  |_____|\\___/ \\____| |___||_| \\_|         │", ConsoleColor.Green);
                PrintLine("  ├──────────────────────────────────────────┤", ConsoleColor.Green);
                Print("  │  ", ConsoleColor.Green);
                Print($"Attempt {attempt} of {maxAttempts}".PadRight(40), ConsoleColor.Yellow);
                PrintLine("  │", ConsoleColor.Green);
                PrintLine("  └──────────────────────────────────────────┘", ConsoleColor.Green);
                Console.WriteLine();

                Print("  Username : ", ConsoleColor.White);
                string user = Console.ReadLine();

                Print("  Password : ", ConsoleColor.White);
                string pass = ReadMasked();

                if (user == "user" && pass == "pass")
                {
                    Console.WriteLine();
                    PrintLine("  ✔ Login successful! Welcome!", ConsoleColor.Green);
                    Beep(800, 150);
                    Beep(1000, 200);
                    System.Threading.Thread.Sleep(900);
                    return true;
                }

                Console.WriteLine();
                PrintLine("  ✘ Incorrect username or password.", ConsoleColor.Red);
                Beep(300, 400);
                if (attempt < maxAttempts)
                    PrintLine($"  You have {maxAttempts - attempt} attempt(s) left.", ConsoleColor.Yellow);
                System.Threading.Thread.Sleep(1200);
            }

            Console.Clear();
            PrintLine("\n  ██ ACCESS DENIED ██", ConsoleColor.Red);
            PrintLine("  Maximum of 3 attempts reached. The program will now close.", ConsoleColor.Red);
            Console.ReadKey(true);
            return false;
        }

        static string ReadMasked()
        {
            StringBuilder sb = new StringBuilder();
            while (true)
            {
                ConsoleKeyInfo k = Console.ReadKey(true);
                if (k.Key == ConsoleKey.Enter) break;
                if (k.Key == ConsoleKey.Backspace)
                {
                    if (sb.Length > 0)
                    {
                        sb.Length--;
                        Console.Write("\b \b");
                    }
                }
                else if (!char.IsControl(k.KeyChar))
                {
                    sb.Append(k.KeyChar);
                    Console.Write("*");
                }
            }
            return sb.ToString();
        }

        // ---------- MAIN MENU ----------
        static void MainMenu()
        {
            while (true)
            {
                Header("★  M A I N   M E N U  ★");
                PrintLine("   ┌────────────────────────────────────┐", ConsoleColor.DarkCyan);
                PrintLine("   │  [1]  BASIC                        │", ConsoleColor.Green);
                PrintLine("   │  [2]  INTERMEDIATE                 │", ConsoleColor.Yellow);
                PrintLine("   │  [3]  EXTRA                        │", ConsoleColor.Magenta);
                PrintLine("   │  [4]  EXIT                         │", ConsoleColor.Red);
                PrintLine("   └────────────────────────────────────┘", ConsoleColor.DarkCyan);
                Print("\n   Enter your choice: ", ConsoleColor.White);
                string c = (Console.ReadLine() ?? "").Trim().ToUpper();

                if (c == "1" || c == "BASIC") BasicMenu();
                else if (c == "2" || c == "INTERMEDIATE") IntermediateMenu();
                else if (c == "3" || c == "EXTRA") ExtraMenu();
                else if (c == "4" || c == "EXIT")
                {
                    Header("GOODBYE!");
                    Center("Thank you for using the system!", ConsoleColor.Cyan);
                    Beep(600, 150); Beep(500, 150); Beep(400, 250);
                    System.Threading.Thread.Sleep(800);
                    return;
                }
                else
                {
                    PrintLine("\n   Invalid Selection", ConsoleColor.Red);
                    Beep(300, 300);
                    Pause();
                }
            }
        }

        // ===================== BASIC =====================
        static void BasicMenu()
        {
            while (true)
            {
                Header("BASIC MENU");
                PrintLine("   [1] Simple Calculator   (switch, operators)", ConsoleColor.Green);
                PrintLine("   [2] Grade Checker       (if-else)", ConsoleColor.Green);
                PrintLine("   [3] Multiplication Table(for loop)", ConsoleColor.Green);
                PrintLine("   [4] Back to Main Menu", ConsoleColor.Red);
                Print("\n   Enter your choice: ", ConsoleColor.White);
                string c = (Console.ReadLine() ?? "").Trim();

                if (c == "1") RunApp(Calculator);
                else if (c == "2") RunApp(GradeChecker);
                else if (c == "3") RunApp(MultiplicationTable);
                else if (c == "4") return;
                else { PrintLine("\n   Invalid Selection", ConsoleColor.Red); Pause(); }
            }
        }

        static void Calculator()
        {
            Header("SIMPLE CALCULATOR");
            double a = ReadDouble("  First number  : ");
            double b = ReadDouble("  Second number : ");
            Print("  Operator (+ - * / %): ", ConsoleColor.White);
            string op = (Console.ReadLine() ?? "").Trim();

            double result = 0;
            bool ok = true;
            switch (op)
            {
                case "+": result = a + b; break;
                case "-": result = a - b; break;
                case "*": result = a * b; break;
                case "/":
                    if (b == 0) { PrintLine("  Cannot divide by zero!", ConsoleColor.Red); ok = false; }
                    else result = a / b;
                    break;
                case "%":
                    if (b == 0) { PrintLine("  Cannot divide by zero!", ConsoleColor.Red); ok = false; }
                    else result = a % b;
                    break;
                default:
                    PrintLine("  Invalid operator.", ConsoleColor.Red);
                    ok = false;
                    break;
            }
            if (ok) PrintLine($"\n  {a} {op} {b} = {result}", ConsoleColor.Yellow);
        }

        static void GradeChecker()
        {
            Header("GRADE CHECKER");
            int g = ReadInt("  Enter grade (0-100): ");
            string remark;
            ConsoleColor col;

            if (g < 0 || g > 100) { remark = "Invalid grade!"; col = ConsoleColor.Red; }
            else if (g >= 90) { remark = "Excellent! (A)"; col = ConsoleColor.Green; }
            else if (g >= 80) { remark = "Very Good! (B)"; col = ConsoleColor.Cyan; }
            else if (g >= 75) { remark = "Passed (C)"; col = ConsoleColor.Yellow; }
            else { remark = "Failed (F)"; col = ConsoleColor.Red; }

            PrintLine("\n  Result: " + remark, col);
        }

        static void MultiplicationTable()
        {
            Header("MULTIPLICATION TABLE");
            int n = ReadInt("  Enter a number: ");
            Console.WriteLine();
            for (int i = 1; i <= 10; i++)
                PrintLine($"  {n} x {i,2} = {n * i,4}", i % 2 == 0 ? ConsoleColor.Cyan : ConsoleColor.White);
        }

        // ===================== INTERMEDIATE =====================
        static void IntermediateMenu()
        {
            while (true)
            {
                Header("INTERMEDIATE MENU");
                PrintLine("   [1] Array Operations    (arrays, sort, LINQ)", ConsoleColor.Yellow);
                PrintLine("   [2] String Tools        (string methods)", ConsoleColor.Yellow);
                PrintLine("   [3] Factorial & Fibonacci (methods, recursion)", ConsoleColor.Yellow);
                PrintLine("   [4] Student Records     (classes & objects, List)", ConsoleColor.Yellow);
                PrintLine("   [5] Back to Main Menu", ConsoleColor.Red);
                Print("\n   Enter your choice: ", ConsoleColor.White);
                string c = (Console.ReadLine() ?? "").Trim();

                if (c == "1") RunApp(ArrayOperations);
                else if (c == "2") RunApp(StringTools);
                else if (c == "3") RunApp(RecursionDemo);
                else if (c == "4") RunApp(StudentRecords);
                else if (c == "5") return;
                else { PrintLine("\n   Invalid Selection", ConsoleColor.Red); Pause(); }
            }
        }

        static void ArrayOperations()
        {
            Header("ARRAY OPERATIONS");
            int size = ReadInt("  How many numbers? ");
            if (size <= 0) { PrintLine("  Size must be at least 1.", ConsoleColor.Red); return; }

            int[] nums = new int[size];
            for (int i = 0; i < size; i++)
                nums[i] = ReadInt($"  Number {i + 1}: ");

            int[] sorted = (int[])nums.Clone();
            Array.Sort(sorted);

            Console.WriteLine();
            PrintLine("  Original : " + string.Join(", ", nums), ConsoleColor.White);
            PrintLine("  Ascending: " + string.Join(", ", sorted), ConsoleColor.Cyan);
            Array.Reverse(sorted);
            PrintLine("  Descending: " + string.Join(", ", sorted), ConsoleColor.Cyan);
            PrintLine($"  Sum      : {nums.Sum()}", ConsoleColor.Yellow);
            PrintLine($"  Average  : {nums.Average():F2}", ConsoleColor.Yellow);
            PrintLine($"  Highest  : {nums.Max()}", ConsoleColor.Green);
            PrintLine($"  Lowest   : {nums.Min()}", ConsoleColor.Red);
        }

        static void StringTools()
        {
            Header("STRING TOOLS");
            Print("  Enter a word or sentence: ", ConsoleColor.White);
            string s = Console.ReadLine() ?? "";

            char[] arr = s.ToCharArray();
            Array.Reverse(arr);
            string reversed = new string(arr);

            string cleaned = new string(s.Where(char.IsLetterOrDigit).ToArray()).ToLower();
            string cleanedRev = new string(cleaned.Reverse().ToArray());
            int vowels = s.ToLower().Count(ch => "aeiou".Contains(ch));
            int words = s.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries).Length;

            Console.WriteLine();
            PrintLine($"  Uppercase : {s.ToUpper()}", ConsoleColor.Cyan);
            PrintLine($"  Lowercase : {s.ToLower()}", ConsoleColor.Cyan);
            PrintLine($"  Reversed  : {reversed}", ConsoleColor.Cyan);
            PrintLine($"  Length    : {s.Length}", ConsoleColor.Yellow);
            PrintLine($"  Words     : {words}", ConsoleColor.Yellow);
            PrintLine($"  Vowels    : {vowels}", ConsoleColor.Yellow);
            bool pal = cleaned.Length > 0 && cleaned == cleanedRev;
            PrintLine($"  Palindrome: {(pal ? "YES" : "NO")}", pal ? ConsoleColor.Green : ConsoleColor.Red);
        }

        static long Factorial(int n)
        {
            if (n <= 1) return 1;
            return n * Factorial(n - 1);
        }

        static long Fibonacci(int n)
        {
            if (n <= 1) return n;
            return Fibonacci(n - 1) + Fibonacci(n - 2);
        }

        static void RecursionDemo()
        {
            Header("FACTORIAL & FIBONACCI");
            int n = ReadInt("  Enter a number (0-20): ");
            if (n < 0 || n > 20) { PrintLine("  Number out of range.", ConsoleColor.Red); return; }

            PrintLine($"\n  {n}! = {Factorial(n)}", ConsoleColor.Yellow);
            Print("  Fibonacci: ", ConsoleColor.Cyan);
            int count = Math.Min(n + 1, 20);
            for (int i = 0; i < count; i++)
                Print(Fibonacci(i) + " ", ConsoleColor.Cyan);
            Console.WriteLine();
        }

        static void StudentRecords()
        {
            Header("STUDENT RECORDS");
            int count = ReadInt("  How many students? ");
            if (count <= 0) { PrintLine("  Must be at least 1.", ConsoleColor.Red); return; }

            List<Student> list = new List<Student>();
            for (int i = 1; i <= count; i++)
            {
                Print($"\n  Student {i} name: ", ConsoleColor.White);
                string name = Console.ReadLine() ?? "";
                int grade = ReadInt("  Grade        : ");
                list.Add(new Student(name, grade));
            }

            Console.WriteLine();
            PrintLine("  " + "NAME".PadRight(20) + "GRADE".PadRight(8) + "REMARKS", ConsoleColor.Yellow);
            PrintLine("  " + new string('─', 40), ConsoleColor.DarkGray);
            foreach (Student s in list)
                PrintLine("  " + s.Name.PadRight(20) + s.Grade.ToString().PadRight(8) + s.Remarks(),
                          s.Grade >= 75 ? ConsoleColor.Green : ConsoleColor.Red);
            PrintLine("  " + new string('─', 40), ConsoleColor.DarkGray);
            PrintLine($"  Class average: {list.Average(s => s.Grade):F2}", ConsoleColor.Cyan);
            Student top = list.OrderByDescending(s => s.Grade).First();
            PrintLine($"  Top student  : {top.Name} ({top.Grade})", ConsoleColor.Green);
        }

        // ===================== EXTRA =====================
        static void ExtraMenu()
        {
            while (true)
            {
                Header("EXTRA MENU  -  GAMES & FUN");
                PrintLine("   [1] Number Guessing Game", ConsoleColor.Magenta);
                PrintLine("   [2] Trivia Quiz", ConsoleColor.Magenta);
                PrintLine("   [3] Rock - Paper - Scissors", ConsoleColor.Magenta);
                PrintLine("   [4] Piano Beep Melody (sound)", ConsoleColor.Magenta);
                PrintLine("   [5] Back to Main Menu", ConsoleColor.Red);
                Print("\n   Enter your choice: ", ConsoleColor.White);
                string c = (Console.ReadLine() ?? "").Trim();

                if (c == "1") RunApp(GuessingGame);
                else if (c == "2") RunApp(TriviaQuiz);
                else if (c == "3") RunApp(RockPaperScissors);
                else if (c == "4") RunApp(Melody);
                else if (c == "5") return;
                else { PrintLine("\n   Invalid Selection", ConsoleColor.Red); Pause(); }
            }
        }

        static void GuessingGame()
        {
            Header("NUMBER GUESSING GAME");
            Random rnd = new Random();
            int secret = rnd.Next(1, 101);
            int maxTries = 7;
            PrintLine("  I'm thinking of a number from 1 to 100.", ConsoleColor.Cyan);
            PrintLine($"  You have {maxTries} tries.\n", ConsoleColor.Cyan);

            for (int t = 1; t <= maxTries; t++)
            {
                int g = ReadInt($"  Try #{t}: ");
                if (g == secret)
                {
                    PrintLine($"\n  ★ CORRECT! You got it in {t} tr{(t == 1 ? "y" : "ies")}!", ConsoleColor.Green);
                    Beep(800, 150); Beep(1000, 150); Beep(1200, 300);
                    return;
                }
                PrintLine(g < secret ? "  Too LOW!" : "  Too HIGH!", ConsoleColor.Yellow);
                Beep(400, 100);
            }
            PrintLine($"\n  Game over! The number was {secret}.", ConsoleColor.Red);
            Beep(250, 500);
        }

        static void TriviaQuiz()
        {
            Header("C# TRIVIA QUIZ");
            List<Question> qs = new List<Question>
            {
                new Question("Which keyword is used to create an object?",
                    new[] { "make", "new", "create", "object" }, 2),
                new Question("Which symbol ends a statement in C#?",
                    new[] { ":", ".", ";", "," }, 3),
                new Question("Which data type stores true/false values?",
                    new[] { "int", "string", "char", "bool" }, 4),
                new Question("Which loop runs at least once?",
                    new[] { "for", "while", "do-while", "foreach" }, 3),
                new Question("What method is the entry point of a C# program?",
                    new[] { "Start()", "Main()", "Run()", "Begin()" }, 2)
            };

            Random rnd = new Random();
            qs = qs.OrderBy(q => rnd.Next()).ToList();
            int score = 0;

            for (int i = 0; i < qs.Count; i++)
            {
                Question q = qs[i];
                PrintLine($"\n  Q{i + 1}. {q.Text}", ConsoleColor.Yellow);
                for (int j = 0; j < q.Choices.Length; j++)
                    PrintLine($"     {j + 1}) {q.Choices[j]}", ConsoleColor.White);

                int ans = ReadInt("  Your answer (1-4): ");
                if (ans == q.Answer)
                {
                    PrintLine("  ✔ Correct!", ConsoleColor.Green);
                    Beep(900, 150);
                    score++;
                }
                else
                {
                    PrintLine($"  ✘ Wrong! Answer: {q.Choices[q.Answer - 1]}", ConsoleColor.Red);
                    Beep(300, 250);
                }
            }

            PrintLine($"\n  FINAL SCORE: {score} / {qs.Count}", ConsoleColor.Cyan);
            if (score == qs.Count) PrintLine("  Perfect! You're a C# master!", ConsoleColor.Green);
            else if (score >= 3) PrintLine("  Good job!", ConsoleColor.Yellow);
            else PrintLine("  Keep practicing!", ConsoleColor.Red);
        }

        static void RockPaperScissors()
        {
            Header("ROCK - PAPER - SCISSORS");
            string[] options = { "Rock", "Paper", "Scissors" };
            Random rnd = new Random();
            int you = 0, cpu = 0;

            while (true)
            {
                Console.WriteLine();
                PrintLine($"  Score -> You: {you}   Computer: {cpu}", ConsoleColor.Cyan);
                PrintLine("  [1] Rock  [2] Paper  [3] Scissors  [0] Stop", ConsoleColor.White);
                int p = ReadInt("  Your pick: ");
                if (p == 0) break;
                if (p < 1 || p > 3) { PrintLine("  Invalid Selection", ConsoleColor.Red); continue; }

                int c = rnd.Next(1, 4);
                PrintLine($"  You: {options[p - 1]}   Computer: {options[c - 1]}", ConsoleColor.Yellow);

                if (p == c)
                    PrintLine("  It's a TIE!", ConsoleColor.Yellow);
                else if ((p == 1 && c == 3) || (p == 2 && c == 1) || (p == 3 && c == 2))
                {
                    PrintLine("  You WIN this round!", ConsoleColor.Green);
                    you++;
                    Beep(900, 150);
                }
                else
                {
                    PrintLine("  Computer wins this round.", ConsoleColor.Red);
                    cpu++;
                    Beep(300, 200);
                }
            }
            PrintLine($"\n  FINAL -> You: {you}   Computer: {cpu}", ConsoleColor.Magenta);
        }

        static void Melody()
        {
            Header("PIANO BEEP MELODY");
            PrintLine("  Playing: Twinkle Twinkle Little Star...\n", ConsoleColor.Cyan);
            // C C G G A A G | F F E E D D C
            int[] notes = { 262, 262, 392, 392, 440, 440, 392, 349, 349, 330, 330, 294, 294, 262 };
            string[] names = { "C", "C", "G", "G", "A", "A", "G", "F", "F", "E", "E", "D", "D", "C" };
            ConsoleColor[] colors = { ConsoleColor.Red, ConsoleColor.Yellow, ConsoleColor.Green, ConsoleColor.Cyan, ConsoleColor.Magenta };

            for (int i = 0; i < notes.Length; i++)
            {
                Print($" ♪{names[i]} ", colors[i % colors.Length]);
                Beep(notes[i], i == 6 || i == 13 ? 600 : 300);
            }
            Console.WriteLine();
            PrintLine("\n  (If you heard nothing, your OS may not support Console.Beep.)", ConsoleColor.DarkGray);
        }
    }
}