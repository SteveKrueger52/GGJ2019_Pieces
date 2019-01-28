public class Dialogue : Narration
{
    private Dialogue next; // null if no further dialogue in this chain

    public Dialogue(string speaker, string sprite, string side, string body, string[] flags, Dialogue next) : 
        base(speaker, sprite, side, body, flags)
    {
        this.next = next;
    }

    public override string[] GetReplies()
    {
        return new string[0];
    }

    public override Narration Cycle()
    {
        PopFlags();
        if (next == null)
            return null;
        
        // Cycle Recursively Down
        speaker = next.speaker;
        sprite = next.sprite;
        body = next.body;
        side = next.side;
        flags = next.flags;
        next = next.next;
        return this;
    }
    public override Narration Cycle(int index, out int statChange)
    {
        statChange = 0;
        return Cycle();
    }

    public override string ToString()
    {
        string acc = "";
        if (next != null)
            acc = " -> "+next;
        return "Dialogue (" + speaker + " " + side + "): " + body + acc;
    }
}