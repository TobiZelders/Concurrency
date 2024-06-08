
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
        Console.WriteLine($"Clerk [{_id}] is going to check in the records for a book to put on the counter");

        Book? t_book = null;
        
        foreach (var record in _records)    // the clerk will look in the records
                                            // for a book that is not yet borrowed
        {

            if (record.IsBorrowed == false)
            {
                t_book = record.Book;
                
                record.IsBorrowed = true;
                
                break;
            }

        }
        Console.WriteLine($"Clerk [{_id}] putting book [{t_book.BookId}] on the counter");

        //LibrarySemaphore.counterSemaphore.Wait();
        Program.counter.AddFirst(t_book);
        //LibrarySemaphore.counterSemaphore.Release();
        // the clerk will put the book on the counter for the customer

        Thread.Sleep(new Random().Next(100, 500));
        //the clerk will take a nap for overworking

        //LibrarySemaphore.dropOffSemaphore.Wait();// SEMAPHOREEEEEEEEEEE
        //the clerk will wait for a book in the dropoff

        t_book = Program.dropoff.First();

        Program.dropoff.RemoveFirst();

        //LibrarySemaphore.dropOffSemaphore.Release();// SEMAPHOREEEEEEEEEEE

        //the clerk will check the book in the records
        Console.WriteLine($"Clerk [{_id}] is checking in the book [{t_book.BookId}] in the records");
        
        foreach (BookRecord record in _records)
        {
            if (record.Book.BookId == t_book.BookId)
            {
                record.IsBorrowed = false;

                break;
            }
        }
    }
}


