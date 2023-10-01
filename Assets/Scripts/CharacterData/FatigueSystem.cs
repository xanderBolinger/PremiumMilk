using System;
using UnityEngine;

namespace Character {
    public class FatigueSystem
    {
        public int fatiguePoints { private set; get; }

        float analeticValue;
        float recoveryTime;
        float actionElapsedTime;

        public FatigueSystem() { 
        
        }

        public FatigueSystem(Attributes attributes) {
            CalcAV(attributes.str, attributes.hlt);
        }

        public void CalcAV(int strength, int health)
        {
            analeticValue = (health * strength) / 2f;
        }

        public void AddRecoveryTime(float time)
        {
            if (fatiguePoints == 0)
                return;

            float recovery = (analeticValue / 10.0f) / 12.0f;
            recoveryTime += time;

            if (recoveryTime > recovery)
            {

                var recoveredFp = (int)Math.Floor(recoveryTime / recovery);
                fatiguePoints -= recoveredFp;
                recoveryTime -= recovery * recoveredFp;
            }

            if (fatiguePoints < 0)
                fatiguePoints = 0;
        }

        public void AddWork(float time, int workRating = 1)
        {

            actionElapsedTime += time * workRating;

            if (actionElapsedTime > analeticValue / 80)
            {
                var gainedFp = (int)Math.Floor(actionElapsedTime / (analeticValue / 80));
                fatiguePoints += gainedFp;
                actionElapsedTime -= (analeticValue / 80) * gainedFp;
            }

        }


        public void LogStats() {
            Debug.Log("FP: "+fatiguePoints);
            Debug.Log("Analetic Value: "+analeticValue);
            Debug.Log("Action Elapsed Time: "+actionElapsedTime);
            Debug.Log("Recovery Time:"+ recoveryTime);
        }

    }

}

