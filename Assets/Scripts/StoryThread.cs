public class StoryThread
{
    public Narration[] thread { get; protected set; }
    
    protected int index         { get; set; }

    public StoryThread(Narration[] thread)
    {
        this.thread = thread;
    }

    public virtual Narration GetNext()
    {
        return index < thread.Length ? thread[index++] : null;
    }
}
