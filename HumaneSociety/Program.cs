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



            //Dictionary<int, string> updates = null;
            Animal animal = Query.GetAnimalByID(1);

            UserEmployee userEmployee = new UserEmployee();
            userEmployee.CheckShots(animal);
            
            //Query.SearchForAnimalsByMultipleTraits(updates);
            //Query.GetAnimalByID(1);
          


            //PointOfEntry.Run();
            






            Console.ReadLine();
        }
    }
}
