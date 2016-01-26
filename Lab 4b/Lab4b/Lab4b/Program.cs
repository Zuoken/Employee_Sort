/*
 * Cameron Winters, 000299896 
 * March 20, 2015
 * 
 * Program Purpose: The purpose of this program is to read an HTML file and determine if there are any unclosed tags.
 * The user enters the name of the HTML file they'd like scanned, and is notified of whether or not the code is
 * balanaced.
 * 
 * Statement of Authorship: I, Cameron Winters, 000299896 certify that this material is my original work. No other 
 * person's work has been used without due acknowledgement.
 *  
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.IO;

namespace Lab4b
{
    class Program
    {
        // Class variables
        static string file;
        static List<string> html = new List<string>();
        static List<string> tagsList = new List<string>();
        static Stack<string> tags = new Stack<string>();

        /// <summary>
        /// The main method of the Lab4b class.  Calls the prompt() method, followed by parse() and compare().
        /// </summary>
        /// <param name="args">The program arguments.</param>
        static void Main(string[] args)
        {
            prompt();
            parse();
            compare();
        }

        /// <summary>
        /// This method prompts the user to enter the name of the HTML file they would like parsed.  If the
        /// user doesn't enter a proper filename (e.g. mypage.html or mypage.htm) they are re-prompted.
        /// </summary>
        static void prompt()
        {
            bool inputFlag = false;
            while (!inputFlag)
            {
                System.Console.WriteLine("Enter the name of an html file: ");
                file = System.Console.ReadLine();
                if (file.Contains(".html") || file.Contains(".htm"))
                {
                    inputFlag = true;
                }
                else
                {
                    System.Console.Clear();
                    System.Console.WriteLine("The file must be an HTML file.  Try again:");
                    System.Console.Read();
                }
            }


            // Attempt to find the file w/ the filename provided by the user
            try
            {
                FileStream dataFile = new FileStream(@"..\..\..\..\" + file, FileMode.Open, FileAccess.Read);
                StreamReader reader = new StreamReader(dataFile);
                string line;

                // Push lines onto stack; first line in (presumably <!DOCTYPE> or similar), last one on top (presumably something like </html>)
                while ((line = reader.ReadLine()) != null)
                {
                    html.Add(line);
                }

                reader.Close();
                dataFile.Close();
            }
            catch (Exception e)
            {
                System.Console.Clear();
                System.Console.WriteLine("Sorry, but I can't seem to find that file.");
                System.Console.Read();
            }
        }

        static void parse()
        {
            // Find the instances in every line where a tag appears (either opened or closed).  When a tag is found, it is added to the tags stack.
            foreach (string line in html)
            {
                //Regex reg = new Regex(@"(<\w+>|</\w+>)"); WORKS FOR MOST, just not tags like <a ...>, <img ...>
                Regex reg = new Regex(@"(<\w+(\s|\w|\""|\.|:|=|/)*>|</\w+>)");
                foreach (Match match in reg.Matches(line))
                {
                    tagsList.Add(match.ToString().ToLower());
                }
            }

            // Add the elements in tagsList to the stack, but in an order that allows the top of the HTML document to be at the top of the stack (e.g. <html> would be at the top of the
            // stack, </html> might be at the bottom). 
            for (int i = tagsList.Count() - 1; i > -1; i--)
            {
                tags.Push(tagsList.ElementAt(i));
            }
        }

        static void compare()
        {
            List<string> results = new List<string>();
            int openCount = 0;
            int closedCount = 0;
            bool openTag = false;
            for (int i = tags.Count() - 1; i > -1; i--)
            {
                // If the tag is a closing tag or not a container tag, pop it off the stack
                if (tags.ElementAt(0).Contains("</"))
                {
                    results.Add("Closing tag found: " + tags.ElementAt(0));
                    tags.Pop();
                }
                // If the tag is not a container tag, add it to results as "not a container tag"
                else if (tags.ElementAt(0).Contains("<img") || tags.ElementAt(0).Contains("<hr") || tags.ElementAt(0).Contains("<br") ||
                    tags.ElementAt(0).Contains("<area") || tags.ElementAt(0).Contains("<base") || tags.ElementAt(0).Contains("<embed") ||
                    tags.ElementAt(0).Contains("<input") || tags.ElementAt(0).Contains("<link") || tags.ElementAt(0).Contains("<meta") ||
                    tags.ElementAt(0).Contains("<param") || tags.ElementAt(0).Contains("<source"))
                {
                    results.Add("Non-container tag found: " + tags.ElementAt(0));
                    tags.Pop();
                }
                // If the tag is not a closing tag, look for a correpsonding closing tag; if none found, there's an open tag and loop ends
                else
                {
                    results.Add("Opening tag found: " + tags.ElementAt(0));
                    tags.Pop();
                }
            }

            // Compare the opening and closing tags count.  If discrepancy, tags not balanced.
            foreach (string result in results)
            {
                if (result.Contains("Closing"))
                {
                    closedCount++;
                }
                else if (result.Contains("Opening"))
                {
                    openCount++;
                }
            }

            if (closedCount != openCount)
            {
                openTag = true;
            }

            if (openTag)
            // If there's any unclosed tags, openTag is true
            {
                System.Console.Clear();
                System.Console.WriteLine("Results: ");
                System.Console.WriteLine();
                foreach (string result in results)
                {
                    System.Console.WriteLine(result);
                }
                System.Console.WriteLine();
                System.Console.WriteLine("You have open tags in your HTML, your code is not balanced.");
                System.Console.Read();
            }
            else
            // All open tags have closing tags and code is balanced
            {
                System.Console.Clear();
                System.Console.WriteLine("Results: ");
                System.Console.WriteLine();
                foreach (string result in results)
                {
                    System.Console.WriteLine(result);
                }
                System.Console.WriteLine();
                System.Console.WriteLine("Your HTML is clean, all tags are balanced.");
                System.Console.Read();
            }
        }
    }
}
