using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HumaneSociety
{
    public static class Query
    {        
        static HumaneSocietyDataContext db;

        static Query()
        {
            db = new HumaneSocietyDataContext();
        }

        internal static List<USState> GetStates()
        {
            List<USState> allStates = db.USStates.ToList();       

            return allStates;
        }
            
        internal static Client GetClient(string userName, string password)
        {
            Client client = db.Clients.Where(c => c.UserName == userName && c.Password == password).Single();

            return client;
        }

        internal static List<Client> GetClients()
        {
            List<Client> allClients = db.Clients.ToList();

            return allClients;
        }

        internal static void AddNewClient(string firstName, string lastName, string username, string password, string email, string streetAddress, int zipCode, int stateId)
        {
            Client newClient = new Client();

            newClient.FirstName = firstName;
            newClient.LastName = lastName;
            newClient.UserName = username;
            newClient.Password = password;
            newClient.Email = email;

            Address addressFromDb = db.Addresses.Where(a => a.AddressLine1 == streetAddress && a.Zipcode == zipCode && a.USStateId == stateId).FirstOrDefault();

            // if the address isn't found in the Db, create and insert it
            if (addressFromDb == null)
            {
                Address newAddress = new Address();
                newAddress.AddressLine1 = streetAddress;
                newAddress.City = null;
                newAddress.USStateId = stateId;
                newAddress.Zipcode = zipCode;                

                db.Addresses.InsertOnSubmit(newAddress);
                db.SubmitChanges();

                addressFromDb = newAddress;
            }

            // attach AddressId to clientFromDb.AddressId
            newClient.AddressId = addressFromDb.AddressId;

            db.Clients.InsertOnSubmit(newClient);

            db.SubmitChanges();
        }

        internal static void UpdateClient(Client clientWithUpdates)
        {
            // find corresponding Client from Db
            Client clientFromDb = null;

            try
            {
                clientFromDb = db.Clients.Where(c => c.ClientId == clientWithUpdates.ClientId).Single();
            }
            catch(InvalidOperationException)
            {
                Console.WriteLine("No clients have a ClientId that matches the Client passed in.");
                Console.WriteLine("No update have been made.");
                return;
            }
            
            // update clientFromDb information with the values on clientWithUpdates (aside from address)
            clientFromDb.FirstName = clientWithUpdates.FirstName;
            clientFromDb.LastName = clientWithUpdates.LastName;
            clientFromDb.UserName = clientWithUpdates.UserName;
            clientFromDb.Password = clientWithUpdates.Password;
            clientFromDb.Email = clientWithUpdates.Email;

            // get address object from clientWithUpdates
            Address clientAddress = clientWithUpdates.Address;

            // look for existing Address in Db (null will be returned if the address isn't already in the Db
            Address updatedAddress = db.Addresses.Where(a => a.AddressLine1 == clientAddress.AddressLine1 && a.USStateId == clientAddress.USStateId && a.Zipcode == clientAddress.Zipcode).FirstOrDefault();

            // if the address isn't found in the Db, create and insert it
            if(updatedAddress == null)
            {
                Address newAddress = new Address();
                newAddress.AddressLine1 = clientAddress.AddressLine1;
                newAddress.City = null;
                newAddress.USStateId = clientAddress.USStateId;
                newAddress.Zipcode = clientAddress.Zipcode;                

                db.Addresses.InsertOnSubmit(newAddress);
                db.SubmitChanges();

                updatedAddress = newAddress;
            }

            // attach AddressId to clientFromDb.AddressId
            clientFromDb.AddressId = updatedAddress.AddressId;
            
            // submit changes
            db.SubmitChanges();
        }
        
        internal static void AddUsernameAndPassword(Employee employee)
        {
            Employee employeeFromDb = db.Employees.Where(e => e.EmployeeId == employee.EmployeeId).FirstOrDefault();

            employeeFromDb.UserName = employee.UserName;
            employeeFromDb.Password = employee.Password;

            db.SubmitChanges();
        }

        internal static Employee RetrieveEmployeeUser(string email, int employeeNumber)
        {
            Employee employeeFromDb = db.Employees.Where(e => e.Email == email && e.EmployeeNumber == employeeNumber).FirstOrDefault();

            if (employeeFromDb == null)
            {
                throw new NullReferenceException();
            }
            else
            {
                return employeeFromDb;
            }
        }

        internal static Employee EmployeeLogin(string userName, string password)
        {
            Employee employeeFromDb = db.Employees.Where(e => e.UserName == userName && e.Password == password).FirstOrDefault();

            return employeeFromDb;
        }

        internal static bool CheckEmployeeUserNameExist(string userName)
        {
            Employee employeeWithUserName = db.Employees.Where(e => e.UserName == userName).FirstOrDefault();

            return employeeWithUserName == null;
        }


        
        internal static void RunEmployeeQueries(Employee employee, string crudOperation)
        {
            switch (crudOperation)
            {
                case "update":
                    Employee employeeUpdate = null;
                    try
                    {
                        employeeUpdate =
                            (from e in db.Employees
                             where e.EmployeeNumber == employee.EmployeeNumber
                             select e).Single();
                    }
                    catch(InvalidOperationException)
                    {
                        Console.WriteLine("No employees have a EmployeeId that matches the Employee passed in.");
                        Console.WriteLine("No update have been made.");
                        return;
                    }
                    employeeUpdate.FirstName = employee.FirstName;
                    employeeUpdate.LastName = employee.LastName;
                    employeeUpdate.Email = employee.Email;
                    db.SubmitChanges();
                    break;
                case "read":
                    var employeeRead =
                        (from e in db.Employees
                         where e.EmployeeNumber == employee.EmployeeNumber
                         select e).First();
                    if (employeeRead == null)
                    {
                        throw new NullReferenceException();
                    }
                    else
                    {
                        Console.WriteLine("First Name: " + employeeRead.FirstName);
                        Console.WriteLine("Last Name: " + employeeRead.LastName);
                        Console.WriteLine("Email: " + employeeRead.Email);
                        Console.WriteLine("ID number: " + employeeRead.EmployeeNumber);
                        Console.WriteLine("Username: " + employeeRead.UserName);
                        break;
                    }
                case "delete":
                    var employeeDelete =
                        db.Employees.FirstOrDefault(e => e.EmployeeNumber == employee.EmployeeNumber);
                    db.Employees.DeleteOnSubmit(employeeDelete);
                    db.SubmitChanges();
                    break;
                case "create":
                    db.Employees.InsertOnSubmit(employee);
                    db.SubmitChanges();
                    break;
                default:
                    UserInterface.DisplayUserOptions("Input not recognized please try agian");
                    break;
            }
        }

        // TODO: Animal CRUD Operations
        internal static void AddAnimal(Animal animal)
        {
            db.Animals.InsertOnSubmit(animal);
            db.SubmitChanges();
        }

        internal static Animal GetAnimalByID(int id)
        {
            Animal animal = db.Animals.Where(a => a.AnimalId == id).FirstOrDefault();
            
            return animal;
        }

        internal static void UpdateAnimal(int animalId, Dictionary<int, string> updates)
        {
            Animal animal = db.Animals.Where(a => a.AnimalId == animalId).FirstOrDefault();
            {                
                foreach (int key in updates.Keys)
                {

                    switch (key)
                    {
                        case 1:
                            if (updates[1] == null)
                            {
                                break;
                            }
                            else
                            {
                                animal.Category.Name = updates[1];
                            }
                            break;
                        case 2:
                            if (updates[2] == null)
                            {
                                break;
                            }
                            else
                            {
                                animal.Name = updates[2];
                            }
                            break;
                        case 3:
                            if (updates[3] == null)
                            {
                                break;
                            }
                            else
                            {
                                animal.Age = Convert.ToInt32(updates[3]);
                            }
                            break;
                        case 4:
                            if (updates[4] == null)
                            {
                                break;
                            }
                            else
                            {
                                animal.Demeanor = updates[4];
                            }
                            break;
                        case 5:
                            if (updates[5] == null)
                            {
                                break;
                            }
                            else
                            {
                                if (updates[5] == "true")
                                {
                                    animal.KidFriendly = true;
                                }
                                else
                                {
                                    animal.KidFriendly = false;
                                }

                            }
                            break;
                        case 6:
                            if (updates[6] == null)
                            {
                                break;
                            }
                            else
                            {
                                if (updates[6] == "true")
                                {
                                    animal.PetFriendly = true;
                                }
                                else
                                {
                                    animal.PetFriendly = false;
                                }
                            }
                            break;
                        case 7:
                            if (updates[7] == null)
                            {
                                break;
                            }
                            else
                            {
                                animal.Weight = Convert.ToInt32(updates[7]);
                            }
                            break;
                    }


                }
                db.SubmitChanges();
            }
        }

        internal static void RemoveAnimal(Animal animal)
        {
            Animal deleteAnimal = db.Animals.Where(a => a.AnimalId == animal.AnimalId).FirstOrDefault();
            db.Animals.DeleteOnSubmit(deleteAnimal);
            db.SubmitChanges();
        }
        
        // TODO: Animal Multi-Trait Search
        internal static IQueryable<Animal> SearchForAnimalsByMultipleTraits(Dictionary<int, string> updates) // parameter(s)?
        {
            //get value from key
            IQueryable<Animal> animals = db.Animals;
            foreach (int key in updates.Keys)
            {
                switch (key)
                {
                    case 1:
                        animals = animals.Where(a => a.Category.Name == updates[1]);
                        break;
                    case 2:
                        animals = animals.Where(a => a.Name == updates[2]);
                        break;
                    case 3:
                        animals = animals.Where(a => a.Age.ToString() == updates[3]);
                        break;
                    case 4:
                        animals = animals.Where(a => a.Demeanor == updates[4]);
                        break;
                    case 5:
                        animals = animals.Where(a => a.KidFriendly.ToString() == updates[5]);
                        break;
                    case 6:
                        animals = animals.Where(a => a.PetFriendly.ToString() == updates[6]);
                        break;
                    case 7:
                        animals = animals.Where(a => a.Weight.ToString() == updates[7]);
                        break;
                    case 8:
                        animals = animals.Where(a => a.AnimalId.ToString() == updates[8]);
                        break;
                    default:
                        UserInterface.DisplayUserOptions("Input not recognized please try agian");
                        break;


                }
            }

            return animals;
        }
         
        
        internal static int GetCategoryId(string categoryName)
        {
            Category category = db.Categories.Where(c => c.Name == categoryName).FirstOrDefault();
            
            return category.CategoryId;
        }
        
        internal static Room GetRoom(int animalId)
        {
            Room room = db.Rooms.Where(r => r.AnimalId == animalId).FirstOrDefault();
            
            return room;
        }
        
        internal static int GetDietPlanId(string dietPlanName)
        {
            DietPlan dietPlan = db.DietPlans.FirstOrDefault(d => d.Name == dietPlanName);
            
            return dietPlan.DietPlanId;
        }

        internal static void Adopt(Animal animal, Client client)    
        {
            Adoption adoption = new Adoption();
            adoption.ClientId = client.ClientId;
            adoption.AnimalId = animal.AnimalId;
            adoption.ApprovalStatus = "NOT APPROVED";
            adoption.AdoptionFee = 75;
            adoption.PaymentCollected = false;

            db.Adoptions.InsertOnSubmit(adoption);
            db.SubmitChanges();
        }

        internal static IQueryable<Adoption> GetPendingAdoptions()
        {
            IQueryable<Adoption> adoptions = db.Adoptions;
            var getAdoptions = adoptions.Where(a => a.ApprovalStatus == "NOT APPROVED");
            return getAdoptions;
        }

        internal static void UpdateAdoption(bool isAdopted, Adoption adoption)
        {
            Adoption updateAdoption = db.Adoptions.Where(a => a.ClientId == adoption.ClientId).FirstOrDefault();
            updateAdoption.ApprovalStatus = isAdopted == true ? updateAdoption.ApprovalStatus = "APPROVED" : updateAdoption.ApprovalStatus = "NOT APPROVED";
            db.SubmitChanges();
        }

        internal static void RemoveAdoption(int animalId, int clientId)
        {
            Adoption deleteAdoption = db.Adoptions.Where(a => a.ClientId == clientId && a.AnimalId == animalId).FirstOrDefault();
            db.Adoptions.DeleteOnSubmit(deleteAdoption);
            db.SubmitChanges();
        }

        // TODO: Shots Stuff
        internal static IQueryable<AnimalShot> GetShots(Animal animal)
        {
            IQueryable<AnimalShot> animalShots = db.AnimalShots;
            animalShots = animalShots.Where(a => a.AnimalId == animal.AnimalId);

            return animalShots;



        }

        internal static void UpdateShot(string shotName, Animal animal)
        {
            AnimalShot animalShot = new AnimalShot();
            var shotInDb = db.Shots.Where(s => s.Name == shotName).FirstOrDefault();
            if (shotInDb == null)
            {
                Console.WriteLine("That shot does not exist");
                Shot shot = new Shot() { Name = shotName };
                db.Shots.InsertOnSubmit(shot);
                db.SubmitChanges();
                shotInDb = db.Shots.Where(s => s.Name == shotName).FirstOrDefault();
            }
           
            animalShot.AnimalId = animal.AnimalId;
            animalShot.DateReceived = DateTime.Now;
            animalShot.ShotId = db.Shots.Where(s => s.Name == shotName).Select(s => s.ShotId).FirstOrDefault();
            db.AnimalShots.InsertOnSubmit(animalShot);
            db.SubmitChanges();

        }
    }
}