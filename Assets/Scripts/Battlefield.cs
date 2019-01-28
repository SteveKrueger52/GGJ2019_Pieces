using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class Battlefield : MonoBehaviour
{

    public BattleFile battleDb;
    public List<RingData> rings;
    public GameObject ringPrefab;

    public ResponseController ResponseController;
    public Option currentOption;

    private int randRing;
    private bool advanceable = true;
    public float hitProgress;
    public float overallProgress;
    public int lives = 3;
    public List<Ring> battleRings;

    public ParticleSystem hit1;
    public ParticleSystem hit2;

    public TextMeshProUGUI counter;
    public float currentValue = 0;

    public static Battlefield instance = null;

    void Awake()
    {
        //Check if instance already exists
        if (instance == null)
                
            //if not, set instance to this
            instance = this;
            
        //If instance already exists and it's not this:
        else if (instance != this)
                
            //Then destroy this. This enforces our singleton pattern, meaning there can only ever be one instance of a GameManager.
            Destroy(gameObject);    

    }
    // Start is called before the first frame update
    void Start()
    {
        hit1.Stop();
        hit2.Stop();
        hitProgress = 0;
        rings = battleDb.rings;
        battleRings = new List<Ring>();
        for (int i = 0; i < 3; i++)
        {
            AddRing();
        }

        battleRings[1].scaleFactor = 1.4f;
        battleRings[2].scaleFactor = 1.8f;

    }

    // Update is called once per frame
    void Update()
    {
        counter.text = "";
        for (int i = 0; i < lives; i++)
        {
            counter.text += "I";
        }

        if ((lives <= 0))
        {
            BrokenRingRotate();
            if (!GameManager.instance.GetFlag("NO_LIVES")){
                GameManager.instance.TriggerFlag("NO_LIVES");
            }
        }
    }

    public IEnumerator Advance()
    {
        if (advanceable) {
            if (hitProgress >= battleDb.ringCount)
            {
                DealDamage(currentValue);
            }
            else
            {
                hitProgress++;
            }
            advanceable = false;
            foreach (Ring r in battleRings)
            {
                r.IncrementSize();
            }

            for (int i = 0; i < battleRings.Count; i++)
            {
                Ring b = battleRings[i];
                if (b.scaleFactor > 1.99)
                {
                    battleRings.Remove(b);
                    Destroy(b.gameObject);
                    i--;
                }
            }
    
            yield return new WaitForSeconds(0.3f);
            
            AddRing();
            advanceable = true;
        }
    }

    public void DealDamage(float f)
    {
        hit1.Play();
        hit2.Play();

        if (f < 0)
        {
            lives--;
        }

        hitProgress = 0;
    }

    public void BrokenRingRotate()
    {
        foreach (Ring r in battleRings)
        {
            r.transform.Rotate(new Vector3(Random.Range(-3f, 3f), Random.Range(-3f, 3f), Random.Range(-3f, 3f)).normalized * 2);
        }
    }

    public void AddRing()
    {
        battleRings.Add(Instantiate(ringPrefab, transform.position, Quaternion.identity, transform).GetComponent<Ring>());
        battleRings[battleRings.Count - 1].rotateSpeed = rings[Random.Range(0, 8)].speed * (Random.Range(0, 1) > 0.5 ? 1 : -1);
        battleRings[battleRings.Count - 1].mesh = rings[Random.Range(0, 8)].mesh;
        battleRings[battleRings.Count - 1].mesh.transform.GetChild(0).tag = "Ring";

    }

    public IEnumerator SuccessfulHit()
    {
        yield return null;
    }

    public IEnumerator FailedHit()
    {
        yield return null;
    }

    public IEnumerator ListenToDialogue()
    {
        yield return null;
    }
}
