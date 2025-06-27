using System.Security.Cryptography.X509Certificates;

public class Example
{
    public class Allergy
    // Allergy condition created
    {
        public bool _Allergy;

        public Allergy(bool allergy)
        {
            _Allergy = allergy;
            allergy = false;
        }
    }

    public abstract class EventCycle
    // Class created for the cycle to be followed
    {
        protected EventCycle NextEvent;

        public void SetNextEvent(EventCycle next)
        {
            NextEvent = next;
        }

        public abstract void Chain(Allergy allergy);
    }

    // First stage of the cycle
    public class Detection : EventCycle
    {
        public override void Chain(Allergy allergy)
        {
            if (allergy._Allergy == false) // allergy was initialized as false above
            {
                Console.WriteLine("ALLERGY DETECTED !!!!!");
                Console.WriteLine();
                Console.WriteLine("!!! Use medication !!!");
                Console.WriteLine();

                if (NextEvent != null)
                {
                    allergy._Allergy = false;
                    // We kept allergy as false using the defined variable
                    bool a = allergy._Allergy;

                    NextEvent.Chain(new Allergy(a));
                }
            }
            else
            {
                Console.WriteLine("Normal condition");
                if (NextEvent == null)
                {
                    NextEvent.Chain(new Allergy(false));
                }
            }
        }
    }

    // Second stage of the cycle
    public class UseMedication : EventCycle
    {
        public override void Chain(Allergy allergy)
        {
            bool medication = allergy._Allergy; // medication returns false here   
            if (medication)
            {
                Console.WriteLine("Medication used, reaction stopped.");
                Console.WriteLine();
            }
            else
            {
                Console.WriteLine("Could not use medication, calling 112...");
                Console.WriteLine();
                medication = true;
                bool a = medication; // here medication is set to true, so allergy becomes true too
                NextEvent.Chain(new Allergy(a));
            }
        }
    }

    // Third stage of the cycle
    public class Alert : EventCycle
    {
        public override void Chain(Allergy allergy)
        {
            bool alert = allergy._Allergy; // alert returns true here because we changed it to true above
            if (alert)
            {
                Console.WriteLine("Medical teams and your contacts have been notified");
                Console.WriteLine();
            }
            else
            {
                Console.WriteLine("Help request failed");
            }
        }
    }

    public static void Main(string[] args)
    {
        // Objects created
        Detection detection = new Detection();
        UseMedication medication = new UseMedication();
        Alert alert = new Alert();

        // Chain configured
        detection.SetNextEvent(medication);
        medication.SetNextEvent(alert);

        Console.WriteLine("Case where allergy is detected but medication cannot be used");
        Console.WriteLine("----------------------------------------------");
        var case1 = new Allergy(false);
        detection.Chain(case1);

        Console.WriteLine("*********************************************");

        Console.WriteLine("Case where allergy is not detected");
        Console.WriteLine("----------------------------------------------");

        var case2 = new Allergy(true);
        detection.Chain(case2);
        Console.WriteLine("----------------------------------------------");
    }
}