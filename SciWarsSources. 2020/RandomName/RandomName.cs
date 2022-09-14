using System;
using System.Collections.Generic;
using System.Text;

using UnityEngine;
using Random = System.Random;

namespace RandomNameGen {

    

    /// <summary>
    /// Class for holding the lists of names from names.json
    /// </summary>
    [Serializable]
    public class NameList {
     
        public string[] boys;
    
        public string[] girls;
     
        public string[] last;

    }
    /// <summary>
    /// RandomName class, used to generate a random name.
    /// </summary>
    public class RandomName {


        Random rand;
        List<string> Male;
        List<string> Female;
        List<string> Last;


        /// <summary>
        /// Initialises a new instance of the RandomName class.
        /// </summary>
        /// <param name="rand">A Random that is used to pick names</param>
        public RandomName(Random rand) {
            this.rand = rand;


            var resource = Resources.Load<TextAsset>("names");
            var l = JsonUtility.FromJson<NameList>(resource.text);


            Male = new List<string>(l.boys);
            Female = new List<string>(l.girls);
            Last = new List<string>(l.last);
        }

        /// <summary>
        /// Returns a new random name
        /// </summary>
        /// <param name="sex">The sex of the person to be named. true for male, false for female</param>
        /// <param name="middle">How many middle names do generate</param>
        /// <param name="isInital">Should the middle names be initials or not?</param>
        /// <returns>The random name as a string</returns>
        public string Generate(Sex sex, int middle = 0, bool isInital = false) {
            string first = sex == Sex.Male ? Male[rand.Next(Male.Count)] : Female[rand.Next(Female.Count)]; // determines if we should select a name from male or female, and randomly picks
            string last = Last[rand.Next(Last.Count)]; // gets the last name

            List<string> middles = new List<string>();

            for (int i = 0; i < middle; i++) {
                if (isInital) {
                    middles.Add("ABCDEFGHIJKLMNOPQRSTUVWXYZ"[rand.Next(0, 25)].ToString() + "."); // randomly selects an uppercase letter to use as the inital and appends a dot
                }
                else {
                    middles.Add(sex == Sex.Male ? Male[rand.Next(Male.Count)] : Female[rand.Next(Female.Count)]); // randomly selects a name that fits with the sex of the person
                }
            }

            StringBuilder b = new StringBuilder();
            b.Append(first + " "); // put a space after our names;
            foreach (string m in middles) {
                b.Append(m + " ");
            }
            b.Append(last);

            return b.ToString();
        }

        /// <summary>
        /// Generates a list of random names
        /// </summary>
        /// <param name="number">The number of names to be generated</param>
        /// <param name="maxMiddleNames">The maximum number of middle names</param>
        /// <param name="sex">The sex of the names, if null sex is randomised</param>
        /// <param name="initials">Should the middle names have initials, if null this will be randomised</param>
        /// <returns>List of strings of names</returns>
        public List<string> RandomNames(int number, int maxMiddleNames, Sex? sex = null, bool? initials = null) {
            List<string> names = new List<string>();

            for (int i = 0; i < number; i++) {
                if (sex != null && initials != null) {
                    names.Add(Generate((Sex)sex, rand.Next(0, maxMiddleNames + 1), (bool)initials));
                }
                else if (sex != null) {
                    bool init = rand.Next(0, 2) != 0;
                    names.Add(Generate((Sex)sex, rand.Next(0, maxMiddleNames + 1), init));
                }
                else if (initials != null) {
                    Sex s = (Sex)rand.Next(0, 2);
                    names.Add(Generate(s, rand.Next(0, maxMiddleNames + 1), (bool)initials));
                }
                else {
                    Sex s = (Sex)rand.Next(0, 2);
                    bool init = rand.Next(0, 2) != 0;
                    names.Add(Generate(s, rand.Next(0, maxMiddleNames + 1), init));
                }
            }

            return names;
        }
    }

    public enum Sex {
        Male,
        Female
    }

    public static class RndName
    {
        public static string RandomNickName {
            get {
                var rand = new Random(UnityEngine.Random.Range(Int32.MinValue, Int32.MaxValue));

                var nameGen = new RandomName(rand);
                var randomName = nameGen.Generate((Sex)UnityEngine.Random.Range(0, 2));


                return randomName;
            }
        }
    }
}
