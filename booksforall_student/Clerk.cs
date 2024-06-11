namespace booksforall;
public class Clerk
{
    // feel free to add necessary fields

    // this is where we store the records of books borrowed and returned
    // only the clerk can access this list
    private static LinkedList<BookRecord> _records; //do not alter this line
    private int _id;

    public Clerk(int clerkId) // feel free to change this constructor
    {
        _id = clerkId;
        if (_records == null)
        {
            _records = new LinkedList<BookRecord>();
        }
    }

    public static void initRecords(LinkedList<Book> books) //do not alter this method
    // this method is called when a new batch of books arrives at the library
    {
        if (_records == null)
        {
            _records = new LinkedList<BookRecord>();
        }
        foreach (Book book in books)
        {
            _records.AddFirst(new BookRecord(book, false));
        }
    }

    internal static int checkBookInInventory() //do not alter this method
    // this method is called when the library is closing
    // this method should return the number of books left in the library
    {
        int counter = 0;
        foreach (var record in _records)
        {
            if (record.IsBorrowed == false) //we are counting the books that are int the library (not borrowed)
            {
                counter++;
            }
        }

        if (counter != _records.Count)
        {
            Console.WriteLine("Error: the number of books left in the library does not match the number of records." + counter + _records.Count);
        }
        return counter;
    }

// this is the work that the clerk does
    public void DoWork()    //feel free to add code to this method,
                            //  but DO NOT remove the existing one
                            // do not alter the order of the instructions.
    {
        //the clerk will put the book in the counter
        // find an available book, but do not remove it from the original records
        //Console.WriteLine($"Clerk [{_id}] is going to check in the records for a book to put on the counter");

        Book? t_book = null;
//CRITICAL SECTION
        using (Mutex mutex = new Mutex(false, Program.recordMutex, out bool createdNew))
        {
//            Console.WriteLine($"CLERK [{_id}] waiting to acquire the record mutex.");
            mutex.WaitOne();
            Console.WriteLine($"CLERK [{_id}] entered the record mutex.");

            foreach (var record in _records)    // the clerk will look in the records
                                                // for a book that is not yet borrowed
            {
                //System.Console.WriteLine(record.Book.BookId + $"of clerk: {_id}");
                if (record.IsBorrowed == false){
                    t_book = record.Book;
                    record.IsBorrowed = true;
                    break;      
                }
            }
            Console.WriteLine($"CLERK [{_id}] releasing the record mutex.");
            mutex.ReleaseMutex();
        }
//EXIT

        //Console.WriteLine($"Clerk [{_id}] putting book [{t_book.BookId}] on the counter");

//CRITICAL SECTION
        using (Mutex mutex = new Mutex(false, Program.counterMutex, out bool createdNew))
        {
            //Console.WriteLine($"CLERK [{_id}] waiting to acquire the counter mutex.");
            mutex.WaitOne();
            Console.WriteLine($"CLERK [{_id}] entered the counter matrix.");
            Program.counter.AddFirst(t_book);
            // the clerk will put the book on the counter for the customer
//Thread SLeep in Critical Section???
            Thread.Sleep(new Random().Next(100, 500));
            //the clerk will take a nap for overworking
            Console.WriteLine($"CLERK [{_id}] releasing the counter mutex.");
            mutex.ReleaseMutex();
        }   
        Program.counterSemaphore.Release(); //Now Customer should be able to enter
//EXIT     
//Notify??

        //the clerk will wait for a book in the dropoff
//CRITICAL SECTION
//Get notified???
        using (Mutex mutex = new Mutex(false, Program.dropoffMutex, out bool createdNew))
        {
            Program.dropoffSemaphore.Wait(); //Gets notified when item is added
            mutex.WaitOne();
            t_book = Program.dropoff.FirstOrDefault();
            Program.dropoff.RemoveFirst();
            mutex.ReleaseMutex();
        }
//EXIT
        //the clerk will check the book in the records
        Console.WriteLine($"Clerk [{_id}] is checking in the book [{t_book.BookId}] in the records");
//CRITICAL SECTION
        using (Mutex mutex = new Mutex(false, Program.recordMutex, out bool createdNew))
        {
            mutex.WaitOne();
            foreach (BookRecord record in _records)
            {
                if (record.Book.BookId == t_book.BookId)
                {
                    record.IsBorrowed = false;

                    break;
                }
            }
            mutex.ReleaseMutex();
        }
//EXIT
    }
}


