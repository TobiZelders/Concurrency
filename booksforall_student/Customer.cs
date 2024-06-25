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
    public void DoWork() // feel free to add code to this method, 
                //  but DO NOT remove the existing one
                // do not alter the order of the instructions.
    {
        // the customer will come to the library when the book is ready
        // the customer picks up a book that he requested
        using (Mutex mutex = new Mutex(false, Program.counterMutex, out bool createdNew))
        {
            Program.counterConsumerSemaphore.Wait();
            mutex.WaitOne();
            _currentBook = Program.counter.First();
            Program.counter.RemoveFirst();
            mutex.ReleaseMutex();
        }
        
        Console.WriteLine($"Customer {_id} is about to read the book {_currentBook.BookId}");

        // the customer will take the book to read
        Thread.Sleep(new Random().Next(100, 500));

        //the customer will return the book to the dropoff
        Console.WriteLine($"Customer {_id} is dropping off the book {_currentBook.BookId}");

        using (Mutex mutex = new Mutex(false, Program.dropoffMutex, out bool createdNew))
        {
            mutex.WaitOne();
            Program.dropoff.AddFirst(_currentBook);
            mutex.ReleaseMutex();
            Program.dropoffConsumerSemaphore.Release();
        }
        _currentBook = null;

        Console.WriteLine($"Customer {_id} is leaving the library");
    }
}