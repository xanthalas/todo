using System;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace todo
{
    public class Item
    {
        private static int Next_Index = 1;

        private string REGEX_BREAK_STRING= @"(?<completed>x\s)?(?<priority>\([A-Z]\)\s)?(?<date1>\d{4}-\d{2}-\d{2}\s)?(?<date2>\d{4}-\d{2}-\d{2}\s)?(?<text>.*)";

        public int Id { get; private set; }
        public bool IsCompleted { get; set; }

        public DateTime DateAdded { get; set; }

        public DateTime DateCompleted { get; set; }

        public string Priority { get; set; }

        public string Text { get; set; }

        public string IsCompletedString
        {
            get
            {
                return IsCompleted ? "x " : "";
            }
        }

        public string DateAddedString
        {
            get
            {
                return DateAdded == DateTime.MinValue ? "" : DateAdded.ToString("yyyy-MM-dd") + " ";
            }
        }

        public string DateCompletedString
        {
            get
            {
                return DateCompleted == DateTime.MinValue ? "" : DateCompleted.ToString("yyyy-MM-dd") + " ";
            }
        }

        public string PriorityString
        {
            get
            {
                return Priority == null ? "" : $"({Priority}) ";
            }
        }

        public Item(string todoString)
        {
            Id = Item.Next_Index;
            Item.Next_Index++;

            var regex = new Regex(REGEX_BREAK_STRING);

            var match = regex.Match(todoString);

            if (match.Success)
            {

                IsCompleted = (match.Groups["completed"].ToString().Trim() == "x") ? true : false;

                if (match.Groups["priority"].Value.Trim().Length > 2)
                {
                    Priority = match.Groups["priority"].Value.Substring(1, 1);
                }
            }

            if (match.Groups["date1"].Success)
            {
                var datestring = match.Groups["date1"].Value;
                DateTime date1;

                try
                {
                    date1 = new DateTime(int.Parse(datestring.Substring(0, 4)), int.Parse(datestring.Substring(5, 2)), int.Parse(datestring.Substring(8, 2)));

                }
                catch (Exception)
                {

                    throw new Exception("An error occurred while reading the first date in the following todo line: " + todoString);
                }

                //If this todo is completed then date1 will contain the completed date
                if (IsCompleted)
                {
                    DateCompleted = date1;
                }
                else
                {
                    DateAdded = date1;
                }
            }

            if (match.Groups["date2"].Success && IsCompleted) 
            {
                //If we have two dates and the item is completed then the second date will be the date added
                var datestring = match.Groups["date2"].Value;

                DateTime date2;

                try
                {
                    date2 = new DateTime(int.Parse(datestring.Substring(0, 4)), int.Parse(datestring.Substring(5, 2)), int.Parse(datestring.Substring(8, 2)));

                }
                catch (Exception)
                {

                    throw new Exception("An error occurred while reading the second date in the following todo line: " + todoString);
                }
                DateAdded = date2;
            }

            Text = match.Groups["text"].Value;
        }

        public override string ToString()
        {
            return $"{IsCompletedString}{PriorityString}{DateCompletedString}{DateAddedString}{Text}";
        }
    }
}
