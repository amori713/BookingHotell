using BookingHotell.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace BookingHotell
{
    internal class Program
    {
        static void Main(string[] args)
        {

            var builder = new ConfigurationBuilder()
                 .SetBasePath(Directory.GetCurrentDirectory()) 
                 .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true); 

            var config = builder.Build(); 

            var connectionString = config.GetConnectionString("DefaultConnection"); 

            
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseSqlServer(connectionString)
                .Options;

            
            using (var dbContext = new ApplicationDbContext(options))
            {
                dbContext.Database.Migrate();
            }

            Console.WriteLine("Database has been created!");

            
            SeedData(options);

            
            RunMenu(options);
        }

        static void SeedData(DbContextOptions<ApplicationDbContext> options)
        {
            using var dbContext = new ApplicationDbContext(options);

            
            if (!dbContext.Rooms.Any())
            {
                dbContext.Rooms.AddRange(
                    new Room { RoomType = "Single", Capacity = 1, PricePerNight = 500, HasExtraBedOption = false },
                    new Room { RoomType = "Double", Capacity = 2, PricePerNight = 1000, HasExtraBedOption = true },
                    new Room { RoomType = "Double", Capacity = 2, PricePerNight = 1200, HasExtraBedOption = true },
                    new Room { RoomType = "Single", Capacity = 1, PricePerNight = 450, HasExtraBedOption = false }
                );
                dbContext.SaveChanges();
            }

           
            if (!dbContext.Customers.Any())
            {
                dbContext.Customers.AddRange(
                    new Customer { FirstName = "Alice", LastName = "Smith", PhoneNumber = "123456789", Email = "alice@example.com" },
                    new Customer { FirstName = "Bob", LastName = "Johnson", PhoneNumber = "987654321", Email = "bob@example.com" },
                    new Customer { FirstName = "Charlie", LastName = "Brown", PhoneNumber = "456789123", Email = "charlie@example.com" },
                    new Customer { FirstName = "Diana", LastName = "Jones", PhoneNumber = "789123456", Email = "diana@example.com" }
                );
                dbContext.SaveChanges();
            }
        }

        static void RunMenu(DbContextOptions<ApplicationDbContext> options)
        {
            while (true)
            {
                Console.WriteLine("\nVälkommen till HotellBokningsappen!");
                Console.WriteLine("1. Visa rum");
                Console.WriteLine("2. Lägg till en bokning");
                Console.WriteLine("3. Visa kunder");
                Console.WriteLine("4. Registrera ett rum");
                Console.WriteLine("5. Ändra ett rum");
                Console.WriteLine("6. Registrera en kund");
                Console.WriteLine("7. Ändra en kund");
                Console.WriteLine("8. Ta bort ett rum");
                Console.WriteLine("9. Ta bort en bokning");
                Console.WriteLine("10. Ta bort en kund");
                Console.WriteLine("11. Avsluta");
                Console.Write("Välj ett alternativ: ");

                var choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        ShowRooms(options);
                        break;
                    case "2":
                        AddBooking(options);
                        break;
                    case "3":
                        ShowCustomers(options);
                        break;
                    case "4":
                        RegisterRoom(options);
                        break;
                    case "5":
                        EditRoom(options);
                        break;
                    case "6":
                        RegisterCustomer(options);
                        break;
                    case "7":
                        EditCustomer(options);
                        break;
                    case "8":
                        DeleteRoom(options);
                        break;
                    case "9":
                        DeleteBooking(options);
                        break;
                    case "10":
                        DeleteCustomer(options);
                        break;
                    case "11":
                        Console.WriteLine("Avslutar programmet...");
                        return;
                    default:
                        Console.WriteLine("Ogiltigt val. Försök igen.");
                        break;
                }
            }
        }

        static void ShowRooms(DbContextOptions<ApplicationDbContext> options)
        {
            using var dbContext = new ApplicationDbContext(options);
            var rooms = dbContext.Rooms.ToList();

            Console.WriteLine("\nTillgängliga rum:");
            foreach (var room in rooms)
            {
                Console.WriteLine($"Rum ID: {room.RoomId}, Typ: {room.RoomType}, Kapacitet: {room.Capacity}, Pris/Natt: {room.PricePerNight}, Extrasäng: {(room.HasExtraBedOption ? "Ja" : "Nej")}");
            }
        }

        static void AddBooking(DbContextOptions<ApplicationDbContext> options)
        {
            using var dbContext = new ApplicationDbContext(options);

            Console.Write("Ange Kundens ID: ");
            int customerId = int.Parse(Console.ReadLine());
            Console.Write("Ange Rummets ID: ");
            int roomId = int.Parse(Console.ReadLine());
            Console.Write("Ange Startdatum (yyyy-mm-dd): ");
            DateTime startDate = DateTime.Parse(Console.ReadLine());
            Console.Write("Ange Slutdatum (yyyy-mm-dd): ");
            DateTime endDate = DateTime.Parse(Console.ReadLine());

            var room = dbContext.Rooms.FirstOrDefault(r => r.RoomId == roomId);

            if (room == null)
            {
                Console.WriteLine("Rummet finns inte.");
                return;
            }

            bool isRoomBooked = dbContext.Bookings.Any(b =>
                b.RoomId == roomId &&
                ((startDate >= b.StartDate && startDate < b.EndDate) ||
                 (endDate > b.StartDate && endDate <= b.EndDate) ||
                 (startDate <= b.StartDate && endDate >= b.EndDate)));

            if (isRoomBooked)
            {
                Console.WriteLine("Rummet är redan bokat under den valda perioden.");
                return;
            }

            int extraBeds = 0;
            if (room.HasExtraBedOption)
            {
                Console.Write($"Rummet har {room.ExtraBedsAvailable} extrasängar tillgängliga. Hur många extra sängar vill du lägga till? ");
                extraBeds = int.Parse(Console.ReadLine());

                if (extraBeds > room.ExtraBedsAvailable)
                {
                    Console.WriteLine($"Maximalt antal extrasängar som kan läggas till är {room.ExtraBedsAvailable}.");
                    return;
                }
            }

            var booking = new Booking
            {
                CustomerId = customerId,
                RoomId = roomId,
                StartDate = startDate,
                EndDate = endDate,
                ExtraBeds = extraBeds
            };

            dbContext.Bookings.Add(booking);
            dbContext.SaveChanges();
            Console.WriteLine("Bokningen har lagts till!");
        }

        static void ShowCustomers(DbContextOptions<ApplicationDbContext> options)
        {
            using var dbContext = new ApplicationDbContext(options);
            var customers = dbContext.Customers.ToList();

            Console.WriteLine("\nRegistrerade kunder:");
            foreach (var customer in customers)
            {
                Console.WriteLine($"Kund ID: {customer.CustomerId}, Namn: {customer.FirstName} {customer.LastName}, Telefon: {customer.PhoneNumber}, E-post: {customer.Email}");
            }
        }

        static void RegisterRoom(DbContextOptions<ApplicationDbContext> options)
        {
            using var dbContext = new ApplicationDbContext(options);

            Console.Write("Ange rumstyp (Single/Double): ");
            var roomType = Console.ReadLine();
            Console.Write("Ange kapacitet: ");
            int capacity = int.Parse(Console.ReadLine());
            Console.Write("Ange pris per natt: ");
            decimal pricePerNight = decimal.Parse(Console.ReadLine());
            Console.Write("Har rummet extrasängar? (ja/nej): ");
            bool hasExtraBedOption = Console.ReadLine().ToLower() == "ja";

            var room = new Room
            {
                RoomType = roomType,
                Capacity = capacity,
                PricePerNight = pricePerNight,
                HasExtraBedOption = hasExtraBedOption
            };

            dbContext.Rooms.Add(room);
            dbContext.SaveChanges();
            Console.WriteLine("Rummet har registrerats!");
        }

        static void EditRoom(DbContextOptions<ApplicationDbContext> options)
        {
            using var dbContext = new ApplicationDbContext(options);

            Console.Write("Ange rum ID för att ändra: ");
            int roomId = int.Parse(Console.ReadLine());

            var room = dbContext.Rooms.FirstOrDefault(r => r.RoomId == roomId);
            if (room == null)
            {
                Console.WriteLine("Rummet finns inte.");
                return;
            }

            Console.Write($"Nuvarande rumstyp ({room.RoomType}): ");
            room.RoomType = Console.ReadLine();
            Console.Write($"Nuvarande kapacitet ({room.Capacity}): ");
            room.Capacity = int.Parse(Console.ReadLine());
            Console.Write($"Nuvarande pris per natt ({room.PricePerNight}): ");
            room.PricePerNight = decimal.Parse(Console.ReadLine());
            Console.Write($"Har rummet extrasängar? ({(room.HasExtraBedOption ? "ja" : "nej")}): ");
            room.HasExtraBedOption = Console.ReadLine().ToLower() == "ja";

            dbContext.SaveChanges();
            Console.WriteLine("Rummet har uppdaterats!");
        }

        static void DeleteRoom(DbContextOptions<ApplicationDbContext> options)
        {
            using var dbContext = new ApplicationDbContext(options);

            Console.Write("Ange rum ID för att ta bort: ");
            int roomId = int.Parse(Console.ReadLine());

            var room = dbContext.Rooms.FirstOrDefault(r => r.RoomId == roomId);
            if (room == null)
            {
                Console.WriteLine("Rummet finns inte.");
                return;
            }

            dbContext.Rooms.Remove(room);
            dbContext.SaveChanges();
            Console.WriteLine("Rummet har tagits bort!");
        }

        static void DeleteBooking(DbContextOptions<ApplicationDbContext> options)
        {
            using var dbContext = new ApplicationDbContext(options);

            Console.Write("Ange bokning ID för att ta bort: ");
            int bookingId = int.Parse(Console.ReadLine());

            var booking = dbContext.Bookings.FirstOrDefault(b => b.BookingId == bookingId);
            if (booking == null)
            {
                Console.WriteLine("Bokningen finns inte.");
                return;
            }

            dbContext.Bookings.Remove(booking);
            dbContext.SaveChanges();
            Console.WriteLine("Bokningen har tagits bort!");
        }

        static void DeleteCustomer(DbContextOptions<ApplicationDbContext> options)
        {
            using var dbContext = new ApplicationDbContext(options);

            Console.Write("Ange kund ID för att ta bort: ");
            int customerId = int.Parse(Console.ReadLine());

            var customer = dbContext.Customers.FirstOrDefault(c => c.CustomerId == customerId);
            if (customer == null)
            {
                Console.WriteLine("Kunden finns inte.");
                return;
            }

            dbContext.Customers.Remove(customer);
            dbContext.SaveChanges();
            Console.WriteLine("Kunden har tagits bort!");
        }

        static void RegisterCustomer(DbContextOptions<ApplicationDbContext> options)
        {
            using var dbContext = new ApplicationDbContext(options);

            Console.Write("Förnamn: ");
            var firstName = Console.ReadLine();
            Console.Write("Efternamn: ");
            var lastName = Console.ReadLine();
            Console.Write("Telefonnummer: ");
            var phoneNumber = Console.ReadLine();
            Console.Write("E-post: ");
            var email = Console.ReadLine();

            var customer = new Customer
            {
                FirstName = firstName,
                LastName = lastName,
                PhoneNumber = phoneNumber,
                Email = email
            };

            dbContext.Customers.Add(customer);
            dbContext.SaveChanges();
            Console.WriteLine("Kunden har registrerats!");
        }

        static void EditCustomer(DbContextOptions<ApplicationDbContext> options)
        {
            using var dbContext = new ApplicationDbContext(options);

            Console.Write("Ange kund ID för att ändra: ");
            int customerId = int.Parse(Console.ReadLine());

            var customer = dbContext.Customers.FirstOrDefault(c => c.CustomerId == customerId);
            if (customer == null)
            {
                Console.WriteLine("Kunden finns inte.");
                return;
            }

            Console.Write($"Nuvarande förnamn ({customer.FirstName}): ");
            customer.FirstName = Console.ReadLine();
            Console.Write($"Nuvarande efternamn ({customer.LastName}): ");
            customer.LastName = Console.ReadLine();
            Console.Write($"Nuvarande telefonnummer ({customer.PhoneNumber}): ");
            customer.PhoneNumber = Console.ReadLine();
            Console.Write($"Nuvarande e-post ({customer.Email}): ");
            customer.Email = Console.ReadLine();

            dbContext.SaveChanges();
            Console.WriteLine("Kunden har uppdaterats!");
        }
    }
 }

  

