// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;
// using GlobalVars;
// using TMPro;
// using UnityEngine.UI;
// using System.Linq;
// using UnityEngine.Networking;
// using System;
// using UnityEditor.PackageManager;
// using System.Runtime.CompilerServices;
// using Ink.Parsed;
// using static UnityEditor.Progress;
//
// public class ClassSelection : MonoBehaviour
// {
//     private Club[] fullClubList;
//     private Course[] fullCourseList;
//     private int currentIndex;
//     
//     private int courseButtonIndex = 0;
//     public SerializableDictionary<TextMeshProUGUI, Course> menuButtons = new SerializableDictionary<TextMeshProUGUI, Course>();
//     [SerializeField] private GameObject courseLayout;
//     [SerializeField] private GameObject clubLayout;
//     [SerializeField] private GameObject registrationButton;
//
//     private List<Course> playerCourseList = new List<Course>();
//     private List<Club> playerClubList = new List<Club>();
//
//     void Start()
//     {
//         fullClubList = Resources.LoadAll("Clubs").Cast<Club>().ToArray();
//         fullCourseList = Resources.LoadAll("Courses").Cast<Course>().ToArray();
//         Button currentButton;
//         foreach (Course course in fullCourseList)
//         {
//             currentButton = Instantiate(registrationButton, courseLayout.transform).GetComponent<Button>();
//             currentButton.transform.Find("Text").GetComponent<TextMeshProUGUI>().text = course.courseName;
//             currentButton.onClick.AddListener(delegate { AddOrRemoveCourse(course); });
//         }
//         foreach (Club club in fullClubList)
//         {
//             currentButton = Instantiate(registrationButton, courseLayout.transform).GetComponent<Button>();
//             currentButton.transform.Find("Text").GetComponent<TextMeshProUGUI>().text = club.clubName;
//             currentButton.onClick.AddListener(delegate { AddOrRemoveClub(club); });
//         }
//     }
//
//     void AddOrRemoveCourse(Course course)
//     {
//         if(playerCourseList.Contains(course))
//             playerCourseList.Remove(course);
//         else
//             playerCourseList.Add(course);
//     }
//
//     void AddOrRemoveClub(Club club)
//     {
//         if (playerClubList.Contains(club))
//             playerClubList.Remove(club);
//         else
//             playerClubList.Add(club);
//     }
//
//     public void ConfirmCourses() 
//     {
//         Debug.Log("hooray");
//         //Push list to player class    
//     }
//
//     /*
//     void Start()
//     {
//         
//
//         ChangeCoursePage(courseButtonIndex, 1);
//     }
//
//     public void ChangeCoursePage(int index, int pageForward)
//     {
//
//         ClearButtonListeners(index, pageForward);
//
//         int endingIndex;
//         if (fullCourseList.Length - index < 6) endingIndex = fullCourseList.Length - index;
//         else endingIndex = index + 6;
//         courseButtonIndex = endingIndex;
//         for (int i = index; i < endingIndex; i++)
//         {
//             TextMeshProUGUI currentKey = menuButtons.ElementAt(i).Key;
//             currentKey.text = fullCourseList[i].courseName;
//             menuButtons.SetValue(currentKey, fullCourseList[i]);
//
//             GlobalVars.Course currentCourse = menuButtons.ElementAt(i).Value;
//             menuButtons.ElementAt(i).Key.gameObject.transform.parent.GetComponent<Button>().
//         }
//     }
//
//     public void ShowNextClubs(int index)
//     { 
//         
//     }
//
//     private void ClearButtonListeners(int index, int pageForward)
//     {
//         if (index != 0)
//             for (int i = index - 6; i < index; i++)
//                 menuButtons.ElementAt(i).Key.gameObject.GetComponent<Button>().onClick.RemoveAllListeners();
//     }
//
//
//    
//
//     void ConfirmCourses(List<GlobalVars.Course> courseList)
//     { 
//         //Convert to array and push to player class
//
//     }
//     */
// }