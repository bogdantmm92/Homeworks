/*
 * ideone.com
 * API sample
 * 
 * This program shows how to use ideone api.
 * 
 * How to run it?
 *  1. Create C# Windows Console Application project in the Visual Studio;
 *  2. Include Program.cs and Ideone_ServiceService.cs files to the project
 *      (you can generate the stub - Ideone_ServiceService.cs - by yourself 
 *      using wsdl.exe tool from Microsoft SDK);
 *  3. Add System.Web.Services reference to the project (right click on the
 *      project name in the Solution Explorer -> click Add Reference ...);
 *  4. Run the project.
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace csharp_test
{
    class Program
    {
        static void Main(string[] args)
        {
            Dictionary<string, string> result = new Dictionary<string, string>();
            Ideone_ServiceService client = new Ideone_ServiceService(); // instantiating the stub
            Object[] ret = client.testFunction("test", "test"); // calling the method

            // filling result with data returned by testFunction
            foreach (object o in ret)
            {
                if (o is XmlElement)
                {
                    XmlNodeList x = ((XmlElement)o).ChildNodes;
                    result.Add(x.Item(0).InnerText, x.Item(1).InnerText);
                }
            }

            // checking if everything went ok
            if ("OK" == result["error"])
            {
                // printing result
                foreach (KeyValuePair<string, string> kvp in result)
                {
                    Console.WriteLine(kvp.Key + " : " + kvp.Value);
                }
                // you can add here some type conversion, for example:
                // int answerToLifeAndEverything = int.Parse(result["answerToLifeAndEverything"]);
            }
            else
            {
                Console.WriteLine("Error occured: " + result["error"]);
            }

            Console.Read(); // waiting for enter
        }
    }
}
