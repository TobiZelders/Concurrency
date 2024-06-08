namespace booksforall
{
    public static class LibrarySemaphore
    {
        public static SemaphoreSlim dropOffSemaphore = new SemaphoreSlim(2);
        public static ManualResetEvent CustomerHasDroppedOffBook = new ManualResetEvent(false);
        public static SemaphoreSlim counterSemaphore = new SemaphoreSlim(2);
    }
}