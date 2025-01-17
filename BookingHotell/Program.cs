using BookingHotell.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Text.RegularExpressions;

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
                    new Room { RoomType = RoomType.Single, Capacity = 1, PricePerNight = 500, HasExtraBedOption = false },
                    new Room { RoomType = RoomType.Double, Capacity = 2, PricePerNight = 1000, HasExtraBedOption = true },
                    new Room { RoomType = RoomType.Double, Capacity = 2, PricePerNight = 1200, HasExtraBedOption = true },
                    new Room { RoomType = RoomType.Single, Capacity = 1, PricePerNight = 450, HasExtraBedOption = false }
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
                Console.Clear();
                Console.WriteLine("\nVälkommen till HotellBokningsappen!");
                Console.WriteLine("Välj en kategori:");
                Console.WriteLine("1. Bokningar");
                Console.WriteLine("2. Kunder");
                Console.WriteLine("3. Rum");
                Console.WriteLine("4. Avsluta");
                Console.Write("Välj ett alternativ: ");

                var choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        RunBookingMenu(options);
                        break;
                    case "2":
                        RunCustomerMenu(options);
                        break;
                    case "3":
                        RunRoomMenu(options);
                        break;
                    case "4":
                        Console.WriteLine("Avslutar programmet...");
                        return;
                    default:
                        Console.WriteLine("Ogiltigt val. Försök igen.");
                        break;
                }
            }
        }
        static void RunBookingMenu(DbContextOptions<ApplicationDbContext> options)
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("\nBokningar:");
                Console.WriteLine("1. Lägg till en bokning");
                Console.WriteLine("2. Visa bokningar");
                Console.WriteLine("3. Ta bort en bokning");
                Console.WriteLine("4. Tillbaka");
                Console.Write("Välj ett alternativ: ");

                var choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        AddBooking(options);
                        break;
                    case "2":
                        ShowBookings(options);
                        break;
                    case "3":
                        DeleteBooking(options);
                        break;
                    case "4":
                        return;
                    default:
                        Console.WriteLine("Ogiltigt val. Försök igen.");
                        break;
                }
            }
        }
        static void RunCustomerMenu(DbContextOptions<ApplicationDbContext> options)
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("\nKunder:");
                Console.WriteLine("1. Registrera en kund");
                Console.WriteLine("2. Visa kunder");
                Console.WriteLine("3. Ändra en kund");
                Console.WriteLine("4. Ta bort en kund");
                Console.WriteLine("5. Tillbaka");
                Console.Write("Välj ett alternativ: ");

                var choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        RegisterCustomer(options);
                        break;
                    case "2":
                        ShowCustomers(options);
                        break;
                    case "3":
                        EditCustomer(options);
                        break;
                    case "4":
                        DeleteCustomer(options);
                        break;
                    case "5":
                        return;
                    default:
                        Console.WriteLine("Ogiltigt val. Försök igen.");
                        break;
                }
            }
        }
        static void RunRoomMenu(DbContextOptions<ApplicationDbContext> options)
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("\nRum:");
                Console.WriteLine("1. Visa rum");
                Console.WriteLine("2. Registrera ett rum");
                Console.WriteLine("3. Ändra ett rum");
                Console.WriteLine("4. Ta bort ett rum");
                Console.WriteLine("5. Tillbaka");
                Console.Write("Välj ett alternativ: ");

                var choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        ShowRooms(options);
                        break;
                    case "2":
                        RegisterRoom(options);
                        break;
                    case "3":
                        EditRoom(options);
                        break;
                    case "4":
                        DeleteRoom(options);
                        break;
                    case "5":
                        return;
                    default:
                        Console.WriteLine("Ogiltigt val. Försök igen.");
                        break;
                }
            }
        }
        static int ReadInt(string prompt)
        {
            while (true)
            {
                Console.Write(prompt);
                if (int.TryParse(Console.ReadLine(), out int result))
                {
                    return result;
                }
                Console.WriteLine("Ogiltigt heltalsvärde. Försök igen.");
            }
        }
        static decimal ReadDecimal(string prompt)
        {
            while (true)
            {
                Console.Write(prompt);
                if (decimal.TryParse(Console.ReadLine(), out decimal result))
                {
                    return result;
                }
                Console.WriteLine("Ogiltigt decimalvärde. Försök igen.");
            }
        }

        static void ShowRooms(DbContextOptions<ApplicationDbContext> options)
        {
            using var dbContext = new ApplicationDbContext(options);
            var rooms = dbContext.Rooms.OrderBy(r => r.RoomId).ToList();

            Console.WriteLine("\nTillgängliga rum:");
            int roomNumber = 1;
            foreach (var room in rooms)
            {
                Console.WriteLine($"Rum {roomNumber}, Typ: {room.RoomType}, Kapacitet: {room.Capacity}, Pris/Natt: {room.PricePerNight:C}, Extrasäng: {(room.HasExtraBedOption ? "Ja" : "Nej")}");
                roomNumber++;
            }
            Console.WriteLine("\nTryck Enter för att återgå till föregående meny.");
            Console.ReadLine();
            
        }
        static void ShowBookings(DbContextOptions<ApplicationDbContext> options)
        {
            using var dbContext = new ApplicationDbContext(options);
            var bookings = dbContext.Bookings
                .Include(b => b.Room)
                .Include(b => b.Customer)
                .ToList();

            if (bookings.Any())
            {
                Console.WriteLine("\nBokningar:");
                foreach (var booking in bookings)
                {
                    Console.WriteLine($"Bokning ID: {booking.BookingId}, Kund: {booking.Customer.FirstName} {booking.Customer.LastName}, Rumstyp: {booking.Room.RoomType}, Startdatum: {booking.StartDate:yyyy-MM-dd}, Slutdatum: {booking.EndDate:yyyy-MM-dd}");
                }
            }
            else
            {
                Console.WriteLine("\nInga bokningar finns.");
            }
            Console.WriteLine("\nTryck Enter för att gå tillbaka.");
            Console.ReadLine();
        }

        static void AddBooking(DbContextOptions<ApplicationDbContext> options)
        {
            try
            {
                using var dbContext = new ApplicationDbContext(options);

                Console.Write("Ange Kundens ID: ");
                if (!int.TryParse(Console.ReadLine(), out int customerId))
                {
                    Console.WriteLine("Ogiltigt kund-ID.");
                    return;
                }

                var customer = dbContext.Customers.FirstOrDefault(c => c.CustomerId == customerId);
                if (customer == null)
                {
                    Console.WriteLine("Kunden hittades inte.");
                    return;
                }

                Console.WriteLine("Tillgängliga rum:");
                var availableRooms = dbContext.Rooms.ToList();
                if (!availableRooms.Any())
                {
                    Console.WriteLine("Inga tillgängliga rum.");
                    return;
                }

                foreach (var room in availableRooms)
                {
                    Console.WriteLine($"Rum ID: {room.RoomId}, Typ: {room.RoomType}, Kapacitet: {room.Capacity}, Pris per natt: {room.PricePerNight}");
                }

                Console.Write("Ange Rummets ID för bokning: ");
                if (!int.TryParse(Console.ReadLine(), out int roomId))
                {
                    Console.WriteLine("Ogiltigt rum-ID.");
                    return;
                }

                var roomToBook = dbContext.Rooms.FirstOrDefault(r => r.RoomId == roomId);
                if (roomToBook == null)
                {
                    Console.WriteLine("Rummet finns inte.");
                    return;
                }

                DateTime startDate;
                while (true)
                {
                    Console.Write("Startdatum (yyyy-mm-dd): ");
                    if (DateTime.TryParse(Console.ReadLine(), out startDate) && startDate >= DateTime.Today)
                        break;
                    Console.WriteLine("Ogiltigt datum. Startdatum måste vara idag eller ett framtida datum.");
                }

                DateTime endDate;
                while (true)
                {
                    Console.Write("Slutdatum (yyyy-mm-dd): ");
                    if (DateTime.TryParse(Console.ReadLine(), out endDate) && endDate > startDate)
                        break;
                    Console.WriteLine("Ogiltigt datum. Slutdatum måste vara efter startdatum.");
                }

                // Kontrollera om rummet redan är bokat
                bool isRoomBooked = dbContext.Bookings.Any(b => b.RoomId == roomId &&
                    ((startDate >= b.StartDate && startDate < b.EndDate) ||
                     (endDate > b.StartDate && endDate <= b.EndDate) ||
                     (startDate <= b.StartDate && endDate >= b.EndDate)));

                if (isRoomBooked)
                {
                    Console.WriteLine("Rummet är redan bokat under de valda datumen.");
                    return;
                }

                var booking = new Booking
                {
                    CustomerId = customerId,
                    RoomId = roomId,
                    StartDate = startDate,
                    EndDate = endDate
                };

                dbContext.Bookings.Add(booking);
                dbContext.SaveChanges();
                Console.WriteLine("Bokningen har lagts till!");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ett fel inträffade: {ex.Message}");
            }
        }


        static void ShowCustomers(DbContextOptions<ApplicationDbContext> options)
        {
            using var dbContext = new ApplicationDbContext(options);
            var customers = dbContext.Customers.ToList();

            Console.WriteLine("\nRegistrerade kunder:");
            foreach (var customer in customers)
            {

                Console.WriteLine($"Kund ID{customer.CustomerId}, Namn: {customer.FirstName} {customer.LastName}, Telefon: {customer.PhoneNumber}, E-post: {customer.Email}");
            }

            Console.WriteLine("\nSkriv 'M' för att återgå till huvudmenyn.");
            string choice = Console.ReadLine();
            if (choice?.ToLower() == "m")
            {
                return;
            }
        }


        static void RegisterRoom(DbContextOptions<ApplicationDbContext> options)
        {
            while (true)
            {
                try
                {
                    using var dbContext = new ApplicationDbContext(options);

                    RoomType roomType;
                    while (true)
                    {
                        Console.Write("Ange rumstyp (Single/Double): ");
                        var roomTypeInput = Console.ReadLine();
                        if (Enum.TryParse(typeof(RoomType), roomTypeInput, true, out var parsedType) && Enum.IsDefined(typeof(RoomType), parsedType))
                        {
                            roomType = (RoomType)parsedType;
                            break;
                        }
                        Console.WriteLine("Ogiltig rumstyp. Ange 'Single' eller 'Double'. Försök igen.");
                    }

                    int capacity;
                    while (true)
                    {
                        Console.Write("Ange kapacitet: ");
                        if (int.TryParse(Console.ReadLine(), out capacity) && capacity > 0)
                            break;
                        Console.WriteLine("Ogiltig kapacitet. Ange ett positivt heltal.");
                    }

                    decimal pricePerNight;
                    while (true)
                    {
                        Console.Write("Ange pris per natt: ");
                        if (decimal.TryParse(Console.ReadLine(), out pricePerNight) && pricePerNight > 0)
                            break;
                        Console.WriteLine("Ogiltigt pris. Ange ett positivt värde.");
                    }

                    bool hasExtraBedOption;
                    while (true)
                    {
                        Console.Write("Har rummet extrasängar? (ja/nej): ");
                        var extraBedInput = Console.ReadLine()?.ToLower();
                        if (extraBedInput == "ja")
                        {
                            hasExtraBedOption = true;
                            break;
                        }
                        else if (extraBedInput == "nej")
                        {
                            hasExtraBedOption = false;
                            break;
                        }
                        Console.WriteLine("Ogiltigt svar. Ange 'ja' eller 'nej'.");
                    }

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
                catch (Exception ex)
                {
                    Console.WriteLine($"Ett fel inträffade: {ex.Message}");
                    if (ex.InnerException != null)
                    {
                        Console.WriteLine($"Inre undantag: {ex.InnerException.Message}");
                    }
                }

                Console.WriteLine("\nSkriv 'M' för att återgå till huvudmenyn.");
                string choice = Console.ReadLine();
                if (choice?.ToLower() == "m")
                {
                    break;
                }
            }
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
            string roomTypeInput = Console.ReadLine();

            
            RoomType roomType;
            if (Enum.TryParse(roomTypeInput, true, out roomType) && Enum.IsDefined(typeof(RoomType), roomType))
            {
                room.RoomType = roomType;
            }
            else
            {
                Console.WriteLine("Ogiltig rumstyp. Försök igen.");
                return;  
            }

            Console.Write($"Nuvarande kapacitet ({room.Capacity}): ");
            room.Capacity = int.Parse(Console.ReadLine());

            Console.Write($"Nuvarande pris per natt ({room.PricePerNight}): ");
            room.PricePerNight = decimal.Parse(Console.ReadLine());

            Console.Write($"Har rummet extrasängar? ({(room.HasExtraBedOption ? "ja" : "nej")}): ");
            room.HasExtraBedOption = Console.ReadLine().ToLower() == "ja";

            dbContext.SaveChanges();
            Console.WriteLine("Rummet har uppdaterats!");
            Console.WriteLine("\nSkriv 'M' för att återgå till huvudmenyn.");
            string choice = Console.ReadLine();
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
            Console.WriteLine("\nSkriv 'M' för att återgå till huvudmenyn.");
            string choice = Console.ReadLine();

        }

        static void DeleteBooking(DbContextOptions<ApplicationDbContext> options)
        {
            using var dbContext = new ApplicationDbContext(options);

            while (true)
            {
                try
                {
                    Console.Write("Ange bokningens ID för att ta bort: ");
                    if (!int.TryParse(Console.ReadLine(), out int bookingId))
                    {
                        Console.WriteLine("Ogiltigt ID. Försök igen.");
                        continue;
                    }

                    var booking = dbContext.Bookings.FirstOrDefault(b => b.BookingId == bookingId);
                    if (booking == null)
                    {
                        Console.WriteLine("Bokningen finns inte.");
                    }
                    else
                    {
                        dbContext.Bookings.Remove(booking);
                        dbContext.SaveChanges();
                        Console.WriteLine("Bokningen har tagits bort!");
                    }

                    Console.WriteLine("\nTryck på Enter för att återgå till huvudmenyn.");
                    Console.ReadLine();
                    break;
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Ett fel uppstod: {ex.Message}");
                }
            }
        }

        static void DeleteCustomer(DbContextOptions<ApplicationDbContext> options)
        {
            using var dbContext = new ApplicationDbContext(options);

            while (true)
            {
                try
                {
                    Console.Write("Ange kund-ID för att ta bort: ");
                    if (!int.TryParse(Console.ReadLine(), out int customerId))
                    {
                        Console.WriteLine("Ogiltigt ID. Försök igen.");
                        continue;
                    }

                    var customer = dbContext.Customers.FirstOrDefault(c => c.CustomerId == customerId);
                    if (customer == null)
                    {
                        Console.WriteLine("Kunden finns inte.");
                    }
                    else
                    {
                        dbContext.Customers.Remove(customer);
                        dbContext.SaveChanges();
                        Console.WriteLine("Kunden har tagits bort!");
                    }

                    Console.WriteLine("\nTryck på Enter för att återgå till menyn.");
                    Console.ReadLine();
                    break;
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Ett fel uppstod: {ex.Message}");
                }
            }
        }
        static void RegisterCustomer(DbContextOptions<ApplicationDbContext> options)
        {
            using var dbContext = new ApplicationDbContext(options);

            string firstName;
            
            do
            {
                Console.Write("Förnamn: ");
                firstName = Console.ReadLine();
            } while (string.IsNullOrEmpty(firstName) || !IsValidName(firstName));

            string lastName;
           
            do
            {
                Console.Write("Efternamn: ");
                lastName = Console.ReadLine();
            } while (string.IsNullOrEmpty(lastName) || !IsValidName(lastName));

            string phoneNumber;
            do
            {
                Console.Write("Telefonnummer (endast siffror): ");
                phoneNumber = Console.ReadLine();
            } while (!Regex.IsMatch(phoneNumber, @"^\d{7,15}$"));

            string email;
            do
            {
                Console.Write("E-post: ");
                email = Console.ReadLine();
            } while (!Regex.IsMatch(email, @"^[^@\s]+@[^@\s]+\.[^@\s]+$"));

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
            Console.WriteLine("\nSkriv 'M' för att återgå till huvudmenyn.");
            string choice = Console.ReadLine();
            if (choice?.ToLower() == "m")
            {
                return;
            }
        }

       
        static bool IsValidName(string name)
        {
            return !string.IsNullOrEmpty(name) && name.All(c => Char.IsLetter(c) || Char.IsWhiteSpace(c));
        }

        static void EditCustomer(DbContextOptions<ApplicationDbContext> options)
        {
            using var dbContext = new ApplicationDbContext(options);

            Console.Write("Ange kundens ID för att ändra: ");
            if (!int.TryParse(Console.ReadLine(), out int customerId))
            {
                Console.WriteLine("Ogiltigt kund-ID.");
                return;
            }

            var customer = dbContext.Customers.FirstOrDefault(c => c.CustomerId == customerId);
            if (customer == null)
            {
                Console.WriteLine("Kunden finns inte.");
                return;
            }

            
            string firstName;
            while (true)
            {
                Console.Write($"Nuvarande förnamn ({customer.FirstName}): ");
                firstName = Console.ReadLine();

                
                if (firstName.Any(char.IsDigit))
                {
                    Console.WriteLine("Förnamnet kan inte innehålla siffror. Försök igen.");
                }
                else if (string.IsNullOrEmpty(firstName))
                {
                    Console.WriteLine("Förnamn kan inte vara tomt. Försök igen.");
                }
                else
                {
                    break;
                }
            }

            
            string lastName;
            while (true)
            {
                Console.Write($"Nuvarande efternamn ({customer.LastName}): ");
                lastName = Console.ReadLine();

               
                if (lastName.Any(char.IsDigit))
                {
                    Console.WriteLine("Efternamnet kan inte innehålla siffror. Försök igen.");
                }
                else if (string.IsNullOrEmpty(lastName))
                {
                    Console.WriteLine("Efternamn kan inte vara tomt. Försök igen.");
                }
                else
                {
                    break;
                }
            }

            // Telefon och E-post validering kan också läggas till här om du vill

            customer.FirstName = firstName;
            customer.LastName = lastName;

            dbContext.SaveChanges();
            Console.WriteLine("Kunden har uppdaterats!");
        }



    }
}
 

  
        
    

            
        


    

 

  

