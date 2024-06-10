using System;

namespace booksforall
{
    internal class Program
    {
        //feel free to change the following values and if needed add variables
        public static int n_threads = 5;// feel free to change this value

        private static List<Thread> CustomerThreads = new List<Thread>();
        private static List<Thread> ClerkThreads = new List<Thread>();
        public const string recordMutex = "Global\\recordMutex";
        public const string counterMutex = "Global\\counterMutex";

        private static readonly string studentname1 = "Daniel Jong";   //name and surname of the student1
        private static readonly string studentnum1 = "0997226";    //student number
        private static readonly string studentname2 = "Tobias Zelders";   //name and surname of the student2
        private static readonly string studentnum2 = "0981403";    //student number2


        // do not alter the following lines of code 
        // if you do, put them back as they were before submitting
        public static int n_books = n_threads;
        public static int n_customers = n_threads;
        public static readonly Clerk[] clerks = new Clerk[n_threads];
        public static readonly Customer[] customers = new Customer[n_threads];
        public static LinkedList<Book> counter = new();
        public static LinkedList<Book> dropoff = new();

        static void Main(string[] args)
        {
            Console.WriteLine("Hello, we are starting our new pickup LIBRARY!");
            InitLibrary(); //do not alter this method
            
            //init the customers
            InitCustomers(); //do not alter this call

            //init the clerks
            InitClerks(); //do not alter this call

            //init records
            Clerk.initRecords(dropoff); //do not alter this line
            //clean the dropoff
            dropoff.Clear(); //do not alter this line

            //start the clerks
            StartClerks(); //do not alter this call

            //start the customers
            StartCustomers(); //do not alter this call
            // DO NOT CHANGE THE CODE ABOVE
            // use the space below to add your code if needed



            // DO NOT CHANGE THE CODE BELOW
            //the library is closing, DO NOT ALTER the following lines
            Console.WriteLine("Book left in the library " + Clerk.checkBookInInventory());

            if (counter.Count != 0)
            {
                Console.WriteLine("Books left and not picked up: " + counter.Count);
            }
            else
            {
                Console.WriteLine("Books left and not picked up: NOTHING LEFT!");
            }

            Console.WriteLine("Books left on the dropoff and not processed: " + dropoff.Count);
            // the lists should be empty
            Console.WriteLine("Name: " + studentname1 + " Student number: " + studentnum1);
            Console.WriteLine("Name: " + studentname2 + " Student number: " + studentnum2);

        }
        public static void InitLibrary() //do not alter this method
        {
            //a huge load of books arrives to the library, all at once.
            //init the books
//CRITICAL SECTION???
            for (int i = 0; i < n_books; i++)
            {
                Book book = new Book(i);    //books are all different
                dropoff.AddLast(book);      //we load the books in the dropoff just
                                            // for easy access of the clerks
            }
///
        }
        public static void InitCustomers() // feel free to alter this method if needed
//INITIEER HIER THREADS
        {
            //init the customers
            for (int i = 0; i < customers.Length; i++){
                int index = i;
                customers[index] = new Customer(index);
                Thread t = new Thread (() => {customers[index].DoWork(); });
                CustomerThreads.Add(t);
            }
        }
        public static void InitClerks() // feel free to alter this method if needed
//INITIEER HIER THREADS
        {
            //init the clerks
            for (int i = 0; i < clerks.Length; i++){
                int index = i;
                clerks[index] = new Clerk(index);
                Thread t = new Thread (() => {clerks[index].DoWork(); });
                ClerkThreads.Add(t);
            }
        }
        public static void StartClerks() // feel free to alter this method if needed
        {
            //start the clerks
            for(int i = 0; i < clerks.Length; i++)
                ClerkThreads.ElementAt(i).Start();
        }
        public static void StartCustomers() // feel free to alter this method if needed
        {
            //start the customers
            for(int i = 0; i < customers.Length; i++)
                CustomerThreads.ElementAt(i).Start();
        }

    }

    public class Book // do not alter this class
    {
        public int BookId { get; set; } // Book identifier should always be something
        public Book(int bookId)
        {
            BookId = bookId;
        }
    }

    public class BookRecord // do not alter this class
    {
        public Book Book { get; set; }
        public bool IsBorrowed { get; set; } // True for borrowed, False for returned

        public BookRecord(Book book, bool isBorrowed)
        {
            Book = book;
            IsBorrowed = isBorrowed;
        }
    }
}