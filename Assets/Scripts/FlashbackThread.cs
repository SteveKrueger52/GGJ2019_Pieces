using UnityEngine;

public class FlashbackThread : StoryThread
{
    private StoryThread preMemory  { get; set; }
    private Narration[] memScript  { get; set; }
    private string scene           { get; set; }
    private StoryThread postMemory { get; set; }
    private int state; // 0 - pre memory, 1 - in memory, 2 - post memory

    public FlashbackThread(StoryThread pre, Narration[] mem, string scene, StoryThread post) : base(mem)
    {
        preMemory = pre;
        this.scene = scene;
        postMemory = post;
    }

    public override Narration GetNext()
    {
        Narration result;
        switch (state)
        {
            case 0:
                result = preMemory.GetNext();
                if (result == null)
                {
                    state++;
                    DialogBox.instance.UpdateBG(Resources.Load<Sprite>(scene));
                    result = index < thread.Length ? thread[index++] : null;
                    
                    if (result != null)
                        return result;
                    
                    // SURPRISE! memScript WAS EMPTY!
                    DialogBox.instance.UpdateBG(null);
                    state++;
                    return postMemory.GetNext();
                }
                return result;
            case 1:
                result = index < thread.Length ? thread[index++] : null;
                if (result != null)
                    return result;
                DialogBox.instance.UpdateBG(null);
                state++;
                return postMemory.GetNext();
            case 2:
                return postMemory.GetNext();
        }

        return null;
    }
}
