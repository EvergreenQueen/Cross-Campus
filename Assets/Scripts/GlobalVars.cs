using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// script for all global variables and structs that are used by multiple scripts. if there is a better way to do this please enlighten me
namespace GlobalVars
{
    [CreateAssetMenu(fileName = "NewClub", menuName = "Clubs/ClubScriptableObject")]
    public class Club : ScriptableObject
    {
        public string name;
        public int clubLevel;
        public int[] dates;
        public int[] times;
    }

    [CreateAssetMenu(fileName = "NewCourse", menuName = "Courses/CourseScriptableObject")]
    public class Course : ScriptableObject
    {
        public string name;
        public int[] dates;
        public int[] times;
        public int grade;
    }
    public enum Location {in_class, belltower, hub, wilcoxs_office, et_cetera}; // naming conventions not final
}
