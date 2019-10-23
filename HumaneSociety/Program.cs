using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HumaneSociety
{
    class Program
    {
        static void Main(string[] args)
        {



            Dictionary<int, string> updates = UserInterface.GetAnimalSearchCriteria();
            Query.SearchForAnimalsByMultipleTraits(updates);

          


            //PointOfEntry.Run();
            






            Console.ReadLine();
        }
    }
}
