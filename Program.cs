using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace todo
{
    class Program
    {
        private const string FILENAME = "todo.txt";
        private const string PRIORITIES = "A,B,C,D,E,F,G,H,I,J,K,L,M,N,O,P,Q,R,S,T,U,V,W,X,Y,Z";
        private static string filepath = "";
        private static string command = "";
        private static List<Item> items = new List<Item>();
        private static bool fileChanged = false;

        static void Main(string[] args)
        {
            filepath = Path.Combine(Environment.GetEnvironmentVariable("TODOPATH", EnvironmentVariableTarget.User), FILENAME);

            if (!File.Exists(filepath))
            {
                filepath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), FILENAME);
                if (!File.Exists(filepath))
                {
                    Console.WriteLine($"Cannot find existing todo.txt file so creating a new one here: {filepath}");
                    File.Create(filepath);
                }
            }

            parseArguments(args);

            loadItems();

            switch (command)
            {
                case "":
                case "ls":
                case "list":
                    printTodo(false);
                    break;

                case "lsa":
                case "listall":
                    printTodo(true);
                    break;

                case "do":
                    doItem(args);
                    break;

                case "a":
                case "add":
                    addItem(args);
                    break;

                case "pri":
                case "p":
                    increasePriority(args);
                    break;

                case "depri":
                case "dp":
                    removePriority(args);
                    break;

                case "help":
                case "h":
                case "-h":
                case "--help":
                case "/?":
                    printHelp();
                    break;

                default:
                break;
            }

            if (fileChanged)
            {
                saveItems();
            }
            //Console.ReadLine();
        }

        private static void printHelp()
        {
            Console.WriteLine("Text-based todo list manager. Based on the format from todotxt.com");
            Console.WriteLine("\nActions:");
            Console.WriteLine("  add|a \"THING I NEED TO DO +project @context\"");
            Console.WriteLine("  do ITEM#");
            Console.WriteLine("  pri|p ITEM# PRIORITY");
            Console.WriteLine("  depri|dp ITEM#");
            Console.WriteLine("  list|ls TERM");
            Console.WriteLine("  listall|lsa TERM");
            Console.WriteLine("\n\nBy default it will create the todo.txt file in your documents folder. To specify a different folder set a user environment variable");
            Console.WriteLine("called TODOPATH with the folder you want it stored in.");
        }

        private static void removePriority(string[] args)
        {
            if (args.Length < 2)
            {
                Console.WriteLine("Usage: todo depri ITEM#");
                return;
            }

            int index = -1;
            bool successful = int.TryParse(args[1], out index);

            if (!successful)
            {
                Console.WriteLine("Invalid item number given. Usage: todo depri ITEM#");
                return;
            }

            var item = (from i in items where i.Id == index select i).FirstOrDefault();

            if (item == null)
            {
                Console.WriteLine($"No item {index}");
                return;
            }

            item.Priority = null;
            fileChanged = true;
        }

        private static void increasePriority(string[] args)
        {
            if (args.Length < 3)
            {
                Console.WriteLine("Usage: todo pri ITEM# PRIORITY");
                return;
            }
            int index = -1;
            bool successful = int.TryParse(args[1], out index);

            if (!successful)
            {
                Console.WriteLine("Invalid item number given. Usage: todo pri ITEM# PRIORITY");
                return;
            }

            var valid_priorities = PRIORITIES.Split(',');

            if (!valid_priorities.Contains(args[2]))
            {
                Console.WriteLine("Priority must be a single letter between A and Z. Usage: todo pri ITEM# PRIORITY");
                return;
            }

            var item = (from i in items where i.Id == index select i).FirstOrDefault();

            if (item == null)
            {
                Console.WriteLine($"No item {index}");
                return;
            }

            item.Priority = args[2];
            fileChanged = true;
        }

        private static void addItem(string[] args)
        {
            string text;
            if (args.Length < 2)
            {
                Console.Write("Add:");
                text = Console.ReadLine();
            }
            else
            {
                text = args[1];
            }

            var newItem = new Item(text);

            items.Add(newItem);
            fileChanged = true;
        }

        private static void saveItems()
        {
            using (StreamWriter writer = new StreamWriter(filepath))
            {
                foreach(var it in items)
                {
                    writer.WriteLine(it.ToString());
                }
                writer.Close();
            }
        }

        private static void doItem(string[] args)
        {
            if (args.Length < 2)
            {
                Console.WriteLine("Usage: todo do ITEM#");
                return;
            }
            int index = -1;
            bool successful = int.TryParse(args[1], out index);

            if (!successful)
            {
                Console.WriteLine("Invalid item number given. Usage: todo do ITEM#");
                return;
            }

            var item = (from i in items where i.Id == index select i).FirstOrDefault();

            if (item == null)
            {
                Console.WriteLine($"No item {index}");
                return;
            }

            item.IsCompleted = true;
            item.DateCompleted = DateTime.Now.Date;
            fileChanged = true;
        }

        private static void printTodo(bool showCompleted)
        {
            var defaultForegroundColour = Console.ForegroundColor;

            IEnumerable<Item> query;

            if (showCompleted)
            {
                query = from i in items select i;
            }
            else
            {
                query = from i in items where i.IsCompleted == false select i;
            }

            foreach(var it in query)
            {
                Console.ForegroundColor = (it.Priority == null ? defaultForegroundColour : ConsoleColor.Red);
                Console.WriteLine($"{it.Id} {it.ToString()}");
                Console.ForegroundColor = defaultForegroundColour;
            }

        }


        private static void parseArguments(string[] args)
        {
            if (args.Length > 0)
            {
                command = args[0];
            }
        }

        private static bool loadItems()
        {
            using (StreamReader reader = new StreamReader(filepath))
            {

                string line;

                try
                {
                    while ((line = reader.ReadLine()) != null)
                    {
                        if (line.Trim().Length > 0)
                            items.Add(new Item(line.Trim()));
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine("Sorry your todo list has a line in an invalid format. Further information follows:");
                    Console.WriteLine(e.Message);
                    return false;
                }
                finally
                {
                    reader.Close();
                }
            }
            return true;
        }
    }
}
