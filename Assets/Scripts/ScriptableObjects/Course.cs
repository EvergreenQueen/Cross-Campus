using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewCourse", menuName = "Courses/CourseScriptableObject")]
public class Course : ScriptableObject
{
    public string courseName;
    public int[] dates;
    public int[] times;
    public int grade;
    public string description;
}