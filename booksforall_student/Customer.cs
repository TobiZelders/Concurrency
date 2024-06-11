namespace booksforall;
public class Customer
{
    // feel free to add necessary fields
    private Book? _currentBook; // Book currently held by the customer
    private readonly int _id;
    public Customer(int customerId) // feel free to change this constructor
    {
        _currentBook = null;
        _id = customerId++;
    }

    public Book? GetCurrentBook()
    {
        return _currentBook;
    }
    // this is the work that the customer does



    //ADD LOCKS
    //NOTIFY WHEN BOOK READY
    public void DoWork() // feel free to add code to this method, 
                //  but DO NOT remove the existing one
                // do not alter the order of the instructions.
    {

        // the customer will come to the library when the book is ready
        // the customer picks up a book that he requested
//GETS SIGNALED
//CRITICAL SECTION
    using (Mutex mutex = new Mutex(false, Program.counterMutex, out bool createdNew))
    {
        Program.counterSemaphore.Wait(); //Gets notified when item is added
        mutex.WaitOne();
        _currentBook = Program.counter.First();
        Program.counter.RemoveFirst();
        Console.WriteLine($"Customer {_id} is about to read the book {_currentBook.BookId}");
        mutex.ReleaseMutex();
    }
//EXIT
        // the customer will take the book to read
        Thread.Sleep(new Random().Next(100, 500));
//CRITICAL SECTION
        //the customer will return the book to the dropoff
    using (Mutex mutex = new Mutex(false, Program.counterMutex, out bool createdNew))
    {
        mutex.WaitOne();
        Console.WriteLine($"Customer {_id} is dropping off the book {_currentBook.BookId}");
        Program.dropoff.AddFirst(_currentBook);
        mutex.ReleaseMutex();
    }
    Program.dropoffSemaphore.Release(); //Now Clerk should be able to go to dropoff & doWork
//EXIT
        _currentBook = null;

        Console.WriteLine($"Customer {_id} is leaving the library");
//CW NIET IN LOCK!!!!!!!!!
    }
}