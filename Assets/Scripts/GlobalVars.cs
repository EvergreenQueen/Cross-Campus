using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// script for all global variables and structs that are used by multiple scripts. if there is a better way to do this please enlighten me

namespace GlobalVariables
{
    public class GlobalVars
    {
        [CreateAssetMenu(fileName = "NewCourse", menuName = "Courses/CourseScriptableObject")]
        public class Course : ScriptableObject
        {
            public string courseName;
            public int[] dates;
            public int[] times; 
            public int grade;
            public string description;
        }

        [CreateAssetMenu(fileName = "NewClub", menuName = "Clubs/ClubScriptableObject")]
        public class Club : ScriptableObject
        {
            public string clubName;
            public int[] dates;
            public int[] times;
            public string description;
        }

        public enum Location { in_class, belltower, hub, wilcoxs_office, et_cetera }; // naming conventions not final
    }
}
