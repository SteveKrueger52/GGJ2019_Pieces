using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Question : Narration
{
    private List<string> replies = new List<string>(); // MUST NOT BE NULL OR EMPTY
    private List<Dialogue> responses = new List<Dialogue>();
    private Dialogue fail;
    private List<int> deltas = new List<int>();

    public Question(string speaker, string sprite, string side, string body, string[] flags,
        string[] replies, Dialogue fail, Dialogue[] responses, int[] deltas) :
        base(speaker, sprite, side, body, flags)
    {
        int min_length = replies.Length < responses.Length ? replies.Length : responses.Length;
        min_length = min_length < deltas.Length ? min_length : deltas.Length;

        this.fail = fail;
        for (int i = 0; i < min_length; i++)
        {
            this.replies.Add(replies[i]);
            this.responses.Add(responses[i]);
            this.deltas.Add(deltas[i]);
        }
    }


    public override string[] GetReplies()
    {
        return replies.ToArray();
    }

    public override Narration Cycle()
    {
        return this;
    }
    
    // Index 0 is the Failure State
    public override Narration Cycle(int index, out int statChange)
    {
        if (index == -1) // Failstate, return fail
        {
            statChange = -1;
            return fail;
        } 
            
        if (index < 0 || index >= replies.Count) // Index out of bounds, no change
        {
            statChange = 0;
            return this;
        }
        
        // Index in range - Reply selected
        statChange = deltas[index];
        Debug.Log("Reply: "+responses[index]);
        return responses[index];
    }

    public override string ToString()
    {
        string str = "Question (" + speaker + " " + side + "): " + body;
        for (int i = 0; i < replies.Count; i++)
        {
            str += " [" + replies[i] + ":" + deltas[i] + " -> " + responses[i] + "]";
        }
        return str;
    }
}