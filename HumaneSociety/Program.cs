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

            //PointOfEntry.Run();
            //Query.GetAnimalByID(1);
            //Query.GetCategoryId("MAMMAL");
            //Query.GetRoom(1);
            //Query.GetDietPlanId("BIRD FEED");
            Dictionary<int, string> updates = UserInterface.GetAnimalSearchCriteria();
            Query.SearchForAnimalsByMultipleTraits(updates);

            // PointOfEntry.Run();
            //Employee employee = new Employee();
            //employee.EmployeeNumber = int.Parse(UserInterface.GetStringData("employee number", "the employee's"));
            //Query.RunEmployeeQueries(employee, "read");


            Console.ReadLine();
        }
    }
}
