public class FightThread : StoryThread
{
    private StoryThread preFight   { get; set; }
    private StoryThread winScript  { get; set; }
    private StoryThread lossScript { get; set; }
    private int state; // 0 - pre fight, 1 - in fight, 2 - post fight
    private bool gameWon;
    



    public FightThread(StoryThread pre, Narration[] fightScript, StoryThread win, StoryThread loss) :
        base(fightScript)
    {
        preFight = pre;
        winScript = win;
        lossScript = loss;
    }
    
    public override Narration GetNext()
    {
        switch (state)
        {
               case 0:
                   Narration result = preFight.GetNext();
                   if (result == null)
                   {
                       state++;
                       GameManager.instance.TriggerFlag("IN_BATTLE");
                       GameManager.instance.TriggerFlag("SHIP_CONTROL");
                       return preFight.thread[preFight.thread.Length - 1];
                   }
                    return result;   
               case 1:
                   if (!GameManager.instance.GetFlag("NO_LIVES"))
                   {
                       Narration r = (index < thread.Length ? thread[index++] : null);
                       int stat = 0;
                       thread[index - 1].Cycle(-1, out stat);
                       Battlefield.instance.currentValue = stat;
                       if (r == null)
                       {
                           gameWon = true;
                           state++;
                           GameManager.instance.TriggerFlag("IN_BATTLE");
                           GameManager.instance.TriggerFlag("SHIP_CONTROL");
                           return winScript.GetNext();
                       }
                       return r;
                   }
                   
                   // Get bad text(TM), increment state
                   int BAD;
                   state++;
                   GameManager.instance.TriggerFlag("IN_BATTLE");
                   GameManager.instance.TriggerFlag("SHIP_CONTROL");
                   return thread[index - 1].Cycle(-1, out BAD);
               case 2:
                   return gameWon ? winScript.GetNext() : lossScript.GetNext();
               default: 
                   return null;
        }
    }
}
