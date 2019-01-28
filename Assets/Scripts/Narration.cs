    using System;
using System.Collections.Generic;
    using System.Linq;
    using UnityEngine;

public abstract class Narration
{
    // Static databases of Dialogues, Questions, and NarrationChains
    // Parse and populate in order of Dialogues, Questions, then full Threads
    // to avoid broken or missing references.
    public static Dictionary<string, Dialogue> DialogueLibrary = new Dictionary<string, Dialogue>(); 
    public static Dictionary<string, Question> QuestionLibrary = new Dictionary<string, Question>();
    public static Dictionary<string, StoryThread> SceneLibrary = new Dictionary<string, StoryThread>();
    
    // STATIC PARSER STUFF
    public static class Parser
    {
        // returns the grid of strings in the CSV, separated but not categorized
        private static List<List<string>> ParseCSV(string resourcePath)
        {
            string file = Resources.Load<TextAsset>(resourcePath).text;

            // Initialize Intermediary List
            List<List<string>> csv = new List<List<string>>();
            csv.Add(new List<string>());
            string cell = "";
            int index = 0; // Line index 
            bool escaped = false; // ignore CSV Formatting
            bool quote = false; // the last character was a "
            
            // Parse CSV
            foreach (char c in file)
            {
                switch (c)
                {
                    // " is the escape character in CSV
                    case '\"':
                        escaped = !escaped;
                        // "" translates to a literal " character
                        if (quote)
                            cell += "\"";
                        break;
                    // , is the separator value, proceed to next cell if not escaped
                    // newline is a line separator, proceed to next line if not escaped
                    case ',':
                    case '\n':
                        if (!escaped)
                        {
                            csv[index].Add(cell); // Add cell to list
                            cell = "";
                            if (c == '\n')
                            {
                                csv.Add(new List<string>()); // finish row, move on
                                // Debug.Log("CSV Parsed. # of Elems: "+csv[index].Count);
                                index++;
                            }
                        }
                        else
                            cell += c;
                        break;
                    default:
                        cell += c;
                        break;
                }

                quote = c == '\"' && !quote;
            }
            csv[index].Add(cell);
            // Debug.Log("CSV Parsed. # of Elems: "+csv[index].Count);
            return csv;
        }

        // Populates the DialogueLibrary with new Dialogues from the CSV
        public static void ParseDialogues(string resourcePath)
        {
            List<List<string>> csv = ParseCSV(resourcePath);
            Dialogue acc = null;
            Dialogue builder;
            for (int i = csv.Count - 1; i >= 0; i--)            // LINE PARSING, in reverse for recursion
            {
                // Debug.Log("Line "+i+", # of Elems: " + csv[i].Count);
                builder = new Dialogue(csv[i][1], csv[i][2], csv[i][3], csv[i][4], csv[i][5].Split(' '), acc);
                // Debug.Log("New Line: " + builder.speaker + " : " + builder.body);
                if (csv[i][0] == "")
                    acc = builder;
                else
                {
                    //Debug.Log(csv[i][0]+" : "+builder);
                    DialogueLibrary.Add(csv[i][0],builder);
                    acc = null;
                }
            }
        }
        
        // Populates the QuestionLibrary with new Questions from the CSV
        public static void ParseQuestions(string resourcePath)
        {
            List<List<string>> csv = ParseCSV(resourcePath);
            List<string> replies = new List<string>();
            List<Dialogue> responses = new List<Dialogue>();
            List<int> deltas = new List<int>();
            
            // Questions are exactly four lines long each
            for (int i = 0; i < csv.Count - csv.Count % 4; i += 4)
            {
                for (int j = 0; j < csv[i+1].Count; j++)
                {
                    if (!string.IsNullOrWhiteSpace(csv[i + 1][j]) && 
                        !string.IsNullOrWhiteSpace(csv[i + 2][j]) && 
                        !string.IsNullOrWhiteSpace(csv[i + 3][j]))
                    {
                        //Debug.Log(" [" + csv[i + 1][j] + ":" + csv[i + 2][j] + ":" + csv[i + 3][j] + "]");
                        replies.Add(csv[i + 1][j]);
                        //Debug.Log("("+i+":"+j+") "+csv[i + 2][j]);
                        responses.Add(DialogueLibrary[csv[i + 2][j]]);
                        deltas.Add(int.Parse(csv[i + 3][j]));
                        //Debug.Log(" [" + replies[j] + ":" + deltas[j] + " -> " + responses[j] + "]");
                    }
                }

                string[] filteredTags = csv[i][5].Split(' ').Where(a => !String.IsNullOrWhiteSpace(a)).ToArray();
                QuestionLibrary.Add(csv[i][0],
                    new Question(csv[i][1],
                        csv[i][2],
                        csv[i][3],
                        csv[i][4],
                        filteredTags,
                        replies.ToArray(), 
                        DialogueLibrary[csv[i + 2][0]],
                        responses.ToArray(), 
                        deltas.ToArray()));
                
                replies = new List<string>();
                responses = new List<Dialogue>();
                deltas = new List<int>();
            }
        }
     
        // Populates the SceneLibrary with new StoryThreads from the CSV
        public static void ParseScenes(string resourcePath)
        {
            List<List<string>> csv = ParseCSV(resourcePath);
            StoryThread pre;     // pre-scene in fights/flashbacks
            StoryThread post1;   // post-scene in fights (win)/flashbacks
            StoryThread post2;   // post-scene for lost fights
            List <Narration> builder = new List<Narration>();
            for (int i = 0; i < csv.Count; i++)
            {
                if (csv[i][0] == "NULL")
                {
                    Debug.Log("We found a null!");
                }
                else
                {
                    switch (csv[i][1])
                    {
                        // Simple Story Thread
                        case "S":
                        case "s":
                            for (int j = 2; j < csv[1].Count; j += 2)
                            {
                                switch (csv[i][j])
                                {
                                    case "Q":
                                    case "q":
                                        builder.Add(QuestionLibrary[csv[i][j+1]]);
                                        break;
                                    case "D":
                                    case "d":
                                        builder.Add(DialogueLibrary[csv[i][j+1]]);
                                        break;
                                }
                            }
                            SceneLibrary.Add(csv[i][0],new StoryThread(builder.ToArray()));
                            builder = new List<Narration>();
                            break;
                    
                        // Fight Story Thread
                        case "F":
                        case "f":
                            try
                            {
                                pre = SceneLibrary[csv[i][3]];
                            }
                            catch (Exception e)
                            {
                                pre = new StoryThread(new Narration[0]);
                            }

                            try
                            {
                                post1 = SceneLibrary[csv[i][7]];
                            }
                            catch (Exception e)
                            {
                                post1 = new StoryThread(new Narration[0]);
                            }
                            
                            try
                            {
                                post2 = SceneLibrary[csv[i][9]];
                            }
                            catch (Exception e)
                            {
                                post2 = new StoryThread(new Narration[0]);
                            }
                            
                            SceneLibrary.Add(csv[i][0], new FightThread(pre,SceneLibrary[csv[i][5]].thread,post1,post2));
                            break;
                    
                        // Flashback Story Thread
                        case "M":
                        case "m":
                            try
                            {
                                pre = SceneLibrary[csv[i][3]];
                            }
                            catch (Exception e)
                            {
                                pre = new StoryThread(new Narration[0]);
                            }

                            try
                            {
                                post1 = SceneLibrary[csv[i][7]];
                            }
                            catch (Exception e)
                            {
                                post1 = new StoryThread(new Narration[0]);
                            }

                            SceneLibrary.Add(csv[i][0], new FlashbackThread(pre,SceneLibrary[csv[i][5]].thread,csv[i][4],post1));
                            break;
                    }
                }
            }         
        }
    }
    
    public enum Direction
    {
        Left,Right
    }
    
    public string speaker { get; protected set; }
    public string sprite  { get; protected set; }
    public string body    { get; protected set; }
    public Direction side { get; protected set; }
    public string[] flags { get; protected set; }

    protected Narration(string speaker, string sprite, string side, string body, string[] flags)
    {
        this.speaker = speaker;
        this.sprite = sprite;
        this.body = body;
        switch (side)
        {
            case "r":
            case "R":
            case "right":
            case "Right":
            case "RIGHT":
                this.side = Direction.Right;
                break;
            default:
                this.side = Direction.Left;
                break;
        }
        this.flags = flags.Select<String, String>(a => a.Replace("\r", String.Empty)).Where(a => a != "").ToArray();
    }

    public abstract string[] GetReplies();
    public abstract Narration Cycle();
    public abstract Narration Cycle(int index, out int stat);

    public void PopFlags()
    {
        foreach (string flag in flags)
            GameManager.instance.TriggerFlag(flag);
    }
}
